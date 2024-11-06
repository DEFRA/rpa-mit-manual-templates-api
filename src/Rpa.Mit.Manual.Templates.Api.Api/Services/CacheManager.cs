using System.Diagnostics.CodeAnalysis;
using System.Runtime.Caching;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

using MemoryCache = System.Runtime.Caching.MemoryCache;

namespace Rpa.Mit.Manual.Templates.Api.Api.Services
{
    [ExcludeFromCodeCoverage]
    public class CacheManager : ICacheManager
    {
        private static ObjectCache Cache
        {
            get
            {
                return MemoryCache.Default;
            }
        }

        public T Get<T>(string key)
        {
            return (T)Cache[key];
        }

        public void Set(string key, object data, int cacheTime)
        {
            if (data == null)
            {
                return;
            }

            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromHours(cacheTime);

            Cache.Add(new CacheItem(key, data), policy);
        }

        public bool IsSet(string key)
        {
            return (Cache.Contains(key));
        }

        public void Remove(string key)
        {
            Cache.Remove(key);
        }

        public void Clear()
        {
            foreach (var item in Cache)
            {
                Remove(item.Key);
            }
        }
    }
}
