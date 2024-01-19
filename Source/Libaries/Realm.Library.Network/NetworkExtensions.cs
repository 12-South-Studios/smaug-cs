using System;
using System.Net;

namespace Realm.Library.Network
{
    public static class NetworkExtensions
    {
        public static IPAddress ConvertToIpAddress(this string ipAddress)
        {
            try
            {
                return IPAddress.Parse(ipAddress.Equals("localhost", StringComparison.OrdinalIgnoreCase)
                    ? "127.0.0.1" : ipAddress);
            }
            catch (FormatException)
            {
                return IPAddress.Parse("127.0.0.1");
            }
        }
    }
}