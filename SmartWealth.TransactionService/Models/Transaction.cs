namespace SmartWealth.TransactionService.Models;

public class Transaction
{
    public required Guid Id { get; set; }

    public required Guid CategoryId { get; set; }

    public required string Note { get; set; }

    public required decimal Amount { get; set; }

    public required Guid AccountId { get; set; }

    public required DateTime CreatedAt { get; set; }
}