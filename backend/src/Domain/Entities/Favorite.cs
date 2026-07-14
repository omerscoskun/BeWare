namespace BeWare.Movies.Domain.Entities;
public class Favorite
{
    public int Id { get; set; }
    public required string DeviceId { get; set; }
    public required int TmdbMovieId { get; set; }
    public required string Title { get; set; }
    public string? PosterPath { get; set; }
    public double VoteAverage { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}