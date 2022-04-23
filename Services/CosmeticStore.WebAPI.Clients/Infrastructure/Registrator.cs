using CosmeticStore.Domain.Entities;
using CosmeticStore.Interfaces.Base.Repositories;
using CosmeticStore.WebAPI.Clients.Repositories;
using Microsoft.Extensions.DependencyInjection;

using Polly.Extensions.Http;
using Polly;
using CosmeticStore.Interfaces.Repositories;

namespace CosmeticStore.WebAPI.Clients.Infrastructure;

public static class Registrator
{
    public static IServiceCollection AddWebAPI(this IServiceCollection services, string Address)
    {
        services.AddHttpClient("ConsmeticApi", client => client.BaseAddress = new(Address))
           .AddTypedClient<IProductsRepository, ProductsClient>()
           .AddTypedClient<IRepository<Product>, ProductsClient>()
           .AddTypedClient<IRepository<Order>, OrdersClient>()
           .AddTypedClient<IRepository<Customer>, CustomersClient>()
           .AddTypedClient<IRepository<Category>, CategoriesClient>()
           .AddTypedClient<ImagesClient>()
           .AddPolicyHandler(GetRetryPolicy())
           .AddPolicyHandler(GetCircuitBreakerPolicy())
           .SetHandlerLifetime(TimeSpan.FromMinutes(15));

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int MaxRetryCount = 5, int MaxJitterTime = 1000)
    {
        var jitter = new Random();
        return HttpPolicyExtensions
           .HandleTransientHttpError()
           .WaitAndRetryAsync(
                MaxRetryCount, RetryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, RetryAttempt)) +
                    TimeSpan.FromMilliseconds(jitter.Next(0, MaxJitterTime)));
    }

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() =>
        HttpPolicyExtensions
           .HandleTransientHttpError()
           .CircuitBreakerAsync(handledEventsAllowedBeforeBreaking: 5, TimeSpan.FromSeconds(30));
}
