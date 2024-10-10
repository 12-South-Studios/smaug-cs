using System;
using Library.Common.Objects;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Library.Common.Logging;

namespace Library.Common.Data;

/// <summary>
/// Class defines a dictionary atom
/// </summary>
public class DictionaryAtom : Atom
{
  private readonly ConcurrentDictionary<Atom, Atom> _map;

  /// <summary>
  /// Constructor
  /// </summary>
  public DictionaryAtom()
    : base(AtomType.Dictionary)
  {
    _map = new ConcurrentDictionary<Atom, Atom>();
  }

  /// <summary>
  /// Copy constructor
  /// </summary>
  /// <param name="atom"></param>
  public DictionaryAtom(DictionaryAtom atom)
    : base(AtomType.Dictionary)
  {
    Validation.IsNotNull(atom, "atom");

    _map = atom._map;
  }

  /// <summary>
  /// Deconstructor
  /// </summary>
  ~DictionaryAtom()
  {
    if (_map.IsNotNull())
      _map.Clear();
  }

  /// <summary>
  /// Gets if teh dictionary is emtpy
  /// </summary>
  /// <returns></returns>
  public bool IsEmpty() => _map.IsEmpty;

  /// <summary>
  /// Gets the number of objects in the dictionary
  /// </summary>
  public int Count => _map.Count;

  /// <summary>
  /// Gets if the dictionary contains a string key
  /// </summary>
  /// <param name="key"></param>
  /// <returns></returns>
  public bool ContainsKey(string key) => _map.ContainsKey(new StringAtom(key));

  /// <summary>
  ///
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="key"></param>
  /// <returns></returns>
  public T GetAtom<T>(string key) where T : Atom
  {
    Atom atom = null;
    StringAtom atomKey = new(key);

    if (_map.ContainsKey(atomKey))
      _map.TryGetValue(atomKey, out atom);

    return atom.IsNotNull() ? (T)atom : null;
  }

  /// <summary>
  ///
  /// </summary>
  public int GetInt(string key)
  {
    IntAtom atom = GetAtom<IntAtom>(key);
    return atom.IsNotNull() ? atom.CastAs<IntAtom>().Value : 0;
  }

  /// <summary>
  ///
  /// </summary>
  public string GetString(string key)
  {
    StringAtom atom = GetAtom<StringAtom>(key);
    return atom.IsNotNull() ? atom.CastAs<StringAtom>().Value : string.Empty;
  }

  /// <summary>
  ///
  /// </summary>
  public bool GetBool(string key)
  {
    BoolAtom atom = GetAtom<BoolAtom>(key);
    return atom.IsNotNull() && atom.CastAs<BoolAtom>().Value;
  }

  /// <summary>
  ///
  /// </summary>
  public double GetReal(string key)
  {
    RealAtom atom = GetAtom<RealAtom>(key);
    return atom.IsNotNull() ? atom.CastAs<RealAtom>().Value : 0.0D;
  }

  /// <summary>
  ///
  /// </summary>
  public object GetObject(string key)
  {
    ObjectAtom atom = GetAtom<ObjectAtom>(key);
    return atom.IsNotNull() ? atom.CastAs<ObjectAtom>().Value : null;
  }

  #region Set/Add

  /// <summary>
  /// Adds an atom with the given key
  /// </summary>
  /// <param name="key"></param>
  /// <param name="value"></param>
  public void Add(Atom key, Atom value)
  {
    if (value.IsNull())
    {
      _map.TryRemove(key, out _);
    }
    else
      _map.TryAdd(key, value);
  }

  /// <summary>
  /// Sets a string value
  /// </summary>
  /// <param name="key"></param>
  /// <param name="value"></param>
  public void Set(string key, string value)
  {
    StringAtom valueAtom = new(value);
    _map.AddOrUpdate(new StringAtom(key), valueAtom, (k, o) => valueAtom);
  }

