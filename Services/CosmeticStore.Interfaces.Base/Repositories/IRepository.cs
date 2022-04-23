using CosmeticStore.Interfaces.Base.Entities;

namespace CosmeticStore.Interfaces.Base.Repositories;

public interface IRepository<T> where T : class, IEntity
{
    Task<int> Count(CancellationToken Cancel = default);

    Task<IEnumerable<T>> GetAllAsync(CancellationToken Cancel = default);

    Task<IEnumerable<T>> GetAsync(int Skip, int Take, CancellationToken Cancel = default);

    Task<T?> GetByIdAsync(int Id, CancellationToken Cancel = default);

    Task<int> AddAsync(T Item, CancellationToken Cancel = default);

    Task<bool> UpdateAsync(T Item, CancellationToken Cancel = default);

    Task<T?> RemoveAsync(int Id, CancellationToken Cancel = default);
}
