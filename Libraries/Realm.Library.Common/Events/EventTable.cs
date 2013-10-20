using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

// ReSharper disable CheckNamespace
namespace Realm.Library.Common
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// The event table encapsulates a dictionary for the Realm Events
    /// </summary>
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class EventTable : Dictionary<string, object>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public EventTable()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected EventTable(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}