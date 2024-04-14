using ImTexterApi.Services.ServiceAttribute;
using Microsoft.Extensions.Caching.Memory;

namespace ImTexterApi.Services
{
    [ScopedRegistration]
    public class CachingService(IMemoryCache memoryCache) : ICachingService
    {
        private readonly IMemoryCache _memoryCache = memoryCache;

        public T? GetCacheData<T>(string key)
        {
            if (_memoryCache.TryGetValue(key, value: out T cachedData))
            {
                return cachedData;
            }
            return default;
        }

        public void SetCacheData<T>(string key, T data, TimeSpan timeSpan)
        {
            _memoryCache.Set(key, data, timeSpan);
        }

        public object ClearApiCacheData(string key)
        {
            ClearCacheData(key);

            return new
            {
                status = $"Cahce cleared for {key}"
            };
        }
        public void ClearCacheData(string key) => _memoryCache.Remove(key);

    }
}
