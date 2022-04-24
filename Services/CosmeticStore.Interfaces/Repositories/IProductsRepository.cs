using CosmeticStore.Domain.Entities;
using CosmeticStore.Interfaces.Base.Repositories;

namespace CosmeticStore.Interfaces.Repositories;

public interface IProductsRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetCategoryProducts(int CategoryId, CancellationToken Cancel = default);
}