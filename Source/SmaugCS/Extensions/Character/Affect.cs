using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Common.Enumerations;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions.Objects;
using SmaugCS.Repository;

namespace SmaugCS.Extensions.Character;

public static class Affect
{
  private static int Depth { get; set; }

  public static void ModifyAffect(this CharacterInstance ch, AffectData affect, bool add)
  {
    int mod = affect.Modifier;
    if (add)
      mod = ModifyAndAddAffect(ch, affect, mod);
    else
    {
      ch.AffectedBy.RemoveBit((int)affect.Type);

      if ((int)affect.Location % Program.REVERSE_APPLY == (int)ApplyTypes.RecurringSpell)
      {
        mod = Math.Abs(mod);
        SkillData skill = Program.RepositoryManager.SKILLS.Values.ToList()[mod];

        if (!Macros.IS_VALID_SN(mod) || skill == null || skill.Type != SkillTypes.Spell)
          throw new InvalidDataException($"RecurringSpell with bad SN {mod}");
        ch.AffectedBy.RemoveBit((int)AffectedByTypes.RecurringSpell);
        return;
      }

      switch ((int)affect.Location % Program.REVERSE_APPLY)
      {
        case (int)ApplyTypes.Affect:
          return;
        case (int)ApplyTypes.ExtendedAffect:
          ch.AffectedBy.RemoveBit(mod);
          return;
        case (int)ApplyTypes.Resistance:
          ch.Resistance.RemoveBit(mod);
          return;
        case (int)ApplyTypes.Immunity:
          ch.Immunity.RemoveBit(mod);
          return;
        case (int)ApplyTypes.Susceptibility:
          ch.Susceptibility.RemoveBit(mod);
          return;
        case (int)ApplyTypes.Remove:
          return;
      }

      mod = 0 - mod;
    }

    ApplyTypes applyType =
      Common.EnumerationExtensions.GetEnum<ApplyTypes>((int)affect.Location % Program.REVERSE_APPLY);
    switch (applyType)
    {
      case ApplyTypes.Strength:
        ch.ModStrength += mod;
        break;
      case ApplyTypes.Dexterity:
        ch.ModDexterity += mod;
        break;
      case ApplyTypes.Intelligence:
        ch.ModIntelligence += mod;
        break;
      case ApplyTypes.Wisdom:
        ch.ModWisdom += mod;
        break;
      case ApplyTypes.Constitution:
        ch.ModConstitution += mod;
        break;
      case ApplyTypes.Charisma:
        ch.ModCharisma += mod;
        break;
      case ApplyTypes.Luck:
        ch.ModLuck += mod;
        break;
      case ApplyTypes.Gender:
        //ch.Gender = (ch.Gender + mod) % 3;
        // TODO Fix this
        //if (ch.Gender < 0)
        //    ch.Gender += 2;
        //ch.Gender = Check.Range(0, ch.Gender, 2);
        break;

      case ApplyTypes.Height:
        ch.Height += mod;
        break;
      case ApplyTypes.Weight:
        ch.Weight += mod;
        break;
      case ApplyTypes.Mana:
        ch.MaximumMana += mod;
        break;
      case ApplyTypes.Hit:
        ch.MaximumHealth += mod;
        break;
      case ApplyTypes.Movement:
        ch.MaximumMovement += mod;
        break;
      case ApplyTypes.ArmorClass:
        ch.ArmorClass += mod;
        break;
      case ApplyTypes.HitRoll:
        ch.HitRoll.SizeOf += mod;
        break;
      case ApplyTypes.DamageRoll:
        ch.DamageRoll.SizeOf += mod;
        break;

      case ApplyTypes.SaveVsPoison:
        ch.SavingThrows.SaveVsPoisonDeath += mod;
        break;
      case ApplyTypes.SaveVsRod:
        ch.SavingThrows.SaveVsWandRod += mod;
        break;
      case ApplyTypes.SaveVsParalysis:
        ch.SavingThrows.SaveVsParalysisPetrify += mod;
        break;
      case ApplyTypes.SaveVsBreath:
        ch.SavingThrows.SaveVsBreath += mod;
        break;
      case ApplyTypes.SaveVsSpell:
        ch.SavingThrows.SaveVsSpellStaff += mod;
        break;

      case ApplyTypes.Affect:
        ch.AffectedBy.Bits[0].SetBit(mod);
        break;
      case ApplyTypes.ExtendedAffect:
        ch.AffectedBy.SetBit(mod);
        break;
      case ApplyTypes.Resistance:
        ch.Resistance.SetBit(mod);
        break;
      case ApplyTypes.Immunity:
        ch.Immunity.SetBit(mod);
        break;
      case ApplyTypes.Susceptibility:
        ch.Susceptibility.SetBit(mod);
        break;
      case ApplyTypes.Remove:
        ch.AffectedBy.Bits[0].RemoveBit(mod);
        break;

      case ApplyTypes.Full:
      case ApplyTypes.Thirst:
      case ApplyTypes.Drunk:
      case ApplyTypes.Blood:
        HandlePlayerCondition(ch, applyType, mod);
        break;

      case ApplyTypes.MentalState:
        ch.MentalState = (ch.MentalState + mod).GetNumberThatIsBetween(-100, 100);
        break;
      case ApplyTypes.Emotion:
        ch.EmotionalState = ch.EmotionalState.GetNumberThatIsBetween(-100, 100);
        break;

      case ApplyTypes.StripSN:
        if (Macros.IS_VALID_SN(mod))
          ch.StripAffects(mod);
        else
          Program.LogManager.Bug("apply_modify: ApplyTypes.StripSN invalid SN %d", mod);
        break;

      case ApplyTypes.WearSpell:
      case ApplyTypes.RemoveSpell:
        if (ch.CurrentRoom.Flags.IsSet(RoomFlags.NoMagic)
            || ch.Immunity.IsSet(ResistanceTypes.Magic)
            || (applyType == ApplyTypes.WearSpell && !add)
            || (applyType == ApplyTypes.RemoveSpell && add)
            || handler.SavingCharacter == ch
            || handler.LoadingCharacter == ch)
          return;

        mod = Math.Abs(mod);
        SkillData skill = Program.RepositoryManager.SKILLS.Values.ToList()[mod];

        if (Macros.IS_VALID_SN(mod) && skill != null && skill.Type == SkillTypes.Spell)
        {
          if (skill.Target == TargetTypes.Ignore ||
              skill.Target == TargetTypes.InventoryObject)
          {
            Program.LogManager.Bug("ApplyTypes.WearSpell trying to apply bad target spell. SN is %d.", mod);
            return;
          }

          ReturnTypes retcode = skill.SpellFunction.Value.Invoke(mod, ch.Level, ch, ch);
          if (retcode == ReturnTypes.CharacterDied || ch.CharDied())
            return;
        }

        break;

      case ApplyTypes.None:
      case ApplyTypes.Class:
      case ApplyTypes.Level:
      case ApplyTypes.Age:
      case ApplyTypes.Gold:
      case ApplyTypes.Experience:
      case ApplyTypes.WeaponSpell:
      case ApplyTypes.Backstab:
      case ApplyTypes.Pick:
      case ApplyTypes.Track:
      case ApplyTypes.Steal:
      case ApplyTypes.Sneak:
      case ApplyTypes.Hide:
      case ApplyTypes.Palm:
      case ApplyTypes.Detrap:
      case ApplyTypes.Dodge:
      case ApplyTypes.Peek:
      case ApplyTypes.Scan:
      case ApplyTypes.Gouge:
      case ApplyTypes.Search:
      case ApplyTypes.Mount:
      case ApplyTypes.Disarm:
      case ApplyTypes.Kick:
      case ApplyTypes.Parry:
      case ApplyTypes.Bash:
      case ApplyTypes.Stun:
      case ApplyTypes.Punch:
      case ApplyTypes.Climb:
      case ApplyTypes.Grip:
      case ApplyTypes.Scribe:
      case ApplyTypes.Brew:
      case ApplyTypes.Dig:
      case ApplyTypes.Cook:
      case ApplyTypes.RecurringSpell:
      case ApplyTypes.Contagious:
      case ApplyTypes.Odor:
      case ApplyTypes.RoomFlag:
      case ApplyTypes.SectorType:
      case ApplyTypes.RoomLight:
      case ApplyTypes.TeleportVnum:
      case ApplyTypes.TeleportDelay:
      default:
        SkillData skillData = Program.RepositoryManager.GetEntity<SkillData>(applyType.GetName());
        if (skillData != null)
          ch.ModifySkill((int)skillData.Type, mod, add);
        else
          Program.LogManager.Bug("affect_modify: unknown location %d", affect.Location);
        break;
    }

    ObjectInstance wield = ch.GetEquippedItem(WearLocations.Wield);
    int strWieldMod = (int)Program.LookupManager.GetStatMod("Strength", ch.GetCurrentStrength(),
      StrengthModTypes.Wield);

    if (ch.IsNpc() || handler.SavingCharacter == ch || wield == null || wield.GetWeight() <= strWieldMod) return;
    if (Depth != 0) return;
    Depth++;
    comm.act(ATTypes.AT_ACTION, "You are too weak to wield $p any longer.", ch, wield, null,
      ToTypes.Character);
    comm.act(ATTypes.AT_ACTION, "$n stops wielding $p.", ch, wield, null, ToTypes.Room);
    ch.Unequip(wield);
    Depth--;
  }

