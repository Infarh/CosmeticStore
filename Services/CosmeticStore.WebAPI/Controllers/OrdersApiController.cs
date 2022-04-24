using CosmeticStore.Domain.DTO;
using CosmeticStore.Domain.Entities;
using CosmeticStore.Interfaces.Base;
using CosmeticStore.Interfaces.Repositories;
using CosmeticStore.WebAPI.Controllers.Base;

using Microsoft.AspNetCore.Mvc;

namespace CosmeticStore.WebAPI.Controllers;

[ApiController]
[Route(WebAPIAddress.Orders)]
public class OrdersApiController : EntityApiController<Order>
{
    public OrdersApiController(IOrdersRepository Repository) : base(Repository) { }

    [HttpPost("new")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOderDTO Model)
    {
        var repository = (IOrdersRepository)Repository;
        
        var order = await repository.CreateOrderAsync(
            Model.CustomerName,
            Model.PhoneNumber,
            Model.Description,
            Model.Items);

        foreach (var order_item in order.Items)
            order_item.Order = null!;

        return CreatedAtAction(nameof(GetById), new { order.Id }, order);
    }

    public override async Task<IActionResult> GetAll()
    {
        var result = await base.GetAll();

        if (result is OkObjectResult { Value: IEnumerable<Order> orders })
            foreach (var item in orders.SelectMany(order => order.Items))
                item.Order = null!;

        return result;
    }

    public override async Task<IActionResult> Get(int Skip, int Take)
    {
        var result = await base.Get(Skip, Take);

        if (result is OkObjectResult { Value: IEnumerable<Order> orders })
            foreach (var item in orders.SelectMany(order => order.Items))
                item.Order = null!;

        return result;
    }

    public override async Task<IActionResult> GetById(int Id)
    {
        var result = await base.GetById(Id);

        if (result is CreatedAtActionResult { Value: Order order })
            foreach (var order_item in order.Items)
                order_item.Order = null!;

        return result;
    }

    public override async Task<IActionResult> Delete(int Id)
    {
        var result = await base.Delete(Id);

        if (result is CreatedAtActionResult { Value: Order order })
            foreach (var order_item in order.Items)
                order_item.Order = null!;

        return result;
    }
}