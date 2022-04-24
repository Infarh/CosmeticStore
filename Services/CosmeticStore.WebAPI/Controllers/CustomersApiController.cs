using CosmeticStore.Domain.Entities;
using CosmeticStore.Interfaces.Base;
using CosmeticStore.Interfaces.Repositories;
using CosmeticStore.WebAPI.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticStore.WebAPI.Controllers;

[ApiController]
[Route(WebAPIAddress.Customers)]
public class CustomersApiController : EntityApiController<Customer>
{
    public CustomersApiController(ICustomersRepository Repository) : base(Repository) { }

    [HttpGet("name/{Name}")]
    public async Task<IActionResult> FindByName(string Name)
    {
        var repository = (ICustomersRepository)Repository;
        var customer = await repository.FindByName(Name);
        if (customer is null)
            return NotFound();
        return Ok(customer);
    }
}
