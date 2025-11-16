using Microsoft.AspNetCore.Mvc;
using ECommerceApi.Data;
using ECommerceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ProductsController(AppDbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1, int pageSize = 20, string? sortBy = null)
        {
            var q = _db.Products.AsQueryable();

            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy == "price_desc") q = q.OrderByDescending(p => p.Price);
                else if (sortBy == "price_asc") q = q.OrderBy(p => p.Price);
            }

            var total = await q.CountAsync();
            var items = await q
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return Ok(new { total, items });
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string name, int page = 1, int pageSize = 20)
        {
            var q = _db.Products.Where(p => p.Name.Contains(name));
            var total = await q.CountAsync();
            var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return Ok(new { total, items });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var p = await _db.Products.FindAsync(id);
            if (p == null) return NotFound();
            return Ok(p);
        }
    }
}
