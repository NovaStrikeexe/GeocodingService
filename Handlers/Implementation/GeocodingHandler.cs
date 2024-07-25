using GeocodingService.Handlers.Interfaces;
using GeocodingService.Models;
using GeocodingService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GeocodingService.Handlers.Implementation;

public class GeocodingHandler(IGeocodingService geocodingService, ILogger<GeocodingHandler> logger)
    : IHandler<GeocodeRequest>
{
    public async Task<IActionResult> HandleAsync(GeocodeRequest request)
    {
        logger.LogInformation("Handling geocode request for country: {Country}, city: {City}, street: {Street}",
            request.Country, request.City, request.Street);

        try
        {
            var result = await geocodingService.GeocodeAsync(request);
            logger.LogInformation("Geocode request succeeded");
            return new OkObjectResult(result);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Geocode request failed");
            return new ObjectResult(new { error = exception.Message })
            {
                StatusCode = 500
            };
        }
    }
}