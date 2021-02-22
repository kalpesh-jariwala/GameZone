using System;
using System.Collections;
using System.Web;
using System.Web.Caching;

namespace BLL_Core.Infrastructure.Caching
{
    public class WebSessionCache : ICache
    {
        public T Get<T>(string key) where T : class
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");
            //var item = _cache[key] as T;
            var item = HttpContext.Current.Session[key] as T;
            return item;
        }

        public T Get<T>(string key, Func<T> getUncachedItem, CacheDependency cacheDependency, DateTime cacheDuration,
                        TimeSpan slidingExpiration) where T : class
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");
            var item = HttpContext.Current.Session[key] as T;
            if (item == null)
            {
                item = getUncachedItem();
                HttpContext.Current.Session.Add(key, item);
            }
            return item;
        }

        public void Insert(string key, object obj)
        {
            Insert(key, obj, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration);
            //HttpContext.Current.Session.Add(key, obj);
        }


        public void Insert(string key, object obj, CacheDependency cacheDependency, DateTime cacheDuration,
                           TimeSpan slidingExpiration)
        {
            //HttpContext.Current.Session.Add(key, obj);
            this[key] = obj;
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
            if (this[key] != null)
            {
                HttpContext.Current.Session.Remove(key);
            }
        }

        public object this[string key]
        {
            get { return HttpContext.Current.Session[key]; }
            set { HttpContext.Current.Session[key] = value; }
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            //return  _cache.GetEnumerator();
            //TODO : Cannot convert IEnumerator to IDictionaryEnumberator error
            throw new NotImplementedException();
        }
    }
}