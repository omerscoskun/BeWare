namespace BeWare.Movies.Application.Models;
public record MovieDetail(
    int TmdbId,
    string Title,
    string? Overview,
    string? PosterPath,
    string? BackdropPath,
    double VoteAverage,
    string? ReleaseDate,
    int? Runtime,
    IReadOnlyList<string> Genres,
    IReadOnlyList<string> Cast,
    string? TrailerYoutubeKey
);