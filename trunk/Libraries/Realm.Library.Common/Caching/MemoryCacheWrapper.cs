using System;
using System.Collections.Specialized;
using System.Runtime.Caching;

// ReSharper disable CheckNamespace
namespace Realm.Library.Common
// ReSharper restore CheckNamespace
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MemoryCacheWrapper<T> : IMemoryCacheWrapper<T>, IDisposable
    {
        private readonly MemoryCache _memoryCache;
        private CacheItemPolicy _cacheItemPolicy;
        private bool _isDisposed;

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <param name="config"></param>
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

        /// <summary>
        ///
        /// </summary>
        ~MemoryCacheWrapper()
        {
            // Garbage collection has kicked in tidy up your object.
            Dispose(false);
        }

        /// <summary>
        ///
        /// </summary>
        public string Name { get { return _memoryCache.Name; } }

        /// <summary>
        ///
        /// </summary>
        public long CacheMemoryLimitInBytes
        {
            get { return _memoryCache.CacheMemoryLimit; }
        }

        /// <summary>
        ///
        /// </summary>
        public long PhysicalMemoryLimit
        {
            get { return _memoryCache.PhysicalMemoryLimit; }
        }

        /// <summary>
        ///
        /// </summary>
        public TimeSpan PollingInterval
        {
            get { return _memoryCache.PollingInterval; }
        }

        /// <summary>
        ///
        /// </summary>
        public CacheItemPolicy CacheItemPolicy
        {
            get { return _cacheItemPolicy; }
            set
            {
                if (value.IsNotNull())
                    _cacheItemPolicy = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddOrUpdate(string key, T value)
        {
            _memoryCache.Set(key, value, CacheItemPolicy);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return _memoryCache.Contains(key);
        }

        /// <summary>
        ///
        /// </summary>
        public long Count
        {
            get { return _memoryCache.GetCount(); }
        }

        #region region Implement IDisposable

        /// <summary>
        ///
        /// </summary>
        public void Dispose()
        {
            // Dispose has been called clean up your object and members that
            // are disposable.
            Dispose(true);

            // Now Make sure that you don't call the cleanup again via the finalizer
            // Effectively you are taking over garbage collection so make sure you
            // don't do it again
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            // Only do this once.
            if (!_isDisposed)
            {
                // If called via IDispose interface then clean up sub-objects.
                // That implement the IDispose interface.
                if (disposing)
                {
                    // Dispose managed resources.
                    _memoryCache.Dispose();
                }

                // Now clean-up and objects that don't implement dispose.
                // i.e close any file handles

                // Currently nothing to do.
            }
            _isDisposed = true;
        }

        #endregion region Implement IDisposable
    }
}