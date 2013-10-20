using System;
using System.Collections.Generic;

namespace Realm.Library.Patterns.Factory
{
    /// <summary>
    /// Abstract class that establishes the framework for a factory
    /// </summary>
    /// <typeparam name="TK"></typeparam>
    /// <typeparam name="TV"></typeparam>
    public abstract class Factory<TK, TV> : IFactory<TK, TV>
        where TK : Type
        where TV : class, new()
    {
        private readonly SortedList<TK, CreateFunctor> _products = new SortedList<TK, CreateFunctor>();

        private delegate TV CreateFunctor();

        /// <summary>
        /// Registers the key and object type
        /// </summary>
        /// <typeparam name="TU"></typeparam>
        /// <param name="key"></param>
        public virtual void Register<TU>(TK key) where TU : TV, new()
        {
            CreateFunctor createFunctor = Creator<TU>;
            _products.Add(key, createFunctor);
        }

        /// <summary>
        /// Creates a new object of the type assigned to the key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual TV Create(TK key)
        {
            if (!_products.ContainsKey(key))
                return null;

            var createFunctor = _products[key];
            return createFunctor();
        }

        /// <summary>
        /// Creates a new type
        /// </summary>
        /// <typeparam name="TU"></typeparam>
        /// <returns></returns>
        public virtual TV Creator<TU>() where TU : TV, new()
        {
            return new TU();
        }
    }
}