using SmartWealth.AccountService.Utilities.Enums;

namespace SmartWealth.AccountService.ViewModels;

public class Request
{
    public required ApiType ApiType { get; set; } = ApiType.Get;

    public required ContentType ContentType { get; set; } = ContentType.Json;

    public required string Url { get; set; } = string.Empty;

    public object? Data { get; set; } = null;

    public string? AccessToken { get; set; } = string.Empty;
}