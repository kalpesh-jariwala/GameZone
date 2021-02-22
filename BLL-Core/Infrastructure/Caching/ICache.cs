using System;
using System.Collections;
using System.Web.Caching;

namespace BLL_Core.Infrastructure.Caching
{
    public interface ICache
    {
        object this[string key] { get; }
        T Get<T>(string key) where T : class;

        T Get<T>(string key, Func<T> getUncachedItem, CacheDependency cacheDependency, DateTime cacheDuration,
                 TimeSpan slidingExpiration) where T : class;

        void Insert(string key, object obj);

        void Insert(string key, object obj, CacheDependency cacheDependency, DateTime cacheDuration,
                    TimeSpan slidingExpiration);

        void Insert(string key, object obj, DateTime expiresAbsolute, TimeSpan expiresRelative);
        void Insert(string key, object obj, DateTime expiresAbsolute);
        void Insert(string key, object obj, TimeSpan expiresRelative);

        void Remove(string key);

        // default indexer

        IDictionaryEnumerator GetEnumerator();
    }
}