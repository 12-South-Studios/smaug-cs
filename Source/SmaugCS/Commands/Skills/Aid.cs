using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Exceptions;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Player;
using SmaugCS.Helpers;
using SmaugCS.Managers;
using SmaugCS.Properties;

namespace SmaugCS.Commands.Skills
{
    public static class Aid
    {
        public static void do_aid(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfTrue(ch, ch.IsNpc() && ch.IsAffected(AffectedByTypes.Charm),
                Resources.CANNOT_CONCENTRATE)) return;

            var arg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, arg, Resources.AID_WHO)) return;

            var victim = ch.GetCharacterInRoom(arg);
            if (CheckFunctions.CheckIfNullObject(ch, victim, "They aren't here.")) return;
            if (CheckFunctions.CheckIfNpc(ch, victim, "Not on mobs.")) return;
            if (CheckFunctions.CheckIfNotNullObject(ch, ch.CurrentMount, "You can't do that while mounted.")) return;
            if (CheckFunctions.CheckIfEquivalent(ch, ch, victim, "Aid yourself?")) return;

            if ((int)victim.CurrentPosition >= (int) PositionTypes.Stunned)
            {
                comm.act(ATTypes.AT_PLAIN, "$N doesn't need your help.", ch, null, victim, ToTypes.Character);
                return;
            }

            if (victim.CurrentHealth <= -6)
            {
                comm.act(ATTypes.AT_PLAIN, "$N's condition is beyond your aiding ability.", ch, null, victim, ToTypes.Character);
                return;
            }

            var percent = SmaugRandom.D100() - ch.GetCurrentLuck() - 13;

            var skill = DatabaseManager.Instance.GetEntity<SkillData>("aid");
            if (skill == null)
                throw new ObjectNotFoundException("Skill 'aid' was not found.");

            Macros.WAIT_STATE(ch, skill.Rounds);
            if (!ch.CanUseSkill(percent, skill))
            {
                ch.SendTo("You fail.");
               skill.LearnFromFailure((PlayerInstance)ch);
                return;
            }

            comm.act(ATTypes.AT_SKILL, "You aid $N!", ch, null, victim, ToTypes.Character);
            comm.act(ATTypes.AT_SKILL, "$n aids $N!", ch, null, victim, ToTypes.Room);
            skill.LearnFromSuccess((PlayerInstance)ch);
            ((PlayerInstance)ch).AdjustFavor(DeityFieldTypes.Aid, 1);

            if (victim.CurrentHealth < 1)
                victim.CurrentHealth = 1;

            victim.UpdatePositionByCurrentHealth();
            comm.act(ATTypes.AT_SKILL, "$n aids you!", ch, null, victim, ToTypes.Victim);
        }
    }
}
