using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Exceptions;
using SmaugCS.Lookup;
using SmaugCS.Managers;

namespace SmaugCS.Extensions
{
    public static class SkillDataExtensions
    {
        public static void LearnFromSuccess(this SkillData skill, CharacterInstance ch)
        {
            if (ch.IsNpc() || ch.PlayerData == null)
                return;

            int val = ch.PlayerData.Learned[(int) skill.ID];
            if (val <= 0)
                return;

            int mastery = skill.GetMasteryLevel(ch);
            int skillLevel = skill.SkillLevels.ToList()[(int) ch.CurrentClass];
            if (skillLevel == 0)
                skillLevel = ch.Level;

            if (ch.PlayerData.Learned[(int)skill.ID] >= mastery)
                return;

            GainLearningInSkill(skill, ch, mastery);
            GainExperienceFromSkill(skill, ch, mastery, skillLevel);
        }

        private static void GainExperienceFromSkill(SkillData skill, CharacterInstance ch, int mastery, int skillLevel)
        {
            int gain = ch.PlayerData.Learned[(int) skill.ID] == mastery
                ? GainMasteryOfSkill(skill, ch, skillLevel)
                : GainExperienceInSkill(ch, skillLevel);

            ch.GainXP(gain);
        }

        private static int GainExperienceInSkill(CharacterInstance ch, int skillLevel)
        {
            int gain = 20*skillLevel;
            if (ch.CurrentClass == ClassTypes.Mage)
                gain *= 6;
            if (ch.CurrentClass == ClassTypes.Cleric)
                gain *= 3;

            color.set_char_color(ATTypes.AT_WHITE, ch);
            color.ch_printf(ch, "You gain %d experience points from your success!", gain);
            return gain;
        }

        private static int GainMasteryOfSkill(SkillData skill, CharacterInstance ch, int skillLevel)
        {
            int gain = 1000*skillLevel;
            if (ch.CurrentClass == ClassTypes.Mage)
                gain *= 5;
            if (ch.CurrentClass == ClassTypes.Cleric)
                gain *= 2;

            color.set_char_color(ATTypes.AT_WHITE, ch);
            color.ch_printf(ch, "You are now an adept of %s!  You gain %d bonus experience!", skill.Name, gain);
            return gain;
        }

        private static void GainLearningInSkill(SkillData skill, CharacterInstance ch, int mastery)
        {
            int chance = ch.PlayerData.Learned[(int) skill.ID] + (5*skill.difficulty);
            int percent = SmaugRandom.D100();
            int learn;

            if (percent >= chance)
                learn = 2;
            else if ((chance - percent) > 25)
                return;
            else
                learn = 1;

            mastery.GetLowestOfTwoNumbers(ch.PlayerData.Learned[(int) skill.ID] + learn);
        }

        public static void LearnFromFailure(this SkillData skill, CharacterInstance ch)
        {
            if (ch.IsNpc() || ch.PlayerData == null)
                return;

            int val = ch.PlayerData.Learned[(int) skill.ID];
            if (val <= 0)
                return;

            int chance = ch.PlayerData.Learned[(int) skill.ID] + (5*skill.difficulty);
            if ((chance - SmaugRandom.D100()) > 25)
                return;

            int mastery = skill.GetMasteryLevel(ch);
            if (ch.PlayerData.Learned[(int) skill.ID] < (mastery - 1))
                ch.PlayerData.Learned[(int) skill.ID] =
                    mastery.GetLowestOfTwoNumbers(ch.PlayerData.Learned[(int) skill.ID] + 1);
        }

        public static int GetMasteryLevel(this SkillData skill, CharacterInstance ch)
        {
            SkillMasteryData mastery = skill.SkillMasteries.FirstOrDefault(x => x.ClassType == ch.CurrentClass);
            if (mastery == null)
                throw new EntryNotFoundException("Mastery value for Class {0} not found in Skill {1}", ch.CurrentClass, skill.ID);

            return mastery.MasteryLevel;
        }

        public static bool CheckSave(this SkillData skill, int level, CharacterInstance ch, CharacterInstance victim)
        {
            bool saved = false;
            int localLevel = level;

            if (skill.Flags.IsSet(SkillFlags.PKSensitive) && !ch.IsNpc() && !victim.IsNpc())
                localLevel /= 2;

            switch (skill.SaveVs)
            {
                case SaveVsTypes.PoisonOrDeath:
                    saved = victim.SavingThrows.CheckSaveVsPoisonDeath(localLevel, victim);
                    break;
                case SaveVsTypes.RodsOrWands:
                    saved = victim.SavingThrows.CheckSaveVsWandRod(localLevel, victim);
                    break;
                case SaveVsTypes.ParalysisOrPetrification:
                    saved = victim.SavingThrows.CheckSaveVsParalysisPetrify(localLevel, victim);
                    break;
                case SaveVsTypes.Breath:
                    saved = victim.SavingThrows.CheckSaveVsBreath(localLevel, victim);
                    break;
                case SaveVsTypes.SpellsOrStaves:
                    saved = victim.SavingThrows.CheckSaveVsSpellStaff(localLevel, victim);
                    break;
            }

            return saved;
        }

        public static void AbilityLearnFromSuccess(this SkillData skill, CharacterInstance ch)
        {
            int sn = (int) skill.ID;

            if (ch.IsNpc() || ch.PlayerData.Learned[sn] <= 0)
                return;

            int adept = skill.RaceAdept[(int)ch.CurrentRace];
            int skillLevel = skill.RaceLevel[(int)ch.CurrentRace];

            if (skillLevel == 0)
                skillLevel = ch.Level;
            if (ch.PlayerData.Learned[sn] < adept)
            {
                int schance = ch.PlayerData.Learned[sn] + (5 * skill.difficulty);
                int percent = SmaugRandom.D100();

                int learn = 1;
                if (percent >= schance)
                    learn = 2;
                else if (schance - percent > 25)
                    return;

                ch.PlayerData.Learned[sn] = adept.GetLowestOfTwoNumbers(ch.PlayerData.Learned[sn] + learn);

                int gain;
                if (ch.PlayerData.Learned[sn] == adept)
                {
                    gain = 1000 * skillLevel;
                    color.set_char_color(ATTypes.AT_WHITE, ch);
                    color.ch_printf(ch, "You are now an adept of %s!  You gain %d bonus experience!\r\n", skill.Name,
                                    gain);
                }
                else
                {
                    gain = 20 * skillLevel;
                    if (ch.CurrentFighting == null) // TODO: Check gsn_hide && gsn_sneak
                    {
                        color.set_char_color(ATTypes.AT_WHITE, ch);
                        color.ch_printf(ch, "You gain %d experience points from your success!\r\n", gain);
                    }
                }
                ch.GainXP(gain);
            }
        }
    }
}
