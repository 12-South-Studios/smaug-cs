using System.IO;
using Realm.Library.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;

namespace SmaugCS.Extensions
{
    public static class Comm
    {
        private static void SendFileToBuffer(this PlayerInstance ch, SystemFileTypes fileType)
        {
            string path = SystemConstants.GetSystemFile(fileType);
            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(path)))
            {
                string buffer = proxy.ReadToEnd();
                ch.Descriptor.WriteToBuffer(buffer, buffer.Length);
            } 
        }

        public static void SendRIPScreen(this PlayerInstance ch)
        {
            SendFileToBuffer(ch, SystemFileTypes.RIPScreen);
        }

        public static void SendRIPTitle(this PlayerInstance ch)
        {
            SendFileToBuffer(ch, SystemFileTypes.RIPTitle);
        }

        public static void SendANSITitle(this PlayerInstance ch)
        {
            SendFileToBuffer(ch, SystemFileTypes.ANSITitle);
        }

        public static void SendASCIITitle(this PlayerInstance ch)
        {
            SendFileToBuffer(ch, SystemFileTypes.ASCTitle);
        }
    }
}
