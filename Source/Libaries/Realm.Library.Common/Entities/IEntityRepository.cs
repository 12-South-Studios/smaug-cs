using Realm.Standard.Patterns.Repository;

namespace Realm.Library.Common.Entities

{
    /// <summary>
    /// Public interface for a repository to store objects that implement IEntity
    /// </summary>
    public interface IEntityRepository : IRepository<long, IEntity>
    {
    }
}