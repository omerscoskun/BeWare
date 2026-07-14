using BeWare.Movies.Api.Handlers;
using BeWare.Movies.Application.Services;
using BeWare.Movies.Infrastructure;
using BeWare.Movies.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

const string CorsPolicy = "frontend";

// --- Katmanlar ---
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();

// --- Web ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(o => o.AddPolicy(CorsPolicy, p => p
    .WithOrigins("http://localhost:4200")
    .AllowAnyHeader()
    .AllowAnyMethod()));

// Global hata yönetimi (ProblemDetails)
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Dev kolaylığı: DB ayaktaysa migration'ları otomatik uygula.
    using var scope = app.Services.CreateScope();
    await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.MigrateAsync();
}

app.UseHttpsRedirection();
app.UseCors(CorsPolicy);
app.MapControllers();

app.Run();
