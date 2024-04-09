using System.ComponentModel.DataAnnotations;

namespace SmartWealth.AuthService.Utilities.Validators;

public class ImageFileAttribute : ValidationAttribute
{
    private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png"];
    private const int MaxFileSizeInBytes = 2_500_000;

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success!;
        }

        if (value is not IFormFile file)
        {
            return new ValidationResult("Invalid file");
        }

        string extension = Path.GetExtension(file.FileName);
        if (!_allowedExtensions.Contains(extension.ToLower()))
        {
            return new ValidationResult($"Invalid file extension. Allowed extensions are: {string.Join(", ", _allowedExtensions)}");
        }

        if (file.Length > MaxFileSizeInBytes)
        {
            return new ValidationResult($"File size exceeds the limit of {MaxFileSizeInBytes / 1024 / 1024} MB.");
        }

        return ValidationResult.Success!;
    }
}