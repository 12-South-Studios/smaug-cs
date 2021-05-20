using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Realm.Standard.Patterns.Repository
{
    public abstract class Repository<TKey, T> : IRepository<TKey, T>
        where T : class
    {
        private readonly IDictionary<TKey, T> _entities = new ConcurrentDictionary<TKey, T>();
        
        public virtual bool Add(TKey key, T entity)
        {
            if (Contains(key)) return false;

            _entities.Add(key, entity);
            return true;
        }
        
        public virtual bool Delete(TKey key)
        {
            return Contains(key) && _entities.Remove(key);
        }
        
        public virtual bool Contains(TKey key)
        {
            return _entities.ContainsKey(key);
        }
        
        public virtual T Get(TKey key)
        {
            T value;
            _entities.TryGetValue(key, out value);
            return value;
        }
        
        public virtual void Clear()
        {
            _entities.Clear();
        }
        
        public virtual int Count => _entities.Count;
        
        public IEnumerable<TKey> Keys => _entities.Keys;
        
        public IEnumerable<T> Values => _entities.Values;
    }
}