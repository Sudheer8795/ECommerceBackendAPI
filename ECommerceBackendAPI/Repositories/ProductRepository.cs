using ECommerce.Api.Data;
using ECommerce.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _db;
    public ProductRepository(AppDbContext db) => _db = db;

    public async Task AddAsync(Product p) => await _db.Products.AddAsync(p);

    public async Task<Product?> GetByIdAsync(int id) => await _db.Products.Include(x=>x.Category).FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

    public async Task<IEnumerable<Product>> SearchAsync(string? name, int page, int pageSize)
    {
        var q = _db.Products.Where(p => p.IsActive);
        if (!string.IsNullOrEmpty(name)) q = q.Where(p => p.Name.Contains(name));
        return await q.Skip((page-1)*pageSize).Take(pageSize).ToListAsync();
    }

    public async Task SaveChangesAsync() => await _db.SaveChangesAsync();

    public async Task UpdateAsync(Product p)
    {
        _db.Products.Update(p);
    }

    public async Task<bool> ExistsWithNameInCategoryAsync(string name, int categoryId, int? excludeId = null)
    {
        var q = _db.Products.Where(p => p.Name == name && p.CategoryId == categoryId && p.IsActive);
        if (excludeId.HasValue) q = q.Where(p => p.Id != excludeId.Value);
        return await q.AnyAsync();
    }
}
