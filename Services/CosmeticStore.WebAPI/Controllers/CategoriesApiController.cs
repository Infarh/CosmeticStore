using CosmeticStore.Domain.Entities;
using CosmeticStore.Interfaces;
using CosmeticStore.Interfaces.Base;
using CosmeticStore.Interfaces.Base.Repositories;
using CosmeticStore.WebAPI.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticStore.WebAPI.Controllers;

[ApiController]
[Route(WebAPIAddress.Categories)]
public class CategoriesApiController : EntityApiController<Category>
{
    public CategoriesApiController(IRepository<Category> Products) : base(Products) { }
}