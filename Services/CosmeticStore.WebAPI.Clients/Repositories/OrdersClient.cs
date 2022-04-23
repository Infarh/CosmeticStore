using CosmeticStore.Domain.Entities;
using CosmeticStore.Interfaces;
using CosmeticStore.WebAPI.Clients.Repositories.Base;

namespace CosmeticStore.WebAPI.Clients.Repositories;

public class OrdersClient : RepositoryClient<Order>
{
    public OrdersClient(HttpClient Client) : base(Client, WebAPIAddress.Orders) { }
}