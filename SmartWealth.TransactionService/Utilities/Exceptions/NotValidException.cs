namespace SmartWealth.TransactionService.Utilities.Exceptions;

public class NotValidException : Exception
{
    public NotValidException(string message) : base(message) { }

    public NotValidException(string[] validationErrors) : base(string.Join(Environment.NewLine, validationErrors)) { }
}