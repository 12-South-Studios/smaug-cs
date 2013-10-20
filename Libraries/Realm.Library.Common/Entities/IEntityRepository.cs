using Realm.Library.Patterns.Repository;

// ReSharper disable CheckNamespace
namespace Realm.Library.Common
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Public interface for a repository to store objects that implement IEntity
    /// </summary>
    public interface IEntityRepository : IRepository<long, IEntity>
    {
    }
}