using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;

namespace SmaugCS.Commands
{
    public static class AFK
    {
        public static void do_afk(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfNpc(ch, ch)) return;

            if (ch.Act.IsSet((int)PlayerFlags.AwayFromKeyboard))
                ch.Act.RemoveBit((int)PlayerFlags.AwayFromKeyboard);
            else
                ch.Act.SetBit((int)PlayerFlags.AwayFromKeyboard);

            var isAfkSet = ch.Act.IsSet((int)PlayerFlags.AwayFromKeyboard);
            ch.SendTo(isAfkSet ? "You are no longer afk." : "You are now afk.");
            comm.act(ATTypes.AT_GREY, isAfkSet ? "$n is no longer afk." : "$n is now afk.", ch, null, null,
                ToTypes.CanSee);
        }
    }
}
