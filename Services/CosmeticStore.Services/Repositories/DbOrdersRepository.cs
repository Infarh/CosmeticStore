using CosmeticStore.DAL.Context;
using CosmeticStore.Domain.DTO;
using CosmeticStore.Domain.Entities;
using CosmeticStore.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CosmeticStore.Services.Repositories;

public class DbOrdersRepository : DbRepository<Order>, IOrdersRepository
{
    public DbOrdersRepository(CosmeticDB db, ILogger<DbOrdersRepository> Logger) : base(db, Logger) { }

    public async Task<Order> CreateOrderAsync(
        string CustomerName,
        string PhoneNumber,
        string? Description,
        IEnumerable<OrderItemInfo> Items,
        CancellationToken Cancel = default)
    {
        var customer = await _db.Set<Customer>().FirstOrDefaultAsync(c => c.Name == CustomerName, Cancel).ConfigureAwait(false);
        if (customer is null)
        {
            customer = new()
            {
                Name = CustomerName
            };
            await _db.AddAsync(customer, Cancel);
        }

        var product_ids = Items.Select(item => item.ProductId).ToArray();

        var products = await _db.Set<Product>()
           .Where(p => product_ids.Contains(p.Id))
           .ToArrayAsync(Cancel);

        var items = Items.Join(
            products,
            item => item.ProductId,
            product => product.Id,
            (item, product) => new OrderItem
            {
                Product = product,
                Quantity = item.Quantity
            })
           .ToArray();

        var order = new Order
        {
            Customer = customer,
            Phone = PhoneNumber,
            Description = Description,
            Items = items
        };

        await using var transaction = await _db.Database.BeginTransactionAsync(Cancel);

        await _db.AddAsync(order, Cancel);
        await _db.SaveChangesAsync(Cancel);

        await transaction.CommitAsync(Cancel);

        _Logger.LogInformation("Создан заказ {0}", order);

        return order;
    }
}
