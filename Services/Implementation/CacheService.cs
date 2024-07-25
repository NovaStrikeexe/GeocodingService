using GeocodingService.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace GeocodingService.Services.Implementation
{
    public class CacheService(IMemoryCache cache, ILogger<CacheService> logger) : ICacheService
    {
        public bool TryGetValue<T>(object key, out T value)
        {
            var result = cache.TryGetValue(key, out value);
            logger.LogInformation("Cache {Action} for key: {Key}", result ? "hit" : "miss", key);
            return result;
        }

        public void Set<T>(object key, T value, MemoryCacheEntryOptions options)
        {
            cache.Set(key, value, options);
            logger.LogInformation("Cache set for key: {Key}", key);
        }
    }
}