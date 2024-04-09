using SmartWealth.TransactionService.Models;
using SmartWealth.TransactionService.ViewModels;

namespace SmartWealth.TransactionService.Services.Interfaces;

public interface ITransactionService
{
    public Task<List<Transaction>> GetTransactionsAsync();

    public Task<List<Transaction>> GetTransactionsByAccountAsync(string accountId);

    public Task<Transaction> GetTransactionAsync(Guid id);

    public Task<decimal> CreateTransactionAsync(TransactionViewModel createdTransaction);

    public Task EditTransactionAsync(Guid id, TransactionViewModel editedTransaction);

    public Task DeleteTransactionAsync(Guid id);
}