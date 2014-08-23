using System;
using System.Runtime.Caching;

// ReSharper disable once CheckNamespace
namespace Realm.Library.Common
{
    public interface IMemoryCacheWrapper<T>
    {
        string Name { get; }
        long CacheMemoryLimitInBytes { get; }
        long PhysicalMemoryLimit { get; }
        TimeSpan PollingInterval { get; }
        CacheItemPolicy CacheItemPolicy { get; set; }
        long Count { get; }

        void AddOrUpdate(string key, T value);
        bool TryGetValue(string key, out T value);
        bool TryRemove(string key, out T value);
        void Remove(string key);
        bool ContainsKey(string key);
    }
}