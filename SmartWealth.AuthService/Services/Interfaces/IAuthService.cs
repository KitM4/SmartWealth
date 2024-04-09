using SmartWealth.AuthService.ViewModels;

namespace SmartWealth.AuthService.Services.Interfaces;

public interface IAuthService
{
    public Task<bool> IsUserExistAsync(Guid id);

    public Task<UserResponse> RegisterAsync(UserViewModel userViewModel);

    public Task<UserResponse> LoginAsync(UserViewModel userViewModel);

    public Task<UserResponse> UpdateUserAsync(UserViewModel userViewModel);
}