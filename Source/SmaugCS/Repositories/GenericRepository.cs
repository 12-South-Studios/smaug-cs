using Realm.Library.Patterns.Repository;

namespace SmaugCS.Repositories
{
    public class GenericRepository<T> : Repository<long, T> where T : class
    {
    }
}
