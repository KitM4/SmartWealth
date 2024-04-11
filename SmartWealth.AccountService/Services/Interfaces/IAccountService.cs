using SmartWealth.AccountService.Models;
using SmartWealth.AccountService.ViewModels;

namespace SmartWealth.AccountService.Services.Interfaces;

public interface IAccountService
{
    public Task<List<Account>> GetAccountsAsync();

    public Task<List<Account>> GetAccountsByUserAsync(Guid userId);

    public Task<Account> GetAccountAsync(Guid id);

    public Task<Account> CreateAccountAsync(AccountViewModel createdAccount);

    public Task<Account> EditAccountAsync(AccountViewModel editedAccount);

    public Task<bool> DeleteAccountAsync(Guid id);

    public Task<Guid> GenerateDefaultAccountAsync(Guid userId);
}