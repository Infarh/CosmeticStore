using CosmeticStore.Domain.Entities;
using CosmeticStore.Interfaces;
using CosmeticStore.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticStore.WebAPI.Controllers;

[ApiController]
[Route(WebAPIAddress.Orders)]
public class OrdersApiController : ControllerBase
{
    private readonly IRepository<Order> _Orders;

    public OrdersApiController(IRepository<Order> Orders) => _Orders = Orders;

    [HttpGet("count")]
    public async Task<IActionResult> Count()
    {
        var count = await _Orders.Count();
        return Ok(count);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _Orders.GetAllAsync();
        if (!items.Any())
            return NoContent();
        return Ok(items);
    }

    [HttpGet("({Skip}:{Take})")]
    public async Task<IActionResult> Get(int Skip, int Take)
    {
        var items = await _Orders.GetAsync(Skip, Take);
        if (!items.Any())
            return NoContent();
        return Ok(items);
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById(int Id)
    {
        var item = await _Orders.GetByIdAsync(Id);
        if (item is null)
            return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] Order item)
    {
        var id = await _Orders.AddAsync(item);
        return CreatedAtAction(nameof(GetById), new { Id = id }, item);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Order item)
    {
        var result = await _Orders.UpdateAsync(item);
        if (result)
            return Ok(true);
        return NotFound(false);
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete(int Id)
    {
        var item = await _Orders.RemoveAsync(Id);
        if (item is null)
            return NotFound();
        return Ok(item);
    }
}