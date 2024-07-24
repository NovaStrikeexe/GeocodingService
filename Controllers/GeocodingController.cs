using GeocodingService.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1")]
public class GeocodingController(GeocodingService.Services.GeocodingService geocodingService)
    : ControllerBase
{
    [HttpGet("GetСoordinates")]
    public async Task<IActionResult> Geocode([FromQuery] GeocodeRequest request)
    {
        var result = await geocodingService.GeocodeAsync(request);
        return Ok(result);
    }
}