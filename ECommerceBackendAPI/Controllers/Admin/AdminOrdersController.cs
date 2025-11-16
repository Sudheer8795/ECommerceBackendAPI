using ECommerce.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminOrdersController : ControllerBase
{
    private readonly IOrderRepository _orders;
    public AdminOrdersController(IOrderRepository orders)
    {
        _orders = orders;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _orders.GetAllAsync();
        return Ok(list);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromQuery]string status)
    {
        var order = await _orders.GetByIdAsync(id);
        if (order == null) return NotFound();
        order.Status = status;
        await _orders.SaveChangesAsync();
        return NoContent();
    }
}
