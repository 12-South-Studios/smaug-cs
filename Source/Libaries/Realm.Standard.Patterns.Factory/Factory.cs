using System;
using System.Collections.Generic;

namespace Realm.Standard.Patterns.Factory
{
    public abstract class Factory<TK, TV> : IFactory<TK, TV>
        where TK : Type
        where TV : class, new()
    {
        private readonly SortedList<TK, CreateFunctor> _products = new SortedList<TK, CreateFunctor>();

        private delegate TV CreateFunctor();

        public virtual void Register<TU>(TK key) where TU : TV, new()
        {
            CreateFunctor createFunctor = Creator<TU>;
            _products.Add(key, createFunctor);
        }

        public virtual TV Create(TK key)
        {
            if (!_products.ContainsKey(key))
                return null;

            var createFunctor = _products[key];
            return createFunctor();
        }
        
        public virtual TV Creator<TU>() where TU : TV, new()
        {
            return new TU();
        }
    }
}
