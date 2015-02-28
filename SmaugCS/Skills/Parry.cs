using SmaugCS.Common;
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
    public static class Parry
    {
        public static bool CheckParry(CharacterInstance ch, CharacterInstance victim, IDatabaseManager dbManager = null, 
            IGameManager gameManager = null)
        {
            if (!victim.IsAwake())
                return false;

            if (victim.IsNpc() && !victim.Defenses.IsSet(DefenseTypes.Parry))
                return false;

            int chances;

            SkillData skill = (dbManager ?? DatabaseManager.Instance).GetEntity<SkillData>("parry");
            if (skill == null)
                throw new ObjectNotFoundException("Skill 'parry' not found");

            if (victim.IsNpc())
                chances = 60.GetLowestOfTwoNumbers(2*victim.Level);
            else
            {
                if (victim.GetEquippedItem(WearLocations.Wield) == null)
                    return false;
                chances = Macros.LEARNED(victim, (int) skill.ID)/
                          (gameManager ?? GameManager.Instance).SystemData.ParryMod;
            }

            if (chances != 0 && victim.CurrentMorph != null)
                chances += victim.CurrentMorph.Morph.ParryChances;

            if (!victim.Chance(chances + victim.Level - ch.Level))
            {
                skill.LearnFromFailure(victim);
                return false;
            }

            if (!victim.IsNpc() && !((PlayerInstance)victim).PlayerData.Flags.IsSet(PCFlags.Gag))
                comm.act(ATTypes.AT_SKILL, "You parry $n's attack.", ch, null, victim, ToTypes.Victim);

            if (!ch.IsNpc() && !((PlayerInstance)ch).PlayerData.Flags.IsSet(PCFlags.Gag))
                comm.act(ATTypes.AT_SKILL, "$N parries your attack.", ch, null, victim, ToTypes.Character);

            skill.LearnFromSuccess(victim);
            return true;
        }
    }
}
