using System;

namespace Realm.Library.Network
{
    /// <summary>
    /// Defines the contract for a Tcp Server
    /// </summary>
    public interface ITcpServer
    {
        /// <summary>
        /// Event on a TcpUser status change
        /// </summary>
        event EventHandler<NetworkEventArgs> OnTcpUserStatusChanged;

        /// <summary>
        /// Event on a TcpServer status change
        /// </summary>
        event EventHandler<NetworkEventArgs> OnTcpServerStatusChanged;

        /// <summary>
        /// Event on a TcpServer status change
        /// </summary>
        event EventHandler<NetworkEventArgs> OnNetworkMessageReceived;

        /// <summary>
        ///
        /// </summary>
        TcpServerStatus Status { get; }
    }
}