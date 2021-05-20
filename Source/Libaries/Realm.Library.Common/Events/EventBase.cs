
namespace Realm.Library.Common.Events

{
    /// <summary>
    /// Abstract class that defines the
    /// </summary>
    public abstract class EventBase : IEventBase
    {
        /// <summary>
        /// The name of the event
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// The object that sent this event
        /// </summary>
        public object Sender { get; set; }

        /// <summary>
        /// The function to call when the event is triggered
        /// </summary>
        public EventCallback<RealmEventArgs> Callback { get; set; }

        /// <summary>
        /// The argument package for the event
        /// </summary>
        public RealmEventArgs Args { get; set; }
    }
}