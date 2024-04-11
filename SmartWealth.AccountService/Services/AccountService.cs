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
        return accounts.Where(account => account.UserId == userId).ToList();
    }

    public async Task<Account> GetAccountAsync(Guid id)
    {
        return await _repository.GetAsync(id);
    }

    public async Task<Account> CreateAccountAsync(AccountViewModel accountViewModel)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(accountViewModel);
        if (!validationResult.IsValid)
        {
            throw new NotValidException(string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage)));
        }

        Account account = _mapper.Map<Account>(accountViewModel);
        account.Id = Guid.NewGuid();

        await _repository.AddAsync(account);
        return account;
    }

    public async Task<Account> EditAccountAsync(AccountViewModel accountViewModel)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(accountViewModel);
        if (!validationResult.IsValid)
        {
            throw new NotValidException(string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage)));
        }

        Account account = _mapper.Map<Account>(accountViewModel);

        await _repository.UpdateAsync(account);
        return await _repository.GetAsync(account.Id);
    }

    public async Task<bool> DeleteAccountAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<Guid> GenerateDefaultAccountAsync(Guid userId)
    {
        Guid id = Guid.NewGuid();
        Account cashAccount = new()
        {
            Id = id,
            Name = "My Cash",
            AccountType = AccountType.Cash,
            UserId = userId,
            TransactionTemplatesId = [], // TODO: Generate default templates
            TransactionHistoryId = [],
            Balance = 0m,
        };

        await _repository.AddAsync(cashAccount);
        return id;
    }
}