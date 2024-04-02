using FluentValidation;
using SmartWealth.AccountService.ViewModels;

namespace SmartWealth.AccountService.Utilities.Validators;

public class AccountViewModelValidator : AbstractValidator<AccountViewModel>
{
    public AccountViewModelValidator()
    {
        RuleFor(category => category.Name)
            .NotEmpty().WithMessage("The name must be specified.")
            .MaximumLength(Constants.MaxAccountNameLength).WithMessage($"The name cannot exceed {Constants.MaxAccountNameLength} characters");

        RuleFor(transaction => transaction.UserId)
            .NotEmpty().WithMessage("The user must be specified")
            .MaximumLength(Constants.MaxIdLength).WithMessage($"The user ID cannot exceed {Constants.MaxIdLength} characters")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("User ID must be a Guid");
    }
}