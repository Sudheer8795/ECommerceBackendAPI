using ECommerce.Api.Models;

namespace ECommerce.Api.Repositories;

public interface IOrderRepository
{
    Task AddAsync(Order o);
    Task<Order?> GetByIdAsync(int id);
    Task SaveChangesAsync();
    Task<IEnumerable<Order>> GetAllAsync();
}
