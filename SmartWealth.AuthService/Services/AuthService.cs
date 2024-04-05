using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using CloudinaryDotNet.Actions;
using SmartWealth.AuthService.Models;
using SmartWealth.AuthService.Database;
using SmartWealth.AuthService.ViewModels;
using SmartWealth.AuthService.ViewModels.DTO;
using SmartWealth.AuthService.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace SmartWealth.AuthService.Services;

public class AuthService(
    DatabaseContext context,
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    IMapper mapper,
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
    private readonly ICloudinaryService _cloudinary = cloudinary;
    private readonly IValidator<UserLoginViewModel> _loginValidator = loginValidator;
    private readonly IValidator<UserRegistrationViewModel> _registrationValidator = registrationValidator;

    public async Task<UserResponse> RegisterAsync(UserRegistrationViewModel userRegistration)
    {
        ValidationResult validationResult = await _registrationValidator.ValidateAsync(userRegistration);
        if (validationResult.IsValid)
        {
            Guid id = Guid.NewGuid();
            List<string> accountsId = []; //TODO: implement serivce communication

            User user = _mapper.Map<User>(userRegistration);
            user.Id = id;
            user.AccountsId = accountsId;
            if (userRegistration.ProfileImage != null)
            {
                ImageUploadResult imageUploadResult = await _cloudinary.UploadPhotoAsync(userRegistration.ProfileImage);
                user.ProfileImageUrl = imageUploadResult.Url.ToString();
            }

            IdentityResult result = await _userManager.CreateAsync(user, userRegistration.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, true);

                UserResponse userResponse = _mapper.Map<UserResponse>(user);
                userResponse.Token = _jwtService.GenerateToken(user);

                return userResponse;
            }
            else
            {
                throw new(string.Join(", ", result.Errors.Select(error => error.Description)));
            }
        }
        else
        {
            throw new NotValidException(string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage)));
        }
    }

    public async Task<string> LoginAsync(UserLoginViewModel userLogin)
    {
        ValidationResult validationResult = await _loginValidator.ValidateAsync(userLogin);
        if (validationResult.IsValid)
        {
            User user = await _context.Users.FirstOrDefaultAsync(user => user.UserName!.ToLower() == userLogin.UserName.ToLower()) ??
                        throw new NotFoundException("User", userLogin.UserName);

            if (await _userManager.CheckPasswordAsync(user, userLogin.Password))
            {
                return _jwtService.GenerateToken(user);
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

    //public Task<Response> LogoutAsync(string id)
    //{
    //    throw new NotImplementedException();
    //}

    //public Task<Response> UpdateProfileAsync(UserUpdateViewModel userUpdate)
    //{
    //    throw new NotImplementedException();
    //}

    //public Task<Response> RefreshTokenAsync(string refreshToken)
    //{
    //    throw new NotImplementedException();
    //}

    //public Task<Response> VerifyTokenAsync(string token)
    //{
    //    throw new NotImplementedException();
    //}
}