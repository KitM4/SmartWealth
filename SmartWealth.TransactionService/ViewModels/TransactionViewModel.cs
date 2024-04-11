namespace SmartWealth.TransactionService.ViewModels;

public class TransactionViewModel
{
    public Guid CategoryId { get; set; } = Guid.Empty;

    public string Note { get; set; } = string.Empty;

    public Guid AccountId { get; set; } = Guid.Empty;

    public decimal Amount { get; set; } = 0m;

    public string AccessToken { get; set; } = string.Empty;
}