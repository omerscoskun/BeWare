using BeWare.Movies.Application.Models;

namespace BeWare.Movies.Application.Interfaces;

public interface ITmdbClient
{
    Task<IReadOnlyList<MovieSummary>> GetPopularAsync(int page, CancellationToken ct = default);
    Task<IReadOnlyList<MovieSummary>> GetTrendingAsync(CancellationToken ct = default);
    Task<IReadOnlyList<MovieSummary>> SearchAsync(string query, int page, CancellationToken ct = default);
    Task<MovieDetail?> GetDetailAsync(int tmdbId, CancellationToken ct = default);
}