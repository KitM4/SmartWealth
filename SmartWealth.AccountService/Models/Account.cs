using SmartWealth.AccountService.Utilities.Enums;

namespace SmartWealth.AccountService.Models;

public class Account
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public required AccountType AccountType { get; set; }

    public required Guid UserId { get; set; }

    public required List<Guid> TransactionTemplatesId { get; set; }

    public required List<Guid> TransactionHistoryId { get; set; }

    public required decimal Balance { get; set; }
}