  private static void HandlePlayerCondition(CharacterInstance ch, ApplyTypes applyType, int mod)
  {
    PlayerInstance pc = (PlayerInstance)ch;

    switch (applyType)
    {
      case ApplyTypes.Full:
        if (!ch.IsNpc())
          pc.PlayerData.ConditionTable[ConditionTypes.Full] =
            (pc.PlayerData.ConditionTable[ConditionTypes.Full] + mod).GetNumberThatIsBetween(0, 48);
        break;
      case ApplyTypes.Thirst:
        if (!ch.IsNpc())
          pc.PlayerData.ConditionTable[ConditionTypes.Thirsty] =
            (pc.PlayerData.ConditionTable[ConditionTypes.Thirsty] + mod).GetNumberThatIsBetween(0, 48);
        break;
      case ApplyTypes.Drunk:
        if (!ch.IsNpc())
          pc.PlayerData.ConditionTable[ConditionTypes.Drunk] =
            (pc.PlayerData.ConditionTable[ConditionTypes.Drunk] + mod).GetNumberThatIsBetween(0, 48);
        break;
      case ApplyTypes.Blood:
        if (!ch.IsNpc())
          pc.PlayerData.ConditionTable[ConditionTypes.Bloodthirsty] =
            (pc.PlayerData.ConditionTable[ConditionTypes.Bloodthirsty] + mod).GetNumberThatIsBetween(0, ch.Level + 10);
        break;
      case ApplyTypes.None:
        break;
      case ApplyTypes.Strength:
        break;
      case ApplyTypes.Dexterity:
        break;
      case ApplyTypes.Intelligence:
        break;
      case ApplyTypes.Wisdom:
        break;
      case ApplyTypes.Constitution:
        break;
      case ApplyTypes.Charisma:
        break;
      case ApplyTypes.Luck:
        break;
      case ApplyTypes.Gender:
        break;
      case ApplyTypes.Class:
        break;
      case ApplyTypes.Level:
        break;
      case ApplyTypes.Age:
        break;
      case ApplyTypes.Height:
        break;
      case ApplyTypes.Weight:
        break;
      case ApplyTypes.Mana:
        break;
      case ApplyTypes.Hit:
        break;
      case ApplyTypes.Movement:
        break;
      case ApplyTypes.Gold:
        break;
      case ApplyTypes.Experience:
        break;
      case ApplyTypes.ArmorClass:
        break;
      case ApplyTypes.HitRoll:
        break;
      case ApplyTypes.DamageRoll:
        break;
      case ApplyTypes.SaveVsPoison:
        break;
      case ApplyTypes.SaveVsRod:
        break;
      case ApplyTypes.SaveVsParalysis:
        break;
      case ApplyTypes.SaveVsBreath:
        break;
      case ApplyTypes.SaveVsSpell:
        break;
      case ApplyTypes.Affect:
        break;
      case ApplyTypes.Resistance:
        break;
      case ApplyTypes.Immunity:
        break;
      case ApplyTypes.Susceptibility:
        break;
      case ApplyTypes.WeaponSpell:
        break;
      case ApplyTypes.Backstab:
        break;
      case ApplyTypes.Pick:
        break;
      case ApplyTypes.Track:
        break;
      case ApplyTypes.Steal:
        break;
      case ApplyTypes.Sneak:
        break;
      case ApplyTypes.Hide:
        break;
      case ApplyTypes.Palm:
        break;
      case ApplyTypes.Detrap:
        break;
      case ApplyTypes.Dodge:
        break;
      case ApplyTypes.Peek:
        break;
      case ApplyTypes.Scan:
        break;
      case ApplyTypes.Gouge:
        break;
      case ApplyTypes.Search:
        break;
      case ApplyTypes.Mount:
        break;
      case ApplyTypes.Disarm:
        break;
      case ApplyTypes.Kick:
        break;
      case ApplyTypes.Parry:
        break;
      case ApplyTypes.Bash:
        break;
      case ApplyTypes.Stun:
        break;
      case ApplyTypes.Punch:
        break;
      case ApplyTypes.Climb:
        break;
      case ApplyTypes.Grip:
        break;
      case ApplyTypes.Scribe:
        break;
      case ApplyTypes.Brew:
        break;
      case ApplyTypes.WearSpell:
        break;
      case ApplyTypes.RemoveSpell:
        break;
      case ApplyTypes.Emotion:
        break;
      case ApplyTypes.MentalState:
        break;
      case ApplyTypes.StripSN:
        break;
      case ApplyTypes.Remove:
        break;
      case ApplyTypes.Dig:
        break;
      case ApplyTypes.Cook:
        break;
      case ApplyTypes.RecurringSpell:
        break;
      case ApplyTypes.Contagious:
        break;
      case ApplyTypes.ExtendedAffect:
        break;
      case ApplyTypes.Odor:
        break;
      case ApplyTypes.RoomFlag:
        break;
      case ApplyTypes.SectorType:
        break;
      case ApplyTypes.RoomLight:
        break;
      case ApplyTypes.TeleportVnum:
        break;
      case ApplyTypes.TeleportDelay:
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof(applyType), applyType, null);
    }
  }

  private static int ModifyAndAddAffect(CharacterInstance ch, AffectData affect, int mod)
  {
    ch.AffectedBy.SetBit((int)affect.Type);
    if ((int)affect.Location % Program.REVERSE_APPLY != (int)ApplyTypes.RecurringSpell) return mod;
    mod = Math.Abs(mod);
    SkillData skill = Program.RepositoryManager.SKILLS.Values.ToList()[mod];

    if (Macros.IS_VALID_SN(mod) && skill != null && skill.Type == SkillTypes.Spell)
      ch.AffectedBy.SetBit((int)AffectedByTypes.RecurringSpell);
    else
      throw new InvalidDataException($"RecurringSpell with bad SN {mod}");

    return mod;
  }

  public static void RemoveAffect(this CharacterInstance ch, AffectData paf)
  {
    if (ch.Affects == null || ch.Affects.Count == 0)
      throw new InvalidDataException($"Character {ch.Id} has no affects");

    ch.ModifyAffect(paf, false);

    ch.CurrentRoom?.RemoveAffect(paf);

    ch.Affects.Remove(paf);
  }

  public static void AddAffect(this CharacterInstance ch, AffectData affect)
  {
    ArgumentNullException.ThrowIfNull(affect);

    AffectData newAffect = new()
    {
      Type = affect.Type,
      Duration = affect.Duration,
      Location = affect.Location,
      Modifier = affect.Modifier,
      BitVector = affect.BitVector
    };

    ch.Affects.Add(newAffect);
    ch.ModifyAffect(newAffect, true);

    ch.CurrentRoom?.AddAffect(newAffect);
  }

  public static void StripAffects(this CharacterInstance ch, int sn)
  {
    foreach (AffectData affect in ch.Affects.Where(affect => (int)affect.Type == sn))
      ch.RemoveAffect(affect);
  }

  public static void JoinAffect(this CharacterInstance ch, AffectData paf)
  {
    if (ch.Affects == null || ch.Affects.Count == 0)
      return;

    IEnumerable<AffectData> matchingAffects = ch.Affects.Where(x => x.Type == paf.Type);
    foreach (AffectData affect in matchingAffects)
    {
      paf.Duration = 1000000.GetLowestOfTwoNumbers(paf.Duration + affect.Duration);
      paf.Modifier = paf.Modifier > 0 ? 5000.GetLowestOfTwoNumbers(paf.Modifier + affect.Modifier) : affect.Modifier;
      ch.RemoveAffect(affect);
      break;
    }

    ch.AddAffect(paf);
  }

  public static void aris_affect(this CharacterInstance ch, AffectData paf)
  {
    ch.AffectedBy.SetBits(paf.BitVector);
    switch ((int)paf.Location % Program.REVERSE_APPLY)
    {
      case (int)ApplyTypes.Affect:
        ch.AffectedBy.Bits[0].SetBit(paf.Modifier);
        break;
      case (int)ApplyTypes.Resistance:
        ch.Resistance.SetBit(paf.Modifier);
        break;
      case (int)ApplyTypes.Immunity:
        ch.Immunity.SetBit(paf.Modifier);
        break;
      case (int)ApplyTypes.Susceptibility:
        ch.Susceptibility.SetBit(paf.Modifier);
        break;
    }
  }

  public static void update_aris(this CharacterInstance ch)
  {
    if (ch.IsNpc() || ch.IsImmortal())
      return;

    bool hiding = ch.IsAffected(AffectedByTypes.Hide);

    ch.AffectedBy.ClearBits();
    ch.Resistance = 0;
    ch.Immunity = 0;
    ch.Susceptibility = 0;
    ch.NoAffectedBy.ClearBits();
    ch.NoResistance = 0;
    ch.NoImmunity = 0;
    ch.NoSusceptibility = 0;

    // Race Affects
    RaceData myRace = Program.RepositoryManager.GetRace(ch.CurrentRace);
    ch.AffectedBy.SetBits(myRace.AffectedBy);
    ch.Resistance.SetBit(myRace.Resistance);
    ch.Susceptibility.SetBit(myRace.Susceptibility);

    // Class Affects
    ClassData myClass = Program.RepositoryManager.GetClass(ch.CurrentClass);
    ch.AffectedBy.SetBits(myClass.AffectedBy);
    ch.Resistance.SetBit(myClass.Resistance);
    ch.Susceptibility.SetBit(myClass.Susceptibility);

    // Deity Affects
    if (!ch.IsNpc() && ((PlayerInstance)ch).PlayerData.CurrentDeity != null)
    {
      PlayerInstance pc = (PlayerInstance)ch;

      if (pc.PlayerData.Favor > pc.PlayerData.CurrentDeity.AffectedNum)
        ch.AffectedBy.SetBits(pc.PlayerData.CurrentDeity.Affected);
      if (pc.PlayerData.Favor > pc.PlayerData.CurrentDeity.ElementNum)
        ch.Resistance.SetBit(pc.PlayerData.CurrentDeity.Element);
      if (pc.PlayerData.Favor < pc.PlayerData.CurrentDeity.SusceptNum)
        ch.Susceptibility.SetBit(pc.PlayerData.CurrentDeity.Suscept);
    }

    // Spell Affects
    foreach (AffectData affect in ch.Affects)
      ch.aris_affect(affect);

    // Room Affects
    if (ch.CurrentRoom != null)
    {
      foreach (AffectData affect in ch.CurrentRoom.Affects)
        ch.aris_affect(affect);
    }

    // Equipment Affects
    foreach (ObjectInstance obj in ch.Carrying
               .Where(x => x.WearLocation != WearLocations.None))
    {
      foreach (AffectData affect in obj.Affects)
        ch.aris_affect(affect);

      foreach (AffectData affect in ((ObjectTemplate)obj.Parent).Affects)
        ch.aris_affect(affect);
    }

    // Polymorph Affects
    if (ch.CurrentMorph != null)
    {
      ch.AffectedBy.SetBits(ch.CurrentMorph.AffectedBy);
      ch.Immunity.SetBit(ch.CurrentMorph.immune);
      ch.Resistance.SetBit(ch.CurrentMorph.resistant);
      ch.Susceptibility.SetBit(ch.CurrentMorph.suscept);

      ch.NoAffectedBy.SetBits(ch.CurrentMorph.NotAffectedBy);
      ch.NoImmunity.SetBit(ch.CurrentMorph.no_immune);
      ch.NoResistance.SetBit(ch.CurrentMorph.no_resistant);
      ch.NoSusceptibility.SetBit(ch.CurrentMorph.no_suscept);
    }

    // If the player was hiding before, make them hiding again
    if (hiding)
      ch.AffectedBy.SetBit((int)AffectedByTypes.Hide);
  }
}