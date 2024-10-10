using System.Collections.Generic;
using System.IO;
using System.Text;
using Library.Network.Mxp;

namespace Library.Network.Formatters;

public class MxpFormatter : IFormatter
{
  public string Format(string value)
  {
    bool isInTag = false;
    bool isInEntity = false;
    StringBuilder sb = new();

    foreach (char c in value)
    {
      if (isInTag)
      {
        sb.Append(c == '\x04' ? '>' : c);
        if (c == '\x04')
          isInTag = false;
        continue;
      }

      if (isInEntity)
      {
        sb.Append(c);
        if (c == ';')
          isInEntity = false;
        continue;
      }

      switch (c)
      {
        case '\x03':
          isInTag = true;
          sb.Append('<');
          break;

        case '\x04':
          sb.Append('>');
          break;

        case '\x05':
          isInEntity = true;
          sb.Append('&');
          break;

        default:
          sb.Append(MxpCharToStringFormatTable.TryGetValue(c, out string val)
            ? val
            : c.ToString());
          break;
      }
    }

    return sb.ToString();
  }

  private static readonly Dictionary<char, string> MxpCharToStringFormatTable = new()
  {
    { '<', "&lt;" },
    { '>', "&gt;" },
    { '&', "&amp;" },
    { '"', "&quot;" }
  };

  public void Enable(INetworkUser user, Stream stream)
  {
    ////IAC, SB, TELOPT_MXP, IAC, SE
    byte[] buffer = new byte[6];
    buffer[0] = (byte)MxpExtensions.IAC; //// Command
    buffer[1] = (byte)MxpExtensions.SB; //// Subnegotiation Start
    buffer[2] = (byte)MxpExtensions.TELOPT_MXP; //// Passed in telnet option
    buffer[3] = (byte)MxpExtensions.IAC;
    buffer[4] = (byte)MxpExtensions.SE;
    buffer[5] = (byte)'\0';
    stream.Write(buffer, 0, buffer.Length);

    //// MXPMODE \x1B[6z
    ASCIIEncoding encoder = new();
    byte[] byteBuffer = encoder.GetBytes(MxpExtensions.ESC + "[6z\0");
    stream.Write(byteBuffer, 0, byteBuffer.Length);
    stream.Flush();
  }
}