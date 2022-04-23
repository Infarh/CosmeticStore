using CosmeticStore.Domain.Entities;
using CosmeticStore.Interfaces;
using CosmeticStore.WebAPI.Clients.Repositories.Base;

namespace CosmeticStore.WebAPI.Clients.Repositories;

public class CustomersClient : RepositoryClient<Customer>
{
    public CustomersClient(HttpClient Client) : base(Client, WebAPIAddress.Customers) { }
}
