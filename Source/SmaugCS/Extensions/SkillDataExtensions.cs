using System.Linq;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Exceptions;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Player;

namespace SmaugCS.Extensions;

public static class SkillDataExtensions
{
  public static void LearnFromSuccess(this SkillData skill, CharacterInstance ch)
  {
    if (ch.IsNpc()) return;

    PlayerInstance pch = (PlayerInstance)ch;
    if (pch.PlayerData == null) return;

    long val = pch.GetLearned((int)skill.Id);
    if (val <= 0)
      return;

    int mastery = skill.GetMasteryLevel(pch);
    int skillLevel = skill.SkillLevels.ToList()[(int)ch.CurrentClass];
    if (skillLevel == 0)
      skillLevel = ch.Level;

    if (pch.GetLearned((int)skill.Id) >= mastery)
      return;

    GainLearningInSkill(skill, pch, mastery);
    GainExperienceFromSkill(skill, pch, mastery, skillLevel);
  }

  private static void GainExperienceFromSkill(SkillData skill, PlayerInstance ch, int mastery, int skillLevel)
  {
    int gain = ch.PlayerData.GetSkillMastery(skill.Id) == mastery
      ? GainMasteryOfSkill(skill, ch, skillLevel)
      : GainExperienceInSkill(ch, skillLevel);

    ch.GainXP(gain);
  }

  private static int GainExperienceInSkill(CharacterInstance ch, int skillLevel)
  {
    int gain = 20 * skillLevel;
    switch (ch.CurrentClass)
    {
      case ClassTypes.Mage:
        gain *= 6;
        break;
      case ClassTypes.Cleric:
        gain *= 3;
        break;
    }

    ch.SetColor(ATTypes.AT_WHITE);
    ch.Printf("You gain %d experience points from your success!", gain);
    return gain;
  }

  private static int GainMasteryOfSkill(SkillData skill, CharacterInstance ch, int skillLevel)
  {
    int gain = 1000 * skillLevel;
    switch (ch.CurrentClass)
    {
      case ClassTypes.Mage:
        gain *= 5;
        break;
      case ClassTypes.Cleric:
        gain *= 2;
        break;
    }

    ch.SetColor(ATTypes.AT_WHITE);
    ch.Printf("You are now an adept of %s!  You gain %d bonus experience!", skill.Name, gain);
    return gain;
  }

  private static void GainLearningInSkill(SkillData skill, PlayerInstance ch, int mastery)
  {
    long chance = ch.GetLearned((int)skill.Id) + 5 * skill.difficulty;
    int percent = SmaugRandom.D100();
    int learn;

    if (percent >= chance)
      learn = 2;
    else if (chance - percent > 25)
      return;
    else
      learn = 1;

    mastery.GetLowestOfTwoNumbers((int)(ch.GetLearned(skill.Id) + learn));
  }

  public static void LearnFromFailure(this SkillData skill, CharacterInstance ch)
  {
    if (ch.IsNpc()) return;

    PlayerInstance pch = (PlayerInstance)ch;
    if (pch.PlayerData == null) return;

    long val = pch.GetLearned((int)skill.Id);
    if (val <= 0) return;

    long chance = pch.GetLearned((int)skill.Id) + 5 * skill.difficulty;
    if (chance - SmaugRandom.D100() > 25) return;

    int mastery = skill.GetMasteryLevel(pch);
    if (pch.GetLearned((int)skill.Id) < mastery - 1)
    {
      //pch.PlayerData.Learned.ToList().First(x => x == skill.ID) =
      //    mastery.GetLowestOfTwoNumbers((int)(pch.GetLearned(skill.ID) + 1));
    }
  }

  public static int GetMasteryLevel(this SkillData skill, PlayerInstance ch)
  {
    SkillMasteryData mastery = skill.SkillMasteries.FirstOrDefault(x => x.ClassType == ch.CurrentClass);
    if (mastery == null)
      throw new EntryNotFoundException("Mastery value for Class {0} not found in Skill {1}", ch.CurrentClass,
        skill.Id);

    return mastery.MasteryLevel;
  }

  public static bool CheckSave(this SkillData skill, int level, CharacterInstance ch, CharacterInstance victim)
  {
    bool saved = false;
    int localLevel = level;

    if (skill.Flags.IsSet(SkillFlags.PKSensitive) && !ch.IsNpc() && !victim.IsNpc())
      localLevel /= 2;

    saved = skill.SaveVs switch
    {
      SaveVsTypes.PoisonOrDeath => victim.SavingThrows.CheckSaveVsPoisonDeath(localLevel, victim),
      SaveVsTypes.RodsOrWands => victim.SavingThrows.CheckSaveVsWandRod(localLevel, victim),
      SaveVsTypes.ParalysisOrPetrification => victim.SavingThrows.CheckSaveVsParalysisPetrify(localLevel, victim),
      SaveVsTypes.Breath => victim.SavingThrows.CheckSaveVsBreath(localLevel, victim),
      SaveVsTypes.SpellsOrStaves => victim.SavingThrows.CheckSaveVsSpellStaff(localLevel, victim),
      _ => saved
    };

    return saved;
  }

  public static void AbilityLearnFromSuccess(this SkillData skill, PlayerInstance ch)
  {
    int sn = (int)skill.Id;
    int skillMastery = ch.PlayerData.GetSkillMastery(sn);

    if (ch.IsNpc() || skillMastery <= 0)
      return;

    int adept = skill.RaceAdept.ToList()[(int)ch.CurrentRace];
    int skillLevel = skill.RaceLevel.ToList()[(int)ch.CurrentRace];

    if (skillLevel == 0)
      skillLevel = ch.Level;
    if (skillMastery >= adept) return;
    int schance = skillMastery + 5 * skill.difficulty;
    int percent = SmaugRandom.D100();

    int learn = 1;
    if (percent >= schance)
      learn = 2;
    else if (schance - percent > 25)
      return;

    ch.PlayerData.UpdateSkillMastery(sn, adept.GetLowestOfTwoNumbers(skillMastery + learn));

    int gain;
    if (ch.PlayerData.GetSkillMastery(sn) == adept)
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