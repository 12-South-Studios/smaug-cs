using System.Net.Sockets;
using Realm.Library.Common;

namespace Realm.Library.Network
{
    /// <summary>
    /// Handles the formatting of string data as text
    /// </summary>
    public abstract class TextFormatter : IFormatter
    {
        /// <summary>
        /// Text formatter does nothing at this time and simply returns the input string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual string Format(string value)
        {
            Validation.IsNotNullOrEmpty(value, "value");

            return value;
        }

        public void Enable(ITcpUser tcpUser, NetworkStream clientStream)
        {
            // do nothing
        }
    }
}