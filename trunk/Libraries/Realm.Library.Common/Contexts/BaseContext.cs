
// ReSharper disable once CheckNamespace
namespace Realm.Library.Common
{
    public abstract class BaseContext<T> : IContext 
        where T : IEntity
    {
        protected BaseContext(T owner)
        {
            Owner = owner;
        }

        public IEntity Owner { get; private set; }
    }
}