using Realm.Library.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Extensions;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.PetsAndGroups
{
    public static class Dismiss
    {
        public static void do_dismiss(CharacterInstance ch, string argument)
        {
            string firstArg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Dismiss whom?")) return;

            CharacterInstance victim = ch.GetCharacterInRoom(firstArg);
            if (CheckFunctions.CheckIfNullObject(ch, victim, "They aren't here.")) return;

            if (victim.IsAffected(AffectedByTypes.Charm)
                && victim.IsNpc() && victim.Master == ch)
            {
                victim.StopFollower();
                victim.StopHating();
                victim.StopHunting();
                victim.StopFearing();
                comm.act(ATTypes.AT_ACTION, "$n dismisses $N.", ch, null, victim, ToTypes.NotVictim);
                comm.act(ATTypes.AT_ACTION, "You dismiss $N.", ch, null, victim, ToTypes.Character);
                return;
            }

            color.send_to_char("You cannot dismiss them.", ch);
        }
    }
}
