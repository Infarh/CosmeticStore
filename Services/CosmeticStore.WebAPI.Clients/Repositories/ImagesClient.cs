using System.Drawing;
using System.Net;

namespace CosmeticStore.WebAPI.Clients.Repositories;

public class ImagesClient
{
    protected HttpClient Http { get; }

    public ImagesClient(HttpClient Http) => this.Http = Http;

    public async Task<Stream?> GetImageAsync(string ImageName, CancellationToken Cancel = default)
    {
        var response = await Http.GetAsync($"api/images/{ImageName}", Cancel).ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        if (response.StatusCode == HttpStatusCode.NoContent)
            return null;

        var stream = await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadAsStreamAsync(Cancel);

        var memory_stream = new MemoryStream();
        await stream.CopyToAsync(memory_stream, Cancel);
        memory_stream.Position = 0;
        return memory_stream;
    }
}
