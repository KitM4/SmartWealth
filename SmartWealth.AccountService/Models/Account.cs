using SmartWealth.AccountService.Utilities.Enums;

namespace SmartWealth.AccountService.Models;

public class Account
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public required AccountType AccountType { get; set; }

    public required string UserId { get; set; }

    public required List<string> TransactionTemplatesId { get; set; }

    public required List<string> TransactionHistoryId { get; set; }

    public required decimal Balance { get; set; }
}