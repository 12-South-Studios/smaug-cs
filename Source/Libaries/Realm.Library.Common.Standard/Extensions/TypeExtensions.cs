using System;

namespace Realm.Library.Common.Extensions

{
    /// <summary>
    ///
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Instantiates an object of the given type and passes the supplied
        /// arguments to the constructor
        /// </summary>
        public static T Instantiate<T>(this Type type, params object[] args)
        {
            Validation.IsNotNull(type, "type");

            return (T)Activator.CreateInstance(type, args);
        }
    }
}