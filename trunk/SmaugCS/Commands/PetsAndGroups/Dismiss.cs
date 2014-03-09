using Realm.Library.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;


namespace SmaugCS.Commands.PetsAndGroups
{
    public static class Dismiss
    {
        public static void do_dismiss(CharacterInstance ch, string argument)
        {
            string firstArg = argument.FirstWord();

            if (string.IsNullOrWhiteSpace(firstArg))
            {
                color.send_to_char("Dismiss whom?\r\n", ch);
                return;
            }

            CharacterInstance victim = handler.get_char_room(ch, firstArg);
            if (victim == null)
            {
                color.send_to_char("They aren't here.\r\n", ch);
                return;
            }

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

            color.send_to_char("You cannot dismiss them.\r\n", ch);
        }
    }
}
