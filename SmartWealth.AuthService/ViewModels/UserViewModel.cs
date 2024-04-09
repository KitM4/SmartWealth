using SmartWealth.AuthService.Utilities.Validators;

namespace SmartWealth.AuthService.ViewModels;

public class UserViewModel
{
    public Guid? Id { get; set; } = Guid.Empty;

    public string UserName { get; set; } = string.Empty;

    public string? Email { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;

    [ImageFile(ErrorMessage = "Please upload a valid image file")]
    public IFormFile? ProfileImage { get; set; } = null;

    public List<Guid>? AccountsId { get; set; } = [];
}