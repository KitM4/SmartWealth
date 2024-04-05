namespace SmartWealth.AuthService.ViewModels.DTO;

public class Response
{
    public bool IsSuccess { get; set; } = true;

    public string? Message { get; set; } = string.Empty;

    public object? Data { get; set; } = null;
}