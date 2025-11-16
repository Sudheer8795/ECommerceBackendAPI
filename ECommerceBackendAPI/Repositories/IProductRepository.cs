using ECommerce.Api.Models;

namespace ECommerce.Api.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id);
    Task<IEnumerable<Product>> SearchAsync(string? name, int page, int pageSize);
    Task AddAsync(Product p);
    Task SaveChangesAsync();
    Task UpdateAsync(Product p);
    Task<bool> ExistsWithNameInCategoryAsync(string name, int categoryId, int? excludeId = null);
}
