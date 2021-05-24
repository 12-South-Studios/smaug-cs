using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using System.Linq;

namespace SmaugCS.Commands
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
            else if (char.IsLetter(argument.ToCharArray()[0]) && argument.Length == 1)
                tellIndex = char.ToLower(argument.ToCharArray()[0]) - 'a';
            else
            {
                ch.Printf("You may only index your tell history using a single letter.");
                return;
            }

            if (CheckFunctions.CheckIfEmptyString(ch, ((PlayerInstance)ch).PlayerData.TellHistory.ToList()[tellIndex],
                "No one like that has sent you a tell.")) return;

            ch.SetColor(ATTypes.AT_TELL);
            ch.SendTo(((PlayerInstance)ch).PlayerData.TellHistory.ToList()[tellIndex]);
        }
    }
}
