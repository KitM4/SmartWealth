using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SmartWealth.AuthService.Models;
using SmartWealth.AuthService.Database;
using SmartWealth.AuthService.ViewModels;
using Microsoft.EntityFrameworkCore;
using SmartWealth.AuthService.Utilities.Exceptions;

namespace SmartWealth.AuthService.Services;

public class AuthService(
    DatabaseContext context,
    UserManager<User> userManager,
    SignInManager<User> signInManager) : IAuthService
{
    private readonly DatabaseContext _context = context;
    private readonly UserManager<User> _userManager = userManager;
    private readonly SignInManager<User> _signInManager = signInManager;

    public async Task<string> RegisterAsync(UserRegistrationViewModel userRegistration)
    {
        //TODO: validation

        Guid id = Guid.NewGuid();
        List<string> accountsId = []; //TODO: implement serivce communication
        string profileImageUrl = string.Empty; //TODO: implement image uploading services

        User user = new()
        {
            Id = id,
            UserName = userRegistration.UserName,
            Email = userRegistration.Email,
            AccountsId = accountsId,
            ProfileImageUrl = profileImageUrl,
        };

        IdentityResult result = await _userManager.CreateAsync(user, userRegistration.Password);
        if (!result.Succeeded)
        {
            string errorMessage = string.Empty;
            foreach (IdentityError error in result.Errors)
                errorMessage += string.Join(", ", error.Description);

            throw new($"Register failed:\n{errorMessage}");
        }

        await _signInManager.SignInAsync(user, true);
        //return _jwtService.GenerateToken(user); //TODO: implement jwt service

        return string.Empty;
    }

    public async Task<string> LoginAsync(UserLoginViewModel userLogin)
    {
        //TODO: validation

        User user = await _context.Users.FirstOrDefaultAsync(user => user.UserName!.ToLower() == userLogin.UserName.ToLower()) ??
            throw new NotFoundException("User", userLogin.UserName);

        if (await _userManager.CheckPasswordAsync(user, userLogin.Password))
        {
            //return _jwtService.GenerateToken(user); //TODO: implement jwt service
            return string.Empty;
        }
        else
        {
            throw new("Incorrect username or password");
        }
    }

    public Task LogoutAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<string> RefreshTokenAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> VerifyTokenAsync(string token)
    {
        throw new NotImplementedException();
    }
}