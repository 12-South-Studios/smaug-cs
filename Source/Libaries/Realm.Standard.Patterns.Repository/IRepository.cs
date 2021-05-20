using System.Collections.Generic;

namespace Realm.Standard.Patterns.Repository
{
    public interface IRepository<TKey, T> where T : class
    {
        bool Add(TKey key, T entity);
        
        bool Delete(TKey key);
        
        bool Contains(TKey key);
        
        void Clear();
        
        int Count { get; }
        
        T Get(TKey key);
        
        IEnumerable<TKey> Keys { get; }
        
        IEnumerable<T> Values { get; }
    }
}