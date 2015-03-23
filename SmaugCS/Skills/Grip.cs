﻿using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Exceptions;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Interfaces;
using SmaugCS.Managers;

namespace SmaugCS.Skills
{
    public static class Grip
    {
        public static bool CheckGrip(CharacterInstance ch, CharacterInstance victim, IDatabaseManager dbManager = null)
        {
            if (!victim.IsAwake())
                return false;

            if (victim.IsNpc() && !victim.Defenses.IsSet(DefenseTypes.Grip))
                return false;

            SkillData skill = (dbManager ?? DatabaseManager.Instance).GetEntity<SkillData>("grip");
            if (skill == null)
                throw new ObjectNotFoundException("Skill 'grip' not found");

            int chance;
            if (victim.IsNpc())
                chance = 60.GetLowestOfTwoNumbers(2*victim.Level);
            else
                chance = Macros.LEARNED(victim, (int) skill.ID)/2;

            chance += 2*(victim.GetCurrentLuck()-13);

            if (SmaugRandom.D100() >= (chance + victim.Level - ch.Level))
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