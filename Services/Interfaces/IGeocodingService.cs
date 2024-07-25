using GeocodingService.Models;

namespace GeocodingService.Services.Interfaces
{
    public interface IGeocodingService
    {
        Task<GeocodeResponse> GeocodeAsync(GeocodeRequest request);
    }
}
