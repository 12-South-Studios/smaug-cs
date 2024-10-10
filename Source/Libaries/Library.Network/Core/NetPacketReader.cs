using System;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;

namespace Library.Network.Core;

public class NetPacketReader(NetPacket packet)
{
  private int _currentIndex = 0;

  public void ResetRead() => _currentIndex = 0;

  public T Read<T>()
  {
    Type type = typeof(T);

    if (type == typeof(string))
    {
      int length = Read<int>();
      byte[] stringBytes = packet.Buffer.Skip(_currentIndex).Take(length).ToArray();
      string value = Decompress(stringBytes);
      _currentIndex += length;
      return (T)Convert.ChangeType(value, typeof(T));
    }

    if (type == typeof(int))
    {
      int value = BitConverter.ToInt32(packet.Buffer.ToArray(), _currentIndex);
      _currentIndex += 4;
      return (T)Convert.ChangeType(value, typeof(T));
    }

    if (type == typeof(short))
    {
      short value = BitConverter.ToInt16(packet.Buffer.ToArray(), _currentIndex);
      _currentIndex += 2;
      return (T)Convert.ChangeType(value, typeof(T));
    }

    if (type == typeof(byte))
    {
      short value = packet.Buffer[_currentIndex];
      _currentIndex += 1;
      return (T)Convert.ChangeType(value, typeof(T));
    }

    if (type == typeof(float))
    {
      float value = BitConverter.ToSingle(packet.Buffer.ToArray(), _currentIndex);
      _currentIndex += 4;
      return (T)Convert.ChangeType(value, typeof(T));
    }

    if (type == typeof(double))
    {
      double value = BitConverter.ToDouble(packet.Buffer.ToArray(), _currentIndex);
      _currentIndex += 8;
      return (T)Convert.ChangeType(value, typeof(T));
    }

    if (type == typeof(long))
    {
      long value = BitConverter.ToInt64(packet.Buffer.ToArray(), _currentIndex);
      _currentIndex += 8;
      return (T)Convert.ChangeType(value, typeof(T));
    }

    return (T)Convert.ChangeType(null, typeof(T));
  }

  public static string Decompress(byte[] bytes)
  {
    using MemoryStream msi = new(bytes);
    using MemoryStream mso = new();
    using (GZipStream gs = new(msi, CompressionMode.Decompress))
    {
      gs.CopyTo(mso);
    }

    return Encoding.Unicode.GetString(mso.ToArray());
  }
}