using SmartWealth.TransactionService.Utilities.Enums;

namespace SmartWealth.TransactionService.ViewModels;

public class Request
{
    public required ApiType ApiType { get; set; }

    public required ContentType ContentType { get; set; }

    public required string Url { get; set; }

    public object? Data { get; set; }

    public string? AccessToken { get; set; }
}