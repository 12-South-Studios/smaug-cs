using Library.Common.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Library.Common.Data;

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
  public static T ToAtom<T>(this int value) where T : Atom
  {
    return (T)Activator.CreateInstance(typeof(T), value);
  }

  /// <summary>
  /// Converts the boolean value to an atom
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="value"></param>
  /// <returns></returns>
  public static T ToAtom<T>(this bool value) where T : Atom
  {
    return (T)Activator.CreateInstance(typeof(T), [value]);
  }

  /// <summary>
  /// Converts the 64-bit integer to an atom
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="value"></param>
  /// <returns></returns>
  public static T ToAtom<T>(this long value) where T : Atom
  {
    return (T)Activator.CreateInstance(typeof(T), value);
  }

  /// <summary>
  /// Converts the single value to an atom
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="value"></param>
  /// <returns></returns>
  public static T ToAtom<T>(this float value) where T : Atom
  {
    return (T)Activator.CreateInstance(typeof(T), value);
  }

  /// <summary>
  /// Converts the double value to an atom
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="value"></param>
  /// <returns></returns>
  public static T ToAtom<T>(this double value) where T : Atom
  {
    return (T)Activator.CreateInstance(typeof(T), value);
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
        bool boolResult;
        return !bool.TryParse(value, out boolResult) ? ToAtom<T>(false) : boolResult.ToAtom<T>();

      case "intatom":
        int intResult;
        return !int.TryParse(value, out intResult) ? ToAtom<T>(-1) : intResult.ToAtom<T>();

      case "realatom":
        long result64;
        if (long.TryParse(value, out result64))
          return result64.ToAtom<T>();

        double resultDbl;
        return double.TryParse(value, out resultDbl) ? resultDbl.ToAtom<T>() : ToAtom<T>(0.0f);

      case "stringatom":
      case "objectatom":
        return (T)Activator.CreateInstance(typeof(T), value);
    }

    return null;
  }

  /// <summary>
  /// Converts a collection into a ListAtom
  /// </summary>
  public static ListAtom ToAtom(this ICollection list)
  {
    Validation.IsNotNull(list, "list");

    ListAtom atom = [];

    list.Cast<object>().Where(value => value != null).ToList().ForEach(value =>
    {
      switch (value)
      {
        case int i:
          atom.Add(i.ToAtom<IntAtom>());
          break;
        case long:
        case double:
        case float:
          atom.Add(((long)value).ToAtom<RealAtom>());
          break;
        case bool b:
          atom.Add(b.ToAtom<BoolAtom>());
          break;
        case string s:
          atom.Add(ToAtom<StringAtom>(s));
          break;
        default:
          atom.Add(value.ToDictionaryAtom());
          break;
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
    Type type = obj.GetType();
    FieldInfo[] fieldInfo = type.GetFields();

    DictionaryAtom atom = new DictionaryAtom();

    fieldInfo.ToList().ForEach(info =>
    {
      object value = info.GetValue(obj);

      switch (value)
      {
        case IList list:
          atom.Set(info.Name, list.ToAtom());
          return;
        case string str:
          atom.Set(info.Name, ToAtom<StringAtom>(str));
          return;
        case int i:
          atom.Set(info.Name, i.ToAtom<IntAtom>());
          break;
        case long:
        case double:
        case float:
          atom.Set(info.Name, ((long)value).ToAtom<RealAtom>());
          break;
        case bool b:
          atom.Set(info.Name, b.ToAtom<BoolAtom>());
          break;
        default:
          atom.Set(info.Name, value.ToDictionaryAtom());
          break;
      }
    });

    return atom;
  }
}