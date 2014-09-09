using System;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;

namespace SmaugCS.Extensions
{
    public static class DescriptorExtensions
    {
        public static void ShowTitle(this DescriptorData d)
        {
            PlayerInstance ch = d.Character;
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

            int len = length;
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
