using ECommerce.Api.DTOs;
using ECommerce.Api.Models;
using ECommerce.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminProductsController : ControllerBase
{
    private readonly IProductRepository _products;
    private readonly ICategoryRepository _categories;

    public AdminProductsController(IProductRepository products, ICategoryRepository categories)
    {
        _products = products;
        _categories = categories;
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateDto dto)
    {
        if (await _products.ExistsWithNameInCategoryAsync(dto.Name, dto.CategoryId))
            return BadRequest(new { error = "Product with same name exists in this category" });
        if (dto.StockQuantity < 0) return BadRequest(new { error = "Stock can't be negative" });

        var cat = await _categories.GetByIdAsync(dto.CategoryId);
        if (cat == null) return BadRequest(new { error = "Category not found" });

        var p = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            CategoryId = dto.CategoryId,
            StockQuantity = dto.StockQuantity
        };
        await _products.AddAsync(p);
        await _products.SaveChangesAsync();
        return CreatedAtAction(nameof(Create), new { id = p.Id }, p);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ProductUpdateDto dto)
    {
        var p = await _products.GetByIdAsync(id);
        if (p == null) return NotFound();
        if (await _products.ExistsWithNameInCategoryAsync(dto.Name, dto.CategoryId, id))
            return BadRequest(new { error = "Duplicate product name in category" });
        p.Name = dto.Name;
        p.Description = dto.Description;
        p.Price = dto.Price;
        p.CategoryId = dto.CategoryId;
        p.StockQuantity = dto.StockQuantity;
        p.IsActive = dto.IsActive;
        p.UpdatedDate = DateTime.UtcNow;
        await _products.UpdateAsync(p);
        await _products.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var p = await _products.GetByIdAsync(id);
        if (p == null) return NotFound();
        p.IsActive = false;
        p.DeletedDate = DateTime.UtcNow;
        await _products.UpdateAsync(p);
        await _products.SaveChangesAsync();
        return NoContent();
    }
}
