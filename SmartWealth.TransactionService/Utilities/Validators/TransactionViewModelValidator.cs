using FluentValidation;
using SmartWealth.TransactionService.ViewModels;

namespace SmartWealth.TransactionService.Utilities.Validators;

public class TransactionViewModelValidator : AbstractValidator<TransactionViewModel>
{
    public TransactionViewModelValidator()
    {
        RuleFor(transaction => transaction.CategoryId)
            .NotEmpty().WithMessage("The category must be specified");

        RuleFor(transaction => transaction.Note)
            .NotEmpty().WithMessage("The note must be specified.")
            .MaximumLength(Constants.MaxTransactionNoteLength).WithMessage($"The note cannot exceed {Constants.MaxTransactionNoteLength} characters");

        RuleFor(transaction => transaction.AccountId)
            .NotEmpty().WithMessage("The account must be specified");

        RuleFor(transaction => transaction.Amount)
            .NotEqual(Constants.ProhibitedTransactionAmount).WithMessage($"Amount must not be {Constants.ProhibitedTransactionAmount}");
    }
}