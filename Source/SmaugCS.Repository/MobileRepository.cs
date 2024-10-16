﻿using Library.Common;
using Library.Common.Extensions;
using SmaugCS.Common.Enumerations;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Templates;
using System.IO;

namespace SmaugCS.Repository;

public class MobileRepository : Patterns.Repository.Repository<long, MobileTemplate>, ITemplateRepository<MobileTemplate>
{
  private MobileTemplate LastMob { get; set; }

  public MobileTemplate Create(long id, long cloneId, string name)
  {
    Validation.Validate(cloneId >= 1 && cloneId != id && id >= 1 && !name.IsNullOrWhitespace());
    Validation.Validate(() =>
    {
      if (Contains(id))
        throw new DuplicateIndexException("Invalid ID {0}, Index already exists", id);
      if (!Contains(cloneId))
        throw new InvalidDataException($"Clone ID {cloneId} is not present");
    });

    MobileTemplate newMob = Create(id, name);

    MobileTemplate cloneMob = Get(cloneId);
    if (cloneMob != null)
      CloneMobTemplate(newMob, cloneMob);

    return newMob;
  }

  private static void CloneMobTemplate(MobileTemplate newMob, MobileTemplate cloneMob)
  {
    newMob.LongDescription = cloneMob.LongDescription;
    newMob.Description = cloneMob.Description;
    newMob.Statistics[StatisticTypes.ActFlags] = cloneMob.GetStatistic<string>(StatisticTypes.ActFlags);
    newMob.Statistics[StatisticTypes.AffectedByFlags] = cloneMob.GetStatistic<string>(StatisticTypes.AffectedByFlags);
    newMob.SpecialFunction = cloneMob.SpecialFunction;
    newMob.Statistics[StatisticTypes.Alignment] = cloneMob.GetStatistic<int>(StatisticTypes.Alignment);
    newMob.Level = cloneMob.Level;
    newMob.Statistics[StatisticTypes.ToHitArmorClass0] = cloneMob.GetStatistic<int>(StatisticTypes.ToHitArmorClass0);
    newMob.Statistics[StatisticTypes.ArmorClass] = cloneMob.GetStatistic<int>(StatisticTypes.ArmorClass);
    newMob.HitDice = new DiceData(cloneMob.HitDice);
    newMob.DamageDice = new DiceData(cloneMob.DamageDice);
    newMob.Statistics[StatisticTypes.Coin] = cloneMob.GetStatistic<int>(StatisticTypes.Coin);
    newMob.Statistics[StatisticTypes.Experience] = cloneMob.GetStatistic<int>(StatisticTypes.Experience);
    newMob.Position = cloneMob.Position;
    newMob.DefensivePosition = cloneMob.DefensivePosition;
    newMob.Statistics[StatisticTypes.Gender] = cloneMob.GetStatistic<string>(StatisticTypes.Gender);
    newMob.Statistics[StatisticTypes.PermanentStrength] = cloneMob.GetStatistic<int>(StatisticTypes.PermanentStrength);
    newMob.Statistics[StatisticTypes.PermanentDexterity] =
      cloneMob.GetStatistic<int>(StatisticTypes.PermanentDexterity);
    newMob.Statistics[StatisticTypes.PermanentIntelligence] =
      cloneMob.GetStatistic<int>(StatisticTypes.PermanentIntelligence);
    newMob.Statistics[StatisticTypes.PermanentWisdom] = cloneMob.GetStatistic<int>(StatisticTypes.PermanentWisdom);
    newMob.Statistics[StatisticTypes.PermanentCharisma] = cloneMob.GetStatistic<int>(StatisticTypes.PermanentCharisma);
    newMob.Statistics[StatisticTypes.PermanentConstitution] =
      cloneMob.GetStatistic<int>(StatisticTypes.PermanentConstitution);
    newMob.Statistics[StatisticTypes.PermanentLuck] = cloneMob.GetStatistic<int>(StatisticTypes.PermanentLuck);
    newMob.Race = cloneMob.Race;
    newMob.Class = cloneMob.Class;
    newMob.ExtraFlags = cloneMob.ExtraFlags;
    newMob.Resistance = cloneMob.Resistance;
    newMob.Susceptibility = cloneMob.Susceptibility;
    newMob.Immunity = cloneMob.Immunity;
    newMob.Statistics[StatisticTypes.NumberOfAttacks] = cloneMob.GetStatistic<int>(StatisticTypes.NumberOfAttacks);
    newMob.Attacks = cloneMob.Attacks;
    newMob.Defenses = cloneMob.Defenses;
  }

  public MobileTemplate Create(long id, string name)
  {
    Validation.Validate(id >= 1 && !name.IsNullOrWhitespace());
    Validation.Validate(() =>
    {
      if (Contains(id))
        throw new DuplicateIndexException("Invalid ID {0}, Index already exists", id);
    });

    MobileTemplate newMob = new(id, name)
    {
      Statistics =
      {
        [StatisticTypes.PermanentStrength] = 13,
        [StatisticTypes.PermanentDexterity] = 13,
        [StatisticTypes.PermanentIntelligence] = 13,
        [StatisticTypes.PermanentWisdom] = 13,
        [StatisticTypes.PermanentCharisma] = 13,
        [StatisticTypes.PermanentConstitution] = 13,
        [StatisticTypes.PermanentLuck] = 13,
        [StatisticTypes.ActFlags] = $"{ActFlags.IsNpc} {ActFlags.Prototype}"
      }
    };

    Add(id, newMob);
    LastMob = newMob;
    return newMob;
  }
}