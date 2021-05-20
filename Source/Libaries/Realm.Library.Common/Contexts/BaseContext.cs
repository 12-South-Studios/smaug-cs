
using Realm.Library.Common.Entities;

namespace Realm.Library.Common.Contexts

{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseContext<T> where T : IEntity
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="owner"></param>
        protected BaseContext(T owner)
        {
            Owner = owner;
        }

        /// <summary>
        ///
        /// </summary>
        public T Owner { get; private set; }
    }
}