using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Realm.Library.Common.Data
{
    /// <summary>
    /// Defines extension functions for Atoms
    /// </summary>
    public static class AtomExtensions
    {
        /// <summary>
        /// Converts to 32-bit integer to an atom
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToAtom<T>(this Int32 value) where T : Atom
        {
            return (T)Activator.CreateInstance(typeof(T), new object[] { value });
        }

        /// <summary>
        /// Converts the boolean value to an atom
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToAtom<T>(this Boolean value) where T : Atom
        {
            return (T)Activator.CreateInstance(typeof(T), new object[] { value });
        }

        /// <summary>
        /// Converts the 64-bit integer to an atom
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToAtom<T>(this Int64 value) where T : Atom
        {
            return (T)Activator.CreateInstance(typeof(T), new object[] { value });
        }

        /// <summary>
        /// Converts the single value to an atom
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToAtom<T>(this Single value) where T : Atom
        {
            return (T)Activator.CreateInstance(typeof(T), new object[] { value });
        }

        /// <summary>
        /// Converts the double value to an atom
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToAtom<T>(this Double value) where T : Atom
        {
            return (T)Activator.CreateInstance(typeof(T), new object[] { value });
        }

        /// <summary>
        /// Converts the string value to an atom
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToAtom<T>(this string value) where T : Atom
        {
            Validation.IsNotNullOrEmpty(value, "value");

            switch (typeof(T).Name.ToLower())
            {
                case "boolatom":
                    Boolean boolResult;
                    return !Boolean.TryParse(value, out boolResult) ? ToAtom<T>(false) : boolResult.ToAtom<T>();

                case "intatom":
                    Int32 intResult;
                    return !Int32.TryParse(value, out intResult) ? ToAtom<T>(-1) : intResult.ToAtom<T>();

                case "realatom":
                    Int64 result64;
                    if (Int64.TryParse(value, out result64))
                        return result64.ToAtom<T>();

                    Double resultDbl;
                    return Double.TryParse(value, out resultDbl) ? resultDbl.ToAtom<T>() : ToAtom<T>(0.0f);

                case "stringatom":
                case "objectatom":
                    return (T)Activator.CreateInstance(typeof(T), new object[] { value });
            }

            return null;
        }

        /// <summary>
        /// Converts a collection into a ListAtom
        /// </summary>
        public static ListAtom ToAtom(this ICollection list)
        {
            Validation.IsNotNull(list, "list");

            var atom = new ListAtom();

            list.Cast<object>().Where(value => value != null).ToList().ForEach(value =>
                {
                    if (value is Int32)
                        atom.Add(((Int32) value).ToAtom<IntAtom>());
                    else if (value is Int64 || value is Double || value is Single)
                        atom.Add(((Int64) value).ToAtom<RealAtom>());
                    else if (value is Boolean)
                        atom.Add(((Boolean) value).ToAtom<BoolAtom>());
                    else
                    {
                        var s = value as string;
                        if (s != null)
                            atom.Add(ToAtom<StringAtom>(s));
                        else
                            atom.Add(value.ToDictionaryAtom());
                    }
                });

            return atom;
        }

        /// <summary>
        /// Converts a dictionary atom into a dictionary of string-object pairs
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ToDictionary(this DictionaryAtom source)
        {
            return source.Keys.Select(keyAtom => keyAtom.CastAs<StringAtom>())
                .ToDictionary(key => key.Value, key => source.GetObject(key.Value));
        }

        /// <summary>
        /// Converts an object and its field values into a DictionaryAtom
        /// </summary>
        public static DictionaryAtom ToDictionaryAtom(this object obj)
        {
            var type = obj.GetType();
            var fieldInfo = type.GetFields();

            var atom = new DictionaryAtom();

            fieldInfo.ToList().ForEach(info =>
                {
                    var value = info.GetValue(obj);

                    var list = value as IList;
                    if (list != null)
                    {
                        atom.Set(info.Name, list.ToAtom());
                        return;
                    }

                    var str = value as String;
                    if (str != null)
                    {
                        atom.Set(info.Name, ToAtom<StringAtom>((String)value));
                        return;
                    }

                    if (value is Int32)
                        atom.Set(info.Name, ((Int32)value).ToAtom<IntAtom>());
                    else if (value is Int64 || value is Double || value is Single)
                        atom.Set(info.Name, ((Int64)value).ToAtom<RealAtom>());
                    else if (value is Boolean)
                        atom.Set(info.Name, ((Boolean)value).ToAtom<BoolAtom>());
                    else
                        atom.Set(info.Name, value.ToDictionaryAtom());  
                });

            return atom;
        }
    }
}