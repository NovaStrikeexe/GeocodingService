using GeocodingService.Handlers.Interfaces;
using GeocodingService.Models;
using GeocodingService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GeocodingService.Handlers.Implementation;

public class GeocodingHandler(IGeocodingService geocodingService) : IHandler<GeocodeRequest>
{
    public async Task<IActionResult> HandleAsync(GeocodeRequest request)
    {
        try
        {
            var result = await geocodingService.GeocodeAsync(request);
            return new OkObjectResult(result);
        }
        catch (Exception exception)
        {
            return new ObjectResult(new { error = exception.Message })
            {
                StatusCode = 500
            };
        }
    }
}