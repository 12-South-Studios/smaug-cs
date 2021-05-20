using System;

namespace Realm.Standard.Patterns.Factory
{
    public interface IFactory<in TK, TV>
        where TK : Type
        where TV : class, new()
    {
        void Register<TU>(TK key) where TU : TV, new();

        TV Create(TK key);

        TV Creator<TU>() where TU : TV, new();
    }
}
