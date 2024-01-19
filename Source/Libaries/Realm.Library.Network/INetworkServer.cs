using System;

namespace Realm.Library.Network
{
    public interface INetworkServer
    {
        event EventHandler<NetworkEventArgs> OnNetworkMessageReceived;
        event EventHandler<NetworkEventArgs> OnServerStatusChanged;
        ServerStatus Status { get; }
        bool HasConnectedUsers { get; }

        void Startup();
        void Shutdown();
    }
}