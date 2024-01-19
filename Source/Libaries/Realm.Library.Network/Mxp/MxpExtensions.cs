using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Realm.Library.Network.Mxp
{
    public static class MxpExtensions
    {
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

        public static string MxpTag(this string input, params object[] parameters)
          => parameters == null
             ? MxpBeg() + input + MxpEnd()
             : MxpBeg() + string.Format(input, parameters) + MxpEnd();

        public static string MxpAmp() => "\x06";
        public static string MxpBeg() => "\x03";
        public static string MxpEnd() => "\x04";
        public static string MxpMode(this int arg) => $"{ESC}[{arg}z";

        [ExcludeFromCodeCoverage]
        public static void SendMxpNegotiation(this Stream clientStream)
        {
            var buffer = new byte[3];
            buffer[0] = (byte)IAC;
            buffer[1] = (byte)WILL;
            buffer[2] = (byte)TELOPT_MXP;
            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
        }
    }
}