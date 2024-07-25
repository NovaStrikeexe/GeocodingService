using GeocodingService.Configuration;
using GeocodingService.Models;
using GeocodingService.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace GeocodingService.Services.Implementation;

public class GeocodingService(HttpClient httpClient, IMemoryCache cache, IOptions<AppSettings> settings)
    : IGeocodingService
{
    private readonly GeocodingSettings _geocodingSettings = settings.Value.Geocoding;
    private readonly CacheSettings _cacheSettings = settings.Value.Cache;
    private static readonly SemaphoreSlim _semaphore = new(1, 1); // Для ограничения частоты запросов

    public async Task<GeocodeResponse> GeocodeAsync(GeocodeRequest request)
    {
        var cacheKey = $"{request.Country}-{request.City}-{request.Street}";
        if (!cache.TryGetValue(cacheKey, out GeocodeResponse cachedResponse))
        {
            var url = $"{_geocodingSettings.NominatimUrl}?country={request.Country}&city={request.City}&street={request.Street}&format=json&limit=2";

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            requestMessage.Headers.Add("User-Agent", "GeocodingService/1.0");

            await _semaphore.WaitAsync();
            try
            {
                var response = await httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var geocodeResults = JsonConvert.DeserializeObject<List<GeocodeResponse>>(jsonResponse);

                    // Возвращаем первый элемент, если он существует
                    cachedResponse = geocodeResults?.FirstOrDefault();

                    if (cachedResponse != null)
                    {
                        var cacheOptions = new MemoryCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_cacheSettings.ExpirationSeconds)
                        };
                        cache.Set(cacheKey, cachedResponse, cacheOptions);
                    }
                }
                else
                {
                    throw new Exception($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        return cachedResponse;
    }
}