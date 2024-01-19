using System;
using System.Net.Sockets;

namespace Realm.Library.Network
{
    public interface INetworkUser : IClient
    {
        void OnConnect();
        void Disconnect(SocketError socketError);

        string Id { get; }

        event EventHandler<NetworkEventArgs> OnNetworkMessageReceived;
        void NotifyNetworkMessageReceived(object sender, NetworkEventArgs args);

        DateTime LastMessage { get; }
    }
}
