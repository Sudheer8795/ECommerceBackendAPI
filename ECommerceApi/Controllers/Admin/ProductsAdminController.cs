using Microsoft.AspNetCore.Mvc;
using ECommerceApi.Data;
using ECommerceApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Controllers.Admin
{
    [ApiController]
    [Authorize(Roles = Roles.Admin)]
    [Route("api/admin/products")]
    public class ProductsAdminController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ProductsAdminController(AppDbContext db) { _db = db; }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Product model)
        {
            if (model.StockQuantity < 0) return BadRequest("Stock cannot be negative");
            if (await _db.Products.IgnoreQueryFilters().AnyAsync(p => p.Name == model.Name && p.CategoryId == model.CategoryId))
                return BadRequest("Duplicate product name in same category");
            model.CreatedDate = DateTime.UtcNow;
            _db.Products.Add(model);
            await _db.SaveChangesAsync();
            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Product model)
        {
            var p = await _db.Products.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id);
            if (p == null) return NotFound();
            if (model.StockQuantity < 0) return BadRequest("Stock cannot be negative");

            // check duplicate if name changed
            if (p.Name != model.Name && await _db.Products.IgnoreQueryFilters().AnyAsync(x => x.Id != id && x.Name == model.Name && x.CategoryId == model.CategoryId))
                return BadRequest("Duplicate product name in same category");

            p.Name = model.Name;
            p.Description = model.Description;
            p.Price = model.Price;
            p.CategoryId = model.CategoryId;
            p.StockQuantity = model.StockQuantity;
            p.IsActive = model.IsActive;
            p.UpdatedDate = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return Ok(p);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var p = await _db.Products.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id);
            if (p == null) return NotFound();
            p.IsActive = false;
            p.DeletedDate = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAdmin()
        {
            var list = await _db.Products.IgnoreQueryFilters().ToListAsync();
            return Ok(list);
        }
    }
}
