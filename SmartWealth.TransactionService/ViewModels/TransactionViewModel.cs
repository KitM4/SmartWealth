namespace SmartWealth.TransactionService.ViewModels;

public class TransactionViewModel
{
    public string CategoryId { get; set; } = string.Empty;

    public string Note { get; set; } = string.Empty;

    public string AccountId { get; set; } = string.Empty;

    public decimal Amount { get; set; } = 0m;
}