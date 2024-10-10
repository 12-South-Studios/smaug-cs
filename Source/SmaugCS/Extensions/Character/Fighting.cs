using System.Linq;
using Autofac;
using SmaugCS.Commands;
using SmaugCS.Commands.Polymorph;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.MudProgs;
using SmaugCS.Repository;

namespace SmaugCS.Extensions.Character;

public static class Fighting
{
  public static ObjectInstance RawKill(this CharacterInstance ch, CharacterInstance victim)
  {
    PlayerInstance victimPc = (PlayerInstance)victim;

    if (victim.IsNotAuthorized())
    {
      Program.LogManager.Bug("Killing unauthorized");
      return null;
    }

    victim.StopFighting(true);

    if (victim.CurrentMorph != null)
    {
      UnmorphChar.do_unmorph_char(victim, string.Empty);
      return ch.RawKill(victim);
    }

    MudProgHandler.ExecuteMobileProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Death, ch, victim);
    if (victim.CharDied())
      return null;

    MudProgHandler.ExecuteRoomProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Death, victim);
    if (victim.CharDied())
      return null;

    ObjectInstance corpse = ObjectFactory.CreateCorpse(victim, ch);
    switch (victim.CurrentRoom.SectorType)
    {
      case SectorTypes.OceanFloor:
      case SectorTypes.Underwater:
      case SectorTypes.ShallowWater:
      case SectorTypes.DeepWater:
        comm.act(ATTypes.AT_BLOOD, "$n's blood slowly clouds the surrounding water.", victim, null, null, ToTypes.Room);
        break;
      case SectorTypes.Air:
        comm.act(ATTypes.AT_BLOOD, "$n's blood sprays wildly through the air.", victim, null, null, ToTypes.Room);
        break;
      default:
        ObjectFactory.CreateBlood(victim);
        break;
    }

    if (victim.IsNpc())
    {
      (victim as MobileInstance).MobIndex.TimesKilled++;
      victim.Extract(true);
      victim = null;
      return corpse;
    }

    victim.SetColor(ATTypes.AT_DIEMSG);
    Help.do_help(victim,
      ((PlayerInstance)victim).PlayerData.PvEDeaths + ((PlayerInstance)victim).PlayerData.PvPDeaths < 3
        ? "new_death"
        : "_DIEMSG_");

    victim.Extract(false);

    while (victim.Affects.Count > 0)
      victim.RemoveAffect(victim.Affects.First());

    RaceData victimRace = Program.RepositoryManager.GetRace(victim.CurrentRace);
    victim.AffectedBy = victimRace.AffectedBy;
    victim.Resistance = 0;
    victim.Susceptibility = 0;
    victim.Immunity = 0;
    victim.CarryWeight = 0;
    victim.ArmorClass = 100 + victimRace.ArmorClassBonus;
    victim.Attacks = victimRace.Attacks;
    victim.Defenses = victimRace.Defenses;
    victim.ModStrength = 0;
    victim.ModDexterity = 0;
    victim.ModWisdom = 0;
    victim.ModIntelligence = 0;
    victim.ModConstitution = 0;
    victim.ModCharisma = 0;
    victim.ModLuck = 0;
    victim.DamageRoll = new DiceData();
    victim.HitRoll = new DiceData();
    victim.MentalState = -10;
    victim.CurrentAlignment = Macros.URANGE(-1000, victim.CurrentAlignment, 1000);
    victim.SavingThrows = victimRace.SavingThrows;
    victim.CurrentPosition = PositionTypes.Resting;
    victim.CurrentHealth = Macros.UMAX(1, victim.CurrentHealth);
    victim.CurrentMana = victim.Level < GetLevelAvatar() ? Macros.UMAX(1, victim.CurrentMana) : 1;
    victim.CurrentMovement = Macros.UMAX(1, victim.CurrentMovement);

    if (victim.Act.IsSet((int)PlayerFlags.Killer))
    {
      victim.Act.RemoveBit((int)PlayerFlags.Killer);
      victim.SendTo("The gods have pardoned you for your murderous acts.");
    }

    if (victim.Act.IsSet((int)PlayerFlags.Thief))
    {
      victim.Act.RemoveBit((int)PlayerFlags.Thief);
      victim.SendTo("The gods have pardoned you for your thievery.");
    }

    victimPc.PlayerData.SetConditionValue(ConditionTypes.Full, 12);
    victimPc.PlayerData.SetConditionValue(ConditionTypes.Thirsty, 12);

    if (victimPc.IsVampire())
      victimPc.PlayerData.SetConditionValue(ConditionTypes.Bloodthirsty, victim.Level / 2);

    // TODO if (IS_SET(sysdata.save_flags, SV_DEATH))
    //      save_char_obj(victim);

    return corpse;
  }

  private static int GetLevelAvatar()
  {
    return GameConstants.GetConstant<int>("level_avatar");
  }

  public static CharacterInstance GetMyTarget(this CharacterInstance ch)
  {
    return ch?.CurrentFighting?.Who;
  }

  public static int ModifyDamageWithResistance(this CharacterInstance ch, int dam, ResistanceTypes ris)
  {
    int modifier = 10;
    if (ch.Immunity.IsSet(ris) && !ch.NoImmunity.IsSet(ris))
      modifier -= 10;
    if (ch.Resistance.IsSet(ris) && !ch.NoResistance.IsSet(ris))
      modifier -= 2;
    if (ch.Susceptibility.IsSet(ris) && !ch.NoSusceptibility.IsSet(ris))
    {
      if (ch.IsNpc() && ch.Immunity.IsSet(ris))
        modifier += 0;
      else
        modifier += 2;
    }

    return modifier switch
    {
      <= 0 => -1,
      10 => dam,
      _ => dam * modifier / 10
    };
  }

  public static void CheckAttackForAttackerFlag(this CharacterInstance ch, CharacterInstance victim)
  {
    if (victim.IsNpc() || victim.Act.IsSet((int)PlayerFlags.Killer) || victim.Act.IsSet((int)PlayerFlags.Thief))
      return;

    if (!ch.IsNpc() && !victim.IsNpc() && ch.CanPKill() && victim.CanPKill())
      return;

    if (ch.IsAffected(AffectedByTypes.Charm))
    {
      if (ch.Master != null) return;
      Program.LogManager.Bug("{0} bad AffectedByTypes.Charm", ch.IsNpc() ? ch.ShortDescription : ch.Name);
      // TODO affect_strip
      ch.AffectedBy.RemoveBit((int)AffectedByTypes.Charm);
      return;

      return;
    }

    if (ch.IsNpc() || ch == victim || ch.Level >= LevelConstants.ImmortalLevel ||
        ch.Act.IsSet((int)PlayerFlags.Attacker) || ch.Act.IsSet((int)PlayerFlags.Killer))
      return;

    ch.Act.SetBit((int)PlayerFlags.Attacker);
    save.save_char_obj(ch);
  }

  public static void StopFighting(this CharacterInstance ch, bool includeMyTargetsTarget)
  {
    EndFight(ch);
    ch.UpdatePositionByCurrentHealth();

    if (!includeMyTargetsTarget) return;
    foreach (CharacterInstance fch in Program.RepositoryManager.CHARACTERS.Values
               .Where(fch => fch.GetMyTarget() == ch))
      fch.StopFighting(false);
  }

  private static void EndFight(CharacterInstance ch)
  {
    if (ch.CurrentFighting != null && ch.CurrentFighting.Who.CharDied())
      --ch.CurrentFighting.Who.NumberFighting;

    ch.CurrentFighting = null;
    ch.CurrentPosition = ch.CurrentMount != null
      ? PositionTypes.Mounted
      : PositionTypes.Standing;

    if (!ch.IsAffected(AffectedByTypes.Berserk)) return;
    // TODO affect_strip
    ch.SetColor(ATTypes.AT_WEAROFF);
    ch.SendTo(Program.RepositoryManager.GetEntity<SkillData>("berserk").WearOffMessage);
    ch.SendTo("\r\n");
  }

  public static void UpdatePositionByCurrentHealth(this CharacterInstance victim)
  {
    if (victim.CurrentHealth > 0)
    {
      if (victim.CurrentPosition <= PositionTypes.Stunned)
        victim.CurrentPosition = PositionTypes.Standing;
      if (victim.IsAffected(AffectedByTypes.Paralysis))
        victim.CurrentPosition = PositionTypes.Stunned;
      return;
    }

    if (victim.IsNpc() || victim.CurrentHealth <= -11)
    {
      if (victim.CurrentMount != null)
      {
        comm.act(ATTypes.AT_ACTION, "$n falls from $N.", victim, null, victim.CurrentMount, ToTypes.Room);
        victim.CurrentMount.Act.RemoveBit((int)ActFlags.Mounted);
        victim.CurrentMount = null;
      }

      victim.CurrentPosition = PositionTypes.Dead;
      return;
    }

    victim.CurrentPosition = victim.CurrentHealth switch
    {
      <= -6 => PositionTypes.Mortal,
      <= -3 => PositionTypes.Incapacitated,
      _ => PositionTypes.Stunned
    };

    if (victim.CurrentPosition > PositionTypes.Stunned && victim.IsAffected(AffectedByTypes.Paralysis))
      victim.CurrentPosition = PositionTypes.Stunned;

    if (victim.CurrentMount == null) return;
    comm.act(ATTypes.AT_ACTION, "$n falls unconcious from $N.", victim, null, victim.CurrentMount, ToTypes.Room);
    victim.CurrentMount.Act.RemoveBit((int)ActFlags.Mounted);
    victim.CurrentMount = null;
  }

  public static int ComputeAlignmentChange(this CharacterInstance ch, CharacterInstance victim)
  {
    int align = ch.CurrentAlignment - victim.CurrentAlignment;
    int divalign = ch.CurrentAlignment > -350 && ch.CurrentAlignment < 350 ? 4 : 20;

    int newAlign = align switch
    {
      > 500 => (ch.CurrentAlignment + (align - 500) / divalign).GetLowestOfTwoNumbers(1000),
      < -500 => (ch.CurrentAlignment + (align + 500) / divalign).GetHighestOfTwoNumbers(-1000),
      _ => ch.CurrentAlignment - ch.CurrentAlignment / divalign
    };

    return newAlign;
  }

  public static int ComputeExperienceGain(this CharacterInstance ch, CharacterInstance victim)
  {
    int xp = victim.GetExperienceWorth() * 0.GetNumberThatIsBetween(victim.Level - ch.Level + 10, 13) / 10;
    int align = ch.CurrentAlignment - victim.CurrentAlignment;

    if (align is > 990 or < -990)
      xp = ModifyXPForAttackingOppositeAlignment(xp);
    else if (ch.CurrentAlignment > 300 && align < 250)
      xp = ModifyXPForGoodPlayerAttackingSameAlignment(xp);

    xp = SmaugRandom.Between((xp * 3) >> 2, (xp * 5) >> 2);

    if (!victim.IsNpc())
      xp /= 4;
    else if (!ch.IsNpc())
      xp = ReduceXPForKillingSameMobRepeatedly(ch, (MobileInstance)victim, xp);

    if (!ch.IsNpc() && ch.Level > 5)
      xp = ModifyXPForExperiencedVsNovicePlayer(ch, xp);

    //// Level based experience gain cap.  Cannot get more experience for
    //// a kill than the amount for your current experience level
    return 0.GetNumberThatIsBetween(xp, ch.GetExperienceLevel(ch.Level + 1) - ch.GetExperienceLevel(ch.Level));
  }

  private static int ModifyXPForAttackingOppositeAlignment(int xp)
  {
    int modXp = xp;
    return (modXp * 5) >> 2;
  }

  private static int ModifyXPForGoodPlayerAttackingSameAlignment(int xp)
  {
    int modXp = xp;
    return (modXp * 3) >> 2;
  }

  private static int ModifyXPForExperiencedVsNovicePlayer(CharacterInstance ch, int xp)
  {
    int modXp = xp;
    int xpRatio = ((PlayerInstance)ch).PlayedDuration / ch.Level;

    switch (xpRatio)
    {
      case > 20000:
        modXp = modXp * 5 / 4; //// 5/4
        break;
      case > 16000:
        modXp = modXp * 3 / 4; //// 3/4
        break;
      case > 10000:
        modXp >>= 1; //// 1/2
        break;
      case > 5000:
        modXp >>= 2; //// 1/4th
        break;
      case > 3500:
        modXp >>= 3; //// 1/8th
        break;
      case > 2000:
        modXp >>= 4; //// 1/16th
        break;
    }

    return modXp;
  }

  private static int ReduceXPForKillingSameMobRepeatedly(CharacterInstance ch, MobileInstance victim, int xp)
  {
    int modXp = xp;
    int times = ((PlayerInstance)ch).TimesKilled(victim);

    if (times > 0 && times < 20)
    {
      modXp = modXp * (20 - times) / 20;
      switch (times)
      {
        case > 15:
          modXp /= 3;
          break;
        case > 10:
          modXp >>= 1;
          break;
      }
    }
    else
      modXp = 0;

    return modXp;
  }
}