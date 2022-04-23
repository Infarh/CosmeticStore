using CosmeticStore.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CosmeticStore.DAL.Sqlite.Extensions;

public static class DbRegistrator
{
    public static IServiceCollection AddCosmeticDB(this IServiceCollection services, string ConnectionString) => services
       .AddDbContext<CosmeticDB>(
            opt => opt.UseSqlite(
                ConnectionString,
                o => o.MigrationsAssembly(typeof(DbRegistrator).Assembly.FullName)))
       .AddTransient<DbInitializer>();
}
