namespace SmartWealth.TransactionService.Models;

public class Transaction
{
    public required Guid Id { get; set; }

    public required string CategoryId { get; set; }

    public required string Note { get; set; }

    public required decimal Amount { get; set; }

    public required string AccountId { get; set; }

    public required DateTime CreatedAt { get; set; }
}