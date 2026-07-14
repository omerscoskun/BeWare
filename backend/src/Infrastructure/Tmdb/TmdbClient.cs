using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using BeWare.Movies.Application.Interfaces;
using BeWare.Movies.Application.Models;
using BeWare.Movies.Infrastructure.Caching;
using BeWare.Movies.Infrastructure.Configuration;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace BeWare.Movies.Infrastructure.Tmdb;

public class TmdbClient(HttpClient http, IDistributedCache cache, IOptions<TmdbOptions> options) : ITmdbClient
{
    private readonly TmdbOptions _o = options.Value;
    private TimeSpan Ttl => TimeSpan.FromMinutes(_o.CacheMinutes);

    public Task<IReadOnlyList<MovieSummary>> GetPopularAsync(int page, CancellationToken ct = default)
        => cache.GetOrCreateAsync($"tmdb:popular:{page}", Ttl, async c =>
            TmdbMapper.ToSummaries(await http.GetFromJsonAsync<TmdbListResponse>(
                $"movie/popular?language={_o.Language}&page={page}", c)), ct);

    public Task<IReadOnlyList<MovieSummary>> GetTrendingAsync(CancellationToken ct = default)
        => cache.GetOrCreateAsync("tmdb:trending", Ttl, async c =>
            TmdbMapper.ToSummaries(await http.GetFromJsonAsync<TmdbListResponse>(
                $"trending/movie/day?language={_o.Language}", c)), ct);

    public Task<IReadOnlyList<MovieSummary>> SearchAsync(string query, int page, CancellationToken ct = default)
        => cache.GetOrCreateAsync($"tmdb:search:{query.ToLowerInvariant()}:{page}", Ttl, async c =>
            TmdbMapper.ToSummaries(await http.GetFromJsonAsync<TmdbListResponse>(
                $"search/movie?query={Uri.EscapeDataString(query)}&language={_o.Language}&page={page}", c)), ct);

        public Task<MovieDetail?> GetDetailAsync(int tmdbId, CancellationToken ct = default)
        => cache.GetOrCreateAsync<MovieDetail?>($"tmdb:detail:{tmdbId}", Ttl, async c =>
        {
            var resp = await http.GetAsync(
                $"movie/{tmdbId}?language={_o.Language}&append_to_response=credits,videos", c);
            if (resp.StatusCode == HttpStatusCode.NotFound) return null;
            resp.EnsureSuccessStatusCode();

            var d = await resp.Content.ReadFromJsonAsync<TmdbMovieDetail>(c);
            return d is null ? null : TmdbMapper.ToDetail(d);
        }, ct);
}
file static class TmdbMapper
{
    public static IReadOnlyList<MovieSummary> ToSummaries(TmdbListResponse? res)
        => res?.Results.Select(m => new MovieSummary(
               m.Id, m.Title ?? "", m.PosterPath, m.VoteAverage, m.ReleaseDate)).ToList() ?? [];

    public static MovieDetail ToDetail(TmdbMovieDetail d) => new(
        d.Id, d.Title ?? "", d.Overview, d.PosterPath, d.BackdropPath,
        d.VoteAverage, d.ReleaseDate, d.Runtime,
        d.Genres?.Select(g => g.Name).ToList() ?? [],
        d.Credits?.Cast?.Take(10).Select(c => c.Name).ToList() ?? [],
        d.Videos?.Results?.FirstOrDefault(v => v.Site == "YouTube" && v.Type == "Trailer")?.Key);
}

file sealed record TmdbListResponse(
    [property: JsonPropertyName("results")] List<TmdbMovie> Results);

file sealed record TmdbMovie(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("title")] string? Title,
    [property: JsonPropertyName("poster_path")] string? PosterPath,
    [property: JsonPropertyName("vote_average")] double VoteAverage,
    [property: JsonPropertyName("release_date")] string? ReleaseDate);

file sealed record TmdbMovieDetail(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("title")] string? Title,
    [property: JsonPropertyName("overview")] string? Overview,
    [property: JsonPropertyName("poster_path")] string? PosterPath,
    [property: JsonPropertyName("backdrop_path")] string? BackdropPath,
    [property: JsonPropertyName("vote_average")] double VoteAverage,
    [property: JsonPropertyName("release_date")] string? ReleaseDate,
    [property: JsonPropertyName("runtime")] int? Runtime,
    [property: JsonPropertyName("genres")] List<TmdbGenre>? Genres,
    [property: JsonPropertyName("credits")] TmdbCredits? Credits,
    [property: JsonPropertyName("videos")] TmdbVideos? Videos);

file sealed record TmdbGenre([property: JsonPropertyName("name")] string Name);
file sealed record TmdbCredits([property: JsonPropertyName("cast")] List<TmdbCast>? Cast);
file sealed record TmdbCast([property: JsonPropertyName("name")] string Name);
file sealed record TmdbVideos([property: JsonPropertyName("results")] List<TmdbVideo>? Results);
file sealed record TmdbVideo(
    [property: JsonPropertyName("key")] string Key,
    [property: JsonPropertyName("site")] string Site,
    [property: JsonPropertyName("type")] string Type);
