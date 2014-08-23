using Realm.Library.Patterns.Repository;
using System;
using System.Collections.Generic;
using System.Runtime.Caching;

// ReSharper disable once CheckNamespace
namespace Realm.Library.Common
{
    public sealed class CachedObjectRepository<TKey, T> : IRepository<TKey, T> where T : class
    {
        private ObjectCache Cache { get; set; }
        private CacheItemPolicy Policy { get; set; }

        public CachedObjectRepository(long cacheDurationSeconds)
        {
            Cache = MemoryCache.Default;
            Policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(cacheDurationSeconds) };
        }

        private static string GetCacheKey(TKey key)
        {
            return string.Format("CacheItem_{0}", key.ToString());
        }

        public bool Add(TKey key, T entity)
        {
            Cache.Set(GetCacheKey(key), entity, Policy);
            return true;
        }

        public bool Delete(TKey key)
        {
            if (!Cache.Contains(GetCacheKey(key)))
                return false;
            Cache.Remove(GetCacheKey(key));
            return true;
        }

        public bool Contains(TKey key)
        {
            return Cache.Contains(GetCacheKey(key));
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public int Count { get { return (int)Cache.GetCount(); } }

        public T Get(TKey key)
        {
            return (T)Cache.Get(GetCacheKey(key));
        }

        public IEnumerable<TKey> Keys { get { throw new NotImplementedException(); } }

        public IEnumerable<T> Values { get { throw new NotImplementedException(); } }
    }
}
