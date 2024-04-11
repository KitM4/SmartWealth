using AutoMapper;
using Newtonsoft.Json;
using FluentValidation;
using FluentValidation.Results;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartWealth.AuthService.Models;
using SmartWealth.AuthService.Database;
using SmartWealth.AuthService.ViewModels;
using SmartWealth.AuthService.Utilities.Enums;
using SmartWealth.AuthService.Services.Interfaces;
using SmartWealth.AuthService.Utilities.Exceptions;

namespace SmartWealth.AuthService.Services;

public class AuthService(
    DatabaseContext context,
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    IMapper mapper,
    IHttpService httpService,
    IJwtService jwtService,
    ICloudinaryService cloudinary,
    IValidator<UserViewModel> validator) : IAuthService
{
    private readonly DatabaseContext _context = context;
    private readonly UserManager<User> _userManager = userManager;
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly IMapper _mapper = mapper;
    private readonly IJwtService _jwtService = jwtService;
    private readonly IHttpService _httpService = httpService;
    private readonly ICloudinaryService _cloudinary = cloudinary;
    private readonly IValidator<UserViewModel> _validator = validator;

    public async Task<bool> IsUserExistAsync(Guid id)
    {
        return await _context.Users.AsNoTracking().AnyAsync(user => user.Id == id);
    }

    public async Task<UserResponse> RegisterAsync(UserViewModel userViewModel)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(userViewModel);
        if (!validationResult.IsValid)
        {
            throw new NotValidException(string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage)));
        }

        User user = _mapper.Map<User>(userViewModel);
        user.Id = Guid.NewGuid();
        user.AccountsId = [];

        if (userViewModel.ProfileImage != null)
        {
            ImageUploadResult imageUploadResult = await _cloudinary.UploadPhotoAsync(userViewModel.ProfileImage);
            user.ProfileImageUrl = imageUploadResult.Url.ToString();
        }

        IdentityResult result = await _userManager.CreateAsync(user, userViewModel.Password);
        if (!result.Succeeded)
        {
            throw new(string.Join(", ", result.Errors.Select(error => error.Description)));
        }

        await _signInManager.SignInAsync(user, true);

        UserResponse userResponse = _mapper.Map<UserResponse>(user);
        userResponse.AccessToken = _jwtService.GenerateToken(user);

        Request request = new()
        {
            ApiType = ApiType.Get,
            ContentType = ContentType.Json,
            Url = $"https://localhost:7185/api/account/defaults/{userResponse.Id}", // TODO: create apis config
            AccessToken = userResponse.AccessToken,
        };

        Response apiResponse = await _httpService.SendAsync(request);
        if (apiResponse.IsSuccess)
        {
            Guid accountId = JsonConvert.DeserializeObject<Guid>(apiResponse.Data!.ToString()!);
            user.AccountsId.Add(accountId);
            userResponse.AccountsId.Add(accountId);

            await _context.SaveChangesAsync();
        }

        return userResponse;
    }

    public async Task<UserResponse> LoginAsync(UserViewModel userViewModel)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(userViewModel);
        if (!validationResult.IsValid)
        {
            throw new NotValidException(string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage)));
        }

        User user = await _context.Users.FirstOrDefaultAsync(user => user.UserName!.ToLower() == userViewModel.UserName.ToLower()) ??
                        throw new NotFoundException("User", userViewModel.UserName);

        if (await _userManager.CheckPasswordAsync(user, userViewModel.Password))
        {
            UserResponse userResponse = _mapper.Map<UserResponse>(user);
            userResponse.AccessToken = _jwtService.GenerateToken(user);

            return userResponse;
        }
        else
        {
            throw new("Incorrect username or password");
        }
    }

    public async Task<UserResponse> UpdateUserAsync(UserViewModel userViewModel)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(userViewModel);
        if (!validationResult.IsValid)
        {
            throw new NotValidException(string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage)));
        }

        User user = await _context.Users.FirstOrDefaultAsync(user => user.Id == userViewModel.Id) ??
                        throw new NotFoundException("User", userViewModel.UserName);

        user.UserName = userViewModel.UserName;
        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, userViewModel.Password);
        if (userViewModel.ProfileImage != null)
        {
            if (user.ProfileImageUrl != null)
            {
                string[] urlParts = user.ProfileImageUrl.Split('/');
                string[] filenameParts = urlParts[^1].Split('.');

                await _cloudinary.DeletePhotoAsync(filenameParts[0]);
                user.ProfileImageUrl = string.Empty;
                // TODO: handle error
            }

            ImageUploadResult imageUploadResult = await _cloudinary.UploadPhotoAsync(userViewModel.ProfileImage);
            user.ProfileImageUrl = imageUploadResult.Url.ToString();
        }

        if (userViewModel.AccountsId != null && userViewModel.AccountsId.Count > 0)
        {
            user.AccountsId = userViewModel.AccountsId;
        }

        await _context.SaveChangesAsync();
        return _mapper.Map<UserResponse>(user);
    }
}