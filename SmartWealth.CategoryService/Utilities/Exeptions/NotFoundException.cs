namespace SmartWealth.CategoryService.Utilities.Exeptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }

    public NotFoundException(object @object, string key) : base($"{@object} {key} not found") { }
}