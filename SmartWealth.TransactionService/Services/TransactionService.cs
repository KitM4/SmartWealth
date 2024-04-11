using AutoMapper;
using Newtonsoft.Json;
using FluentValidation;
using FluentValidation.Results;
using SmartWealth.TransactionService.Models;
using SmartWealth.TransactionService.ViewModels;
using SmartWealth.TransactionService.Repositories;
using SmartWealth.TransactionService.Utilities.Exceptions;
using SmartWealth.TransactionService.Services.Interfaces;
using SmartWealth.TransactionService.Utilities.Enums;

namespace SmartWealth.TransactionService.Services;

public class TransactionService(IMapper mapper, IHttpService httpService, IRepository<Transaction> repository, IValidator<TransactionViewModel> validator) : ITransactionService
{
    private readonly IMapper _mapper = mapper;
    private readonly IHttpService _httpService = httpService;
    private readonly IRepository<Transaction> _repository = repository;
    private readonly IValidator<TransactionViewModel> _validator = validator;

    public async Task<List<Transaction>> GetTransactionsAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<List<Transaction>> GetTransactionsByAccountAsync(Guid accountId)
    {
        List<Transaction> transactions = await _repository.GetAllAsync();
        return transactions.Where(transaction => transaction.AccountId == accountId).ToList();
    }

    public async Task<Transaction> GetTransactionAsync(Guid id)
    {
        return await _repository.GetAsync(id);
    }

    public async Task<Transaction> CreateTransactionAsync(TransactionViewModel transactionViewModel)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(transactionViewModel);
        if (!validationResult.IsValid)
        {
            throw new NotValidException(string.Join("\n", validationResult.Errors.Select(x => x.ErrorMessage)));
        }

        Request getAccountRequest = new()
        {
            ApiType = ApiType.Get,
            ContentType = ContentType.Json,
            Url = $"https://localhost:7185/api/account/{transactionViewModel.AccountId}",
            AccessToken = transactionViewModel.AccessToken,
        };

        Response getAccountResponse = await _httpService.SendAsync(getAccountRequest);
        if (!getAccountResponse.IsSuccess)
        {
            throw new NotFoundException("Account not found");
        }

        Transaction transaction = _mapper.Map<Transaction>(transactionViewModel);
        transaction.Id = Guid.NewGuid();
        transaction.CreatedAt = DateTime.UtcNow;

        AccountViewModel? account = JsonConvert.DeserializeObject<AccountViewModel>(getAccountResponse.Data!.ToString()!);
        if (account != null)
        {
            account.Balance += transaction.Amount;
            account.TransactionHistoryId.Add(transaction.Id);
        }

        Request putAccountRequest = new()
        {
            ApiType = ApiType.Put,
            ContentType = ContentType.Json,
            Url = $"https://localhost:7185/api/account/edit",
            Data = account,
            AccessToken = transactionViewModel.AccessToken,
        };

        Response putAccountResponse = await _httpService.SendAsync(putAccountRequest);
        if (putAccountResponse.IsSuccess)
        {
            await _repository.AddAsync(transaction);
            return transaction;
        }
        else
        {
            throw new(putAccountResponse.Message);
        }
    }

    public async Task<Transaction> EditTransactionAsync(Guid id, TransactionViewModel editedTransaction)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(editedTransaction);
        if (!validationResult.IsValid)
        {
            throw new NotValidException(string.Join("\n", validationResult.Errors.Select(x => x.ErrorMessage)));
        }

        Transaction transaction = _mapper.Map<Transaction>(editedTransaction);
        transaction.Id = id;

        await _repository.UpdateAsync(transaction);
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteTransactionAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
        throw new NotImplementedException();
    }

    public Task<List<Transaction>> GenerateDefaultTransactionAsync(Guid accountId)
    {
        throw new NotImplementedException();
    }
}