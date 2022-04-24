using CosmeticStore.DAL.Context;
using CosmeticStore.Interfaces.Base.Entities;
using CosmeticStore.Interfaces.Base.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CosmeticStore.Services.Repositories;

public class DbRepository<T> : IRepository<T> where T : class, IEntity
{
    protected readonly CosmeticDB _db;
    protected readonly ILogger<DbRepository<T>> _Logger;

    protected DbSet<T> Set { get; set; }
    
    public DbRepository(CosmeticDB db, ILogger<DbRepository<T>> Logger)
    {
        _db = db;
        _Logger = Logger;
        Set = _db.Set<T>();
    }

    public virtual async Task<int> Count(CancellationToken Cancel = default)
    {
        var count = await Set.CountAsync(Cancel).ConfigureAwait(false);
        return count;
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken Cancel = default)
    {
        var items = await Set.ToArrayAsync(Cancel).ConfigureAwait(false);
        return items;
    }

    public virtual async Task<IEnumerable<T>> GetAsync(int Skip, int Take, CancellationToken Cancel = default)
    {
        var items = await Set
           .OrderBy(item => item.Id)
           .Skip(Skip)
           .Take(Take)
           .ToArrayAsync(Cancel)
           .ConfigureAwait(false);
        return items;
    }

    public virtual async Task<T?> GetByIdAsync(int Id, CancellationToken Cancel = default)
    {
        var item = await Set.FirstOrDefaultAsync(i => i.Id == Id, Cancel).ConfigureAwait(false);
        return item;
    }

    public virtual async Task<int> AddAsync(T Item, CancellationToken Cancel = default)
    {
        if (Item is null) throw new ArgumentNullException(nameof(Item));

        await Set.AddAsync(Item, Cancel).ConfigureAwait(false);
        await _db.SaveChangesAsync(Cancel);

        _Logger.LogInformation("Добавлено {0}", Item);

        return Item.Id;
    }

    public virtual async Task<bool> UpdateAsync(T Item, CancellationToken Cancel = default)
    {
        Set.Update(Item);
        _Logger.LogInformation("Обновлено {0}", Item);
        return await _db.SaveChangesAsync(Cancel).ConfigureAwait(false) > 0;
    }

    public virtual async Task<T?> RemoveAsync(int Id, CancellationToken Cancel = default)
    {
        var item = await GetByIdAsync(Id, Cancel).ConfigureAwait(false);
        if (item is null)
        {
            _Logger.LogInformation("Удаление id:{0} невозможно. Запись отсутствует.", Id);
            return null;
        }

        Set.Remove(item);
        await _db.SaveChangesAsync(Cancel);

        _Logger.LogInformation("Запись Id:{0} удалена", Id);
        return item;
    }
}
