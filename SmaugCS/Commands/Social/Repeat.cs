using System;
using System.Linq;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.Social
{
    public static class Repeat
    {
        public static void do_repeat(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfTrue(ch,
                ch.IsNpc() || !ch.IsImmortal() || ((PlayerInstance)ch).PlayerData.TellHistory == null || !((PlayerInstance)ch).PlayerData.TellHistory.Any(),
                "Huh?")) return;

            int tellIndex;
            if (string.IsNullOrWhiteSpace(argument))
                tellIndex = ((PlayerInstance)ch).PlayerData.TellHistory.Count - 1;
            else if (Char.IsLetter(argument.ToCharArray()[0]) && argument.Length == 1)
                tellIndex = Char.ToLower(argument.ToCharArray()[0]) - 'a';
            else
            {
                color.ch_printf(ch, "You may only index your tell history using a single letter.");
                return;
            }

            if (CheckFunctions.CheckIfEmptyString(ch, ((PlayerInstance)ch).PlayerData.TellHistory[tellIndex],
                "No one like that has sent you a tell.")) return;

            color.set_char_color(ATTypes.AT_TELL, ch);
            color.send_to_char(((PlayerInstance)ch).PlayerData.TellHistory[tellIndex], ch);
        }
    }
}
