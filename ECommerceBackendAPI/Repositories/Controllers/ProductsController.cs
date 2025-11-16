using ECommerce.Api.DTOs;
using ECommerce.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _products;
    public ProductsController(IProductRepository products) => _products = products;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery]string? name, [FromQuery]int page = 1, [FromQuery]int pageSize = 10)
    {
        var list = await _products.SearchAsync(name, page, pageSize);
        var dto = list.Select(p => new ProductListDto(p.Id, p.Name, p.Description, p.Price, p.StockQuantity, p.IsActive));
        return Ok(dto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var p = await _products.GetByIdAsync(id);
        if (p == null) return NotFound();
        return Ok(new ProductListDto(p.Id, p.Name, p.Description, p.Price, p.StockQuantity, p.IsActive));
    }
}
