using System.Diagnostics.CodeAnalysis;
using GeocodingService.Models;
using Microsoft.AspNetCore.Mvc;
using GeocodingService.Handlers.Interfaces;

namespace GeocodingService.Controllers
{
    [ApiController]
    [Route("api/v1/reverse_geocoding")]
    public class ReverseGeocodingController(IHandler<ReverseGeocodeRequest> reverseGeocodingHandler) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> ReverseGeocode([FromQuery] [NotNull] ReverseGeocodeRequest request) 
            => await reverseGeocodingHandler.HandleAsync(request);
    }
}
