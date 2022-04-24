using CosmeticStore.Interfaces.Base.Entities;
using CosmeticStore.Interfaces.Base.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CosmeticStore.WebAPI.Controllers.Base;

[ApiController]
public abstract class EntityApiController<T> : ControllerBase where T : class, IEntity
{
    protected IRepository<T> Repository { get; }

    protected EntityApiController(IRepository<T> Repository) => this.Repository = Repository;

    [HttpGet("count")]
    public virtual async Task<IActionResult> Count()
    {
        var count = await Repository.Count();
        return Ok(count);
    }

    [HttpGet]
    public virtual async Task<IActionResult> GetAll()
    {
        var items = await Repository.GetAllAsync();
        if (!items.Any())
            return NoContent();
        return Ok(items);
    }

    [HttpGet("({Skip}:{Take})")]
    public virtual async Task<IActionResult> Get(int Skip, int Take)
    {
        var items = await Repository.GetAsync(Skip, Take);
        if (!items.Any())
            return NoContent();
        return Ok(items);
    }

    [HttpGet("{Id}")]
    public virtual async Task<IActionResult> GetById(int Id)
    {
        var item = await Repository.GetByIdAsync(Id);
        if (item is null)
            return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public virtual async Task<IActionResult> Add([FromBody] T item)
    {
        var id = await Repository.AddAsync(item);
        return CreatedAtAction(nameof(GetById), new { Id = id }, item);
    }

    [HttpPut]
    public virtual async Task<IActionResult> Update([FromBody] T item)
    {
        var result = await Repository.UpdateAsync(item);
        if (result)
            return Ok(true);
        return NotFound(false);
    }

    [HttpDelete("{Id}")]
    public virtual async Task<IActionResult> Delete(int Id)
    {
        var item = await Repository.RemoveAsync(Id);
        if (item is null)
            return NotFound();
        return Ok(item);
    }
}
