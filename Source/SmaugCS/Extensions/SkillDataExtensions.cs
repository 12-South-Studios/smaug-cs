using System.Linq;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Exceptions;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Player;

namespace SmaugCS.Extensions
{
    public static class SkillDataExtensions
    {
        public static void LearnFromSuccess(this SkillData skill, CharacterInstance ch)
        {
            if (ch.IsNpc()) return;
            
            var pch = (PlayerInstance)ch;
            if (pch.PlayerData == null) return;

            var val = pch.GetLearned((int)skill.ID);
            if (val <= 0)
                return;

            int mastery = skill.GetMasteryLevel(pch);
            var skillLevel = skill.SkillLevels.ToList()[(int) ch.CurrentClass];
            if (skillLevel == 0)
                skillLevel = ch.Level;

            if (pch.GetLearned((int)skill.ID) >= mastery)
                return;

            GainLearningInSkill(skill, pch, mastery);
            GainExperienceFromSkill(skill, pch, mastery, skillLevel);
        }

        private static void GainExperienceFromSkill(SkillData skill, PlayerInstance ch, int mastery, int skillLevel)
        {
            var gain = ch.PlayerData.Learned.ToList().FirstOrDefault(x => x == skill.ID) == mastery
                ? GainMasteryOfSkill(skill, ch, skillLevel)
                : GainExperienceInSkill(ch, skillLevel);

            ch.GainXP(gain);
        }

        private static int GainExperienceInSkill(CharacterInstance ch, int skillLevel)
        {
            var gain = 20*skillLevel;
            if (ch.CurrentClass == ClassTypes.Mage)
                gain *= 6;
            if (ch.CurrentClass == ClassTypes.Cleric)
                gain *= 3;

           ch.SetColor(ATTypes.AT_WHITE);
            ch.Printf("You gain %d experience points from your success!", gain);
            return gain;
        }

        private static int GainMasteryOfSkill(SkillData skill, CharacterInstance ch, int skillLevel)
        {
            var gain = 1000*skillLevel;
            if (ch.CurrentClass == ClassTypes.Mage)
                gain *= 5;
            if (ch.CurrentClass == ClassTypes.Cleric)
                gain *= 2;

           ch.SetColor(ATTypes.AT_WHITE);
            ch.Printf("You are now an adept of %s!  You gain %d bonus experience!", skill.Name, gain);
            return gain;
        }

        private static void GainLearningInSkill(SkillData skill, PlayerInstance ch, int mastery)
        {
            var chance = ch.GetLearned((int) skill.ID) + (5*skill.difficulty);
            var percent = SmaugRandom.D100();
            int learn;

            if (percent >= chance)
                learn = 2;
            else if ((chance - percent) > 25)
                return;
            else
                learn = 1;

            mastery.GetLowestOfTwoNumbers((int)(ch.GetLearned(skill.ID) + learn));
        }

        public static void LearnFromFailure(this SkillData skill, CharacterInstance ch)
        {
            if (ch.IsNpc()) return;
            
            var pch = (PlayerInstance)ch;
            if (pch.PlayerData == null) return;

            var val = pch.GetLearned((int) skill.ID);
            if (val <= 0) return;

            var chance = pch.GetLearned((int) skill.ID) + (5*skill.difficulty);
            if ((chance - SmaugRandom.D100()) > 25) return;

            int mastery = skill.GetMasteryLevel(pch);
            if (pch.GetLearned((int) skill.ID) < (mastery - 1))
            {
                //pch.PlayerData.Learned.ToList().First(x => x == skill.ID) =
                //    mastery.GetLowestOfTwoNumbers((int)(pch.GetLearned(skill.ID) + 1));
            }
        }

        public static int GetMasteryLevel(this SkillData skill, PlayerInstance ch)
        {
            var mastery = skill.SkillMasteries.FirstOrDefault(x => x.ClassType == ch.CurrentClass);
            if (mastery == null)
                throw new EntryNotFoundException("Mastery value for Class {0} not found in Skill {1}", ch.CurrentClass,
                    skill.ID);

            return mastery.MasteryLevel;
        }

        public static bool CheckSave(this SkillData skill, int level, CharacterInstance ch, CharacterInstance victim)
        {
            var saved = false;
            var localLevel = level;

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

        public static void AbilityLearnFromSuccess(this SkillData skill, PlayerInstance ch)
        {
            var sn = (int) skill.ID;

            if (ch.IsNpc() || ch.PlayerData.Learned.ToList()[sn] <= 0)
                return;

            var adept = skill.RaceAdept.ToList()[(int)ch.CurrentRace];
            var skillLevel = skill.RaceLevel.ToList()[(int)ch.CurrentRace];

            if (skillLevel == 0)
                skillLevel = ch.Level;
            if (ch.PlayerData.Learned.ToList()[sn] < adept)
            {
                var schance = ch.PlayerData.Learned.ToList()[sn] + (5 * skill.difficulty);
                var percent = SmaugRandom.D100();

                var learn = 1;
                if (percent >= schance)
                    learn = 2;
                else if (schance - percent > 25)
                    return;

                ch.PlayerData.Learned.ToList()[sn] = adept.GetLowestOfTwoNumbers((int)ch.PlayerData.Learned.ToList()[sn] + learn);

                int gain;
                if (ch.PlayerData.Learned.ToList()[sn] == adept)
                {
                    gain = 1000 * skillLevel;
                   ch.SetColor(ATTypes.AT_WHITE);
                    ch.Printf("You are now an adept of %s!  You gain %d bonus experience!", skill.Name,
                                    gain);
                }
                else
                {
                    gain = 20 * skillLevel;
                    if (ch.CurrentFighting == null) // TODO: Check gsn_hide && gsn_sneak
                    {
                        ch.SetColor(ATTypes.AT_WHITE);
                        ch.Printf("You gain %d experience points from your success!", gain);
                    }
                }
                ch.GainXP(gain);
            }
        }
    }
}
