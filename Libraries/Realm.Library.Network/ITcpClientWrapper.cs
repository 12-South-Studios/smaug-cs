using System;

namespace Realm.Library.Network
{
    /// <summary>
    /// Defines the contract for a TcpClient wrapper
    /// </summary>
    public interface ITcpClientWrapper
    {
        /// <summary>
        /// Gets the IpAddress of the connected user
        /// </summary>
        string IpAddress { get; }

        /// <summary>
        /// Gets the datetime when a connection occurred
        /// </summary>
        DateTime ConnectedOn { get; }

        /// <summary>
        /// Writes a given message to the buffer which will be queued to send to the client stream.
        /// </summary>
        void WriteToBuffer(string msg);
    }
}