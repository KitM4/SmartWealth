using SmartWealth.AuthService.Models;

namespace SmartWealth.AuthService.Services.Interfaces;

public interface IJwtService
{
    public string GenerateToken(User user);
}