namespace SmartWealth.AuthService.ViewModels;

public class UserUpdateViewModel
{
    public string UserName { get; set; } = string.Empty;

    public IFormFile? ProfileImage { get; set; } = null;
}