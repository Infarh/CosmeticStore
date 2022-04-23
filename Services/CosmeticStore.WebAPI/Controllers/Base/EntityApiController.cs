using CosmeticStore.Interfaces.Entities;
using CosmeticStore.Interfaces.Repositories;

using Microsoft.AspNetCore.Mvc;

namespace CosmeticStore.WebAPI.Controllers.Base;

[ApiController]
public abstract class EntityApiController<T> : ControllerBase where T : class, IEntity
{
    private readonly IRepository<T> _Items;

    protected EntityApiController(IRepository<T> Products) => _Items = Products;

    [HttpGet("count")]
    public async Task<IActionResult> Count()
    {
        var count = await _Items.Count();
        return Ok(count);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _Items.GetAllAsync();
        if (!items.Any())
            return NoContent();
        return Ok(items);
    }

    [HttpGet("({Skip}:{Take})")]
    public async Task<IActionResult> Get(int Skip, int Take)
    {
        var items = await _Items.GetAsync(Skip, Take);
        if (!items.Any())
            return NoContent();
        return Ok(items);
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById(int Id)
    {
        var item = await _Items.GetByIdAsync(Id);
        if (item is null)
            return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] T item)
    {
        var id = await _Items.AddAsync(item);
        return CreatedAtAction(nameof(GetById), new { Id = id }, item);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] T item)
    {
        var result = await _Items.UpdateAsync(item);
        if (result)
            return Ok(true);
        return NotFound(false);
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete(int Id)
    {
        var item = await _Items.RemoveAsync(Id);
        if (item is null)
            return NotFound();
        return Ok(item);
    }
}
