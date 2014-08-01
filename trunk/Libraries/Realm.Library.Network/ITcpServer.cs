using System;
using System.Net;

namespace Realm.Library.Network
{
    public interface ITcpServer
    {
        event EventHandler<NetworkEventArgs> OnTcpUserStatusChanged;
        event EventHandler<NetworkEventArgs> OnTcpServerStatusChanged;
        event EventHandler<NetworkEventArgs> OnNetworkMessageReceived;

        TcpServerStatus Status { get; }
        IPAddress Host { get; }
        int Port { get; }

        ITcpUser GetTcpUser(string ipAddress);
        void Startup(int port, IPAddress host);
        void Shutdown(string message);
        bool DisconnectUser(string id);
    }
}