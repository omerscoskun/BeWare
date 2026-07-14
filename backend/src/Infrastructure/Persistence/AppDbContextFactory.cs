using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BeWare.Movies.Infrastructure.Persistence;
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var connection = Environment.GetEnvironmentVariable("BEWARE_POSTGRES")
            ?? "Host=localhost;Port=5432;Database=beware;Username=beware;Password=beware";

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(connection)
            .Options;

        return new AppDbContext(options);
    }
}
