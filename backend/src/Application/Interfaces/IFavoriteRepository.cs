using BeWare.Movies.Domain.Entities;

namespace BeWare.Movies.Application.Interfaces;

public interface IFavoriteRepository
{
    Task<IReadOnlyList<Favorite>> GetByDeviceAsync(string deviceId, CancellationToken ct = default);
    Task<bool> ExistsAsync(string deviceId, int tmdbMovieId, CancellationToken ct = default);
    Task AddAsync(Favorite favorite, CancellationToken ct = default);
    Task RemoveAsync(string deviceId, int tmdbMovieId, CancellationToken ct = default);
}