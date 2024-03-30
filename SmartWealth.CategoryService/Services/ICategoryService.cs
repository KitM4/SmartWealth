using SmartWealth.CategoryService.Models;
using SmartWealth.CategoryService.ViewModels;

namespace SmartWealth.CategoryService.Services;

public interface ICategoryService
{
    public Task<List<Category>> GetCategoriesAsync();

    public Task<Category> GetCategoryAsync(Guid id);

    public Task CreateCategoryAsync(CategoryViewModel createdCategory);

    public Task EditCategoryAsync(Guid id, CategoryViewModel editedCategory);

    public Task DeleteCategoryAsync(Guid id);
}