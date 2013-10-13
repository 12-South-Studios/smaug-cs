using Realm.Library.Common.Extensions;
using SmaugCS.Enums;
using SmaugCS.Objects;

namespace SmaugCS.Commands.PetsAndGroups
{
    public static class Follow
    {
        public static void do_follow(CharacterInstance ch, string argument)
        {
            string firstArg = argument.FirstWord();
            if (string.IsNullOrWhiteSpace(firstArg))
            {
                color.send_to_char("Follow whom?\r\n", ch);
                return;
            }

            CharacterInstance victim = handler.get_char_room(ch, firstArg);
            if (victim == null)
            {
                color.send_to_char("They aren't here.\r\n", ch);
                return;
            }

            if (ch.IsAffected(AffectedByTypes.Charm) && ch.Master != null)
            {
                comm.act(ATTypes.AT_PLAIN, "But you'd rather follow $N!", ch, null, ch.Master, ToTypes.Character);
                return;
            }

            if (victim == ch)
            {
                if (ch.Master == null)
                {
                    color.send_to_char("You already follow yourself.\r\n", ch);
                    return;
                }

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

            if (ch.IsCircleFollowing(victim))
            {
                color.send_to_char("Following in loops is not allowed... sorry.\r\n", ch);
                return;
            }

            if (ch.Master != null)
                ch.StopFollower();

            ch.AddFollower(victim);
        }
    }
}
