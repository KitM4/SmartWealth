using SmartWealth.AccountService.Utilities.Enums;

namespace SmartWealth.AccountService.ViewModels;

public class AccountViewModel
{
    public string Name { get; set; } = string.Empty;

    public AccountType AccountType { get; set; } = AccountType.Cash;

    public string UserId { get; set; } = string.Empty;

    public List<string> TransactionTemplatesId { get; set; } = [];

    public List<string> TransactionHistoryId { get; set; } = [];

    public decimal Balance { get; set; } = 0m;
}