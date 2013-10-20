using System;
using System.Text;

// ReSharper disable CheckNamespace
namespace Realm.Library.Common
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// A listener class that defines what is being listened to, for and who is doing the listening
    /// </summary>
    public sealed class EventListener
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="listenTo"></param>
        /// <param name="eventType"></param>
        /// <param name="callback"></param>
        public EventListener(object listener, object listenTo, Type eventType, EventCallback<RealmEventArgs> callback)
        {
            Listener = listener;
            ListenTo = listenTo;
            EventType = eventType;
            CallbackFunction = callback;
        }

        /// <summary>
        /// Who is the doing the listening
        /// </summary>
        public object Listener { get; private set; }

        /// <summary>
        /// Who is being listened to
        /// </summary>
        public object ListenTo { get; private set; }

        /// <summary>
        /// Type of event to listen for
        /// </summary>
        public Type EventType { get; private set; }

        /// <summary>
        /// Function to call when the event is triggered
        /// </summary>
        public EventCallback<RealmEventArgs> CallbackFunction { get; private set; }

        /// <summary>
        /// Override of the ToString function
        /// </summary>
        /// <returns></returns>
        public new string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Listener {0}, ", Listener);
            sb.AppendFormat("ListenTo {0}, ", ListenTo);
            sb.AppendFormat("EventType {0}, ", EventType);
            sb.AppendFormat("CallbackFunction {0}", CallbackFunction);
            return sb.ToString();
        }
    }
}