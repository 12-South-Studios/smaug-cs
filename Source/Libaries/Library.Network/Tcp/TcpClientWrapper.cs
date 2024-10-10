﻿using Library.Common.Extensions;
using Library.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Library.Common.Logging;

namespace Library.Network.Tcp;

public abstract class TcpClientWrapper : IClient
{
  [ExcludeFromCodeCoverage]
  protected TcpClientWrapper(ILogWrapper log, TcpClient tcpClient, IEnumerable<IFormatter> formatters)
  {
    Log = log;
    TcpClient = tcpClient;
    Formatters = formatters;

    if (tcpClient.Client.RemoteEndPoint is IPEndPoint ip)
      IpAddress = ip.Address.ToString();

    ClientStream = tcpClient.GetStream();
    ConnectedOn = DateTime.Now;
  }

  protected ILogWrapper Log { get; }
  protected TcpClient TcpClient { get; }
  protected IEnumerable<IFormatter> Formatters { get; }
  public string IpAddress { get; protected set; }
  public DateTime ConnectedOn { get; protected set; }
  public NetworkStream ClientStream { get; protected set; }

  [ExcludeFromCodeCoverage]
  public async Task Write(string msg)
  {
    if (TcpClient == null)
      return;

    ASCIIEncoding encoder = new();
    NetworkStream clientStream = TcpClient.GetStream();

    try
    {
      foreach (string formattedString in Formatters.Select(formatter => formatter.Format(msg)))
      {
        await clientStream.WriteAsync(encoder.GetBytes(formattedString).AsMemory(0, formattedString.Length));
      }

      clientStream.Flush();
    }
    catch (ArgumentNullException ex)
    {
      ex.Handle(ExceptionHandlingOptions.RecordAndThrow, Log);
    }
    catch (ArgumentOutOfRangeException ex)
    {
      ex.Handle(ExceptionHandlingOptions.RecordAndThrow, Log);
    }
    catch (IOException ex)
    {
      ex.Handle(ExceptionHandlingOptions.RecordAndThrow, Log);
    }
    catch (ObjectDisposedException ex)
    {
      ex.Handle(ExceptionHandlingOptions.RecordAndThrow, Log);
    }
    catch (InvalidOperationException ex)
    {
      ex.Handle(ExceptionHandlingOptions.RecordAndThrow, Log);
    }
  }
}