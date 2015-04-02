using Realm.Library.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.PetsAndGroups
{
    public static class Follow
    {
        public static void do_follow(CharacterInstance ch, string argument)
        {
            var firstArg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Follow whom?")) return;


            var victim = ch.GetCharacterInRoom(firstArg);
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
                                            && ((PlayerInstance)victim).PlayerData.Council != null
                                            && !((PlayerInstance)victim).PlayerData.Council.Name.Equals("Newbie Council")))
            {
                ch.SendTo("You are not of the right caliber to follow.");
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
