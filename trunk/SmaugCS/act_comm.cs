using System;
using System.IO;
using System.Linq;
using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using SmaugCS.Constants;
using SmaugCS.Enums;
using SmaugCS.Common;
using SmaugCS.Language;
using SmaugCS.Managers;
using SmaugCS.Objects;

namespace SmaugCS
{
    public static class act_comm
    {
        public static string TranslateLanguage(int percent, string text, string languageName)
        {
            if (percent > 99 || !languageName.Equals("common", StringComparison.OrdinalIgnoreCase))
                return text;

            LanguageData lng = db.GetLanguage(languageName);
            if (lng == null)
            {
                lng = db.GetLanguage("default");
                if (lng == null)
                    return text;
            }

            return lng.Translate(percent, text);
        }

        public static void send_rip_screen(CharacterInstance ch)
        {
            string path = SystemConstants.GetSystemFile(SystemFileTypes.RIPScreen);
            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(path)))
            {
                string buffer = proxy.ReadToEnd();
                comm.write_to_buffer(ch.Descriptor, buffer, buffer.Length);
            }
        }

        public static void send_rip_title(CharacterInstance ch)
        {
            string path = SystemConstants.GetSystemFile(SystemFileTypes.RIPTitle);
            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(path)))
            {
                string buffer = proxy.ReadToEnd();
                comm.write_to_buffer(ch.Descriptor, buffer, buffer.Length);
            }
        }

        public static void send_ansi_title(CharacterInstance ch)
        {
            string path = SystemConstants.GetSystemFile(SystemFileTypes.ANSITitle);
            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(path)))
            {
                string buffer = proxy.ReadToEnd();
                comm.write_to_buffer(ch.Descriptor, buffer, buffer.Length);
            }
        }

        public static void send_ascii_title(CharacterInstance ch)
        {
            string path = SystemConstants.GetSystemFile(SystemFileTypes.ASCTitle);
            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(path)))
            {
                string buffer = proxy.ReadToEnd();
                comm.write_to_buffer(ch.Descriptor, buffer, buffer.Length);
            }
        }
    }
}
