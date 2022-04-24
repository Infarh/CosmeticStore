using System.Net;
using System.Net.Http.Json;
using CosmeticStore.Domain.Entities;
using CosmeticStore.Interfaces.Base;
using CosmeticStore.Interfaces.Repositories;
using CosmeticStore.WebAPI.Clients.Repositories.Base;

namespace CosmeticStore.WebAPI.Clients.Repositories;

public class CustomersClient : RepositoryClient<Customer>, ICustomersRepository
{
    public CustomersClient(HttpClient Client) : base(Client, WebAPIAddress.Customers) { }

    public async Task<Customer?> FindByName(string Name, CancellationToken Cancel = default)
    {
        var response = await Http.GetAsync($"{Address}/name/{Name}", Cancel).ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.NotFound) return null;
        if (response.StatusCode == HttpStatusCode.NoContent) return null;

        var customer = await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadFromJsonAsync<Customer>(cancellationToken: Cancel)
           .ConfigureAwait(false);

        return customer;
    }
}
