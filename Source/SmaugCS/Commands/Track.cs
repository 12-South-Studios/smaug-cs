using Realm.Library.Common;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;
using SmaugCS.Managers;

namespace SmaugCS.Commands
{
    public static class Track
    {
        public static void do_track(CharacterInstance ch, string argument)
        {
            var skill = DatabaseManager.Instance.GetEntity<SkillData>("track");
            if (CheckFunctions.CheckIfTrue(ch, !ch.IsNpc() && ((PlayerInstance)ch).PlayerData.Learned[(int) skill.ID] <= 0,
                "You do not know of this skill yet.")) return;

            var arg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, arg, "Whom are you trying to track?")) return;

            Macros.WAIT_STATE(ch, skill.Rounds);

            var victim = ch.GetCharacterInWorld(arg);
            if (CheckFunctions.CheckIfNullObject(ch, victim, "You can't find a trail of anyone like that.")) return;

        }
    }
}
