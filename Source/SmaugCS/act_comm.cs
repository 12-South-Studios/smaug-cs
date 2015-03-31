using System.IO;
using Realm.Library.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS
{
    public static class act_comm
    {
        private static void SendFileToBuffer(CharacterInstance ch, SystemFileTypes fileType)
        {
            string path = SystemConstants.GetSystemFile(fileType);
            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(path)))
            {
                string buffer = proxy.ReadToEnd();
                comm.write_to_buffer(ch.Descriptor, buffer, buffer.Length);
            } 
        }

        public static void send_rip_screen(CharacterInstance ch)
        {
            SendFileToBuffer(ch, SystemFileTypes.RIPScreen);
        }

        public static void send_rip_title(CharacterInstance ch)
        {
            SendFileToBuffer(ch, SystemFileTypes.RIPTitle);
        }

        public static void send_ansi_title(CharacterInstance ch)
        {
            SendFileToBuffer(ch, SystemFileTypes.ANSITitle);
        }

        public static void send_ascii_title(CharacterInstance ch)
        {
            SendFileToBuffer(ch, SystemFileTypes.ASCTitle);
        }
    }
}
