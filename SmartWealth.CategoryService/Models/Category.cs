namespace SmartWealth.CategoryService.Models;

public class Category
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public string? IconUrl { get; set; }

    public override string ToString() => Name;
}