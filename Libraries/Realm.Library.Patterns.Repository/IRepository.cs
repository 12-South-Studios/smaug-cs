using System.Collections.Generic;

namespace Realm.Library.Patterns.Repository
{
    /// <summary>
    /// Defines the contract for a repository
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<TKey, T> where T : class
    {
        /// <summary>
        /// Adds an entity with a key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Add(TKey key, T entity);

        /// <summary>
        /// Removes an entity that is set with a key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Delete(TKey key);

        /// <summary>
        /// Gets if the key is present in the repository
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Contains(TKey key);

        /// <summary>
        /// Clears all entities and keys
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets the number of entities in the repository
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the entity by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get(TKey key);

        /// <summary>
        /// Gets the keys
        /// </summary>
        IEnumerable<TKey> Keys { get; }

        /// <summary>
        /// Gets the values
        /// </summary>
        IEnumerable<T> Values { get; }
    }
}