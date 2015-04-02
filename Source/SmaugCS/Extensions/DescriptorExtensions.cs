using System;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Extensions.Player;
using SmaugCS.Logging;

namespace SmaugCS.Extensions
{
    public static class DescriptorExtensions
    {
        public static void WriteToPager(this DescriptorData d, string txt, int length)
        {
            var len = length <= 0 ? txt.Length : length;
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
                var bufferArray = d.PageBuffer.ToCharArray();
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

        public static void SendTo(this DescriptorData d, string txt)
        {
            if (d == null || string.IsNullOrEmpty(txt))
                return;

            d.WriteToBuffer(color.colorize(txt, d), 0);
        }

        public static void ShowTitle(this DescriptorData d)
        {
            var ch = d.Character;
            if (ch.PlayerData.Flags.IsSet(PCFlags.NoIntro))
                d.WriteToBuffer("Press enter...", 0);
            else
            {
                if (ch.Act.IsSet(PlayerFlags.Rip))
                    ch.SendRIPTitle();
                else if (ch.Act.IsSet(PlayerFlags.Ansi))
                    ch.SendANSITitle();
                else
                    ch.SendASCIITitle();
            }

            d.ConnectionStatus = ConnectionTypes.PressEnter;
        }

        public static void WriteToBuffer(this DescriptorData d, string txt, int length)
        {
            if (d == null)
                throw new ArgumentNullException("d");

            if (string.IsNullOrEmpty(d.outbuf))
                return;

            var len = length;
            if (len <= 0)
                len = txt.Length;

            if (d.outtop == 0 && !d.fcommand)
            {
                d.outbuf = "\r\n" + d.outbuf;
                d.outtop = 2;
            }

            d.outtop += len;
            d.outbuf = txt;
        }
    }
}
