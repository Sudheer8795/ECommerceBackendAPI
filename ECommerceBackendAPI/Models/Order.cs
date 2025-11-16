using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Api.Models;

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public User? Customer { get; set; }
    public List<OrderItem> OrderItems { get; set; } = new();
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Pending"; // Pending, Completed, Cancelled
}

public class OrderItem
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int Quantity { get; set; }
    public decimal PriceAtOrder { get; set; }
    public int OrderId { get; set; }
    public Order? Order { get; set; }
}
