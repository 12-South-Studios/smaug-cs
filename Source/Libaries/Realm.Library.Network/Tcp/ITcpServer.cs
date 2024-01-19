using System;

namespace Realm.Library.Network.Tcp
{
    public interface ITcpServer
    {
        event EventHandler<TcpNetworkEventArgs> OnTcpUserStatusChanged;

        void Shutdown(string message);
        bool DisconnectUser(string id);
    }
}