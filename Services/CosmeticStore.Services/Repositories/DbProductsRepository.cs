using CosmeticStore.DAL.Context;
using CosmeticStore.Domain.Entities;
using CosmeticStore.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CosmeticStore.Services.Repositories;

public class DbProductsRepository : DbRepository<Product>, IProductsRepository
{
    public DbProductsRepository(CosmeticDB db, ILogger<DbProductsRepository> Logger) : base(db, Logger) { }

    public async Task<IEnumerable<Product>> GetCategoryProductsAsync(int CategoryId, CancellationToken Cancel = default)
    {
        var products = await Set
           .Where(p => p.CategoryId == CategoryId)
           .ToArrayAsync(Cancel)
           .ConfigureAwait(false);
        return products;
    }

    public override async Task<int> AddAsync(Product Item, CancellationToken Cancel = default)
    {
        if (Item.Category is { } category)
        {
            _db.Attach(category);
        }

        var result = await base.AddAsync(Item, Cancel).ConfigureAwait(false);

        return result;
    }
}