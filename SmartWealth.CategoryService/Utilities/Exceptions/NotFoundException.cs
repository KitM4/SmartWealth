namespace SmartWealth.CategoryService.Utilities.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }

    public NotFoundException(object @object, string key) : base($"{@object} {key} not found") { }
}