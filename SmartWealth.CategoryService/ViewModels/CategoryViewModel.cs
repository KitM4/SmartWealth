namespace SmartWealth.CategoryService.ViewModels;

public class CategoryViewModel
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public IFormFile? Icon { get; set; }
}