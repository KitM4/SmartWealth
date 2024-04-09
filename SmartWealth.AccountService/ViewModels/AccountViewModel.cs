using SmartWealth.AccountService.Utilities.Enums;

namespace SmartWealth.AccountService.ViewModels;

public class AccountViewModel
{
    public Guid? Id { get; set; } = Guid.Empty;

    public string Name { get; set; } = string.Empty;

    public AccountType AccountType { get; set; } = AccountType.Cash;

    public Guid UserId { get; set; } = Guid.Empty;

    public List<Guid> TransactionTemplatesId { get; set; } = [];

    public List<Guid> TransactionHistoryId { get; set; } = [];

    public decimal Balance { get; set; } = 0m;

    public string? AccessToken { get; set; } = string.Empty;
}