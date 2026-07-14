using BeWare.Movies.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeWare.Movies.Infrastructure.Persistence;

public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
{
    public void Configure(EntityTypeBuilder<Favorite> b)
    {
        b.ToTable("favorites");
        b.HasKey(f => f.Id);
        b.Property(f => f.DeviceId).IsRequired().HasMaxLength(128);
        b.Property(f => f.Title).IsRequired().HasMaxLength(512);
        b.Property(f => f.PosterPath).HasMaxLength(256);

        // Aynı cihaz aynı filmi iki kez ekleyemesin.
        b.HasIndex(f => new { f.DeviceId, f.TmdbMovieId }).IsUnique();
    }
}
