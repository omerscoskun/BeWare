namespace BeWare.Movies.Application.Models;
public record FavoriteDto(
    int TmdbMovieId,
    string Title,
    string? PosterPath,
    double VoteAverage,
    DateTime CreatedAt);
