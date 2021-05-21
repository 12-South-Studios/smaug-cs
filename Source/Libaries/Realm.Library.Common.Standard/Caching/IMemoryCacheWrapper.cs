using System;
using System.Runtime.Caching;

namespace Realm.Library.Common.Caching

{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMemoryCacheWrapper<T>
    {
        /// <summary>
        ///
        /// </summary>
        string Name { get; }

        /// <summary>
        ///
        /// </summary>
        long CacheMemoryLimitInBytes { get; }

        /// <summary>
        ///
        /// </summary>
        long PhysicalMemoryLimit { get; }

        /// <summary>
        ///
        /// </summary>
        TimeSpan PollingInterval { get; }

        /// <summary>
        ///
        /// </summary>
        CacheItemPolicy CacheItemPolicy { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void AddOrUpdate(string key, T value);

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool TryGetValue(string key, out T value);

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool TryRemove(string key, out T value);

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool ContainsKey(string key);

        /// <summary>
        ///
        /// </summary>
        long Count { get; }
    }
}