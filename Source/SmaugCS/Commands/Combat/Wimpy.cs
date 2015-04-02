using Realm.Library.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.Combat
{
    public static class Wimpy
    {
        public static void do_wimpy(CharacterInstance ch, string argument)
        {
            ch.SetColor(ATTypes.AT_YELLOW);

            var firstArg = argument.FirstWord();

            int wimpy;
            if (firstArg.IsNullOrEmpty())
                wimpy = ch.MaximumHealth/5;
            else if (firstArg.EqualsIgnoreCase("max"))
            {
                if (ch.IsPKill())
                    wimpy = (int) (ch.MaximumHealth/2.25f);
                else
                    wimpy = (int) (ch.MaximumHealth/1.2f);
            }
            else
                wimpy = firstArg.ToInt32();

            if (CheckFunctions.CheckIfTrue(ch, wimpy < 0, "Your courage exceeds your wisdom.")) return;
            if (CheckFunctions.CheckIfTrue(ch, ch.IsPKill() && wimpy > (int) (ch.MaximumHealth/2.25f),
                "Such cowardice ill becomes you.")) return;
            if (CheckFunctions.CheckIfTrue(ch, wimpy > (int) (ch.MaximumHealth/1.2f), "Such cowardice ill becomes you."))
                return;

            ch.wimpy = wimpy;
            ch.Printf("Wimpy set to %d hit points.", wimpy);
        }
    }
}
