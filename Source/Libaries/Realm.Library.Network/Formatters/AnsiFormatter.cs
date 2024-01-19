using System.Collections.Generic;

namespace Realm.Library.Network.Formatters
{
    public class AnsiFormatter : TextFormatter
    {
        private static readonly Dictionary<string, string> _colorTable = new()
        {
            // Foreground Colors
            { "&A", "\033[5m" },
            { "&a", "\033[25m" },
            { "&k", "\033[30m" },
            { "&b", "\033[34m" },
            { "&g", "\033[32m" },
            { "&c", "\033[36m" },
            { "&r", "\033[31m" },
            { "&p", "\033[35m" },
            { "&y", "\033[33m" },
            { "&w", "\033[37m" },
            { "&d", "\033[1;30m" },
            { "&B", "\033[1;34m" },
            { "&G", "\033[1;32m" },
            { "&C", "\033[1;36m" },
            { "&R", "\033[1;31m" },
            { "&P", "\033[1;35m" },
            { "&Y", "\033[1;33m" },
            { "&W", "\033[1;37m" },
            { "&x", "\033[0m" },

            // Background colours
            { "^A", "\033[5m" },
            { "^a", "\033[25m" },
            { "^k", "\033[40m" },
            { "^b", "\033[44m" },
            { "^g", "\033[42m" },
            { "^c", "\033[46m" },
            { "^r", "\033[41m" },
            { "^p", "\033[45m" },
            { "^y", "\033[43m" },
            { "^w", "\033[47m" },
            { "^d", "\033[1;40m" },
            { "^B", "\033[1;44m" },
            { "^G", "\033[1;42m" },
            { "^C", "\033[1;46m" },
            { "^R", "\033[1;41m" },
            { "^P", "\033[1;45m" },
            { "^Y", "\033[1;43m" },
            { "^W", "\033[1;47m" },
            { "^x", "\033[0m" },
        };

        public override string Format(string value)
        {
            var parsed = value;
            foreach (var color in _colorTable)
            {
                parsed = parsed.Replace(color.Key, color.Value);
            }
            return parsed;
        }
    }
}
