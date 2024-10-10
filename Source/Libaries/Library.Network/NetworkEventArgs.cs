using System;

namespace Library.Network;

public class NetworkEventArgs : EventArgs
{
    public string Message { get; set; }
    public ServerStatus ServerStatus { get; set; }
}