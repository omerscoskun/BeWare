using BeWare.Movies.Application.Interfaces;
using BeWare.Movies.Domain.Entities;

namespace BeWare.Movies.Application.Services;

public interface IFavoriteService
{
    Task<IReadOnlyList<Favorite>> GetAsync(string deviceId, CancellationToken ct = default);
    Task AddAsync(string deviceId, Favorite favorite, CancellationToken ct = default);
    Task RemoveAsync(string deviceId, int tmdbMovieId, CancellationToken ct = default);
}

public class FavoriteService(IFavoriteRepository repo) : IFavoriteService
{
    public Task<IReadOnlyList<Favorite>> GetAsync(string deviceId, CancellationToken ct = default)
        => repo.GetByDeviceAsync(deviceId, ct);

    public async Task AddAsync(string deviceId, Favorite favorite, CancellationToken ct = default)
    {
        favorite.DeviceId = deviceId;
        favorite.CreatedAt = DateTime.UtcNow;

        if (await repo.ExistsAsync(deviceId, favorite.TmdbMovieId, ct))
            return;

        await repo.AddAsync(favorite, ct);
    }

    public Task RemoveAsync(string deviceId, int tmdbMovieId, CancellationToken ct = default)
        => repo.RemoveAsync(deviceId, tmdbMovieId, ct);
}
