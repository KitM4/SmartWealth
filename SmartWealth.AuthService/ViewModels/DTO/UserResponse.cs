namespace SmartWealth.AuthService.ViewModels.DTO;

public class UserResponse
{
    public Guid Id { get; set; } = Guid.Empty;

    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;

    public List<string> AccountsId { get; set; } = [];

    public string? ProfileImageUrl { get; set; } = string.Empty;
}