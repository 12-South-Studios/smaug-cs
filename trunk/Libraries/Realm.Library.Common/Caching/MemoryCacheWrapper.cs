using System;
using System.Collections.Specialized;
using System.Runtime.Caching;

// ReSharper disable once CheckNamespace
namespace Realm.Library.Common
{
    public class MemoryCacheWrapper<T> : IMemoryCacheWrapper<T>, IDisposable
    {
        private readonly MemoryCache _memoryCache;
        private CacheItemPolicy _cacheItemPolicy;
        private bool _isDisposed;

        public MemoryCacheWrapper(string name, NameValueCollection config = null)
        {
            _memoryCache = config.IsNotNull()
                ? new MemoryCache(name, config)
                : new MemoryCache(name);

            _isDisposed = false;
            CacheItemPolicy = new CacheItemPolicy
                                {
                                    SlidingExpiration = new TimeSpan(1, 0, 0)
                                };
        }

        ~MemoryCacheWrapper()
        {
            Dispose(false);
        }

        public string Name { get { return _memoryCache.Name; } }

        public long CacheMemoryLimitInBytes
        {
            get { return _memoryCache.CacheMemoryLimit; }
        }

        public long PhysicalMemoryLimit
        {
            get { return _memoryCache.PhysicalMemoryLimit; }
        }

        public TimeSpan PollingInterval
        {
            get { return _memoryCache.PollingInterval; }
        }

        public CacheItemPolicy CacheItemPolicy
        {
            get { return _cacheItemPolicy; }
            set
            {
                if (value.IsNotNull())
                    _cacheItemPolicy = value;
            }
        }

        public void AddOrUpdate(string key, T value)
        {
            _memoryCache.Set(key, value, CacheItemPolicy);
        }

        public bool TryGetValue(string key, out T value)
        {
            bool result = false;
            value = default(T);

            object item = _memoryCache.Get(key);
            if (item.IsNotNull())
            {
                value = (T)item;
                result = true;
            }

            return result;
        }

        public bool TryRemove(string key, out T value)
        {
            bool result = false;
            value = default(T);

            object item = _memoryCache.Remove(key);
            if (item.IsNotNull())
            {
                result = true;
                value = (T)item;
            }

            return result;
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public bool ContainsKey(string key)
        {
            return _memoryCache.Contains(key);
        }

        public long Count
        {
            get { return _memoryCache.GetCount(); }
        }

        #region region Implement IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                    _memoryCache.Dispose();
            }
            _isDisposed = true;
        }

        #endregion region Implement IDisposable
    }
}