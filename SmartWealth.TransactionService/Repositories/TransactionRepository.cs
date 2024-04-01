using SmartWealth.TransactionService.Models;
using SmartWealth.TransactionService.Database;
using SmartWealth.TransactionService.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace SmartWealth.TransactionService.Repositories;

public class TransactionRepository(DatabaseContext context) : IRepository<Transaction>
{
    private readonly DatabaseContext _context = context;

    public async Task<List<Transaction>> GetAllAsync()
    {
        return await _context.Transactions.AsNoTracking().ToListAsync();
    }

    public async Task<Transaction> GetAsync(Guid id)
    {
        return await _context.Transactions.FirstOrDefaultAsync(transaction => transaction.Id == id) ??
            throw new NotFoundException("Transaction", id.ToString());
    }

    public async Task AddAsync(Transaction item)
    {
        await _context.Transactions.AddAsync(item);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Transaction updatedItem)
    {
        Transaction transactionToUpdate = await GetAsync(updatedItem.Id);

        await _context.Transactions.ExecuteUpdateAsync(setter => setter
            .SetProperty(transaction => transaction.Note, updatedItem.Note)
            .SetProperty(transaction => transaction.Amount, updatedItem.Amount)
            .SetProperty(transaction => transaction.CategoryId, updatedItem.CategoryId));

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        _context.Transactions.Remove(await GetAsync(id));
        await _context.SaveChangesAsync();
    }
}