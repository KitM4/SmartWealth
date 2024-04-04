using SmartWealth.AuthService.ViewModels;

namespace SmartWealth.AuthService.Services;

public interface IAuthService
{
    public Task<string> RegisterAsync(UserRegistrationViewModel userRegistration);

    public Task<string> LoginAsync(UserLoginViewModel userLogin);

    public Task LogoutAsync(string userId);

    public Task<string> RefreshTokenAsync(string refreshToken);

    public Task<bool> VerifyTokenAsync(string token);
}