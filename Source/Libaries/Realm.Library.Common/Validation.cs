using Realm.Library.Common.Extensions;
using Realm.Library.Common.Objects;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Realm.Library.Common
{
    /// <summary>
    ///
    /// </summary>
    public static class Validation
    {
        /// <summary>
        /// Validates the argument to determine if it is null
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="parameterName"></param>
        public static void IsNotNull(object obj, string parameterName)
        {
            if (obj.IsNull()) throw new ArgumentNullException(parameterName, "Parameter is null");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        public static void IsInstanceOfType(object obj, Type type)
        {
            if (obj.IsNull()) throw new ArgumentNullException(nameof(obj), "Parameter is null");
            if (type.IsNull()) throw new ArgumentNullException(nameof(type), "Parameter is null");
            if (obj.GetType() != type && !obj.GetType().IsSubclassOf(type))
                throw new ArgumentException(string.Format("Object is not and does not derive from type {0}.", type));
        }

        /// <summary>
        /// Validates the string argument for null or empty
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="parameterName"></param>
        public static void IsNotNullOrEmpty(string obj, string parameterName)
        {
            if (string.IsNullOrEmpty(obj)) throw new ArgumentNullException(parameterName, "Parameter is null");
        }

        /// <summary>
        /// Validates the collection argument to size
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parameterName"></param>
        public static void IsNotEmpty<T>(ICollection<T> value, string parameterName = "")
        {
            if (value.IsNull()) throw new ArgumentNullException(parameterName, "Parameter is null");
            if (value.Count == 0) throw new ArgumentException("Collection contains no values", parameterName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parameterName"></param>
        public static void IsNotEmpty(ICollection value, string parameterName = "")
        {
            if (value.IsNull()) throw new ArgumentNullException(parameterName, "Parameter is null");
            if (value.Count == 0) throw new ArgumentException("Collection contains no values", parameterName);
        }

        /// <summary>
        /// Validates the argument and if it fails throws an ArgumentException
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Validate(bool arg, string message = "", params object[] args)
        {
            Validate<ArgumentException>(arg, message, args);
        }

        /// <summary>
        /// Validates the argument and if it fails throws an Exception of the given type
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Validate<T>(bool arg, string message = "", params object[] args) where T : Exception
        {
            if (!arg) throw typeof(T).Instantiate<T>(string.Format(message, args));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationAction"></param>
        public static void Validate(Action validationAction)
        {
            validationAction.Invoke();
        }
    }
}