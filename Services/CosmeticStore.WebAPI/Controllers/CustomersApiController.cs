using CosmeticStore.Domain.Entities;
using CosmeticStore.Interfaces;
using CosmeticStore.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticStore.WebAPI.Controllers;

[ApiController]
[Route(WebAPIAddress.Customers)]
public class CustomersApiController : ControllerBase
{
    private readonly IRepository<Customer> _Customers;

    public CustomersApiController(IRepository<Customer> Customers) => _Customers = Customers;

    [HttpGet("count")]
    public async Task<IActionResult> Count()
    {
        var count = await _Customers.Count();
        return Ok(count);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _Customers.GetAllAsync();
        if (!items.Any())
            return NoContent();
        return Ok(items);
    }

    [HttpGet("({Skip}:{Take})")]
    public async Task<IActionResult> Get(int Skip, int Take)
    {
        var items = await _Customers.GetAsync(Skip, Take);
        if (!items.Any())
            return NoContent();
        return Ok(items);
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById(int Id)
    {
        var item = await _Customers.GetByIdAsync(Id);
        if (item is null)
            return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] Customer item)
    {
        var id = await _Customers.AddAsync(item);
        return CreatedAtAction(nameof(GetById), new { Id = id }, item);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Customer item)
    {
        var result = await _Customers.UpdateAsync(item);
        if (result)
            return Ok(true);
        return NotFound(false);
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete(int Id)
    {
        var item = await _Customers.RemoveAsync(Id);
        if (item is null)
            return NotFound();
        return Ok(item);
    }
}
