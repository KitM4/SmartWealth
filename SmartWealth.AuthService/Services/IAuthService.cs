using SmartWealth.AuthService.ViewModels;
using SmartWealth.AuthService.ViewModels.DTO;

namespace SmartWealth.AuthService.Services;

public interface IAuthService
{
    public Task<UserResponse> RegisterAsync(UserRegistrationViewModel userRegistration);

    public Task<string> LoginAsync(UserLoginViewModel userLogin);

    //public Task<Response> LogoutAsync(string id);

    //public Task<Response> UpdateProfileAsync(UserUpdateViewModel userUpdate);

    //public Task<Response> RefreshTokenAsync(string refreshToken);

    //public Task<Response> VerifyTokenAsync(string token);
}