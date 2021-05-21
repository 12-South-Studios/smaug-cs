using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.Player
{
    public static class Title
    {
        public static void do_title(CharacterInstance ch, string argument)
        {
            if (ch.IsNpc())
                return;

            ch.SetColor(ATTypes.AT_SCORE);

            if (CheckFunctions.CheckIfTrue(ch, ch.Level < 5,
                "Sorry... you must be at least level 5 to set your title...")) return;
            if (CheckFunctions.CheckIfSet(ch, ((PlayerInstance)ch).PlayerData.Flags, PCFlags.NoTitle,
                "The gods prohibit you from changing your title.")) return;
            if (CheckFunctions.CheckIfEmptyString(ch, argument, "Change your title to what?")) return;

            var buffer = argument.Length > 50 ? argument.Substring(0, 50) : argument;

            buffer.SmashTilde();
            player.set_title((PlayerInstance)ch, buffer);
            ch.SendTo("Ok.");
        }
    }
}
