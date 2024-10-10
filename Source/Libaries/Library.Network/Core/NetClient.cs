using System.Net.Sockets;
using System.Net;

namespace Library.Network.Core;

public class NetClient
{
  private readonly UdpClient _clientUdp;

  public delegate void Receive(NetPacket packet);

  public event Receive OnReceive;
  private readonly IPAddress _address;
  private readonly int _port;

  private bool _stopListening;

  public UdpClient GetUdpClient() => _clientUdp;

  public NetClient(string address, int port)
  {
    _clientUdp = new UdpClient();
    _clientUdp.Connect(address, port);
    _address = IPAddress.Parse(address);
    _port = port;
  }

  /// <summary>
  /// Start receiving packets
  /// </summary>
  public void Listen()
  {
    while (!_stopListening)
    {
      IPEndPoint endpoint = new(_address, _port);
      byte[] data = _clientUdp.Receive(ref endpoint);
      OnReceive?.Invoke(new NetPacket(data));
    }
  }

  /// <summary>
  /// Send a packet to the specified endpoint
  /// </summary>
  public void Send(NetPacket packet)
  {
    byte[] data = packet.Buffer.ToArray();
    _clientUdp.Send(data, data.Length);
  }

  public void Stop()
  {
    _stopListening = true;
  }
}