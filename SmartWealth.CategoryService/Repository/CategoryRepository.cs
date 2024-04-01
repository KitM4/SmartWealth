using Microsoft.EntityFrameworkCore;
using SmartWealth.CategoryService.Models;
using SmartWealth.CategoryService.Database;
using SmartWealth.CategoryService.Utilities.Exceptions;

namespace SmartWealth.CategoryService.Repository;

public class CategoryRepository(DatabaseContext context) : IRepository<Category>
{
    private readonly DatabaseContext _context = context;

    public async Task<List<Category>> GetAllAsync()
    {
        return await _context.Categories.AsNoTracking().ToListAsync();
    }

    public async Task<Category> GetAsync(Guid id)
    {
        return await _context.Categories.FirstOrDefaultAsync(category => category.Id == id) ??
            throw new NotFoundException("Category", id.ToString());
    }

    public async Task AddAsync(Category item)
    {
        if (_context.Categories.Any(category => category.Name.ToLower() == item.Name.ToLower()))
            throw new AlreadyExistException(item);

        await _context.Categories.AddAsync(item);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Category updatedItem)
    {
        Category categoryToUpdate = await GetAsync(updatedItem.Id);

        await _context.Categories.ExecuteUpdateAsync(setter => setter
            .SetProperty(category => category.Name, updatedItem.Name)
            .SetProperty(category => category.Description, updatedItem.Description)
            .SetProperty(category => category.IconUrl, updatedItem.IconUrl));

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        _context.Categories.Remove(await GetAsync(id));
        await _context.SaveChangesAsync();
    }
}