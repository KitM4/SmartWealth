using FluentValidation;
using SmartWealth.AuthService.ViewModels;

namespace SmartWealth.AuthService.Utilities.Validators;

public class UserViewModelValidatior : AbstractValidator<UserViewModel>
{
    public UserViewModelValidatior()
    {
        RuleFor(user => user.UserName)
            .NotEmpty().WithMessage("The user name must be specified")
            .MaximumLength(Constants.MaxUserNameLength).WithMessage($"The user name cannot exceed {Constants.MaxUserNameLength} characters");

        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("The password must be specified");

        RuleFor(user => user.Email)
            .EmailAddress().WithMessage("The email is not correct");
    }
}