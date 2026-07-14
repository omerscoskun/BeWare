using BeWare.Movies.Application.Models;
using BeWare.Movies.Application.Interfaces;

namespace BeWare.Movies.Application.Services;

public interface IMovieService
{
    Task<IReadOnlyList<MovieSummary>> GetPopularAsync(int page, CancellationToken ct = default);
    Task<IReadOnlyList<MovieSummary>> GetTrendingAsync(CancellationToken ct = default);
    Task<IReadOnlyList<MovieSummary>> SearchAsync(string query, int page, CancellationToken ct = default);
    Task<MovieDetail?> GetDetailAsync(int tmdbId, CancellationToken ct = default);
}

public class MovieService(ITmdbClient tmdb) : IMovieService
{
    public Task<IReadOnlyList<MovieSummary>> GetPopularAsync(int page, CancellationToken ct = default)
        => tmdb.GetPopularAsync(page <= 0 ? 1 : page, ct);

    public Task<IReadOnlyList<MovieSummary>> GetTrendingAsync(CancellationToken ct = default)
        => tmdb.GetTrendingAsync(ct);

    public Task<IReadOnlyList<MovieSummary>> SearchAsync(string query, int page, CancellationToken ct = default)
        => string.IsNullOrWhiteSpace(query)
            ? Task.FromResult<IReadOnlyList<MovieSummary>>([])
            : tmdb.SearchAsync(query.Trim(), page <= 0 ? 1 : page, ct);

    public Task<MovieDetail?> GetDetailAsync(int tmdbId, CancellationToken ct = default)
        => tmdb.GetDetailAsync(tmdbId, ct);
}