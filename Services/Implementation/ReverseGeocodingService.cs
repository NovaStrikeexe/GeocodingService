using Dadata;
using GeocodingService.Configuration;
using GeocodingService.Models;
using GeocodingService.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace GeocodingService.Services.Implementation
{
    public class ReverseGeocodingService : IReverseGeocodingService
    {
        private readonly ICacheService _cache;
        private readonly CacheSettings _cacheSettings;
        private readonly SuggestClientAsync _suggestClient;
        private readonly ILogger<ReverseGeocodingService> _logger;

        public ReverseGeocodingService(ICacheService cache, IOptions<AppSettings> settings, ILogger<ReverseGeocodingService> logger)
        {
            _cache = cache;
            var geocodingSettings = settings.Value.Geocoding;
            _cacheSettings = settings.Value.Cache;
            var token = geocodingSettings.DadataApiToken;
            _suggestClient = new SuggestClientAsync(token);
            _logger = logger;
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
}
