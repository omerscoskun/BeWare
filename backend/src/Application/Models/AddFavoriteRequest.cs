namespace BeWare.Movies.Application.Models;
public record AddFavoriteRequest(
    int TmdbMovieId,
    string Title,
    string? PosterPath,
    double VoteAverage);
