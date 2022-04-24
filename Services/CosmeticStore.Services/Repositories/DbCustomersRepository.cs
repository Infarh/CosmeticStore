using CosmeticStore.DAL.Context;
using CosmeticStore.Domain.Entities;
using CosmeticStore.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CosmeticStore.Services.Repositories;

public class DbCustomersRepository : DbRepository<Customer>, ICustomersRepository
{
    public DbCustomersRepository(CosmeticDB db, ILogger<DbCustomersRepository> Logger) : base(db, Logger) { }

    public async Task<Customer?> FindByName(string Name, CancellationToken Cancel = default)
    {
        var customer = await Set
           .FirstOrDefaultAsync(c => c.Name == Name, Cancel)
           .ConfigureAwait(false);
        return customer;
    }
}
