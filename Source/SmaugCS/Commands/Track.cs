using Realm.Library.Common.Extensions;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;
using SmaugCS.Repository;
using System.Linq;

namespace SmaugCS.Commands
{
    public static class Track
    {
        public static void do_track(CharacterInstance ch, string argument)
        {
            var skill = RepositoryManager.Instance.GetEntity<SkillData>("track");
            if (CheckFunctions.CheckIfTrue(ch, !ch.IsNpc() && ((PlayerInstance)ch).PlayerData.Learned.ToList().FirstOrDefault(x => x == skill.ID) <= 0,
                "You do not know of this skill yet.")) return;

            var arg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, arg, "Whom are you trying to track?")) return;

            Macros.WAIT_STATE(ch, skill.Rounds);

            var victim = ch.GetCharacterInWorld(arg);
            if (CheckFunctions.CheckIfNullObject(ch, victim, "You can't find a trail of anyone like that.")) return;

        }
    }
}
