using Microsoft.AspNetCore.Mvc;
using ECommerceApi.Data;
using ECommerceApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Controllers.Admin
{
    [ApiController]
    [Authorize(Roles = Roles.Admin)]
    [Route("api/admin/orders")]
    public class OrdersAdminController : ControllerBase
    {
        private readonly AppDbContext _db;
        public OrdersAdminController(AppDbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1, int pageSize = 20)
        {
            var q = _db.Orders.Include(o => o.Items).Include(o => o.Customer).AsQueryable();
            var total = await q.CountAsync();
            var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return Ok(new { total, items });
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] OrderStatus status)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order == null) return NotFound();
            order.Status = status;
            await _db.SaveChangesAsync();
            return Ok(order);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var order = await _db.Orders.Include(o => o.Items).Include(o => o.Customer).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound();
            return Ok(order);
        }
    }
}
