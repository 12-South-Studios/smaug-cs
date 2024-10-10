using System.Collections.Generic;

namespace Patterns.Repository;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="T"></typeparam>
public interface IRepository<TKey, T>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    bool Add(TKey key, T entity); 
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    bool Delete(TKey key);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    bool Contains(TKey key);

    /// <summary>
    /// 
    /// </summary>
    void Clear();

    /// <summary>
    /// 
    /// </summary>
    int Count { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    T Get(TKey key);

    /// <summary>
    /// 
    /// </summary>
    IEnumerable<TKey> Keys { get; }

    /// <summary>
    /// 
    /// </summary>
    IEnumerable<T> Values { get; }
}