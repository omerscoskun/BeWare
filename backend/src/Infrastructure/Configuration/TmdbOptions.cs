namespace BeWare.Movies.Infrastructure.Configuration;

public class TmdbOptions
{
    public const string SectionName = "Tmdb";

    public string BaseUrl { get; set; } = "https://api.themoviedb.org/3/";
    public string ApiToken { get; set; } = string.Empty;
    public string Language { get; set; } = "tr-TR";
    public int CacheMinutes { get; set; } = 30;
}
