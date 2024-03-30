using FluentValidation;
using SmartWealth.CategoryService.ViewModels;

namespace SmartWealth.CategoryService.Utilities.Validators;

public class CategoryViewModelValidator : AbstractValidator<CategoryViewModel>
{
    public CategoryViewModelValidator()
    {
        RuleFor(category => category.Name)
            .NotEmpty().WithMessage("The name must be specified")
            .MaximumLength(Constants.MaxCategoryNameLength)
            .WithMessage($"The name cannot exceed {Constants.MaxCategoryNameLength} characters");

        RuleFor(category => category.Description)
            .MaximumLength(Constants.MaxCategoryDescriptionLength)
            .WithMessage($"Description cannot exceed {Constants.MaxCategoryDescriptionLength} characters");
    }
}