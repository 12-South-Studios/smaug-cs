using System;
using Realm.Library.Common.Objects;

namespace Realm.Library.Common.Events

{
    /// <summary>
    /// Event Argument class for the MUD
    /// </summary>
    public sealed class RealmEventArgs : EventArgs
    {
        /// <summary>
        /// The type of event
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// A dictionary table of key-value data
        /// </summary>
        public EventTable Data { get; }

        /// <summary>
        /// The object that sent the event
        /// </summary>
        public object Sender { get; set; }

        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        public RealmEventArgs()
        {
            Data = new EventTable();
        }

        /// <summary>
        /// Initializes a new instance of the class with the given event table
        /// </summary>
        public RealmEventArgs(EventTable table)
        {
            Validation.IsNotNull(table, "table");

            Data = table;
        }

        /// <summary>
        /// Initializes a new instance of the class with the given type
        /// </summary>
        public RealmEventArgs(string type)
        {
            Validation.IsNotNullOrEmpty(type, "type");

            Type = type;
            Data = null;
        }

        /// <summary>
        /// Initializes a new instance of the class with the given type and event table
        /// </summary>
        public RealmEventArgs(string type, EventTable table)
        {
            Validation.IsNotNullOrEmpty(type, "type");
            Validation.IsNotNull(table, "table");

            Type = type;
            Data = table;
        }

        /// <summary>
        /// Gets the given value from the data table
        /// </summary>
        /// <param name="key">Name of the data value</param>
        /// <returns>Returns an object from the data table</returns>
        public object GetValue(string key)
        {
            Validation.IsNotNullOrEmpty(key, "key");

            if (Data.IsNull() || !Data.ContainsKey(key)) return null;
            return Data[key];
        }

        /// <summary>
        /// Gets whether or not the value exists in the data table
        /// </summary>
        /// <param name="key">Name of the data value</param>
        /// <returns>Returns a flag indicating whether or not the data value exists</returns>
        public bool HasValue(string key)
        {
            return Data.IsNotNull() && Data.ContainsKey(key);
        }
    }
}