using System.Net.Sockets;
using System.Net;

namespace Library.Network.Core;

public class NetServer(int port)
{
  public delegate void Receive(IPEndPoint address, NetPacket packet);
  public event Receive OnReceive;

  private readonly UdpClient _serverUdp = new(port);
  private IPEndPoint _endpoint = new(IPAddress.Any, port);
  private bool _stopListening;

  public UdpClient GetUdpClient() => _serverUdp;

  /// <summary>
  /// Start receiving packets
  /// </summary>
  public void Listen()
  {
    while (!_stopListening)
    {
      byte[] data = _serverUdp.Receive(ref _endpoint);
      OnReceive?.Invoke(_endpoint, new NetPacket(data));
    }
  }

  /// <summary>
  /// Send a packet to the specified endpoint
  /// </summary>
  public void Send(IPEndPoint endpoint, NetPacket packet)
  {
    byte[] data = packet.Buffer.ToArray();
    _serverUdp.Send(data, data.Length, endpoint);
  }

  /// <summary>
  /// Stop listening
  /// </summary>
  public void Stop()
  {
    _stopListening = true;
  }
}