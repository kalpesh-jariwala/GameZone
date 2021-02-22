using System;
using System.Collections;
using System.Web;
using System.Web.Caching;

namespace BLL_Core.Infrastructure.Caching
{
    public class WebApplicationCache : ICache
    {
        private readonly Cache _cache;

        public WebApplicationCache()
        {
            _cache = HttpContext.Current.Cache;
        }

        public WebApplicationCache(HttpContextBase contextbase)
        {
            _cache = contextbase.Cache;
        }

        public T Get<T>(string key) where T : class
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            var item = _cache[key] as T;

            return item;
        }

        public T Get<T>(string key, Func<T> getUncachedItem, CacheDependency cacheDependency, DateTime cacheDuration,
                        TimeSpan slidingExpiration) where T : class
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            var item = _cache[key] as T;

            if (item == null)
            {
                item = getUncachedItem();
                _cache.Insert(key, item, cacheDependency, cacheDuration, TimeSpan.Zero);
            }

            return item;
        }

        public void Insert(string key, object obj)
        {
            Insert(key, obj, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration);
        }

        public void Insert(string key, object obj, DateTime cacheDuration, TimeSpan slidingExpiration)
        {
            Insert(key, obj, null, cacheDuration, slidingExpiration);
        }

        public void Insert(string key, object obj, DateTime expiresAbsolute)
        {
            Insert(key, obj, expiresAbsolute, Cache.NoSlidingExpiration);
        }

        public void Insert(string key, object obj, TimeSpan expiresRelative)
        {
            Insert(key, obj, Cache.NoAbsoluteExpiration, expiresRelative);
        }

        public void Insert(string key, object obj, CacheDependency cacheDependency, DateTime cacheDuration,
                           TimeSpan slidingExpiration)
        {
            //HttpContext.Current.Cache.Add(key, value, null, expiresAbsolute, expiresRelative, CacheItemPriority.Normal, null);
            _cache.Insert(key, obj, cacheDependency, cacheDuration, slidingExpiration);
        }

        public void Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            _cache.Remove(key);
        }

        public object this[string key]
        {
            get { return _cache[key]; }
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            return _cache.GetEnumerator();
        }
    }
}