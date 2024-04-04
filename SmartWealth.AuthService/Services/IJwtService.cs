using SmartWealth.AuthService.Models;

namespace SmartWealth.AuthService.Services;

public interface IJwtService
{
    public string GenerateToken(User user);
}