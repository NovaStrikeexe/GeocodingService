using GeocodingService.Handlers.Interfaces;
using GeocodingService.Models;
using GeocodingService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GeocodingService.Handlers.Implementation;

public class ReverseGeocodingHandler(
    IReverseGeocodingService reverseGeocodingService,
    ILogger<ReverseGeocodingHandler> logger)
    : IHandler<ReverseGeocodeRequest>
{
    public async Task<IActionResult> HandleAsync(ReverseGeocodeRequest request)
    {
        logger.LogInformation("Handling reverse geocode request for latitude: {Latitude}, longitude: {Longitude}",
            request.Latitude, request.Longitude);

        try
        {
            var result = await reverseGeocodingService.ReverseGeocodeAsync(request);
            logger.LogInformation("Reverse geocode request succeeded");
            return new OkObjectResult(result);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Reverse geocode request failed");
            return new ObjectResult(new { error = exception.Message })
            {
                StatusCode = 500
            };
        }
    }
}