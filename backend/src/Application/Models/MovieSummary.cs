namespace BeWare.Movies.Application.Models;
public record MovieSummary(
    int TmdbId,
    string Title,
    string? PosterPath,
    double VoteAverage,
    string? ReleaseDate
);