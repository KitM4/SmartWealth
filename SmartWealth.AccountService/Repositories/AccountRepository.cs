﻿using SmartWealth.AccountService.Models;
using SmartWealth.AccountService.Database;
using SmartWealth.AccountService.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace SmartWealth.AccountService.Repositories;

public class AccountRepository(DatabaseContext context) : IRepository<Account>
{
    private readonly DatabaseContext _context = context;

    public async Task<List<Account>> GetAllAsync()
    {
        return await _context.Accounts.AsNoTracking().ToListAsync();
    }

    public async Task<Account> GetAsync(Guid id)
    {
        return await _context.Accounts.FirstOrDefaultAsync(account => account.Id == id) ??
            throw new NotFoundException("Account", id.ToString());
    }

    public async Task AddAsync(Account item)
    {
        if (_context.Accounts.Any(account => account.Name.ToLower() == item.Name.ToLower()))
            throw new AlreadyExistException(item);

        await _context.Accounts.AddAsync(item);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Account updatedItem)
    {
        await _context.Accounts
            .Where(transaction => transaction.Id == updatedItem.Id)
            .ExecuteUpdateAsync(setter => setter
                .SetProperty(account => account.Name, updatedItem.Name)
                .SetProperty(account => account.AccountType, updatedItem.AccountType)
                .SetProperty(account => account.TransactionTemplatesId, updatedItem.TransactionTemplatesId)
                .SetProperty(account => account.TransactionHistoryId, updatedItem.TransactionHistoryId)
                .SetProperty(account => account.Balance, updatedItem.Balance));

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        _context.Accounts.Remove(await GetAsync(id));
        await _context.SaveChangesAsync();
    }
}