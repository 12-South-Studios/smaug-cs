using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Realm.Library.Patterns.Repository
{
    /// <summary>
    /// Abstract class that can be derived to create a repository
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="T"></typeparam>
    public abstract class Repository<TKey, T> : IRepository<TKey, T>
        where T : class
    {
        private readonly IDictionary<TKey, T> _entities = new ConcurrentDictionary<TKey, T>();

        /// <summary>
        /// Adds an entity with a key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool Add(TKey key, T entity)
        {
            if (Contains(key)) return false;

            _entities.Add(key, entity);
            return true;
        }

        /// <summary>
        /// Removes an entity that is set with a key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool Delete(TKey key)
        {
            return Contains(key) && _entities.Remove(key);
        }

        /// <summary>
        /// Gets if the key is present in the repository
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool Contains(TKey key)
        {
            return _entities.ContainsKey(key);
        }

        /// <summary>
        /// Gets the entity by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual T Get(TKey key)
        {
            T value;
            _entities.TryGetValue(key, out value);
            return value;
        }

        /// <summary>
        /// Clears all entities and keys
        /// </summary>
        public virtual void Clear()
        {
            _entities.Clear();
        }

        /// <summary>
        /// Gets the number of entities in the repository
        /// </summary>
        public virtual int Count
        {
            get { return _entities.Count; }
        }

        /// <summary>
        /// Gets the keys
        /// </summary>
        public IEnumerable<TKey> Keys
        {
            get { return _entities.Keys; }
        }

        /// <summary>
        /// Gets the values
        /// </summary>
        public IEnumerable<T> Values
        {
            get { return _entities.Values; }
        }
    }
}