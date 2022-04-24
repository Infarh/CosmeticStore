using CosmeticStore.Domain.DTO;
using CosmeticStore.Domain.Entities;
using CosmeticStore.Interfaces.Base.Repositories;

namespace CosmeticStore.Interfaces.Repositories;

public interface IOrdersRepository : IRepository<Order>
{
    Task<Order> CreateOrderAsync(
        string CustomerName,
        string PhoneNumber,
        string? Description,
        IEnumerable<OrderItemInfo> Items,
        CancellationToken Cancel = default);

    Task<IEnumerable<Order>> GetCustomerOrdersAsync(string Name, CancellationToken Cancel = default);
}