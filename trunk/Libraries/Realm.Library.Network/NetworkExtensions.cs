using System;
using System.Net;
using Realm.Library.Common;
using Realm.Library.Network.Properties;

namespace Realm.Library.Network
{
    /// <summary>
    /// Defines extensions to various network functions and objects
    /// </summary>
    public static class NetworkExtensions
    {
        /// <summary>
        /// Converts the given string to an actual Ip Address structure.
        /// If the IP isn't valid return "localhost" as the default.
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static IPAddress ConvertToIpAddress(this string ipAddress)
        {
            Validation.IsNotNullOrEmpty(ipAddress, "ipAddress");

            try
            {
                return IPAddress.Parse(ipAddress.Equals("localhost", StringComparison.OrdinalIgnoreCase)
                    ? Resources.LOCALHOST_IP : ipAddress);
            }
            catch (FormatException)
            {
                return IPAddress.Parse(Resources.LOCALHOST_IP);
            }
        }
    }
}