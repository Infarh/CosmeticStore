using CosmeticStore.Domain.Entities;
using CosmeticStore.Interfaces;
using CosmeticStore.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticStore.WebAPI.Controllers;

[ApiController]
[Route(WebAPIAddress.Categories)]
public class CategoriesApiController : ControllerBase
{
    private readonly IRepository<Category> _Categories;

    public CategoriesApiController(IRepository<Category> Categories) => _Categories = Categories;

    [HttpGet("count")]
    public async Task<IActionResult> Count()
    {
        var count = await _Categories.Count();
        return Ok(count);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _Categories.GetAllAsync();
        if (!items.Any())
            return NoContent();
        return Ok(items);
    }

    [HttpGet("({Skip}:{Take})")]
    public async Task<IActionResult> Get(int Skip, int Take)
    {
        var items = await _Categories.GetAsync(Skip, Take);
        if (!items.Any())
            return NoContent();
        return Ok(items);
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById(int Id)
    {
        var item = await _Categories.GetByIdAsync(Id);
        if (item is null)
            return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] Category item)
    {
        var id = await _Categories.AddAsync(item);
        return CreatedAtAction(nameof(GetById), new { Id = id }, item);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Category item)
    {
        var result = await _Categories.UpdateAsync(item);
        if (result)
            return Ok(true);
        return NotFound(false);
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete(int Id)
    {
        var item = await _Categories.RemoveAsync(Id);
        if (item is null)
            return NotFound();
        return Ok(item);
    }
}