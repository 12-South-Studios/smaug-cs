using System.Collections.Generic;
using System.Data;
using Realm.Library.Common.Objects;
using Realm.Library.Common.Properties;

namespace Realm.Library.Common.Events

{
    /// <summary>
    /// The BooleanSet class provides a mechanism for waiting for a series of things to
    /// occur before the next step can proceed.  The object waits for the series to
    /// be completed and once they are invokes the indicated callback function.
    /// </summary>
    /// <remarks>Adapted from BurstOnline BooleanSet, Jon Arney</remarks>
    public class BooleanSet
    {
        private readonly Dictionary<string, bool> _set;
        private readonly EventTable _table;
        private readonly EventCallback<RealmEventArgs> _callback;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="table"></param>
        /// <param name="callback"></param>
        public BooleanSet(EventTable table, EventCallback<RealmEventArgs> callback)
        {
            _callback = callback;
            _set = new Dictionary<string, bool>();
            _table = table;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="callback"></param>
        public BooleanSet(string msg, EventCallback<RealmEventArgs> callback)
        {
            _callback = callback;
            _set = new Dictionary<string, bool>();
            _table = new EventTable { { "message", msg } };
        }

        /// <summary>
        /// Adds the given itemName to the list of things that this boolean set is waiting for.
        /// </summary>
        /// <param name="itemName"></param>
        public void AddItem(string itemName)
        {
            _set.Add(itemName, false);
        }

        /// <summary>
        /// Gets if the item name exists in the list of things this set is waiting for
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public bool HasItem(string itemName)
        {
            return _set.ContainsKey(itemName);
        }

        /// <summary>
        /// Gets if all of the items within the set are complete
        /// </summary>
        public bool IsComplete => _set.Count == 0;

        /// <summary>
        /// Indicates to the boolean set that the given item is now complete.
        /// If that item was the last incomplete item in the boolean set,
        /// it raises the event for the boolean set on this object.
        /// </summary>
        /// <param name="itemName">Name of the item to complete</param>
        /// <exception cref="KeyNotFoundException">KeyNotFoundException is thrown if the itemName does not exist in the set</exception>
        /// <exception cref="NoNullAllowedException">NoNullAllowedException is thrown if the Callback function was not defined</exception>
        public void CompleteItem(string itemName)
        {
            Validation.IsNotNullOrEmpty(itemName, "itemName");
            Validation.Validate<KeyNotFoundException>(_set.ContainsKey(itemName), Resources.ERR_KEY_NOT_FOUND, itemName);
            Validation.Validate<NoNullAllowedException>(_callback.IsNotNull(), Resources.ERR_NULL_CALLBACK);

            _set.Remove(itemName);

            if (_set.Count == 0)
                _callback.Invoke(new RealmEventArgs(_table["message"].ToString(), _table));
        }
    }
}