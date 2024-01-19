using System;
using System.Threading.Tasks;

namespace Realm.Library.Network
{
    public interface IClient
    {
        string IpAddress { get; }
        DateTime ConnectedOn { get; }

        Task Write(string msg);
    }
}
