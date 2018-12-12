using System;
using Microsoft.Extensions.Caching.Memory;
using Stations.Core.Interfaces;

namespace Stations.Infrastructure.Cache
{
    public class InMemCache : ICache
    {
        private readonly IMemoryCache _cache;

        public InMemCache(IMemoryCache cache)
        {
            _cache = cache;
            DefaultTtl = TimeSpan.FromSeconds(20);
        }

        public void Add<T>(string key, T item)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(DefaultTtl);

            _cache.Set(key, item, cacheEntryOptions);
        }
        
        public T Get<T>(string key)
        {
            var item = _cache.Get<T>(key);
            return item;
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }
        
        public TimeSpan DefaultTtl { get; }
    }
}
