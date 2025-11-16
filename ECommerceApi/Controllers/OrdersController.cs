using Microsoft.AspNetCore.Mvc;
using ECommerceApi.Data;
using ECommerceApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ECommerceApi.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _db;
        public OrdersController(AppDbContext db) { _db = db; }

        // Place an order as a customer
        [Authorize(Roles = Roles.Customer)]
        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] Order order)
        {
            if (order.Items == null || !order.Items.Any())
                return BadRequest("Order must contain at least one item");

            // Validate stock
            foreach (var item in order.Items)
            {
                var prod = await _db.Products.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Id == item.ProductId);
                if (prod == null || !prod.IsActive || prod.DeletedDate.HasValue) return BadRequest($"Product {item.ProductId} not found or inactive");
                if (prod.StockQuantity < item.Quantity) return BadRequest($"Insufficient stock for {prod.Name}");
            }

            // Calculate totals and reduce stock
            decimal total = 0;
            foreach (var item in order.Items)
            {
                var prod = await _db.Products.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Id == item.ProductId);
                item.PriceAtOrder = prod!.Price;
                total += prod.Price * item.Quantity;
                prod.StockQuantity -= item.Quantity;
            }

            // link customer id from token if available
            var userIdClaim = User.FindFirst("id")?.Value;
            if (int.TryParse(userIdClaim, out var userId))
                order.CustomerId = userId;

            order.TotalAmount = total;
            order.OrderDate = DateTime.UtcNow;
            order.Status = OrderStatus.Pending;

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            return Ok(order);
        }

        [Authorize(Roles = Roles.Customer)]
        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var userId = User.FindFirst("id")?.Value;
            if (!int.TryParse(userId, out var uid)) return Unauthorized();

            var order = await _db.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id && o.CustomerId == uid);
            if (order == null) return NotFound();
            if (order.Status != OrderStatus.Pending) return BadRequest("Only pending orders can be cancelled");

            order.Status = OrderStatus.Cancelled;
            // restore stock
            foreach (var item in order.Items)
            {
                var prod = await _db.Products.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Id == item.ProductId);
                if (prod != null) prod.StockQuantity += item.Quantity;
            }
            await _db.SaveChangesAsync();
            return Ok(order);
        }

        // Get own orders
        [Authorize(Roles = Roles.Customer)]
        [HttpGet("my")]
        public async Task<IActionResult> MyOrders()
        {
            var userId = User.FindFirst("id")?.Value;
            if (!int.TryParse(userId, out var uid)) return Unauthorized();

            var orders = await _db.Orders.Include(o => o.Items).Where(o => o.CustomerId == uid).ToListAsync();
            return Ok(orders);
        }
    }
}
