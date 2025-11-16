using ECommerce.Api.Data;
using ECommerce.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _db;
    public OrderRepository(AppDbContext db) => _db = db;

    public async Task AddAsync(Order o) => await _db.Orders.AddAsync(o);

    public async Task<Order?> GetByIdAsync(int id) => await _db.Orders.Include(x=>x.OrderItems).ThenInclude(oi=>oi.Product).Include(x=>x.Customer).FirstOrDefaultAsync(o=>o.Id==id);

    public async Task<IEnumerable<Order>> GetAllAsync() => await _db.Orders.Include(x=>x.Customer).Include(x=>x.OrderItems).ToListAsync();

    public async Task SaveChangesAsync() => await _db.SaveChangesAsync();
}
