using CosmeticStore.DAL.Context;
using CosmeticStore.DAL.Sqlite.Extensions;
using CosmeticStore.Interfaces.Base.Repositories;
using CosmeticStore.Services.Repositories;

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
           .AddScoped(typeof(IRepository<>), typeof(DbRepository<>));
        break;
}

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
    await initializer.InitializeAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
