using BeWare.Movies.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeWare.Movies.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Favorite> Favorites => Set<Favorite>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
}
