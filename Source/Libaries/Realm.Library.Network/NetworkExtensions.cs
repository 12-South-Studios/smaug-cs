﻿using System;
using System.Net;

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
            if (string.IsNullOrEmpty(ipAddress)) throw new ArgumentNullException();

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