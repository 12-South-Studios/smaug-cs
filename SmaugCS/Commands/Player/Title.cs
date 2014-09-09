using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.Player
{
    public static class Title
    {
        public static void do_title(CharacterInstance ch, string argument)
        {
            if (ch.IsNpc())
                return;

            color.set_char_color(ATTypes.AT_SCORE, ch);

            if (CheckFunctions.CheckIfTrue(ch, ch.Level < 5,
                "Sorry... you must be at least level 5 to set your title...")) return;
            if (CheckFunctions.CheckIfSet(ch, ((PlayerInstance)ch).PlayerData.Flags, PCFlags.NoTitle,
                "The gods prohibit you from changing your title.")) return;
            if (CheckFunctions.CheckIfEmptyString(ch, argument, "Change your title to what?")) return;

            string buffer = argument.Length > 50 ? argument.Substring(0, 50) : argument;

            buffer.SmashTilde();
            player.set_title((PlayerInstance)ch, buffer);
            color.send_to_char("Ok.", ch);
        }
    }
}
