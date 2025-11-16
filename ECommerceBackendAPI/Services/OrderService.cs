using ECommerce.Api.DTOs;
using ECommerce.Api.Models;
using ECommerce.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orders;
    private readonly IProductRepository _products;
    private readonly IUserRepository _users;
    private readonly AppDbContext _db;

    public OrderService(IOrderRepository orders, IProductRepository products, IUserRepository users, AppDbContext db)
    {
        _orders = orders;
        _products = products;
        _users = users;
        _db = db;
    }

    public async Task<int> PlaceOrderAsync(int customerId, CreateOrderDto dto)
    {
        var user = await _users.GetByIdAsync(customerId) ?? throw new Exception("Customer not found");
        // Start transaction
        using var tx = await _db.Database.BeginTransactionAsync();
        try
        {
            var order = new Order { CustomerId = customerId, OrderDate = DateTime.UtcNow };
            decimal total = 0m;
            foreach (var item in dto.Items)
            {
                var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId && p.IsActive);
                if (product == null) throw new Exception($"Product {item.ProductId} not found");
                if (product.StockQuantity < item.Quantity) throw new Exception($"Insufficient stock for {product.Name}");
                product.StockQuantity -= item.Quantity;
                var oi = new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    PriceAtOrder = product.Price
                };
                order.OrderItems.Add(oi);
                total += product.Price * item.Quantity;
                _db.Products.Update(product);
            }
            order.TotalAmount = total;
            await _orders.AddAsync(order);
            await _orders.SaveChangesAsync();
            await tx.CommitAsync();
            return order.Id;
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }
    }

    public async Task CancelOrderAsync(int orderId, int customerId)
    {
        var order = await _orders.GetByIdAsync(orderId) ?? throw new Exception("Order not found");
        if (order.CustomerId != customerId) throw new Exception("Not authorized");
        if (order.Status != "Pending") throw new Exception("Only pending orders can be cancelled");
        // restore stock
        foreach (var oi in order.OrderItems)
        {
            var product = await _db.Products.FindAsync(oi.ProductId);
            if (product != null)
            {
                product.StockQuantity += oi.Quantity;
                _db.Products.Update(product);
            }
        }
        order.Status = "Cancelled";
        await _orders.SaveChangesAsync();
    }
}
