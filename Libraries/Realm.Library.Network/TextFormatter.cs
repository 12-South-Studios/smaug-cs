using Realm.Library.Common;

namespace Realm.Library.Network
{
    /// <summary>
    /// Handles the formatting of string data as text
    /// </summary>
    public class TextFormatter : IFormatter
    {
        /// <summary>
        /// Text formatter does nothing at this time and simply returns the input string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Format(string value)
        {
            Validation.IsNotNullOrEmpty(value, "value");

            return value;
        }
    }
}