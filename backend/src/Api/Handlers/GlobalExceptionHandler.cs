using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BeWare.Movies.Api.Handlers;

/// <summary>İşlenmeyen tüm hataları tek noktada ProblemDetails'e çevirir.</summary>
public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context, Exception exception, CancellationToken ct)
    {
        logger.LogError(exception, "İşlenmeyen hata oluştu");

        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Sunucu hatası",
            Detail = "Beklenmeyen bir hata oluştu."
        };

        context.Response.StatusCode = problem.Status.Value;
        await context.Response.WriteAsJsonAsync(problem, ct);
        return true;
    }
}
