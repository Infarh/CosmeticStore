using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using CosmeticStore.Interfaces.Entities;
using CosmeticStore.Interfaces.Repositories;

namespace CosmeticStore.WebAPI.Clients.Repositories.Base;

public abstract class RepositoryClient<T> : IRepository<T> where T : class, IEntity
{
    protected HttpClient Http { get; }

    protected string Address { get; }

    protected RepositoryClient(HttpClient Client, string Address)
    {
        Http = Client;
        this.Address = Address;
    }

    public async Task<int> Count(CancellationToken Cancel = default)
    {
        var count = await Http
           .GetFromJsonAsync<int>($"{Address}/count", Cancel)
           .ConfigureAwait(false);
        return count;
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken Cancel = default)
    {
        var items = await Http
           .GetFromJsonAsync<IEnumerable<T>>(Address, Cancel)
           .ConfigureAwait(false);
        return items!;
    }

    public async Task<IEnumerable<T>> GetAsync(int Skip, int Take, CancellationToken Cancel = default)
    {
        var items = await Http
           .GetFromJsonAsync<IEnumerable<T>>($"{Address}/({Skip}:{Take})", Cancel)
           .ConfigureAwait(false);
        return items!;
    }

    public async Task<T?> GetByIdAsync(int Id, CancellationToken Cancel = default)
    {
        var item = await Http.GetFromJsonAsync<T>($"{Address}/{Id}", Cancel).ConfigureAwait(false);
        return item;
    }

    public async Task<int> AddAsync(T Item, CancellationToken Cancel = default)
    {
        var response = await Http.PostAsJsonAsync(Address, Item, Cancel).ConfigureAwait(false);
        var id = await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadFromJsonAsync<int>(cancellationToken: Cancel);
        return id;
    }

    public async Task<bool> UpdateAsync(T Item, CancellationToken Cancel = default)
    {
        var response = await Http.PutAsJsonAsync(Address, Item, Cancel).ConfigureAwait(false);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return false;
        return response.EnsureSuccessStatusCode().StatusCode == HttpStatusCode.OK;
    }

    public async Task<T?> RemoveAsync(int Id, CancellationToken Cancel = default)
    {
        var response = await Http.DeleteAsync($"{Address}/{Id}", Cancel).ConfigureAwait(false);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;
        return await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadFromJsonAsync<T>(cancellationToken: Cancel);
    }
}
