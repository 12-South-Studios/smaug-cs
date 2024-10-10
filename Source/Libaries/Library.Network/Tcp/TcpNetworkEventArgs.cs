namespace Library.Network.Tcp;

public class TcpNetworkEventArgs : NetworkEventArgs
{
    public TcpSocketStatus SocketStatus { get; set; }
}