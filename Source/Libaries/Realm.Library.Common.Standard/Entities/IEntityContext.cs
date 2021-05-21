using System.Collections.Generic;
using Realm.Library.Common.Contexts;

namespace Realm.Library.Common.Entities

{
    /// <summary>
    /// Public interface for a context that allows the managing of objects
    /// that implement IContext
    /// </summary>
    public interface IEntityContext<T> : IContext where T : IEntity
    {
        /// <summary>
        /// Gets if the handler contains the entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Contains(T entity);

        /// <summary>
        /// Gets if the handler contains an entity with the given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Contains(long id);

        /// <summary>
        /// Adds the given entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool AddEntity(T entity);

        /// <summary>
        /// Adds a given list of entities
        /// </summary>
        /// <param name="entityList"></param>
        void AddEntities(IEnumerable<T> entityList);

        /// <summary>
        /// Removes an entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool RemoveEntity(T entity);

        /// <summary>
        /// Gets a list of entities
        /// </summary>
        IList<T> Entities { get; }

        /// <summary>
        /// Gets an entity by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetEntity(long id);

        /// <summary>
        /// Gets the number of entities
        /// </summary>
        int Count { get; }
    }
}