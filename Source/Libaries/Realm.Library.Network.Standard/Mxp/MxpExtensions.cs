using System.Diagnostics.CodeAnalysis;
using System.IO;
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
            return parameters == null
                ? MxpBeg() + input + MxpEnd()
                : MxpBeg() + string.Format(input, parameters) + MxpEnd();
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
            return $"{ESC}[{arg}z";
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
    }
}