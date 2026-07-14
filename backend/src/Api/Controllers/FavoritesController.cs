using BeWare.Movies.Application.Models;
using BeWare.Movies.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BeWare.Movies.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FavoritesController(IFavoriteService favorites) : ControllerBase
{
    private const string DeviceHeader = "X-Device-Id";

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<FavoriteDto>>> Get(CancellationToken ct)
    {
        if (!TryGetDeviceId(out var deviceId))
            return BadRequest($"{DeviceHeader} header gerekli.");

        return Ok(await favorites.GetAsync(deviceId, ct));
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddFavoriteRequest request, CancellationToken ct)
    {
        if (!TryGetDeviceId(out var deviceId))
            return BadRequest($"{DeviceHeader} header gerekli.");

        await favorites.AddAsync(deviceId, request, ct);
        return NoContent();
    }

    [HttpDelete("{tmdbMovieId:int}")]
    public async Task<IActionResult> Remove(int tmdbMovieId, CancellationToken ct)
    {
        if (!TryGetDeviceId(out var deviceId))
            return BadRequest($"{DeviceHeader} header gerekli.");

        await favorites.RemoveAsync(deviceId, tmdbMovieId, ct);
        return NoContent();
    }

    private bool TryGetDeviceId(out string deviceId)
    {
        if (Request.Headers.TryGetValue(DeviceHeader, out var value) &&
            !string.IsNullOrWhiteSpace(value))
        {
            deviceId = value!;
            return true;
        }
        deviceId = string.Empty;
        return false;
    }
}
