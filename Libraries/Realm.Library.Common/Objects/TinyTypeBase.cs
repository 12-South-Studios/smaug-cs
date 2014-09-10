using System;

// ReSharper disable CheckNamespace
namespace Realm.Library.Common
// ReSharper restore CheckNamespace
{
    public class TinyTypeBase<T>
    {
        private readonly T _value;

        public TinyTypeBase(T value)
        {
            _value = value;
        }

        public T Value
        {
            get { return _value; }
        }

        public override string ToString()
        {
            throw new InvalidOperationException("Do not use ToString on a TinyType.");
        }
    }
}
