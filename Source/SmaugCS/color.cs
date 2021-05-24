using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EnumerationExtensions = Realm.Library.Common.Extensions.EnumerationExtensions;

namespace SmaugCS
{
    public static class color
    {
        public static void show_colorthemes(CharacterInstance ch)
        {
            ch.SendToPager("&Ythe following themes are available:\r\n");

            var proxy = new DirectoryProxy();
            var path = SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Color);

            var count = 0;
            var col = 0;
            foreach (var file in proxy.GetFiles(path).ToList()
                                         .Where(x => !x.EqualsIgnoreCase("cvs") && !x.StartsWith(".")))
            {
                ++count;
                ch.PagerPrintf("%s%-15.15s", color_str(ATTypes.AT_PLAIN, ch), file);
                if (++col % 6 == 0)
                    ch.SendToPager("\r\n");
            }

            if (count == 0)
                ch.SendToPager("No themes defined yet.\r\n");

            if (col % 6 != 0)
                ch.SendToPager("\r\n");
        }

        public static void show_colors(CharacterInstance ch)
        {
            ch.SendToPager("&BSyntax: color [color type] [color] | default\r\n");
            ch.SendToPager("&BSyntax: color _reset_ (Resets all colors to default set)\r\n");
            ch.SendToPager("&BSyntax: color _all_ [color] (Sets all color types to [color])\r\n\r\n");
            ch.SendToPager("&BSyntax: color theme [name] (Sets all color types to a defined theme)\r\n\r\n");

            ch.SendToPager("&W********************************[ COLORS ]*********************************\r\n");

            for (var count = 0; count < 16; ++count)
            {
                if (count % 8 == 0 && count != 0)
                    ch.SendToPager("\r\n");

                var atType = EnumerationExtensions.GetEnum<ATTypes>(count);
                ch.PagerPrintf("%s%-10s", color_str(atType, ch), LookupConstants.pc_displays[count]);
            }

            ch.SendToPager("\r\n\r\n&W******************************[ COLOR TYPES ]******************************\r\n");

            // todo foreach through colors instead of using MAX_COLORS
            //for (var count = 32; count < (int)ATTypes.MAX_COLORS; ++count)
            //{
            //    if ((count % 8) == 0 && count != 32)
            //        ch.SendToPager("\r\n");

            //    var atType = EnumerationExtensions.GetEnum<ATTypes>(count);
            //    ch.PagerPrintf("%s%-10s%s", color_str(atType, ch), LookupConstants.pc_displays[count], AnsiCodes.Reset);
            //}

            ch.SendToPager("\r\n\r\n");
            ch.SendToPager("&YAvailable colors are:\r\n");

            var numColors = 0;
            foreach (var color in LookupManager.Instance.GetLookups("ValidColors"))
            {
                if (numColors % 8 == 0 && numColors != 0)
                    ch.SendToPager("\r\n");

                ch.PagerPrintf("%s%-10s", color_str(ATTypes.AT_PLAIN, ch), color);
                numColors++;
            }

            ch.SendToPager("\r\n");
            show_colorthemes(ch);
        }

