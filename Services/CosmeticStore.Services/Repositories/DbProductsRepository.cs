using CosmeticStore.DAL.Context;
using CosmeticStore.Domain.Entities;
using CosmeticStore.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CosmeticStore.Services.Repositories;

public class DbProductsRepository : DbRepository<Product>, IProductsRepository
{
    public DbProductsRepository(CosmeticDB db, ILogger<DbProductsRepository> Logger) : base(db, Logger) { }

    public async Task<IEnumerable<Product>> GetCategoryProducts(int CategoryId, CancellationToken Cancel = default)
    {
        var products = await Set
           .Where(p => p.CategoryId == CategoryId)
           .ToArrayAsync(Cancel)
           .ConfigureAwait(false);
        return products;
    }
}