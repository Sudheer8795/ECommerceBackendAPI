using Microsoft.AspNetCore.Mvc;
using ECommerceApi.Data;
using ECommerceApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Controllers.Admin
{
    [ApiController]
    [Authorize(Roles = Roles.Admin)]
    [Route("api/admin/categories")]
    public class CategoriesAdminController : ControllerBase
    {
        private readonly AppDbContext _db;
        public CategoriesAdminController(AppDbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _db.Categories.ToListAsync());

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Category model)
        {
            model.CreatedDate = DateTime.UtcNow;
            _db.Categories.Add(model);
            await _db.SaveChangesAsync();
            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Category model)
        {
            var cat = await _db.Categories.FindAsync(id);
            if (cat == null) return NotFound();
            cat.Name = model.Name;
            cat.Description = model.Description;
            cat.UpdatedDate = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return Ok(cat);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cat = await _db.Categories.FindAsync(id);
            if (cat == null) return NotFound();
            _db.Categories.Remove(cat);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
