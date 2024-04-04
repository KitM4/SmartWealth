using Microsoft.AspNetCore.Identity;

namespace SmartWealth.AuthService.Models;

public class User : IdentityUser<Guid>
{
    public string? ProfileImageUrl { get; set; }

    public required List<string> AccountsId { get; set; }
}