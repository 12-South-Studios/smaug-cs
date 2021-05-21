using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Exceptions;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Interfaces;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Managers;
using SmaugCS.Repository;

namespace SmaugCS.Skills
{
    public static class Dodge
    {
        public static bool CheckDodge(CharacterInstance ch, CharacterInstance victim,
            IRepositoryManager databaseManager = null, IGameManager gameManager = null)
        {
            if (!victim.IsAwake())
                return false;

            if (victim.IsNpc() && !victim.Defenses.IsSet(DefenseTypes.Dodge))
                return false;

            var skill = (databaseManager ?? RepositoryManager.Instance).GetEntity<SkillData>("dodge");
            if (skill == null)
                throw new ObjectNotFoundException("Skill 'dodge' not found");

            int chances;

            if (victim.IsNpc())
                chances = 60.GetLowestOfTwoNumbers(2 * victim.Level);
            else
                chances = (int)Macros.LEARNED(victim, (int)skill.ID) /
                          (gameManager ?? GameManager.Instance).SystemData.DodgeMod;

            if (chances != 0 && victim.CurrentMorph != null)
                chances += victim.CurrentMorph.Morph.DodgeChances;

            if (!victim.IsNpc() && !victim.Chance(chances + victim.Level - ch.Level))
            {
                skill.LearnFromFailure(victim);
                return false;
            }

            if (!victim.IsNpc() && !((PlayerInstance)victim).PlayerData.Flags.IsSet(PCFlags.Gag))
                comm.act(ATTypes.AT_SKILL, "You dodge $n's attack.", ch, null, victim, ToTypes.Victim);

            if (!ch.IsNpc() && !((PlayerInstance)ch).PlayerData.Flags.IsSet(PCFlags.Gag))
                comm.act(ATTypes.AT_SKILL, "$N dodges your attack.", ch, null, victim, ToTypes.Character);

            skill.LearnFromSuccess(victim);
            return true;
        }
    }
}
