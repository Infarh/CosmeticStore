using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text.Json;

using CosmeticStore.DAL.Context;
using CosmeticStore.DAL.Sqlite.Extensions;
using CosmeticStore.Interfaces.Base.Repositories;
using CosmeticStore.Interfaces.Repositories;
using CosmeticStore.Services.Repositories;

using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var services = builder.Services;

var db_type = configuration["Database"];
switch (db_type)
{
    default: throw new InvalidOperationException($"Тип базы данных {db_type} не поддерживается");

    case "Sqlite":
        services
           .AddCosmeticDB(configuration.GetConnectionString(db_type))
           .AddScoped(typeof(IRepository<>), typeof(DbRepository<>))
           .AddScoped<IProductsRepository, DbProductsRepository>()
           .AddScoped<IOrdersRepository, DbOrdersRepository>()
           .AddScoped<ICustomersRepository, DbCustomersRepository>()
            ;
        break;
}

services.AddSingleton(builder.Environment.WebRootFileProvider);

services.AddControllers(options =>
{

});
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
    if(scope.ServiceProvider.GetService<DbInitializer>() is { } initializer)
        await initializer.InitializeAsync(
            RemoveBefore: configuration.GetValue<bool>("DatabaseReinitialize"),
            AddTestData: configuration.GetValue<bool>("DatabaseAddTestData")
            );

//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
