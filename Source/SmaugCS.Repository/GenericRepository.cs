using Realm.Library.Patterns.Repository;

namespace SmaugCS.Repository
{
    public class GenericRepository<T> : Repository<long, T> where T : class
    {
    }
}
