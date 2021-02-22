using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Caching;

namespace BLL_Core.Infrastructure.Caching
{
    public class DictionaryCache : ICache
    {
        private readonly Dictionary<string, object> _cache = new Dictionary<string, object>();

        public DictionaryCache()
        {
            _cache = new Dictionary<string, object>();
        }

        public DictionaryCache(Dictionary<string, object> dictionarycache)
        {
            _cache = dictionarycache;
        }

        public T Get<T>(string key) where T : class
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");
            if (!_cache.ContainsKey(key))
            {
                return null;
            }
            var item = _cache[key] as T;
            return item;
        }

        public T Get<T>(string key, Func<T> getUncachedItem, CacheDependency cacheDependency, DateTime cacheDuration,
                        TimeSpan slidingExpiration) where T : class
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");
            T item = null;
            if (!_cache.ContainsKey(key))
            {
                item = getUncachedItem();
                _cache.Add(key, item);
            }
            else
            {
                item = _cache[key] as T;
            }
            return item;
        }

        public void Insert(string key, object obj)
        {
            Insert(key, obj, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration);
        }

        public void Insert(string key, object obj, CacheDependency cacheDependency, DateTime cacheDuration,
                           TimeSpan slidingExpiration)
        {
            if (_cache.ContainsKey(key))
                _cache[key] = obj;
            else
                _cache.Add(key, obj);
        }

        public void Insert(string key, object obj, DateTime expiresAbsolute, TimeSpan expiresRelative)
        {
            Insert(key, obj, null, expiresAbsolute, expiresRelative);
        }

        public void Insert(string key, object obj, DateTime expiresAbsolute)
        {
            Insert(key, obj, expiresAbsolute, Cache.NoSlidingExpiration);
        }

        public void Insert(string key, object obj, TimeSpan expiresRelative)
        {
            Insert(key, obj, Cache.NoAbsoluteExpiration, expiresRelative);
        }

        public void Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");
            if (_cache.ContainsKey(key))
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