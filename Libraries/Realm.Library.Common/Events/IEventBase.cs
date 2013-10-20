// ReSharper disable CheckNamespace
namespace Realm.Library.Common
// ReSharper restore CheckNamespace
{
    /// <summary>
    ///
    /// </summary>
    public interface IEventBase
    {
        /// <summary>
        /// The name of the event
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The object that sent this event
        /// </summary>
        object Sender { get; set; }

        /// <summary>
        /// The argument package for the event
        /// </summary>
        RealmEventArgs Args { get; set; }
    }
}