using SmartWealth.AuthService.ViewModels;
using SmartWealth.AuthService.ViewModels.DTO;

namespace SmartWealth.AuthService.Services.Interfaces;

public interface IAuthService
{
    public Task<bool> IsUserExistAsync(Guid id);

    public Task<UserResponse> RegisterAsync(UserRegistrationViewModel userRegistration);

    public Task<UserResponse> LoginAsync(UserLoginViewModel userLogin);

    public Task<bool> LogoutAsync(Guid id);

    public Task<UserResponse> UpdateUserAsync(UserUpdateViewModel userUpdate);
}