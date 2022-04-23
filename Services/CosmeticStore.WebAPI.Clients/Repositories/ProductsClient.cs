using CosmeticStore.Domain.Entities;
using CosmeticStore.Interfaces;
using CosmeticStore.WebAPI.Clients.Repositories.Base;

namespace CosmeticStore.WebAPI.Clients.Repositories;

public class ProductsClient : RepositoryClient<Product>
{
    public ProductsClient(HttpClient Client) : base(Client, WebAPIAddress.Products) { }
}