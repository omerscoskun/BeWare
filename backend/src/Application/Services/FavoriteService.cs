using BeWare.Movies.Application.Interfaces;
using BeWare.Movies.Application.Models;
using BeWare.Movies.Domain.Entities;

namespace BeWare.Movies.Application.Services;

public interface IFavoriteService
{
    Task<IReadOnlyList<FavoriteDto>> GetAsync(string deviceId, CancellationToken ct = default);
    Task AddAsync(string deviceId, AddFavoriteRequest request, CancellationToken ct = default);
    Task RemoveAsync(string deviceId, int tmdbMovieId, CancellationToken ct = default);
}

public class FavoriteService(IFavoriteRepository repo) : IFavoriteService
{
    public async Task<IReadOnlyList<FavoriteDto>> GetAsync(string deviceId, CancellationToken ct = default)
    {
        var favorites = await repo.GetByDeviceAsync(deviceId, ct);
        return favorites
            .Select(f => new FavoriteDto(f.TmdbMovieId, f.Title, f.PosterPath, f.VoteAverage, f.CreatedAt))
            .ToList();
    }

    public async Task AddAsync(string deviceId, AddFavoriteRequest request, CancellationToken ct = default)
    {
        if (await repo.ExistsAsync(deviceId, request.TmdbMovieId, ct))
            return;

        var favorite = new Favorite
        {
            DeviceId = deviceId,
            TmdbMovieId = request.TmdbMovieId,
            Title = request.Title,
            PosterPath = request.PosterPath,
            VoteAverage = request.VoteAverage,
            CreatedAt = DateTime.UtcNow,
        };

        await repo.AddAsync(favorite, ct);
    }

    public Task RemoveAsync(string deviceId, int tmdbMovieId, CancellationToken ct = default)
        => repo.RemoveAsync(deviceId, tmdbMovieId, ct);
}
