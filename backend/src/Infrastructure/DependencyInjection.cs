using System.Net.Http.Headers;
using BeWare.Movies.Application.Interfaces;
using BeWare.Movies.Infrastructure.Configuration;
using BeWare.Movies.Infrastructure.Persistence;
using BeWare.Movies.Infrastructure.Tmdb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BeWare.Movies.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration config)
    {
        services.Configure<TmdbOptions>(config.GetSection(TmdbOptions.SectionName));

        // PostgreSQL
        services.AddDbContext<AppDbContext>(opt =>
            opt.UseNpgsql(config.GetConnectionString("Postgres")));

        // Redis
        services.AddStackExchangeRedisCache(opt =>
        {
            opt.Configuration = config.GetConnectionString("Redis");
            opt.InstanceName = "beware:";
        });

        // TMDB typed HttpClient (BaseAddress + Bearer token merkezi)
        services.AddHttpClient<ITmdbClient, TmdbClient>((sp, http) =>
        {
            var o = sp.GetRequiredService<IOptions<TmdbOptions>>().Value;
            http.BaseAddress = new Uri(o.BaseUrl);
            http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", o.ApiToken);
        });

        services.AddScoped<IFavoriteRepository, FavoriteRepository>();

        return services;
    }
}
