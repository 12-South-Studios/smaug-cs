using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Exceptions;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Managers;

namespace SmaugCS.Skills
{
    public static class Tumble
    {
        public static bool CheckTumble(CharacterInstance ch, CharacterInstance victim)
        {
            if (victim.CurrentClass != ClassTypes.Thief || !victim.IsAwake())
                return false;

            SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>("tumble");
            if (skill == null)
                throw new ObjectNotFoundException("Skill 'tumble' not found");

            if (!victim.IsNpc() && !(((PlayerInstance)victim).PlayerData.Learned[(int) skill.ID] > 0))
                return false;

            int chances;
            if (victim.IsNpc())
                chances = 60.GetLowestOfTwoNumbers(2*victim.Level);
            else
                chances = Macros.LEARNED(victim, (int) skill.ID)/GameManager.Instance.SystemData.TumbleMod +
                         (victim.GetCurrentDexterity() - 13);

            if (chances != 0 && victim.CurrentMorph != null)
                chances += victim.CurrentMorph.Morph.TumbleChances;

            if (!victim.Chance(chances + victim.Level - ch.Level))
                return false;

            if (!victim.IsNpc() && !((PlayerInstance)victim).PlayerData.Flags.IsSet(PCFlags.Gag))
                comm.act(ATTypes.AT_SKILL, "You tumble away from $n's attack.", ch, null, victim, ToTypes.Victim);

            if (!ch.IsNpc() && !((PlayerInstance)ch).PlayerData.Flags.IsSet(PCFlags.Gag))
                comm.act(ATTypes.AT_SKILL, "$N tumbles away from your attack.", ch, null, victim, ToTypes.Character);

            skill.LearnFromSuccess((PlayerInstance)victim);
            return true;
        }
    }
}
