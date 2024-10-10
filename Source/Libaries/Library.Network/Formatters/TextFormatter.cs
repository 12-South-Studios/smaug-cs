using System.IO;

namespace Library.Network.Formatters;

public class TextFormatter : IFormatter
{
    public virtual string Format(string value) => value;

    public void Enable(INetworkUser user, Stream stream) { }
}