using AutoMapper;
using Newtonsoft.Json;
using FluentValidation;
using FluentValidation.Results;
using CloudinaryDotNet.Actions;
using SmartWealth.AuthService.Models;
using SmartWealth.AuthService.Database;
using SmartWealth.AuthService.ViewModels;
using SmartWealth.AuthService.ViewModels.DTO;
using SmartWealth.AuthService.Utilities.Enums;
using SmartWealth.AuthService.Services.Interfaces;
using SmartWealth.AuthService.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace SmartWealth.AuthService.Services;

public class AuthService(
    DatabaseContext context,
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    IMapper mapper,
    IHttpService httpService,
    IJwtService jwtService,
    ICloudinaryService cloudinary,
    IValidator<UserLoginViewModel> loginValidator,
    IValidator<UserRegistrationViewModel> registrationValidator) : IAuthService
{
    private readonly DatabaseContext _context = context;
    private readonly UserManager<User> _userManager = userManager;
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly IMapper _mapper = mapper;
    private readonly IJwtService _jwtService = jwtService;
    private readonly IHttpService _httpService = httpService;
    private readonly ICloudinaryService _cloudinary = cloudinary;
    private readonly IValidator<UserLoginViewModel> _loginValidator = loginValidator;
    private readonly IValidator<UserRegistrationViewModel> _registrationValidator = registrationValidator;

    public async Task<bool> IsUserExistAsync(Guid id)
    {
        return await _context.Users.AsNoTracking().AnyAsync(user => user.Id == id);
    }

    public async Task<UserResponse> RegisterAsync(UserRegistrationViewModel userRegistration)
    {
        ValidationResult validationResult = await _registrationValidator.ValidateAsync(userRegistration);
        if (!validationResult.IsValid)
        {
            throw new NotValidException(string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage)));
        }

        User user = _mapper.Map<User>(userRegistration);
        user.Id = Guid.NewGuid();
        user.AccountsId = [];

        if (userRegistration.ProfileImage != null)
        {
            ImageUploadResult imageUploadResult = await _cloudinary.UploadPhotoAsync(userRegistration.ProfileImage);
            user.ProfileImageUrl = imageUploadResult.Url.ToString();
        }

        IdentityResult result = await _userManager.CreateAsync(user, userRegistration.Password);
        if (!result.Succeeded)
        {
            throw new(string.Join(", ", result.Errors.Select(error => error.Description)));
        }

        await _signInManager.SignInAsync(user, true);

        UserResponse userResponse = _mapper.Map<UserResponse>(user);
        userResponse.Token = _jwtService.GenerateToken(user);

        user.AccountsId = await GetUserAccounts(user.Id, userResponse.Token);
        userResponse.AccountsId = user.AccountsId;

        return userResponse;
    }

    public async Task<UserResponse> LoginAsync(UserLoginViewModel userLogin)
    {
        ValidationResult validationResult = await _loginValidator.ValidateAsync(userLogin);
        if (validationResult.IsValid)
        {
            User user = await _context.Users.FirstOrDefaultAsync(user => user.UserName!.ToLower() == userLogin.UserName.ToLower()) ??
                        throw new NotFoundException("User", userLogin.UserName);

            if (await _userManager.CheckPasswordAsync(user, userLogin.Password))
            {
                UserResponse userResponse = _mapper.Map<UserResponse>(user);
                userResponse.Token = _jwtService.GenerateToken(user);

                return userResponse;
            }
            else
            {
                throw new("Incorrect username or password");
            }
        }
        else
        {
            throw new NotValidException(string.Join("\n", validationResult.Errors.Select(error => error.ErrorMessage)));
        }
    }

    public Task<bool> LogoutAsync(Guid id) { throw new NotImplementedException(); }

    public Task<UserResponse> UpdateUserAsync(UserUpdateViewModel userUpdate) { throw new NotImplementedException(); }

    private async Task<List<string>> GetUserAccounts(Guid userId, string accessToken)
    {
        Request request = new()
        {
            ApiType = ApiType.GET,
            ContentType = ContentType.Json,
            Url = $"https://localhost:7185/api/account/defaults/{userId}",
            AccesToken = accessToken,
        };

        Response apiResponse = await _httpService.SendAsync(request);
        return apiResponse.IsSuccess ? JsonConvert.DeserializeObject<List<string>>(apiResponse.Data?.ToString()!) ?? [] : throw new(apiResponse.Message);
    }
}