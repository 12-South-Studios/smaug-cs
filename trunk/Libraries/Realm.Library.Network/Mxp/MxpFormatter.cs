using System.Text;
using Realm.Library.Common;

namespace Realm.Library.Network.Mxp
{
    /// <summary>
    /// Handles the formatting of string data into a MXP compatible string
    /// </summary>
    public class MxpFormatter : IFormatter
    {
        /// <summary>
        /// MxpFormatter takes the input string and formats it for Mxp-compatibility
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Format(string value)
        {
            Validation.IsNotNullOrEmpty(value, "value");

            var bInTag = false;
            var bInEntity = false;
            var sb = new StringBuilder();

            foreach (var c in value)
            {
                if (bInTag)
                {
                    sb.Append((c == '\x04') ? '>' : c);
                    if (c == '\x04')
                        bInTag = false;
                }

                else if (bInEntity)
                {
                    sb.Append(c);
                    if (c == ';')
                        bInEntity = false;
                }
                else
                {
                    switch (c)
                    {
                        case '\x03':
                            bInTag = true;
                            sb.Append('<');
                            break;

                        case '\x04':
                            sb.Append('>');
                            break;

                        case '\x05':
                            bInEntity = true;
                            sb.Append('&');
                            break;

                        default:
                            switch (c)
                            {
                                case '<':
                                    sb.Append("&lt;");
                                    break;

                                case '>':
                                    sb.Append("&gt;");
                                    break;

                                case '&':
                                    sb.Append("&amp;");
                                    break;

                                case '"':
                                    sb.Append("&quot;");
                                    break;

                                default:
                                    sb.Append(c);
                                    break;
                            }
                            break;
                    }
                }
            }

            return sb.ToString();
        }
    }
}