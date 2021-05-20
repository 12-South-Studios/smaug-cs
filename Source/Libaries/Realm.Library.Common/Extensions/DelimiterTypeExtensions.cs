using System.ComponentModel;

namespace Realm.Library.Common.Extensions

{
    /// <summary>
    /// Extension class for the DelimiterType enum
    /// </summary>
    public static class DelimiterTypeExtensions
    {
        /// <summary>
        /// Gets the string value of the enum
        /// </summary>
        /// <param name="type">enum reference</param>
        /// <returns>Returns the value string</returns>
        public static string ValueOf(this DelimiterType type)
        {
            var fi = type.GetType().GetField(type.ToString());

            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : type.ToString();
        }
    }
}