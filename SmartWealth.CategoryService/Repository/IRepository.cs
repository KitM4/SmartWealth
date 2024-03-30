namespace SmartWealth.CategoryService.Repository;

public interface IRepository<T> where T : class
{
    public Task<List<T>> GetAllAsync();

    public Task<T> GetAsync(Guid id);

    public Task AddAsync(T item);

    public Task UpdateAsync(T updatedItem);

    public Task DeleteAsync(Guid id);
}