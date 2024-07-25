using System.Diagnostics.CodeAnalysis;
using GeocodingService.Models;
using Microsoft.AspNetCore.Mvc;
using GeocodingService.Handlers.Interfaces;

namespace GeocodingService.Controllers;

[ApiController]
[Route("api/v1/geocode")]
public class GeocodingController(IHandler<GeocodeRequest> geocodingHandler) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Geocode([FromQuery] [NotNull] GeocodeRequest request) 
        => await geocodingHandler.HandleAsync(request);
}