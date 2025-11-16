using ECommerce.Api.Data;
using ECommerce.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _db;
    public CategoryRepository(AppDbContext db) => _db = db;

    public async Task AddAsync(Category c) => await _db.Categories.AddAsync(c);
    public async Task<IEnumerable<Category>> GetAllAsync() => await _db.Categories.ToListAsync();
    public async Task<Category?> GetByIdAsync(int id) => await _db.Categories.FindAsync(id);
    public async Task SaveChangesAsync() => await _db.SaveChangesAsync();
}
