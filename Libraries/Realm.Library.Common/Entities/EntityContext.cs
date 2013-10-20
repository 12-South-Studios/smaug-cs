using System.Collections.Generic;
using System.Linq;

// ReSharper disable CheckNamespace
namespace Realm.Library.Common
// ReSharper restore CheckNamespace
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EntityContext<T> : BaseContext<T>, IEntityContext<T>
        where T : IEntity
    {
        private readonly List<T> _contents = new List<T>();

        /// <summary>
        ///
        /// </summary>
        /// <param name="parent"></param>
        protected EntityContext(T parent)
            : base(parent)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool Contains(T entity)
        {
            return _contents.Contains(entity);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool Contains(long id)
        {
            return _contents.Any(entity => entity.ID == id);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool AddEntity(T entity)
        {
            var returnVal = false;

            if (!Contains(entity))
            {
                _contents.Add(entity);
                returnVal = true;
            }

            return returnVal;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="entityList"></param>
        public virtual void AddEntities(IEnumerable<T> entityList)
        {
            _contents.AddRange(entityList);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool RemoveEntity(T entity)
        {
            return Contains(entity) && _contents.Remove(entity);
        }

        /// <summary>
        ///
        /// </summary>
        public virtual IList<T> Entities
        {
            get { return _contents; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T GetEntity(long id)
        {
            return _contents.FirstOrDefault(entity => entity.ID == id);
        }

        /// <summary>
        ///
        /// </summary>
        public virtual int Count
        {
            get { return _contents.Count; }
        }
    }
}