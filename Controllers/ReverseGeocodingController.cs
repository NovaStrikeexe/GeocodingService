using GeocodingService.Models;
using GeocodingService.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1")]
public class ReverseGeocodingController(ReverseGeocodingService reverseGeocodingService) : ControllerBase
{
    [HttpGet("ReverseGeocoding")]
    public async Task<IActionResult> ReverseGeocode([FromQuery] ReverseGeocodeRequest request)
    {
        var result = await reverseGeocodingService.ReverseGeocodeAsync(request);
        return Ok(result);
    }
}