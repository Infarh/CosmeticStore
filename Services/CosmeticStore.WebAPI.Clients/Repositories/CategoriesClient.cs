using CosmeticStore.Domain.Entities;
using CosmeticStore.Interfaces;
using CosmeticStore.WebAPI.Clients.Repositories.Base;

namespace CosmeticStore.WebAPI.Clients.Repositories;

public class CategoriesClient : RepositoryClient<Category>
{
    public CategoriesClient(HttpClient Client) : base(Client, WebAPIAddress.Categories) { }
}