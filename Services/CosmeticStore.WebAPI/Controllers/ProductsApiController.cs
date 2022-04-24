using CosmeticStore.Domain.Entities;
using CosmeticStore.Interfaces.Base;
using CosmeticStore.Interfaces.Repositories;
using CosmeticStore.WebAPI.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticStore.WebAPI.Controllers;

[ApiController]
[Route(WebAPIAddress.Products)]
public class ProductsApiController : EntityApiController<Product>
{
    public ProductsApiController(IProductsRepository Repository) : base(Repository) { }

    [HttpGet("category/{CategoryId}")]
    public async Task<IActionResult> GetCategoryProducts(int CategoryId)
    {
        var repository = (IProductsRepository)Repository;
        var products = await repository.GetCategoryProducts(CategoryId);

        if (!products.Any())
            return NoContent();
        return Ok(products);
    }
}