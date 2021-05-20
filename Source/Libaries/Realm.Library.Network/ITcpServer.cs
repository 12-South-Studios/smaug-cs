using System;
using System.Net;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <param name="host"></param>
        void Startup(int port, IPAddress host);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        void Shutdown(string message);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DisconnectUser(string id);
    }
}