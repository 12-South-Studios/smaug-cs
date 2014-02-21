using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using Realm.Library.Common;

namespace Realm.Library.Network.Mxp
{
    /// <summary>
    ///
    /// </summary>
    public static class MxpExtensions
    {
#pragma warning disable 1591
        public const char SE = '\xF0';
        public const char SB = '\xFA';
        public const char WILL = '\xFB';
        public const char WONT = '\xFC';
        public const char DO = '\xFD';
        public const char DONT = '\xFE';
        public const char IAC = '\xFF';
        public const char TELOPT_MXP = '\x5B';
        public const char GA = '\xF9';
        public const char ESC = '\x1B';
#pragma warning restore 1591

        /// <summary>
        /// Converts the string into a Mxp Tag
        /// </summary>
        public static string MxpTag(this string input, params object[] parameters)
        {
            return parameters.IsNull()
                ? (MxpBeg() + input + MxpEnd())
                : (MxpBeg() + string.Format(input, parameters) + MxpEnd());
        }

        /// <summary>
        /// Gets an ampersand in Mxp format
        /// </summary>
        public static string MxpAmp()
        {
            return "\x06";
        }

        /// <summary>
        /// Gets the beginning of a mxp tag
        /// </summary>
        public static string MxpBeg()
        {
            return "\x03";
        }

        /// <summary>
        /// Gets the end of a mxp tag
        /// </summary>
        public static string MxpEnd()
        {
            return "\x04";
        }

        /// <summary>
        /// Gets the enable mxp mode string
        /// </summary>
        public static string MxpMode(this int arg)
        {
            return String.Format("{0}[{1}z", ESC, arg);
        }

        /// <summary>
        /// Sends mxp negotiation to the given stream
        /// </summary>
        [ExcludeFromCodeCoverage]
        public static void SendMxpNegotiation(this Stream clientStream)
        {
            Validation.IsNotNull(clientStream, "clientStream");
            Validation.Validate(clientStream.CanWrite);

            var buffer = new byte[3];
            buffer[0] = (byte)IAC;
            buffer[1] = (byte)WILL;
            buffer[2] = (byte)TELOPT_MXP;
            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
        }

        /// <summary>
        /// Sends the enable mxp negotitation to the user
        /// </summary>
        /// <param name="tcpUser"></param>
        /// <param name="clientStream"></param>
        [ExcludeFromCodeCoverage]
        public static void EnableMxp(this ITcpUser tcpUser, Stream clientStream)
        {
            Validation.IsNotNull(tcpUser, "tcpUser");
            Validation.IsNotNull(clientStream, "clientStream");
            Validation.Validate(clientStream.CanWrite);

            ////IAC, SB, TELOPT_MXP, IAC, SE
            var buffer = new byte[6];
            buffer[0] = (byte)IAC;           //// Command
            buffer[1] = (byte)SB;            //// Subnegotiation Start
            buffer[2] = (byte)TELOPT_MXP;    //// Passed in telnet option
            buffer[3] = (byte)IAC;
            buffer[4] = (byte)SE;
            buffer[5] = (byte)'\0';
            clientStream.Write(buffer, 0, buffer.Length);

            //// MXPMODE \x1B[6z
            var encoder = new ASCIIEncoding();
            var byteBuffer = encoder.GetBytes(ESC + "[6z\0");
            clientStream.Write(byteBuffer, 0, byteBuffer.Length);
            clientStream.Flush();
        }
    }
}