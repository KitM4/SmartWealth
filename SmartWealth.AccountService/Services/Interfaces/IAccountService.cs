using SmartWealth.AccountService.Models;
using SmartWealth.AccountService.ViewModels;

namespace SmartWealth.AccountService.Services.Interfaces;

public interface IAccountService
{
    public Task<List<Account>> GetAccountsAsync();

    public Task<List<Account>> GetAccountsByUserAsync(Guid userId);

    public Task<Account> GetAccountAsync(Guid id);

    public Task CreateAccountAsync(AccountViewModel createdAccount);

    public Task EditAccountAsync(Guid id, AccountViewModel editedAccount);

    public Task DeleteAccountAsync(Guid id);

    public Task<List<string>> GenerateDefaultAccountsAsync(Guid userId);
}