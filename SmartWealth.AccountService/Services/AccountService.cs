using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using SmartWealth.AccountService.Models;
using SmartWealth.AccountService.ViewModels;
using SmartWealth.AccountService.Repositories;
using SmartWealth.AccountService.Utilities.Enums;
using SmartWealth.AccountService.Services.Interfaces;
using SmartWealth.AccountService.Utilities.Exceptions;

namespace SmartWealth.AccountService.Services;

public class AccountService(IMapper mapper, IRepository<Account> repository, IValidator<AccountViewModel> validator) : IAccountService
{
    private readonly IMapper _mapper = mapper;
    private readonly IRepository<Account> _repository = repository;
    private readonly IValidator<AccountViewModel> _validator = validator;

    public async Task<List<Account>> GetAccountsAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<List<Account>> GetAccountsByUserAsync(Guid userId)
    {
        List<Account> accounts = await _repository.GetAllAsync();
        return accounts.Where(account => account.UserId == userId.ToString()).ToList();
    }

    public async Task<Account> GetAccountAsync(Guid id)
    {
        return await _repository.GetAsync(id);
    }

    public async Task CreateAccountAsync(AccountViewModel createdAccount)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(createdAccount);
        if (validationResult.IsValid)
        {
            Account account = _mapper.Map<Account>(createdAccount);
            account.Id = Guid.NewGuid();

            await _repository.AddAsync(account);
        }
        else
        {
            throw new NotValidException(string.Join("\n", validationResult.Errors.Select(x => x.ErrorMessage)));
        }
    }

    public async Task EditAccountAsync(Guid id, AccountViewModel editedAccount)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(editedAccount);
        if (validationResult.IsValid)
        {
            Account account = _mapper.Map<Account>(editedAccount);
            account.Id = id;

            await _repository.UpdateAsync(account);
        }
        else
        {
            throw new NotValidException(string.Join("\n", validationResult.Errors.Select(x => x.ErrorMessage)));
        }
    }

    public async Task DeleteAccountAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }
        
    public async Task<List<string>> GenerateDefaultAccountsAsync(Guid userId)
    {
        List<string> accountsId = [];

        Account cashAccount = new()
        {
            Id = Guid.NewGuid(),
            Name = "Cash",
            AccountType = AccountType.Cash,
            UserId = userId.ToString(),
            TransactionTemplatesId = [], // TODO: Generate default templates
            TransactionHistoryId = [],
            Balance = 0m,
        };
        accountsId.Add(cashAccount.Id.ToString());
        await _repository.AddAsync(cashAccount);

        Account cardAccount = new()
        {
            Id = Guid.NewGuid(),
            Name = "Card",
            AccountType = AccountType.Card,
            UserId = userId.ToString(),
            TransactionTemplatesId = [], // TODO: Generate defaults templates
            TransactionHistoryId = [],
            Balance = 0m,
        };
        accountsId.Add(cardAccount.Id.ToString());
        await _repository.AddAsync(cardAccount);

        return accountsId;
    }
}