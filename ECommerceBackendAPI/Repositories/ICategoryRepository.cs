using ECommerce.Api.Models;

namespace ECommerce.Api.Repositories;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(int id);
    Task<IEnumerable<Category>> GetAllAsync();
    Task AddAsync(Category c);
    Task SaveChangesAsync();
}
