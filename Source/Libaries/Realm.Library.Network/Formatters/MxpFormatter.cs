using Realm.Library.Network.Mxp;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Realm.Library.Network.Formatters
{
    public class MxpFormatter : IFormatter
    {
        public string Format(string value)
        {
            var isInTag = false;
            var isInEntity = false;
            var sb = new StringBuilder();

            foreach (var c in value)
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
                        sb.Append(MxpCharToStringFormatTable.ContainsKey(c)
                            ? MxpCharToStringFormatTable[c]
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
            var buffer = new byte[6];
            buffer[0] = (byte)MxpExtensions.IAC;           //// Command
            buffer[1] = (byte)MxpExtensions.SB;            //// Subnegotiation Start
            buffer[2] = (byte)MxpExtensions.TELOPT_MXP;    //// Passed in telnet option
            buffer[3] = (byte)MxpExtensions.IAC;
            buffer[4] = (byte)MxpExtensions.SE;
            buffer[5] = (byte)'\0';
            stream.Write(buffer, 0, buffer.Length);

            //// MXPMODE \x1B[6z
            var encoder = new ASCIIEncoding();
            var byteBuffer = encoder.GetBytes(MxpExtensions.ESC + "[6z\0");
            stream.Write(byteBuffer, 0, byteBuffer.Length);
            stream.Flush();
        }
    }
}
