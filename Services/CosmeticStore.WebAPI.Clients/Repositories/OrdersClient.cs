using System.Net;
using System.Net.Http.Json;
using CosmeticStore.Domain.DTO;
using CosmeticStore.Domain.Entities;
using CosmeticStore.Interfaces.Base;
using CosmeticStore.Interfaces.Repositories;
using CosmeticStore.WebAPI.Clients.Repositories.Base;

namespace CosmeticStore.WebAPI.Clients.Repositories;

public class OrdersClient : RepositoryClient<Order>, IOrdersRepository
{
    public OrdersClient(HttpClient Client) : base(Client, WebAPIAddress.Orders) { }

    public async Task<Order> CreateOrderAsync(
        string CustomerName,
        string PhoneNumber,
        string? Description,
        IEnumerable<OrderItemInfo> Items,
        CancellationToken Cancel = default)
    {
        var model = new CreateOderDTO(CustomerName, PhoneNumber, Description, Items);
        var response = await Http.PostAsJsonAsync($"{Address}/new", model, Cancel).ConfigureAwait(false);

        var order = await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadFromJsonAsync<Order>(cancellationToken: Cancel)
            ?? throw new InvalidOperationException("Не удалось получить сформированный заказ от сервиса");

        return order;
    }

    public async Task<IEnumerable<Order>> GetCustomerOrdersAsync(string Name, CancellationToken Cancel = default)
    {
        var response = await Http.GetAsync($"{Address}/customer/{Name}", Cancel).ConfigureAwait(false);

        if(response.StatusCode == HttpStatusCode.NoContent)
            return Enumerable.Empty<Order>();

        var orders = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<IEnumerable<Order>>(cancellationToken: Cancel)
            ?? throw new InvalidOperationException("Не удалось получить перечень заказов");

        return orders;
    }
}