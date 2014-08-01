using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Exceptions;
using SmaugCS.Extensions;
using SmaugCS.Managers;

namespace SmaugCS.Skills
{
    public static class Grip
    {
        public static bool CheckGrip(CharacterInstance ch, CharacterInstance victim)
        {
            if (!victim.IsAwake())
                return false;

            if (victim.IsNpc() && !victim.Defenses.IsSet(DefenseTypes.Grip))
                return false;

            SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>("grip");
            if (skill == null)
                throw new ObjectNotFoundException("Skill 'grip' not found");

            int chance;
            if (victim.IsNpc())
                chance = 60.GetLowestOfTwoNumbers(2*victim.Level);
            else
                chance = Macros.LEARNED(victim, (int) skill.ID)/2;

            chance += 2*(victim.GetCurrentLuck()-13);

            if (SmaugRandom.Percent() >= (chance + victim.Level - ch.Level))
            {
                skill.LearnFromFailure(victim);
                return false;
            }

            comm.act(ATTypes.AT_SKILL, "You evade $n's attempt to disarm you.", ch, null, victim, ToTypes.Victim);
            comm.act(ATTypes.AT_SKILL, "$N holds $S weapon strongly, and is not disarmed.", ch, null, victim, ToTypes.Character);
            skill.LearnFromSuccess(victim);
            return true;
        }
    }
}
