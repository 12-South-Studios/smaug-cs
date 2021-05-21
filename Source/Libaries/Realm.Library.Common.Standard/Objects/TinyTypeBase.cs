using System;

namespace Realm.Library.Common.Objects

{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TinyTypeBase<T>
    {
        private readonly T _value;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public TinyTypeBase(T value)
        {
            _value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T GetValue()
        {
            return _value;
        }

        public override string ToString()
        {
            throw new InvalidOperationException("Do not use ToString on a TinyType.");
        }
    }
}
