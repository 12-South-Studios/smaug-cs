using System;

namespace Realm.Library.Network
{
    /// <summary>
    /// EventArgs class for event handling in the Network library
    /// </summary>
    public class NetworkEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the status of the Tcp Socket in the event
        /// </summary>
        public TcpSocketStatus SocketStatus { get; set; }

        /// <summary>
        /// Gets the message associated with the event
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets the status of the Tcp Server in the event
        /// </summary>
        public TcpServerStatus ServerStatus { get; set; }
    }
}