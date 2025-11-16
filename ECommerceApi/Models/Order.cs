using System;
using System.Collections.Generic;

namespace ECommerceApi.Models
{
    public enum OrderStatus { Pending, Completed, Cancelled }

    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public User? Customer { get; set; }
        public List<OrderItem> Items { get; set; } = new();
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
    }

    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtOrder { get; set; }
    }
}
