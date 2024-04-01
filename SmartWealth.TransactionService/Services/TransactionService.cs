using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using SmartWealth.TransactionService.Models;
using SmartWealth.TransactionService.ViewModels;
using SmartWealth.TransactionService.Repositories;
using SmartWealth.TransactionService.Utilities.Exceptions;

namespace SmartWealth.TransactionService.Services;

public class TransactionService(IMapper mapper, IRepository<Transaction> repository, IValidator<TransactionViewModel> validator) : ITransactionService
{
    private readonly IMapper _mapper = mapper;
    private readonly IRepository<Transaction> _repository = repository;
    private readonly IValidator<TransactionViewModel> _validator = validator;

    public async Task<List<Transaction>> GetTransactionsAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<List<Transaction>> GetTransactionsByAccountAsync(string accountId)
    {
        List<Transaction> transactions = await _repository.GetAllAsync();
        return transactions.Where(transaction => transaction.AccountId == accountId).ToList();
    }

    public async Task<Transaction> GetTransactionAsync(Guid id)
    {
        return await _repository.GetAsync(id);
    }

    public async Task CreateTransactionAsync(TransactionViewModel createdTransaction)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(createdTransaction);
        if (!validationResult.IsValid)
            throw new NotValidException(string.Join("\n", validationResult.Errors.Select(x => x.ErrorMessage)));

        Transaction transaction = _mapper.Map<Transaction>(createdTransaction);
        transaction.CreatedAt = DateTime.UtcNow;

        await _repository.AddAsync(transaction);
    }

    public async Task EditTransactionAsync(Guid id, TransactionViewModel editedTransaction)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(editedTransaction);
        if (!validationResult.IsValid)
            throw new NotValidException(string.Join("\n", validationResult.Errors.Select(x => x.ErrorMessage)));

        await _repository.UpdateAsync(_mapper.Map<Transaction>(editedTransaction));
    }

    public async Task DeleteTransactionAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }
}