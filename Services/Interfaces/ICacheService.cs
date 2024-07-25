using Microsoft.Extensions.Caching.Memory;

namespace GeocodingService.Services.Interfaces;

public interface ICacheService
{
    bool TryGetValue<T>(object key, out T value);
    void Set<T>(object key, T value, MemoryCacheEntryOptions options);
}