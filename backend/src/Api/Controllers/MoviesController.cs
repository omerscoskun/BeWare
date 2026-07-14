using BeWare.Movies.Application.Models;
using BeWare.Movies.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BeWare.Movies.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController(IMovieService movies) : ControllerBase
{
    [HttpGet("popular")]
    public Task<IReadOnlyList<MovieSummary>> GetPopular([FromQuery] int page = 1, CancellationToken ct = default)
        => movies.GetPopularAsync(page, ct);

    [HttpGet("trending")]
    public Task<IReadOnlyList<MovieSummary>> GetTrending(CancellationToken ct = default)
        => movies.GetTrendingAsync(ct);

    [HttpGet("search")]
    public Task<IReadOnlyList<MovieSummary>> Search(
        [FromQuery] string query, [FromQuery] int page = 1, CancellationToken ct = default)
        => movies.SearchAsync(query, page, ct);

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MovieDetail>> GetDetail(int id, CancellationToken ct = default)
    {
        var detail = await movies.GetDetailAsync(id, ct);
        return detail is null ? NotFound() : Ok(detail);
    }
}
