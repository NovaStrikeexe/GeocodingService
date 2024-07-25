using GeocodingService.Models;

namespace GeocodingService.Services.Interfaces;

public interface IReverseGeocodingService
{
    Task<List<ReverseGeocodeResponse>> ReverseGeocodeAsync(ReverseGeocodeRequest request);
}