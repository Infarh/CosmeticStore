using CosmeticStore.Domain.Entities;
using CosmeticStore.Interfaces;
using CosmeticStore.Interfaces.Base;
using CosmeticStore.Interfaces.Base.Repositories;
using CosmeticStore.WebAPI.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticStore.WebAPI.Controllers;

[ApiController]
[Route(WebAPIAddress.Products)]
public class ProductsApiController : EntityApiController<Product>
{
    public ProductsApiController(IRepository<Product> Products) : base(Products) { }
}