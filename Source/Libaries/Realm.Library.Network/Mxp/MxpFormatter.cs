using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Realm.Library.Common;

namespace Realm.Library.Network.Mxp
{
    /// <summary>
    /// Handles the formatting of string data into a MXP compatible string
    /// </summary>
    public class MxpFormatter : IFormatter
    {
        /// <summary>
        /// MxpFormatter takes the input string and formats it for Mxp-compatibility
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Format(string value)
        {
            Validation.IsNotNullOrEmpty(value, "value");

            var bInTag = false;
            var bInEntity = false;
            var sb = new StringBuilder();

            foreach (var c in value)
            {
                if (bInTag)
                {
                    sb.Append(c == '\x04' ? '>' : c);
                    if (c == '\x04')
                        bInTag = false;
                }

                else if (bInEntity)
                {
                    sb.Append(c);
                    if (c == ';')
                        bInEntity = false;
                }
                else
                {
                    switch (c)
                    {
                        case '\x03':
                            bInTag = true;
                            sb.Append('<');
                            break;

                        case '\x04':
                            sb.Append('>');
                            break;

                        case '\x05':
                            bInEntity = true;
                            sb.Append('&');
                            break;

                        default:
                            sb.Append(MxpCharToStringFormatTable.ContainsKey(c)
                                ? MxpCharToStringFormatTable[c]
                                : c.ToString());
                            break;
                    }
                }
            }

            return sb.ToString();
        }

        private static readonly Dictionary<char, string> MxpCharToStringFormatTable = new Dictionary<char, string>
        {
            {'<', "&lt;"},
            {'>', "&gt;"},
            {'&', "&amp;"},
            {'"', "&quot;"}
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tcpUser"></param>
        /// <param name="clientStream"></param>
        public void Enable(ITcpUser tcpUser, NetworkStream clientStream)
        {
            Validation.IsNotNull(tcpUser, "tcpUser");
            Validation.IsNotNull(clientStream, "clientStream");
            Validation.Validate(clientStream.CanWrite);

            ////IAC, SB, TELOPT_MXP, IAC, SE
            var buffer = new byte[6];
            buffer[0] = (byte)MxpExtensions.IAC;           //// Command
            buffer[1] = (byte)MxpExtensions.SB;            //// Subnegotiation Start
            buffer[2] = (byte)MxpExtensions.TELOPT_MXP;    //// Passed in telnet option
            buffer[3] = (byte)MxpExtensions.IAC;
            buffer[4] = (byte)MxpExtensions.SE;
            buffer[5] = (byte)'\0';
            clientStream.Write(buffer, 0, buffer.Length);

            //// MXPMODE \x1B[6z
            var encoder = new ASCIIEncoding();
            var byteBuffer = encoder.GetBytes(MxpExtensions.ESC + "[6z\0");
            clientStream.Write(byteBuffer, 0, byteBuffer.Length);
            clientStream.Flush();
        }
    }
}