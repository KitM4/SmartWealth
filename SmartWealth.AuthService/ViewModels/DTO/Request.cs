using SmartWealth.AuthService.Utilities.Enums;

namespace SmartWealth.AuthService.ViewModels.DTO;

public class Request
{
    public required ApiType ApiType { get; set; }

    public required ContentType ContentType { get; set; }

    public required string Url { get; set; }

    public object? Data { get; set; }

    public string? AccesToken { get; set; }
}