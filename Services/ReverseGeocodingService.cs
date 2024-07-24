using Dadata;
using GeocodingService.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace GeocodingService.Services;

public class ReverseGeocodingService
{
    private readonly IMemoryCache _cache;
    private readonly CacheSettings _cacheSettings;
    private readonly SuggestClientAsync _suggestClient;

    public ReverseGeocodingService(IMemoryCache cache, IOptions<AppSettings> settings)
    {
        _cache = cache;
        var geocodingSettings = settings.Value.Geocoding;
        _cacheSettings = settings.Value.Cache;
        var token = geocodingSettings.DadataApiToken;
        _suggestClient = new SuggestClientAsync(token);
    }

    public async Task<List<ReverseGeocodeResponse>> ReverseGeocodeAsync(ReverseGeocodeRequest request)
    {
        var cacheKey = $"{request.Latitude}-{request.Longitude}";
        if (!_cache.TryGetValue(cacheKey, out List<ReverseGeocodeResponse> cachedResponse))
        {
            var result = await _suggestClient.Geolocate(request.Latitude, request.Longitude, 1000, 10);

            cachedResponse = result.suggestions.Select(s => new ReverseGeocodeResponse
            {
                Value = s.value,
                UnrestrictedValue = s.unrestricted_value,
                Latitude = request.Latitude,
                Longitude = request.Longitude
            }).ToList();

            if (cachedResponse != null)
            {
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_cacheSettings.ExpirationSeconds)
                };
                _cache.Set(cacheKey, cachedResponse, cacheOptions);
            }
        }

        return cachedResponse;
    }
}