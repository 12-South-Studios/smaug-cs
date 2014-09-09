using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;

namespace SmaugCS.Commands
{
    public static class AFK
    {
        public static void do_afk(CharacterInstance ch, string argument)
        {
            if (Helpers.CheckFunctions.CheckIfNpc(ch, ch)) return;

            string sendMsg;
            string actMsg;

            if (ch.Act.IsSet(PlayerFlags.AwayFromKeyboard))
            {
                ch.Act.RemoveBit(PlayerFlags.AwayFromKeyboard);
                sendMsg = "You are no longer afk.";
                actMsg = "$n is no longer afk.";
            }
            else 
            {
                ch.Act.SetBit(PlayerFlags.AwayFromKeyboard);
                sendMsg = "You are now afk.";
                actMsg = "$n is now afk.";
            }

            color.send_to_char(sendMsg, ch);
            comm.act(ATTypes.AT_GREY, actMsg, ch, null, null, ToTypes.CanSee);
        }
    }
}
