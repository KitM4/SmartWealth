namespace SmartWealth.CategoryService.Utilities.Exeptions;

public class NotValidException : Exception
{
    public NotValidException(string message) : base(message) { }

    public NotValidException(string[] validationErrors) : base(string.Join(Environment.NewLine, validationErrors)) { }
}