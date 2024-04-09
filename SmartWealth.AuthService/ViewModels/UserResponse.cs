namespace SmartWealth.AuthService.ViewModels;

public class UserResponse
{
    public Guid Id { get; set; } = Guid.Empty;

    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string AccessToken { get; set; } = string.Empty;

    public string? ProfileImageUrl { get; set; } = string.Empty;

    public List<Guid> AccountsId { get; set; } = [];
}