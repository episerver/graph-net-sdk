using EPiServer.ContentGraph.Helpers;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace EPiServer.ContentGraph.Connection
{
    public class RuntimeCacheAdapter : ICache
    {
        private readonly IMemoryCache _memoryCache;

        public RuntimeCacheAdapter(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public TCachedObject Get<TCachedObject>(string key)
        {
            var cachedObject = _memoryCache.Get(key);
            if (cachedObject.IsNull() || !(cachedObject is TCachedObject))
            {
                return default;
            }

            return (TCachedObject)cachedObject;
        }

        public void Add(string key, StaticCachePolicy cachePolicy, object value)
        {
            if (!_memoryCache.TryGetValue(key, out _))
            {
                var options = ToMemoryCacheEntryOptions(cachePolicy);
                _memoryCache.Set(key, value, options);
            }
        }

        public void AddOrUpdate(string key, StaticCachePolicy cachePolicy, object value)
        {
            var options = ToMemoryCacheEntryOptions(cachePolicy);
            _memoryCache.Set(key, value, options);
        }

        private static MemoryCacheEntryOptions ToMemoryCacheEntryOptions(StaticCachePolicy cachePolicy)
        {
            var options = new MemoryCacheEntryOptions();
            if (cachePolicy.Duration != TimeSpan.Zero)
            {
                options.SetSlidingExpiration(cachePolicy.Duration);
            }
            
            if (cachePolicy.ExpirationDate != DateTime.MaxValue)
            {
                options.SetAbsoluteExpiration(cachePolicy.ExpirationDate);
            }
            
            if (cachePolicy.ChangeToken != null)
            {
                options.AddExpirationToken(cachePolicy.ChangeToken);
            }
            options.SetPriority(CacheItemPriority.Normal);
            return options;
        }
    }
}
