using BeWare.Movies.Application.Interfaces;
using BeWare.Movies.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeWare.Movies.Infrastructure.Persistence;

public class FavoriteRepository(AppDbContext db) : IFavoriteRepository
{
    public async Task<IReadOnlyList<Favorite>> GetByDeviceAsync(string deviceId, CancellationToken ct = default)
        => await db.Favorites
            .Where(f => f.DeviceId == deviceId)
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync(ct);

    public Task<bool> ExistsAsync(string deviceId, int tmdbMovieId, CancellationToken ct = default)
        => db.Favorites.AnyAsync(f => f.DeviceId == deviceId && f.TmdbMovieId == tmdbMovieId, ct);

    public async Task AddAsync(Favorite favorite, CancellationToken ct = default)
    {
        db.Favorites.Add(favorite);
        await db.SaveChangesAsync(ct);
    }

    public async Task RemoveAsync(string deviceId, int tmdbMovieId, CancellationToken ct = default)
    {
        var entity = await db.Favorites
            .FirstOrDefaultAsync(f => f.DeviceId == deviceId && f.TmdbMovieId == tmdbMovieId, ct);
        if (entity is null) return;

        db.Favorites.Remove(entity);
        await db.SaveChangesAsync(ct);
    }
}
