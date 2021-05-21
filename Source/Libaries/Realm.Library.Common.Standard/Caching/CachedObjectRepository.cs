using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using Realm.Standard.Patterns.Repository;

namespace Realm.Library.Common.Caching
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="T"></typeparam>
    public class CachedObjectRepository<TKey, T> : IRepository<TKey, T> where T : class
    {
        private ObjectCache Cache { get; }
        private CacheItemPolicy Policy { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheDurationSeconds"></param>
        public CachedObjectRepository(long cacheDurationSeconds)
        {
            Cache = MemoryCache.Default;
            Policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(cacheDurationSeconds) };
        }

        private string GetCacheKey(TKey key)
        {
            return $"CacheItem_{key}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Add(TKey key, T entity)
        {
            Cache.Set(GetCacheKey(key), entity, Policy);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Delete(TKey key)
        {
            if (!Cache.Contains(GetCacheKey(key)))
                return false;
            Cache.Remove(GetCacheKey(key));
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(TKey key)
        {
            return Cache.Contains(GetCacheKey(key));
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count => (int)Cache.GetCount();

        /// <summary>
        /// Gets the entity by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get(TKey key)
        {
            return (T)Cache.Get(GetCacheKey(key));
        }

        /// <summary>
        /// Gets the keys
        /// </summary>
        public IEnumerable<TKey> Keys { get { throw new NotImplementedException(); } }

        /// <summary>
        /// Gets the values
        /// </summary>
        public IEnumerable<T> Values { get { throw new NotImplementedException(); } }
    }
}
