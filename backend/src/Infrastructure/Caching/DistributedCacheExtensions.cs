using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace BeWare.Movies.Infrastructure.Caching;

internal static class DistributedCacheExtensions
{
    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);
    public static async Task<T> GetOrCreateAsync<T>(
        this IDistributedCache cache, string key, TimeSpan ttl,
        Func<CancellationToken, Task<T>> factory, CancellationToken ct)
    {
        var cached = await cache.GetStringAsync(key, ct);
        if (cached is not null)
            return JsonSerializer.Deserialize<T>(cached, Json)!;

        var value = await factory(ct);
        await cache.SetStringAsync(
            key, JsonSerializer.Serialize(value, Json),
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = ttl }, ct);
        return value;
    }
}
