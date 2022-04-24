using System.Net;
using System.Net.Http.Json;
using CosmeticStore.Interfaces.Base.Entities;
using CosmeticStore.Interfaces.Base.Repositories;

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
        var response = await Http.GetAsync(Address, Cancel).ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.NoContent) return Enumerable.Empty<T>();

        var items = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<IEnumerable<T>>(cancellationToken: Cancel)
            ?? throw new InvalidOperationException($"Не удалось получить список {typeof(T).Name}");
        
        return items;
    }

    public async Task<IEnumerable<T>> GetAsync(int Skip, int Take, CancellationToken Cancel = default)
    {
        var response = await Http.GetAsync($"{Address}/({Skip}:{Take})", Cancel).ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.NoContent) return Enumerable.Empty<T>();

        var items = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<IEnumerable<T>>(cancellationToken: Cancel)
            ?? throw new InvalidOperationException($"Не удалось получить список {typeof(T).Name}");

        return items;
    }

    public async Task<T?> GetByIdAsync(int Id, CancellationToken Cancel = default)
    {
        var item = await Http.GetFromJsonAsync<T>($"{Address}/{Id}", Cancel).ConfigureAwait(false);
        return item;
    }

    public async Task<int> AddAsync(T Item, CancellationToken Cancel = default)
    {
        var response = await Http.PostAsJsonAsync(Address, Item, Cancel).ConfigureAwait(false);

        var created_item = await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadFromJsonAsync<T>(cancellationToken: Cancel);

        Item.Id = created_item!.Id;

        return Item.Id;
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

        var result = await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadFromJsonAsync<T>(cancellationToken: Cancel);

        return result;
    }
}
