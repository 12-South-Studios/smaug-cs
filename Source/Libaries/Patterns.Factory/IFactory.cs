using System;

namespace Patterns.Factory;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TK"></typeparam>
/// <typeparam name="TV"></typeparam>
public interface IFactory<in TK, TV>
    where TK : Type
    where TV : class, new()
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <typeparam name="TU"></typeparam>
    void Register<TU>(TK key) where TU : TV, new();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    TV Create(TK key);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TU"></typeparam>
    /// <returns></returns>
    TV Creator<TU>() where TU : TV, new();
}