using SmartWealth.AccountService.Models;
using SmartWealth.AccountService.ViewModels;

namespace SmartWealth.AccountService.Services;

public interface IAccountService
{
    public Task<List<Account>> GetAccountsAsync();

    public Task<List<Account>> GetAccountsByUserAsync(string userId);

    public Task<Account> GetAccountAsync(Guid id);

    public Task CreateAccountAsync(AccountViewModel createdAccount);

    public Task EditAccountAsync(Guid id, AccountViewModel editedAccount);

    public Task DeleteAccountAsync(Guid id);

    public Task<List<string>> GenerateDefaultAccountsAsync(string userId);
}