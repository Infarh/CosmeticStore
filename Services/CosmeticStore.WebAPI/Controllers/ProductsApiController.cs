using CosmeticStore.Domain.Entities;
using CosmeticStore.Interfaces;
using CosmeticStore.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticStore.WebAPI.Controllers;

[ApiController]
[Route(WebAPIAddress.Products)]
public class ProductsApiController : ControllerBase
{
    private readonly IRepository<Product> _Products;

    public ProductsApiController(IRepository<Product> Products) => _Products = Products;

    [HttpGet("count")]
    public async Task<IActionResult> Count()
    {
        var count = await _Products.Count();
        return Ok(count);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _Products.GetAllAsync();
        if (!items.Any())
            return NoContent();
        return Ok(items);
    }

    [HttpGet("({Skip}:{Take})")]
    public async Task<IActionResult> Get(int Skip, int Take)
    {
        var items = await _Products.GetAsync(Skip, Take);
        if (!items.Any())
            return NoContent();
        return Ok(items);
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById(int Id)
    {
        var item = await _Products.GetByIdAsync(Id);
        if (item is null)
            return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] Product item)
    {
        var id = await _Products.AddAsync(item);
        return CreatedAtAction(nameof(GetById), new { Id = id }, item);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Product item)
    {
        var result = await _Products.UpdateAsync(item);
        if (result)
            return Ok(true);
        return NotFound(false);
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete(int Id)
    {
        var item = await _Products.RemoveAsync(Id);
        if (item is null)
            return NotFound();
        return Ok(item);
    }
}