  /// <summary>
  /// Set a boolean value
  /// </summary>
  /// <param name="key"></param>
  /// <param name="value"></param>
  public void Set(string key, bool value)
  {
    BoolAtom valueAtom = new(value);
    _map.AddOrUpdate(new StringAtom(key), valueAtom, (k, o) => valueAtom);
  }

  /// <summary>
  /// Set a long value
  /// </summary>
  /// <param name="key"></param>
  /// <param name="value"></param>
  public void Set(string key, long value)
  {
    IntAtom valueAtom = new(value);
    _map.AddOrUpdate(new StringAtom(key), valueAtom, (k, o) => valueAtom);
  }

  /// <summary>
  /// Sets an integer value
  /// </summary>
  /// <param name="key"></param>
  /// <param name="value"></param>
  public void Set(string key, int value)
  {
    IntAtom valueAtom = new(value);
    _map.AddOrUpdate(new StringAtom(key), valueAtom, (k, o) => valueAtom);
  }

  /// <summary>
  /// Sets a double value
  /// </summary>
  /// <param name="key"></param>
  /// <param name="value"></param>
  public void Set(string key, double value)
  {
    RealAtom valueAtom = new(value);
    _map.AddOrUpdate(new StringAtom(key), valueAtom, (k, o) => valueAtom);
  }

  /// <summary>
  /// Sets a float value
  /// </summary>
  /// <param name="key"></param>
  /// <param name="value"></param>
  public void Set(string key, float value)
  {
    RealAtom valueAtom = new(value);
    _map.AddOrUpdate(new StringAtom(key), valueAtom, (k, o) => valueAtom);
  }

  /// <summary>
  /// SEts a list atom value
  /// </summary>
  /// <param name="key"></param>
  /// <param name="value"></param>
  public void Set(string key, ListAtom value)
  {
    _map.AddOrUpdate(new StringAtom(key), value, (k, o) => value);
  }

  /// <summary>
  /// Sets a dictionary atom value
  /// </summary>
  /// <param name="key"></param>
  /// <param name="value"></param>
  public void Set(string key, DictionaryAtom value)
  {
    _map.AddOrUpdate(new StringAtom(key), value, (k, o) => value);
  }

  /// <summary>
  /// Sets an object value
  /// </summary>
  /// <param name="key"></param>
  /// <param name="value"></param>
  public void Set(string key, object value)
  {
    ObjectAtom valueAtom = new(value);
    _map.AddOrUpdate(new StringAtom(key), valueAtom, (k, o) => valueAtom);
  }

  #endregion Set/Add

  /// <summary>
  /// Gets an enumerable list of keys
  /// </summary>
  public IEnumerable<Atom> Keys => _map.Keys;

  /// <summary>
  /// Gets an enumerable collection of values
  /// </summary>
  public ICollection<Atom> Values => _map.Values;

  /// <summary>
  /// Dumps the contents of the Atom with the given prefix
  /// </summary>
  /// <param name="log"></param>
  /// <param name="prefix"></param>
  public override void Dump(ILogWrapper log, string prefix)
  {
    Validation.IsNotNull(log, "log");

    log.Info("{0} (DictionaryAtom)", prefix);
    _map.Keys.ToList().ForEach(key =>
    {
      Atom value = _map[key];
      switch (key.Type)
      {
        case AtomType.String:
        {
          StringAtom stringKey = (StringAtom)key;
          if (value.IsNotNull())
            value.Dump(log, $"{prefix}.{stringKey.Value}");
          else
            log.Info("{0}.{1} (null)", prefix, stringKey.Value);
        }
          break;

        case AtomType.Integer:
        {
          IntAtom intKey = (IntAtom)key;
          if (value.IsNotNull())
            value.Dump(log, $"{prefix}.{intKey.Value}");
          else
            log.Info("{0}.{1} (null)", prefix, intKey.Value);
        }
          break;
        case AtomType.Boolean:
          break;
        case AtomType.Real:
          break;
        case AtomType.List:
          break;
        case AtomType.Dictionary:
          break;
        case AtomType.Object:
          break;
        case AtomType.Nil:
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    });
  }
}