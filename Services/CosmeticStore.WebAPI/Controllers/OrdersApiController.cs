using CosmeticStore.Domain.Entities;
using CosmeticStore.Interfaces;
using CosmeticStore.Interfaces.Base;
using CosmeticStore.Interfaces.Base.Repositories;
using CosmeticStore.WebAPI.Controllers.Base;

using Microsoft.AspNetCore.Mvc;

namespace CosmeticStore.WebAPI.Controllers;

[ApiController]
[Route(WebAPIAddress.Orders)]
public class OrdersApiController : EntityApiController<Order>
{
    public OrdersApiController(IRepository<Order> Products) : base(Products) { }
}