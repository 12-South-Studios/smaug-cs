using Realm.Library.Patterns.Repository;

namespace SmaugCS.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericRepository<T> : Repository<long, T> where T : class
    {
    }
}
