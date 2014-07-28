using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data;

using SmaugCS.Logging;
using SmaugCS.Managers;

namespace SmaugCS
{
    public static class color
    {
        public static void show_colorthemes(CharacterInstance ch)
        {
            send_to_pager("&Ythe following themes are available:\r\n", ch);

            DirectoryProxy proxy = new DirectoryProxy();
            string path = SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Color);

            int count = 0;
            int col = 0;
            foreach (string file in proxy.GetFiles(path).ToList()
                                         .Where(x => !x.EqualsIgnoreCase("cvs") && !x.StartsWith(".")))
            {
                ++count;
                pager_printf(ch, "%s%-15.15s", color_str(ATTypes.AT_PLAIN, ch), file);
                if (++col % 6 == 0)
                    send_to_pager("\r\n", ch);
            }

            if (count == 0)
                send_to_pager("No themes defined yet.\r\n", ch);

            if (col % 6 != 0)
                send_to_pager("\r\n", ch);
        }

        public static void show_colors(CharacterInstance ch)
        {
            send_to_pager("&BSyntax: color [color type] [color] | default\r\n", ch);
            send_to_pager("&BSyntax: color _reset_ (Resets all colors to default set)\r\n", ch);
            send_to_pager("&BSyntax: color _all_ [color] (Sets all color types to [color])\r\n\r\n", ch);
            send_to_pager("&BSyntax: color theme [name] (Sets all color types to a defined theme)\r\n\r\n", ch);

            send_to_pager("&W********************************[ COLORS ]*********************************\r\n", ch);

            for (int count = 0; count < 16; ++count)
            {
                if ((count % 8) == 0 && count != 0)
                    send_to_pager("\r\n", ch);

                ATTypes atType = Realm.Library.Common.EnumerationExtensions.GetEnum<ATTypes>(count);
                pager_printf(ch, "%s%-10s", color_str(atType, ch), GameConstants.pc_displays[count]);
            }

            send_to_pager("\r\n\r\n&W******************************[ COLOR TYPES ]******************************\r\n",
                          ch);

            for (int count = 32; count < (int)ATTypes.MAX_COLORS; ++count)
            {
                if ((count % 8) == 0 && count != 32)
                    send_to_pager("\r\n", ch);

                ATTypes atType = Realm.Library.Common.EnumerationExtensions.GetEnum<ATTypes>(count);
                pager_printf(ch, "%s%-10s%s", color_str(atType, ch), GameConstants.pc_displays[count], AnsiCodes.Reset);
            }

            send_to_pager("\r\n\r\n", ch);
            send_to_pager("&YAvailable colors are:\r\n", ch);

            int numColors = 0;
            foreach (string color in LookupManager.Instance.GetLookups("ValidColors"))
            {
                if ((numColors % 8) == 0 && numColors != 0)
                    send_to_pager("\r\n", ch);

                pager_printf(ch, "%s%-10s", color_str(ATTypes.AT_PLAIN, ch), color);
                numColors++;
            }

            send_to_pager("\r\n", ch);
            show_colorthemes(ch);
        }

