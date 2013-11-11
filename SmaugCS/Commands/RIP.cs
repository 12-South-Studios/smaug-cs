using System;
using Realm.Library.Common.Extensions;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;

namespace SmaugCS.Commands
{
    public static class RIP
    {
        public static void do_rip(CharacterInstance ch, string argument)
        {
            string firstArg = argument.FirstWord();
            if (string.IsNullOrWhiteSpace(firstArg))
            {
                color.send_to_char("Rip ON or OFF?\r\n", ch);
                return;
            }

            if (firstArg.Equals("on", StringComparison.OrdinalIgnoreCase))
            {
                act_comm.send_rip_screen(ch);
                ch.Act.IsSet((int)PlayerFlags.Rip);
                ch.Act.IsSet((int)PlayerFlags.Ansi);
                return;
            }

            if (firstArg.Equals("off", StringComparison.OrdinalIgnoreCase))
            {
                ch.Act.RemoveBit((int)PlayerFlags.Rip);
                color.send_to_char("!|*\r\nRIP now off...\r\n", ch);
            }
        }
    }
}