        public static void reset_colors(PlayerInstance ch)
        {
            var path = SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Color) + "default";
            using (var proxy = new TextReaderProxy(new StreamReader(path)))
            {
                IEnumerable<string> lines = proxy.ReadIntoList();
                foreach (var line in lines.Where(l => !l.EqualsIgnoreCase("#colortheme")
                                                         && !l.StartsWithIgnoreCase("name") &&
                                                         !l.EqualsIgnoreCase("maxcolors")))
                {
                    var tuple = line.FirstArgument();
                    switch (tuple.Item1.ToLower())
                    {
                        case "colors":
                            var colors = tuple.Item2.Split(' ');
                            for (var i = 0; i < colors.Length; i++)
                            {
                                ch.Colors[EnumerationExtensions.GetEnum<ATTypes>(i)] = (char)colors[i].ToInt32();
                            }
                            break;
                        case "end":
                            return;
                    }
                }
            }
        }

        public static string color_str(ATTypes attype, CharacterInstance ch)
        {
            if (ch.IsNpc() || ch.Act.IsSet((int)PlayerFlags.Ansi))
                return string.Empty;

            var code = EnumerationExtensions.GetEnum<AnsiCodes>((int)attype);
            return code.GetName();
        }

        public static string random_ansi(int type)
        {
            switch (type)
            {
                default:
                    return EnumerationExtensions.GetEnum<AnsiCodes>(SmaugRandom.Between(1, 15)).GetName();
                case 2:
                    var code = EnumerationExtensions.GetEnum<AnsiCodes>(SmaugRandom.Between(1, 15));
                    return code.MakeBlink();
                case 3:
                    return EnumerationExtensions.GetEnum<AnsiCodes>(SmaugRandom.Between(16, 31)).GetName();
            }
        }

        public static int colorcode(string src, string dst, DescriptorData d, int dstlen, int vislen)
        {
            // TODO
            return 0;
        }

        /// <summary>
        /// Returns the intended screen length of a string which has color codes embedded in it. 
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static int color_strlen(string src)
        {
            if (string.IsNullOrEmpty(src))
                return 0;

            var len = 0;
            var dst = string.Empty;

            var chars = src.ToCharArray();
            for (var i = 0; i < chars.Length; i++)
            {
                switch (chars[i])
                {
                    case '&': // normal, foreground color
                    case '^': // background color
                    case '}': // blink foreground color
                        var vislen = 0;
                        i += colorcode(src, dst, null, 20, vislen);
                        len += vislen; // count output token length
                        break;
                    default:
                        ++len;
                        ++i;
                        break;
                }
            }
            return len;
        }

        public static string color_align(string argument, int size, int align)
        {
            var len = color_strlen(argument);
            var space = size - len;
            var buffer = string.Empty;
            const char c = ' ';

            switch (align)
            {
                case (int)TextAlignmentStyle.Right:
                    if (len >= size)
                        buffer = argument.PadLeft(len);
                    break;
                case (int)TextAlignmentStyle.Center:
                    buffer = $"{c.Repeat(space / 2)}{argument}{c.Repeat(space / 2 * 2 == space ? space / 2 : space / 2 + 1)}";
                    break;
                case (int)TextAlignmentStyle.Left:
                    buffer = argument.PadRight(space);
                    break;
            }

            return buffer;
        }

        /// <summary>
        /// Converts a string with color tokens into the desired output tokens 
        /// using the Character's desired preferences
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string colorize(string txt, DescriptorData d)
        {
            if (string.IsNullOrEmpty(txt) || d == null)
                return string.Empty;

            // TODO finish this
            return string.Empty;
        }

        public static void paint(ATTypes attype, CharacterInstance ch, string fmt, params object[] args)
        {
            ch.SetColor(attype);
            ch.SendTo(string.Format(fmt, args));
            ch.SetColor(attype);
        }

        private static readonly List<string> color_list = new List<string>
            {
                "_bla",
                "_red",
                "_dgr",
                "_bro",
                "_dbl",
                "_pur",
                "_cya",
                "_cha",
                "_dch",
                "_ora",
                "_gre",
                "_yel",
                "_blu",
                "_pin",
                "_lbl",
                "_whi"
            };

        private static readonly List<string> blink_list = new List<string>
            {
                "*bla",
                "*red",
                "*dgr",
                "*bro",
                "*dbl",
                "*pur",
                "*cya",
                "*cha",
                "*dch",
                "*ora",
                "*gre",
                "*yel",
                "*blu",
                "*pin",
                "*lbl",
                "*whi"
            };

        /// <summary>
        /// Examines a text string to determine if the first "word" is a color indicator
        /// (e.g. _red_, _whi_, _blu)
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        public static int get_color(string argument)
        {
            var tuple = argument.FirstArgument();
            var color = tuple.Item1;

            if (color[0] != '_' && color[0] != '*')
                return 0;
            if (color_list.Contains(color.ToLower()))
                return color_list.IndexOf(color.ToLower());
            if (blink_list.Contains(color.ToLower()))
                return blink_list.IndexOf(color.ToLower()) + (int)ATTypes.AT_BLINK;
            return 0;
        }
    }
}
