using System.IO;
using Realm.Library.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS
{
    public static class act_comm
    {
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
