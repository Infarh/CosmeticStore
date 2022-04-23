using System.Net.Http.Json;
using CosmeticStore.Domain.Entities;
using CosmeticStore.Interfaces;
using CosmeticStore.Interfaces.Base;
using CosmeticStore.Interfaces.Repositories;
using CosmeticStore.WebAPI.Clients.Repositories.Base;

namespace CosmeticStore.WebAPI.Clients.Repositories;

public class ProductsClient : RepositoryClient<Product>, IProductsRepository
{
    public ProductsClient(HttpClient Client) : base(Client, WebAPIAddress.Products) { }

    public async Task<IEnumerable<Product>> GetCategoryProducts(int CategoryId, CancellationToken Cancel = default)
    {
        var products = await Http
           .GetFromJsonAsync<IEnumerable<Product>>($"{Address}/category/{CategoryId}", Cancel)
           .ConfigureAwait(false);
        return products!;
    }
}