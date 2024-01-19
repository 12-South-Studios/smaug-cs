using System.IO;

namespace Realm.Library.Network
{
    public interface IFormatter
    {
        string Format(string source);
        void Enable(INetworkUser user, Stream stream);
    }
}