        public static void reset_colors(CharacterInstance ch)
        {
            if (ch.IsNpc())
            {
                // log_printf
                return;
            }

            string path = SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Color) + "default";
            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(path)))
            {
                List<string> lines = proxy.ReadIntoList();
                foreach (string line in lines.Where(l => !l.EqualsIgnoreCase("#colortheme")
                                                         && !l.StartsWithIgnoreCase("name") &&
                                                         !l.EqualsIgnoreCase("maxcolors")))
                {
                    Tuple<string, string> tuple = line.FirstArgument();
                    switch (tuple.Item1.ToLower())
                    {
                        case "colors":
                            string[] colors = tuple.Item2.Split(new[] { ' ' });
                            for (int i = 0; i < colors.Length; i++)
                            {
                                ch.Colors[Realm.Library.Common.EnumerationExtensions.GetEnum<ATTypes>(i)] = (char)colors[i].ToInt32();
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

            AnsiCodes code = Realm.Library.Common.EnumerationExtensions.GetEnum<AnsiCodes>((int)attype);
            return code.GetName();
        }

        public static string random_ansi(int type)
        {
            switch (type)
            {
                default:
                    return Realm.Library.Common.EnumerationExtensions.GetEnum<AnsiCodes>(SmaugRandom.Between(1, 15)).GetName();
                case 2:
                    AnsiCodes code = Realm.Library.Common.EnumerationExtensions.GetEnum<AnsiCodes>(SmaugRandom.Between(1, 15));
                    return code.MakeBlink();
                case 3:
                    return Realm.Library.Common.EnumerationExtensions.GetEnum<AnsiCodes>(SmaugRandom.Between(16, 31)).GetName();
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

            int len = 0;
            int vislen;
            string dst = string.Empty;

            char[] chars = src.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                switch (chars[i])
                {
                    case '&': // normal, foreground color
                    case '^': // background color
                    case '}': // blink foreground color
                        vislen = 0;
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
            int len = color_strlen(argument);
            int space = size - len;
            string buffer = string.Empty;
            const char c = ' ';

            switch (align)
            {
                case (int)TextAlignmentStyle.Right:
                    if (len >= size)
                        buffer = argument.PadLeft(len);
                    break;
                case (int)TextAlignmentStyle.Center:
                    buffer = string.Format("{0}{1}{2}",
                                           c.Repeat(space / 2), argument,
                                           c.Repeat((space / 2) * 2 == space ? space / 2 : (space / 2) + 1));
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

        public static void set_char_color(ATTypes attype, CharacterInstance ch)
        {
            if (ch == null || ch.Descriptor == null || ch.IsNpc())
                return;

            comm.write_to_buffer(ch.Descriptor, color_str(attype, ch), 0);
            ch.Descriptor.PageColor = ch.Colors.ContainsKey(attype) ? ch.Colors[attype] : (char)0;
        }

        public static void write_to_pager(DescriptorData d, string txt, int length)
        {
            int len = length <= 0 ? txt.Length : length;
            if (len == 0)
                return;

            if (string.IsNullOrEmpty(d.PageBuffer))
            {
                d.PageSize = Program.MAX_STRING_LENGTH;
                d.PageBuffer = new string('\0', d.PageSize);
            }
            if (string.IsNullOrEmpty(d.PagePoint))
            {
                d.PagePoint = d.PageBuffer;
                d.PageTop = 0;
                d.PageCommand = "";
            }
            if (d.PageTop == 0 && !d.fcommand)
            {
                char[] bufferArray = d.PageBuffer.ToCharArray();
                bufferArray[0] = '\r';
                bufferArray[1] = '\n';
                d.PageTop = 2;
                d.PageBuffer = bufferArray.ToString();
            }

            //int pagerOffset = d.PagePoint - d.PageBuffer;
            while (d.PageTop + len >= d.PageSize)
            {
                if (d.PageSize > Program.MAX_STRING_LENGTH * 16)
                {
                    LogManager.Instance.Bug("Pager overflow. Ignoring.\r\n");
                    d.PageTop = 0;
                    d.PagePoint = string.Empty;
                    d.PageBuffer = string.Empty;
                    d.PageSize = Program.MAX_STRING_LENGTH;
                    return;
                }

                d.PageSize *= 2;
                // recreate?
            }

            // TODO finish this
        }

        public static void set_pager_color(ATTypes attype, CharacterInstance ch)
        {
            if (ch == null || ch.Descriptor == null || ch.IsNpc())
                return;

            write_to_pager(ch.Descriptor, color_str(attype, ch), 0);
            ch.Descriptor.PageColor = ch.Colors.ContainsKey(attype) ? ch.Colors[attype] : (char)0;
        }

        public static void send_to_desc_color(string txt, DescriptorData d)
        {
            if (d == null || string.IsNullOrEmpty(txt))
                return;

            comm.write_to_buffer(d, colorize(txt, d), 0);
        }

        public static void send_to_char(string txt, CharacterInstance ch)
        {
            if (!string.IsNullOrEmpty(txt) && ch.Descriptor != null)
                send_to_desc_color(txt, ch.Descriptor);
        }

        public static void send_to_pager(string txt, CharacterInstance ch)
        {
            if (string.IsNullOrEmpty(txt) || ch.Descriptor == null)
                return;

            CharacterInstance och = ch.Descriptor.Original ?? ch.Descriptor.Character;
            if (och.IsNpc() || !och.PlayerData.Flags.IsSet((int)PCFlags.PagerOn))
                send_to_desc_color(txt, ch.Descriptor);
            else
                write_to_pager(ch.Descriptor, colorize(txt, ch.Descriptor), 0);
        }

        public static void ch_printf(CharacterInstance ch, string fmt, params object[] args)
        {
            send_to_char(string.Format(fmt, args), ch);
        }

        public static void pager_printf(CharacterInstance ch, string fmt, params object[] args)
        {
            send_to_pager(string.Format(fmt, args), ch);
        }

        public static void ch_printf_color(CharacterInstance ch, string fmt, params object[] args)
        {
            send_to_char(string.Format(fmt, args), ch);
        }

        public static void pager_printf_color(CharacterInstance ch, string fmt, params object[] args)
        {
            send_to_pager(string.Format(fmt, args), ch);
        }

        public static void paint(ATTypes attype, CharacterInstance ch, string fmt, params object[] args)
        {
            set_char_color(attype, ch);
            send_to_char(string.Format(fmt, args), ch);
            set_char_color(attype, ch);
        }

        public static void send_to_char_color(string txt, CharacterInstance ch)
        {
            send_to_char(txt, ch);
        }

        public static void send_to_pager_color(string txt, CharacterInstance ch)
        {
            send_to_pager(txt, ch);
        }

        private static readonly List<string> color_list = new List<string>()
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

        private static readonly List<string> blink_list = new List<string>()
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
            Tuple<string, string> tuple = argument.FirstArgument();
            string color = tuple.Item1;

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
