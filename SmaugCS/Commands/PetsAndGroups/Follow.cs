using Realm.Library.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.PetsAndGroups
{
    public static class Follow
    {
        public static void do_follow(CharacterInstance ch, string argument)
        {
            string firstArg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Follow whom?")) return;


            CharacterInstance victim = handler.get_char_room(ch, firstArg);
            if (CheckFunctions.CheckIfNullObject(ch, victim, "They aren't here.")) return;

            if (ch.IsAffected(AffectedByTypes.Charm) && ch.Master != null)
            {
                comm.act(ATTypes.AT_PLAIN, "But you'd rather follow $N!", ch, null, ch.Master, ToTypes.Character);
                return;
            }

            if (victim == ch)
            {
                if (CheckFunctions.CheckIfNullObject(ch, ch.Master, "You already follow yourself.")) return;

                ch.StopFollower();
                return;
            }

            if ((ch.Level - victim.Level < -10 ||
                 ch.Level - victim.Level > 10)
                && !ch.IsHero() && !(ch.Level < 15 && !victim.IsNpc()
                                            && victim.PlayerData.Council != null
                                            && !victim.PlayerData.Council.Name.Equals("Newbie Council")))
            {
                color.send_to_char("You are not of the right caliber to follow.\r\n", ch);
                return;
            }

            if (CheckFunctions.CheckIfTrue(ch, ch.IsCircleFollowing(victim),
                "Following in loops is not allowed... sorry.")) return;

            if (ch.Master != null)
                ch.StopFollower();

            ch.AddFollower(victim);
        }
    }
}
