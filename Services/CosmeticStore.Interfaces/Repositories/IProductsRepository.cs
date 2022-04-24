using CosmeticStore.Domain.Entities;
using CosmeticStore.Interfaces.Base.Repositories;

namespace CosmeticStore.Interfaces.Repositories;

public interface IProductsRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetCategoryProductsAsync(int CategoryId, CancellationToken Cancel = default);

    //Task<Product> CreateProductAsync(
    //    string Name,
    //    decimal Price,
    //    string Description,
    //    string CategoryName,
    //    CancellationToken Cancel = default);
}