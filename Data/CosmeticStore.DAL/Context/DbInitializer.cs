using CosmeticStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CosmeticStore.DAL.Context;

public class DbInitializer
{
    private readonly CosmeticDB _db;
    private readonly ILogger<DbInitializer> _Logger;

    public DbInitializer(CosmeticDB db, ILogger<DbInitializer> Logger)
    {
        _db = db;
        _Logger = Logger;
    }

    public async Task<bool> DeleteAsync(CancellationToken Cancel = default)
    {
        if (await _db.Database.EnsureDeletedAsync(Cancel).ConfigureAwait(false))
        {
            _Logger.LogInformation("База данных удалена");
            return true;
        }

        _Logger.LogInformation("Не удалось удалить БД. Отсутствует.");
        return false;
    }

    public async Task InitializeAsync(bool RemoveBefore = false, CancellationToken Cancel = default)
    {
        if (RemoveBefore)
            await DeleteAsync(Cancel).ConfigureAwait(false);

        var pending_migrations = (await _db.Database.GetPendingMigrationsAsync(Cancel).ConfigureAwait(false)).ToArray();
        if (pending_migrations.Length > 0)
        {
            var applied_migrations = await _db.Database.GetAppliedMigrationsAsync(Cancel);
            _Logger.LogInformation("Применение миграций \r\n{0}", string.Join("\r\n", pending_migrations.Select(m => $"\t{m}")));

            await _db.Database.MigrateAsync(Cancel);
            _Logger.LogInformation("БД обновлена.");
        }

        await AddTestDataAsync(Cancel);
    }

    private async Task AddTestDataAsync(CancellationToken Cancel = default)
    {
        if (await _db.Products.AnyAsync(Cancel).ConfigureAwait(false))
        {
            _Logger.LogInformation("В БД есть данныне. Инициализация не требуется.");
            return;
        }

        _Logger.LogInformation("Данные в БД отсутствуют. Инициализирую тестовый набор данных.");

        var categories = Enumerable.Range(1, 10)
           .Select(i => new Category
            {
                Name = $"Категория-{i}",
            })
           .ToArray();

        var rnd = new Random(10);

        var products = Enumerable.Range(1, 100)
           .Select(i => new Product
            {
               Name = $"Товар-{i}",
               Description = $"Описание товара {i}",
               Price = rnd.Next(500, 2000),
               Category = categories[rnd.Next(categories.Length)],
            });

        await using var transaction = await _db.Database.BeginTransactionAsync(Cancel);

        await _db.AddRangeAsync(categories, Cancel);
        await _db.AddRangeAsync(products, Cancel);

        await _db.SaveChangesAsync(Cancel);
        await transaction.CommitAsync(Cancel);
    }
}
