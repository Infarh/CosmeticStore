using System.Net;
using System.Net.Http.Json;
using CosmeticStore.Domain.Entities;
using CosmeticStore.Interfaces.Base;
using CosmeticStore.Interfaces.Repositories;
using CosmeticStore.WebAPI.Clients.Repositories.Base;

namespace CosmeticStore.WebAPI.Clients.Repositories;

public class ProductsClient : RepositoryClient<Product>, IProductsRepository
{
    public ProductsClient(HttpClient Client) : base(Client, WebAPIAddress.Products) { }

    public async Task<IEnumerable<Product>> GetCategoryProductsAsync(int CategoryId, CancellationToken Cancel = default)
    {
        var response = await Http.GetAsync($"{Address}/category/{CategoryId}", Cancel).ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.NoContent) return Enumerable.Empty<Product>();
        if (response.StatusCode == HttpStatusCode.NotFound) return Enumerable.Empty<Product>();

        var products = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<IEnumerable<Product>>(cancellationToken: Cancel)
            ?? throw new InvalidOperationException("Не удалось загрузить список товаров");


        return products!;
    }
}