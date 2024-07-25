using GeocodingService.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace GeocodingService.Services.Implementation;

public class CacheService(IMemoryCache cache) : ICacheService
{
    public bool TryGetValue<T>(object key, out T value) 
        => cache.TryGetValue(key, out value);

    public void Set<T>(object key, T value, MemoryCacheEntryOptions options) 
        => cache.Set(key, value, options);
}