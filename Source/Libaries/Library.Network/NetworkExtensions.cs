using System;
using System.Net;

namespace Library.Network;

public static class NetworkExtensions
{
  private const string LOCALHOST = "127.0.0.1";
  
  /// <summary>
  /// 
  /// </summary>
  /// <param name="ipAddress"></param>
  /// <returns></returns>
  public static IPAddress ConvertToIpAddress(this string ipAddress)
  {
    try
    {
      return IPAddress.Parse(ipAddress.Equals("localhost", StringComparison.OrdinalIgnoreCase)
        ? LOCALHOST
        : ipAddress);
    }
    catch (FormatException)
    {
      return IPAddress.Parse(LOCALHOST);
    }
  }
}