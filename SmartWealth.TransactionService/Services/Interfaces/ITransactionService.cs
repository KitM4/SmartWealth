using SmartWealth.TransactionService.Models;
using SmartWealth.TransactionService.ViewModels;

namespace SmartWealth.TransactionService.Services.Interfaces;

public interface ITransactionService
{
    public Task<List<Transaction>> GetTransactionsAsync();

    public Task<List<Transaction>> GetTransactionsByAccountAsync(Guid accountId);

    public Task<Transaction> GetTransactionAsync(Guid id);

    public Task<Transaction> CreateTransactionAsync(TransactionViewModel createdTransaction);

    public Task<Transaction> EditTransactionAsync(Guid id, TransactionViewModel editedTransaction);

    public Task<bool> DeleteTransactionAsync(Guid id);

    public Task<List<Transaction>> GenerateDefaultTransactionAsync(Guid accountId);
}