using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Text;

namespace Library.Network.Core;

public class NetPacket
{
  public List<byte> Buffer { get; set; }

  public NetPacket()
  {
    Buffer = [];
  }

  public NetPacket(byte[] data)
  {
    Buffer = [..data];
  }

  public void Write(byte n)
  {
    Buffer.Add(n); // len = 1
  }

  public void Write(short n)
  {
    Buffer.AddRange(BitConverter.GetBytes(n)); // len = 2
  }

  public void Write(bool n)
  {
    Buffer.Add(BitConverter.GetBytes(n)[0]); // len = 1
  }

  public void Write(int n)
  {
    byte[] bytes = BitConverter.GetBytes(n); // len = 4
    Buffer.AddRange(bytes);
  }

  public void Write(float n)
  {
    byte[] bytes = BitConverter.GetBytes(n); // len = 4
    Buffer.AddRange(bytes);
  }

  public void Write(double n)
  {
    byte[] bytes = BitConverter.GetBytes(n); // len = 8
    Buffer.AddRange(bytes);
  }

  public void Write(long n)
  {
    byte[] bytes = BitConverter.GetBytes(n); // len = 8
    Buffer.AddRange(bytes);
  }

  public void Write(string str)
  {
    byte[] buffer = Compress(str);
    Write(buffer.Length); // Write size before appending str
    Buffer.AddRange(buffer);
  }

  public static byte[] Compress(string s)
  {
    byte[] bytes = Encoding.Unicode.GetBytes(s);
    using MemoryStream msi = new(bytes);
    using MemoryStream mso = new();
    using (GZipStream gs = new(mso, CompressionMode.Compress))
    {
      msi.CopyTo(gs);
    }

    return mso.ToArray();
  }
}