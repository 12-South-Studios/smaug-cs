using Realm.Library.Common.Entities;
using Realm.Standard.Patterns.Repository;

namespace Realm.Library.Common.Collections
{
    /// <summary>
    /// Stores objects that implement IEntity, derives from Repository
    /// </summary>
    public class EntityRepository : Repository<long, IEntity>, IEntityRepository
    {
    }
}