using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common.Logging;
using Realm.Library.Common.Properties;

namespace Realm.Library.Common.Data
{
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
        public bool IsEmpty()
        {
            return _map.Count == 0;
        }

        /// <summary>
        /// Gets the number of objects in the dictionary
        /// </summary>
        public int Count { get { return _map.Count; } }

        /// <summary>
        /// Gets if the dictionary contains a string key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return _map.ContainsKey(new StringAtom(key));
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetAtom<T>(string key) where T : Atom
        {
            Atom atom = null;
            var atomKey = new StringAtom(key);

            if (_map.ContainsKey(atomKey))
                _map.TryGetValue(atomKey, out atom);

            return atom.IsNotNull() ? (T)atom : null;
        }

        /// <summary>
        ///
        /// </summary>
        public int GetInt(string key)
        {
            var atom = GetAtom<IntAtom>(key);
            return atom.IsNotNull() ? atom.CastAs<IntAtom>().Value : 0;
        }

        /// <summary>
        ///
        /// </summary>
        public string GetString(string key)
        {
            var atom = GetAtom<StringAtom>(key);
            return atom.IsNotNull() ? atom.CastAs<StringAtom>().Value : string.Empty;
        }

        /// <summary>
        ///
        /// </summary>
        public bool GetBool(string key)
        {
            var atom = GetAtom<BoolAtom>(key);
            return atom.IsNotNull() && atom.CastAs<BoolAtom>().Value;
        }

        /// <summary>
        ///
        /// </summary>
        public double GetReal(string key)
        {
            var atom = GetAtom<RealAtom>(key);
            return atom.IsNotNull() ? atom.CastAs<RealAtom>().Value : 0.0D;
        }

        /// <summary>
        ///
        /// </summary>
        public object GetObject(string key)
        {
            var atom = GetAtom<ObjectAtom>(key);
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
                Atom atom;
                _map.TryRemove(key, out atom);
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
            var valueAtom = new StringAtom(value);
            _map.AddOrUpdate(new StringAtom(key), valueAtom, (k, o) => valueAtom);
        }

        /// <summary>
        /// Set a boolean value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, bool value)
        {
            var valueAtom = new BoolAtom(value);
            _map.AddOrUpdate(new StringAtom(key), valueAtom, (k, o) => valueAtom);
        }

        /// <summary>
        /// Set a long value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, long value)
        {
            var valueAtom = new IntAtom(value);
            _map.AddOrUpdate(new StringAtom(key), valueAtom, (k, o) => valueAtom);
        }

        /// <summary>
        /// Sets an integer value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, int value)
        {
            var valueAtom = new IntAtom(value);
            _map.AddOrUpdate(new StringAtom(key), valueAtom, (k, o) => valueAtom);
        }

        /// <summary>
        /// Sets a double value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, double value)
        {
            var valueAtom = new RealAtom(value);
            _map.AddOrUpdate(new StringAtom(key), valueAtom, (k, o) => valueAtom);
        }

        /// <summary>
        /// Sets a float value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, float value)
        {
            var valueAtom = new RealAtom(value);
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
            var valueAtom = new ObjectAtom(value);
            _map.AddOrUpdate(new StringAtom(key), valueAtom, (k, o) => valueAtom);
        }

        #endregion Set/Add

        /// <summary>
        /// Gets an enumerable list of keys
        /// </summary>
        public IEnumerable<Atom> Keys
        {
            get { return _map.Keys; }
        }

        /// <summary>
        /// Gets an enumerable collection of values
        /// </summary>
        public ICollection<Atom> Values
        {
            get { return _map.Values; }
        }

        /// <summary>
        /// Dumps the contents of the Atom with the given prefix
        /// </summary>
        /// <param name="log"></param>
        /// <param name="prefix"></param>
        public override void Dump(ILogWrapper log, string prefix)
        {
            Validation.IsNotNull(log, "log");

            log.InfoFormat(Resources.LOG_DICT_ATOM_FORMAT, prefix);
            _map.Keys.ToList().ForEach(key =>
                {
                    var value = _map[key];
                    switch (key.Type)
                    {
                        case AtomType.String:
                            {
                                var stringKey = (StringAtom)key;
                                if (value.IsNotNull())
                                    value.Dump(log, string.Format(Resources.LOG_DICT_ATOM_KEY, prefix, stringKey.Value));
                                else
                                    log.InfoFormat(Resources.LOG_DICT_ATOM_NULL_KEY, prefix, stringKey.Value);
                            }
                            break;

                        case AtomType.Integer:
                            {
                                var intKey = (IntAtom)key;
                                if (value.IsNotNull())
                                    value.Dump(log, string.Format(Resources.LOG_DICT_ATOM_KEY, prefix, intKey.Value));
                                else
                                    log.InfoFormat(Resources.LOG_DICT_ATOM_NULL_KEY, prefix, intKey.Value);
                            }
                            break;
                    }
                });
        }
    }
}