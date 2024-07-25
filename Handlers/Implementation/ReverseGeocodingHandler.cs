using GeocodingService.Handlers.Interfaces;
using GeocodingService.Models;
using GeocodingService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GeocodingService.Handlers.Implementation;

public class ReverseGeocodingHandler(IReverseGeocodingService reverseGeocodingService)
    : IHandler<ReverseGeocodeRequest>
{
    public async Task<IActionResult> HandleAsync(ReverseGeocodeRequest request)
    {
        try
        {
            var result = await reverseGeocodingService.ReverseGeocodeAsync(request);
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