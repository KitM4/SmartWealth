using FluentValidation;
using SmartWealth.AccountService.ViewModels;

namespace SmartWealth.AccountService.Utilities.Validators;

public class AccountViewModelValidator : AbstractValidator<AccountViewModel>
{
    public AccountViewModelValidator()
    {
        RuleFor(account => account.Name)
            .NotEmpty().WithMessage("The name must be specified.")
            .MaximumLength(Constants.MaxAccountNameLength).WithMessage($"The name cannot exceed {Constants.MaxAccountNameLength} characters");

        RuleFor(account => account.UserId)
            .NotEmpty().WithMessage("The user ID must be specified");
    }
}