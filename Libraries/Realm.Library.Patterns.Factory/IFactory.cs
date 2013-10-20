using System;

namespace Realm.Library.Patterns.Factory
{
    /// <summary>
    /// Declares the contract for a factory class
    /// </summary>
    /// <typeparam name="TK"></typeparam>
    /// <typeparam name="TV"></typeparam>
    public interface IFactory<in TK, TV>
        where TK : Type
        where TV : class, new()
    {
        /// <summary>
        /// Registers the key and object type
        /// </summary>
        /// <typeparam name="TU"></typeparam>
        /// <param name="key"></param>
        void Register<TU>(TK key) where TU : TV, new();

        /// <summary>
        /// Creates a new object of the type assigned to the key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        TV Create(TK key);

        /// <summary>
        /// Creates a new type
        /// </summary>
        /// <typeparam name="TU"></typeparam>
        /// <returns></returns>
        TV Creator<TU>() where TU : TV, new();
    }
}