using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Autofac;
using Library.Common.Extensions;
using Library.Common.Objects;
using Patterns.Repository;
using SmaugCS.Commands;
using SmaugCS.Commands.Admin;
using SmaugCS.Commands.Movement;
using SmaugCS.Commands.Social;
using SmaugCS.Common;
using SmaugCS.Common.Enumerations;
using SmaugCS.Constants;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Exceptions;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Interfaces;
using SmaugCS.Data.Templates;
using SmaugCS.Exceptions;
using SmaugCS.Extensions.Objects;
using SmaugCS.Extensions.Player;
using SmaugCS.MudProgs;
using SmaugCS.Repository;
using CheckFunctions = SmaugCS.Helpers.CheckFunctions;

namespace SmaugCS.Extensions.Character;

public static class CharacterInstanceExtensions
{
  public static void Printf(this CharacterInstance ch, string fmt, params object[] args)
  {
    ch.SendTo(string.Format(fmt, args));
  }

  public static void PagerPrintf(this CharacterInstance ch, string fmt, params object[] args)
  {
    ch.SendToPager(string.Format(fmt, args));
  }

  public static void PrintfColor(this CharacterInstance ch, string fmt, params object[] args)
  {
    ch.SendTo(string.Format(fmt, args));
  }

  public static void PagerPrintfColor(this CharacterInstance ch, string fmt, params object[] args)
  {
    ch.SendToPager(string.Format(fmt, args));
  }

  public static void SendToPagerColor(this CharacterInstance ch, string txt)
  {
    ch.SendToPager(txt);
  }

  public static void SendToPager(this CharacterInstance ch, string txt)
  {
    if (ch == null || ch.IsNpc() || string.IsNullOrEmpty(txt))
      return;

    PlayerInstance pch = (PlayerInstance)ch;
    if (pch.Descriptor == null)
      return;

    PlayerInstance och = pch.Descriptor.Original ?? pch.Descriptor.Character;
    if (och.IsNpc() || !och.PlayerData.Flags.IsSet(PCFlags.PagerOn))
      pch.Descriptor.SendTo(txt);
    else
      pch.Descriptor.WriteToPager(color.colorize(txt, pch.Descriptor), 0);
  }

  public static void SetPagerColor(this CharacterInstance ch, ATTypes attype)
  {
    if (ch == null || ch.IsNpc())
      return;

    PlayerInstance pch = (PlayerInstance)ch;
    if (pch.Descriptor == null)
      return;

    pch.Descriptor.WriteToPager(color.color_str(attype, ch), 0);
    pch.Descriptor.PageColor = pch.Colors.GetValueOrDefault(attype, (char)0);
  }

  public static void SetColor(this CharacterInstance ch, ATTypes attype)
  {
    if (ch == null || ch.IsNpc())
      return;

    PlayerInstance pch = (PlayerInstance)ch;
    if (pch.Descriptor == null)
      return;

    pch.Descriptor.WriteToBuffer(color.color_str(attype, ch), 0);

    if (pch.Colors.TryGetValue(attype, out char value))
      pch.Descriptor.PageColor = value;
  }

  public static void SendTo(this CharacterInstance ch, string txt)
  {
    if (ch.IsNpc() || string.IsNullOrEmpty(txt))
      return;

    PlayerInstance pch = (PlayerInstance)ch;
    if (pch.Descriptor != null && !string.IsNullOrEmpty(txt))
      pch.Descriptor.WriteToBuffer(color.colorize(txt, pch.Descriptor), 0);
  }

  public static bool IsDeadly(this CharacterInstance ch)
  {
    if (ch.IsNpc()) return false;

    PlayerInstance instance = ch as PlayerInstance;
    if (instance == null) return false;

    PlayerInstance player = instance;
    return player.PlayerData != null && player.PlayerData.Flags.IsSet(PCFlags.Deadly);
  }

  public static bool IsPacifist(this CharacterInstance ch)
  {
    return ch.IsNpc() && ch.Act.IsSet((int)ActFlags.Pacifist);
  }

  public static bool IsBlind(this CharacterInstance ch)
  {
    if (!ch.IsNpc() && ch.Act.IsSet((int)PlayerFlags.HolyLight))
      return true;
    if (ch.IsAffected(AffectedByTypes.TrueSight))
      return true;
    return !ch.IsAffected(AffectedByTypes.Blind);
  }

  public static void Extract(this CharacterInstance ch, bool fPull)
  {
    if (ch?.CurrentRoom == null) return;
    if (ch == db.Supermob) return;
    if (ch.CharDied()) return;

    if (ch == handler.CurrentCharacter)
      handler.CurrentCharacterDied = true;

    handler.queue_extracted_char(ch, fPull);

    foreach (RelationData relation in db.RELATIONS
               .Where(relation => fPull && relation.Types == RelationTypes.MSet_On))
    {
      if (ch == (CharacterInstance)relation.Subject)
        relation.Actor.CastAs<CharacterInstance>().DestinationBuffer = null;
      else if (ch != (CharacterInstance)relation.Actor)
        continue;

      db.RELATIONS.Remove(relation);
    }

    //trworld_char_check(ch);

    if (fPull)
      ch.DieFollower();

    ch.StopFighting(true);

    if (ch.CurrentMount != null)
    {
      reset.update_room_reset(ch, true);
      ch.CurrentMount.Act.RemoveBit((int)ActFlags.Mounted);
      ch.CurrentMount = null;
      ch.CurrentPosition = PositionTypes.Standing;
    }

    if (ch.IsNpc())
    {
      ch.CurrentMount.Act.RemoveBit((int)ActFlags.Mounted);
      foreach (
        CharacterInstance wch in
        Program.RepositoryManager.CHARACTERS.Values.Where(wch => wch.CurrentMount == ch))
      {
        wch.CurrentMount = null;
        wch.CurrentPosition = PositionTypes.Standing;
        if (wch.CurrentRoom == ch.CurrentRoom)
        {
          comm.act(ATTypes.AT_SOCIAL, "Your faithful mount collapses beneath you...", wch, null, ch,
            ToTypes.Character);
          comm.act(ATTypes.AT_SOCIAL, "Sadly you dismount $M for the last time.", wch, null, ch,
            ToTypes.Character);
          comm.act(ATTypes.AT_PLAIN, "$n sadly dismounts $N for the last time.", wch, null, ch,
            ToTypes.Room);
        }

        if (!wch.IsNpc() && ((PlayerInstance)wch).PlayerData != null && ((PlayerInstance)wch).PlayerData.Pet == ch)
        {
          ((PlayerInstance)wch).PlayerData.Pet = null;
          if (wch.CurrentRoom == ch.CurrentRoom)
            comm.act(ATTypes.AT_SOCIAL, "You mourn for the loss of $N.", wch, null, ch,
              ToTypes.Character);
        }
      }
    }

    ObjectInstance lastObj = ch.Carrying.Last();
    lastObj?.Extract();

    ch.CurrentRoom.RemoveFrom(ch);

    if (!fPull)
    {
      RoomTemplate location = null;
      // TODO fix
      //if (!ch.IsNpc() && ((PlayerInstance)ch).PlayerData.Clan != null)
      //    location = Program.RepositoryManager.ROOMS.CastAs<Repository<long, RoomTemplate>>().Get(((PlayerInstance)ch).PlayerData.Clan.RecallRoom);

      if (location == null)
        location = Program.RepositoryManager.ROOMS.CastAs<Repository<long, RoomTemplate>>()
          .Get(VnumConstants.ROOM_VNUM_ALTAR);

      if (location == null)
        location = Program.RepositoryManager.ROOMS.CastAs<Repository<long, RoomTemplate>>().Get(1);

      location.AddTo(ch);

      CharacterInstance wch = ch.GetCharacterInRoom("healer");
      if (wch != null)
      {
        comm.act(ATTypes.AT_MAGIC, "$n mutters a few incantations, waves $s hands and points $s finger.",
          wch, null, null, ToTypes.Room);
        comm.act(ATTypes.AT_MAGIC, "$n appears from some strange swilring mists!", ch, null, null,
          ToTypes.Room);
        Say.do_say(wch,
          $"Welcome back to the land of the living, {ch.Name.CapitalizeFirst()}");
      }
      else
        comm.act(ATTypes.AT_MAGIC, "$n appears from some strange swirling mists!", ch, null, null,
          ToTypes.Room);

      ch.CurrentPosition = PositionTypes.Resting;
    }

    if (ch.IsNpc())
      --((MobileInstance)ch).MobIndex.Count;

    if (!ch.IsNpc() && ((PlayerInstance)ch).Descriptor?.Original != null)
      Return.do_return(ch, "");

    if (((PlayerInstance)ch.Switched)?.Descriptor != null)
      Return.do_return(ch.Switched, "");

    foreach (CharacterInstance wch in Program.RepositoryManager.CHARACTERS.Values)
    {
      if (((PlayerInstance)wch).ReplyTo == ch)
        ((PlayerInstance)wch).ReplyTo = null;
      if (((PlayerInstance)wch).RetellTo == ch)
        ((PlayerInstance)wch).RetellTo = null;
    }

    Program.RepositoryManager.CHARACTERS.CastAs<Repository<long, CharacterInstance>>().Delete(ch.Id);

    if (ch.IsNpc() || ((PlayerInstance)ch).Descriptor == null) return;
    if (((PlayerInstance)ch).Descriptor.Character != ch) return;
    ((PlayerInstance)ch).Descriptor.Character = null;
    // TODO Close the socket
    ((PlayerInstance)ch).Descriptor = null;
  }

  public static bool Chance(this CharacterInstance ch, int percent)
  {
    return SmaugRandom.D100() - ch.GetCurrentLuck() + 13 - (10 - Math.Abs(ch.MentalState)) +
      (ch.IsDevoted() ? ((PlayerInstance)ch).PlayerData.Favor / -500 : 0) <= percent;
  }

  public static int GetVampArmorClass(this CharacterInstance ch, IGameManager gameManager = null)
  {
    if (!ch.IsVampire() || !ch.IsOutside()) return 0;

    ArmorClassAttribute attrib =
      (gameManager ?? Program.GameManager).GameTime.Sunlight.GetAttribute<ArmorClassAttribute>();
    return attrib.ModValue;
  }

  public static bool CanGo(this CharacterInstance ch, DirectionTypes direction)
  {
    ExitData exit = ch.CurrentRoom.GetExit((int)direction);
    return exit != null && exit.Destination > 0 && !exit.Flags.IsSet(ExitFlags.Closed);
  }

  public static bool CanUseSkill(this CharacterInstance ch, int percent, int skillId,
    IRepositoryManager dbManager = null)
  {
    SkillData skill = (dbManager ?? Program.RepositoryManager).SKILLS.Get(skillId);
    if (skill == null)
      throw new EntryNotFoundException("Skill {0} not found", skillId);

    return CanUseSkill(ch, percent, skill);
  }

  [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
  public static bool CanUseSkill(this CharacterInstance ch, int percent, SkillData skill)
  {
    bool check = false;

    if (ch.IsNpc() && percent < 85)
      check = true;
    else if (!ch.IsNpc() && percent < Macros.LEARNED(ch, (int)skill.Id))
      check = true;
    else if (ch.CurrentMorph?.Morph != null && ch.CurrentMorph.Morph.skills.IsAnyEqual(skill.Name) && percent < 85)
      check = true;

    if (ch.CurrentMorph?.Morph != null && ch.CurrentMorph.Morph.no_skills.IsAnyEqual(skill.Name))
      check = false;

    return check;
  }

  public static void AddKill(this PlayerInstance ch, MobileInstance mob)
  {
    if (ch.IsNpc() || !mob.IsNpc())
      return;

    long id = mob.MobIndex.Id;
    int maxTrack = GameConstants.GetConstant<int>("MaxKillTrack");

    KilledData killed = ch.PlayerData.Killed.FirstOrDefault(x => x.ID == id);
    if (killed == null)
    {
      if (ch.PlayerData.Killed.Count < maxTrack) return;
      KilledData oldest = null;
      foreach (KilledData check in ch.PlayerData.Killed)
      {
        if (oldest == null)
          oldest = check;
        else if (oldest != check)
        {
          if (check.Updated < oldest.Updated)
            oldest = check;
        }
      }

      ch.PlayerData.Killed.Remove(oldest);

      KilledData newKilled = new(id);
      newKilled.Increment(1);
      ch.PlayerData.Killed.Add(newKilled);
    }
    else
      killed.Increment(1);
  }

  public static int TimesKilled(this PlayerInstance ch, MobileInstance mob)
  {
    if (ch.IsNpc() || mob == null || !mob.IsNpc())
      return 0;

    return ch.PlayerData.Killed.Any(x => x.ID == mob.MobIndex.Id)
      ? ch.PlayerData.Killed.First(x => x.ID == mob.MobIndex.Id).Count
      : 0;
  }

  public static bool IsAttackSuppressed(this CharacterInstance ch)
  {
    if (ch.IsNpc()) return false;

    TimerData timer = ch.GetTimer(TimerTypes.ASupressed);
    if (timer == null) return false;
    if (timer.Value == -1) return true;
    return timer.Count >= 1;
  }

  public static bool IsWieldedWeaponPoisoned(this CharacterInstance ch)
  {
    if (fight.UsedWeapon == null) return false;

    ObjectInstance obj = ch.GetEquippedItem(WearLocations.Wield);
    if (obj != null && fight.UsedWeapon == obj && obj.ExtraFlags.IsSet((int)ItemExtraFlags.Poisoned))
      return true;

    obj = ch.GetEquippedItem(WearLocations.DualWield);
    return obj != null && fight.UsedWeapon == obj && obj.ExtraFlags.IsSet((int)ItemExtraFlags.Poisoned);
  }

  public static void ImproveMentalState(this CharacterInstance ch, int mod)
  {
    int c = 0.GetNumberThatIsBetween(Math.Abs(mod), 20);
    int con = ch.GetCurrentConstitution();

    c += SmaugRandom.D100() < con ? 1 : 0;

    ch.MentalState = ch.MentalState switch
    {
      < 0 => -100.GetNumberThatIsBetween(ch.MentalState + c, 0),
      > 0 => 0.GetNumberThatIsBetween(ch.MentalState - c, 100),
      _ => ch.MentalState
    };
  }

  public static void WorsenMentalState(this PlayerInstance ch, int mod)
  {
    int c = 0.GetNumberThatIsBetween(Math.Abs(mod), 20);
    int con = ch.GetCurrentConstitution();

    c -= SmaugRandom.D100() < con ? 1 : 0;
    if (c < 1) return;

    if (!ch.IsNpc()
        && ch.PlayerData.Nuisance != null
        && ch.PlayerData.Nuisance.Flags > 2)
      c += (int)(0.4f * ((ch.PlayerData.Nuisance.Flags - 2) * ch.PlayerData.Nuisance.Power));

    switch (ch.MentalState)
    {
      case < 0:
        ch.MentalState = -100.GetNumberThatIsBetween(ch.MentalState - c, 100);
        break;
      case > 0:
        ch.MentalState = -100.GetNumberThatIsBetween(ch.MentalState + c, 100);
        break;
      default:
        ch.MentalState -= c;
        break;
    }
  }


  public static bool CanAstral(this CharacterInstance ch, CharacterInstance victim)
  {
    return victim != ch && victim.CurrentRoom != null &&
           !victim.CurrentRoom.Flags.IsSet(RoomFlags.Private) &&
           !victim.CurrentRoom.Flags.IsSet(RoomFlags.Solitary) &&
           !victim.CurrentRoom.Flags.IsSet(RoomFlags.NoAstral) &&
           !victim.CurrentRoom.Flags.IsSet(RoomFlags.Death) &&
           !victim.CurrentRoom.Flags.IsSet(RoomFlags.Prototype) && victim.Level < ch.Level + 15 &&
           (!victim.CanPKill() || ch.IsNpc() || ch.CanPKill()) &&
           (!victim.IsNpc() || !victim.Act.IsSet((int)ActFlags.Prototype)) &&
           (!victim.IsNpc() || !victim.SavingThrows.CheckSaveVsSpellStaff(ch.Level, victim)) &&
           (!victim.CurrentRoom.Area.Flags.IsSet(AreaFlags.NoPlayerVsPlayer) || !ch.IsPKill());
  }

  public static bool CanDrop(this CharacterInstance ch, ObjectInstance obj)
  {
    if (!obj.ExtraFlags.IsSet((int)ItemExtraFlags.NoDrop))
      return true;
    if (!ch.IsNpc() && ch.Level >= LevelConstants.ImmortalLevel)
      return true;
    return ch.IsNpc() && ((MobileInstance)ch).MobIndex.Id == VnumConstants.MOB_VNUM_SUPERMOB;
  }

  public static bool CanSee(this CharacterInstance ch, ObjectInstance obj)
  {
    if (!ch.IsNpc() && ch.Act.IsSet((int)PlayerFlags.HolyLight))
      return true;
    if (ch.IsNpc() && ((MobileInstance)ch).MobIndex.Id == VnumConstants.MOB_VNUM_SUPERMOB)
      return true;
    if (obj.ExtraFlags.IsSet((int)ItemExtraFlags.Buried))
      return false;
    if (obj.ExtraFlags.IsSet((int)ItemExtraFlags.Hidden))
      return false;
    if (ch.IsAffected(AffectedByTypes.TrueSight))
      return true;
    if (ch.IsAffected(AffectedByTypes.Blind))
      return false;

    //// Can see lights in the dark
    if (obj.ItemType == ItemTypes.Light && obj.Value.ToList()[2] != 0)
      return true;

    if (!ch.CurrentRoom.IsDark())
      return !obj.ExtraFlags.IsSet((int)ItemExtraFlags.Invisible) || ch.IsAffected(AffectedByTypes.DetectInvisibility);
    //// Can see glowing items in teh dark, invisible or not
    if (obj.ExtraFlags.IsSet((int)ItemExtraFlags.Glow))
      return true;
    if (!ch.IsAffected(AffectedByTypes.Infrared))
      return false;

    return !obj.ExtraFlags.IsSet((int)ItemExtraFlags.Invisible) || ch.IsAffected(AffectedByTypes.DetectInvisibility);
  }

  public static bool CanSee(this CharacterInstance ch, CharacterInstance victim)
  {
    if (victim == null)
      return false;

    if (ch == null)
      return !victim.IsAffected(AffectedByTypes.Invisible) && !victim.IsAffected(AffectedByTypes.Hide) &&
             !victim.Act.IsSet((int)PlayerFlags.WizardInvisibility);

    if (ch == victim)
      return true;

    if (!victim.IsNpc() && victim.Act.IsSet((int)PlayerFlags.WizardInvisibility)
                        && ch.Trust < ((PlayerInstance)victim).PlayerData.WizardInvisible)
      return false;

    if (victim.IsNpc() && victim.Act.IsSet((int)ActFlags.MobInvisibility)
                       && victim.IsPKill() && victim.Timer > 1 && ((PlayerInstance)victim).Descriptor == null)
      return false;

    if (!ch.IsNpc() && ch.Act.IsSet((int)PlayerFlags.HolyLight))
      return true;

    if (!ch.IsAffected(AffectedByTypes.TrueSight))
    {
      if (ch.IsAffected(AffectedByTypes.Blind))
        return false;
      if (ch.CurrentRoom.IsDark() && !ch.IsAffected(AffectedByTypes.Infrared))
        return false;
      if (victim.IsAffected(AffectedByTypes.Invisible) && !ch.IsAffected(AffectedByTypes.DetectInvisibility))
        return false;
      if (victim.IsAffected(AffectedByTypes.Hide) && !ch.IsAffected(AffectedByTypes.DetectHidden)
                                                  && victim.CurrentFighting == null
                                                  && (ch.IsNpc() ? !victim.IsNpc() : victim.IsNpc()))
        return false;
    }

    if (!victim.IsNotAuthorized()) return true;
    if (ch.IsNotAuthorized() || ch.IsImmortal() || ch.IsNpc())
      return true;
    return ((PlayerInstance)ch).PlayerData.Council != null &&
           ((PlayerInstance)ch).PlayerData.Council.Name.EqualsIgnoreCase("Newbie Council");

  }

  public static bool IsAllowedToUseObject(this CharacterInstance ch, ObjectInstance obj)
  {
    if (obj.ExtraFlags.IsSet((int)ItemExtraFlags.AntiWarrior))
    {
      if (ch.CurrentClass is ClassTypes.Warrior or ClassTypes.Paladin or ClassTypes.Ranger)
        return false;
    }

    if (obj.ExtraFlags.IsSet((int)ItemExtraFlags.AntiMage))
    {
      if (ch.CurrentClass == ClassTypes.Mage
          || ch.CurrentClass == ClassTypes.Augurer)
        return false;
    }

    if (obj.ExtraFlags.IsSet((int)ItemExtraFlags.AntiThief))
    {
      if (ch.CurrentClass == ClassTypes.Thief)
        return false;
    }

    if (obj.ExtraFlags.IsSet((int)ItemExtraFlags.AntiDruid))
    {
      if (ch.CurrentClass == ClassTypes.Druid)
        return false;
    }

    if (obj.ExtraFlags.IsSet((int)ItemExtraFlags.AntiCleric))
    {
      if (ch.CurrentClass == ClassTypes.Cleric)
        return false;
    }

    if (obj.ExtraFlags.IsSet((int)ItemExtraFlags.AntiVampire))
    {
      if (ch.CurrentClass == ClassTypes.Vampire)
        return false;
    }

    return true;
  }

  public static bool CanWear(this CharacterInstance ch, ObjectInstance obj, ItemWearFlags wearFlag)
  {
    return obj.WearFlags.IsSet((int)wearFlag);
  }

  public static bool CanWearLayer(this CharacterInstance ch, ObjectInstance obj, WearLocations location)
  {
    int bitlayers = 0;
    int objlayers = obj.ObjectIndex.Layers;

    foreach (ObjectInstance otmp in ch.Carrying.Where(otmp => otmp.WearLocation == location))
    {
      if (otmp.ObjectIndex.Layers == 0)
        return false;
      bitlayers |= otmp.ObjectIndex.Layers;
    }

    if ((bitlayers > 0 && objlayers == 0) || bitlayers > objlayers)
      return false;

    return bitlayers == 0 || (bitlayers & ~objlayers) == bitlayers;
  }

  public static bool CouldDualWield(this CharacterInstance ch)
  {
    return ch.IsNpc() ||
           ((PlayerInstance)ch).PlayerData.GetSkillMastery(Program.RepositoryManager.GetEntity<SkillData>("dual wield")
             .Id) > 0;
  }

  public static bool CanDualWield(this CharacterInstance ch)
  {
    bool wield = false;
    bool nwield = false;

    if (!CouldDualWield(ch))
      return false;
    if (ch.GetEquippedItem(WearLocations.Wield) != null)
      wield = true;

    if (ch.GetEquippedItem(WearLocations.WieldMissile) != null
        || ch.GetEquippedItem(WearLocations.DualWield) != null)
      nwield = true;

    if (CheckFunctions.CheckIfTrue(ch, wield && nwield,
          "You are already wielding two weapons... grow some more arms!")) return false;
    if (CheckFunctions.CheckIfTrue(ch, (wield || nwield) && ch.GetEquippedItem(WearLocations.Shield) != null,
          "You cannot dual wield, you're already holding a shield!")) return false;
    return !CheckFunctions.CheckIfTrue(ch, (wield || nwield) && ch.GetEquippedItem(WearLocations.Hold) != null,
      "You cannot hold another weapon, you're already holding something in that hand!");
  }

  public static ObjectInstance HasKey(this CharacterInstance ch, int key)
  {
    foreach (ObjectInstance obj in ch.Carrying)
    {
      if (obj.ObjectIndex.Id == key ||
          (obj.ItemType == ItemTypes.Key && obj.Value.ToList()[0] == key))
        return obj;
      if (obj.ItemType != ItemTypes.KeyRing) continue;
      if (obj.Contents.Any(obj2 => obj.ObjectIndex.Id == key || obj2.Value.ToList()[0] == key))
        return obj;
    }

    return null;
  }

  public static bool CanMorph(this CharacterInstance ch, MorphData morph, bool isCast)
  {
    if (morph == null)
      return false;
    if (!ch.IsImmortal() && !ch.IsNpc())
      return false;
    if (morph.no_cast && isCast)
      return false;
    if (ch.Level < morph.level)
      return false;
    if (morph.pkill == Program.ONLY_PKILL && !ch.IsPKill())
      return false;
    if (morph.pkill == Program.ONLY_PEACEFULL && ch.IsPKill())
      return false;
    if (morph.sex != -1 && morph.sex != (int)ch.Gender)
      return false;
    if (morph.Class != 0 && !morph.Class.IsSet(1 << (int)ch.CurrentClass))
      return false;
    if (morph.race != 0 && morph.race.IsSet(1 << (int)ch.CurrentRace))
      return false;
    if (!string.IsNullOrWhiteSpace(morph.deity) &&
        (((PlayerInstance)ch).PlayerData.CurrentDeity != null ||
         Program.RepositoryManager.GetEntity<DeityData>(morph.deity) == null))
      return false;
    if (morph.timeto == -1 || morph.timefrom == -1)
      return morph.dayfrom == -1 || morph.dayto == -1 ||
             (morph.dayto >= Program.GameManager.GameTime.Day + 1 &&
              morph.dayfrom <= Program.GameManager.GameTime.Day + 1);
    bool found = false;
    int tmp, i;

    for (i = 0, tmp = morph.timefrom; i < 25 && tmp != morph.timeto; i++)
    {
      if (tmp == Program.GameManager.GameTime.Hour)
      {
        found = true;
        break;
      }

      if (tmp == 23)
        tmp = 0;
      else
        tmp++;
    }

    if (!found)
      return false;

    return morph.dayfrom == -1 || morph.dayto == -1 ||
           (morph.dayto >= Program.GameManager.GameTime.Day + 1 &&
            morph.dayfrom <= Program.GameManager.GameTime.Day + 1);
  }

  public static bool CanCharm(this CharacterInstance ch)
  {
    if (ch.IsNpc() || ch.IsImmortal())
      return true;
    return ch.GetCurrentCharisma() / 3 + 1 > ((PlayerInstance)ch).PlayerData.NumberOfCharmies;
  }

  public static bool IsInArena(this CharacterInstance ch)
  {
    if (ch.CurrentRoom.Flags.IsSet(RoomFlags.Arena)) return true;
    if (ch.CurrentRoom.Area.Flags.IsSet(AreaFlags.FreeKill)) return true;
    return ch.CurrentRoom.Id is >= 29 and <= 43 || ch.CurrentRoom.Area.Name.EqualsIgnoreCase("arena");
  }

  public static bool IsInCombatPosition(this CharacterInstance ch)
  {
    return ch.CurrentPosition is PositionTypes.Fighting or PositionTypes.Evasive 
      or PositionTypes.Defensive or PositionTypes.Aggressive or PositionTypes.Berserk;
  }

  public static bool IsImmune(this CharacterInstance ch, ResistanceTypes type)
  {
    return type != ResistanceTypes.Unknown && ch.Immunity.IsSet(type);
  }

  public static bool IsImmune(this CharacterInstance ch, SpellDamageTypes type,
    ILookupManager lookupManager = null)
  {
    ResistanceTypes resType = (lookupManager ?? Program.LookupManager).GetResistanceType(type);

    return resType != ResistanceTypes.Unknown && ch.Immunity.IsSet(resType);
  }

  public static bool IsIgnoring(this CharacterInstance ch, CharacterInstance victim)
  {
    // TODO
    return false;
  }

  public static bool IsRetired(this PlayerInstance ch)
  {
    return ch.PlayerData != null && ch.PlayerData.Flags.IsSet(PCFlags.Retired);
  }

  public static bool IsGuest(this PlayerInstance ch)
  {
    return ch.PlayerData != null && ch.PlayerData.Flags.IsSet(PCFlags.Guest);
  }

  public static bool CanCast(this CharacterInstance ch, IRepositoryManager dbManager = null)
  {
    ClassData cls = (dbManager ?? Program.RepositoryManager).GetClass(ch.CurrentClass);

    return cls.IsSpellcaster;
  }

  public static bool IsVampire(this CharacterInstance ch)
  {
    return !ch.IsNpc() && ch.CurrentRace == RaceTypes.Vampire || ch.CurrentClass == ClassTypes.Vampire;
  }

  public static bool IsGood(this CharacterInstance ch)
  {
    return ch.CurrentAlignment >= 350;
  }

  public static bool IsEvil(this CharacterInstance ch)
  {
    return ch.CurrentAlignment <= -350;
  }

  public static bool IsNeutral(this CharacterInstance ch)
  {
    return !ch.IsGood() && !ch.IsEvil();
  }

  public static bool IsAwake(this CharacterInstance ch)
  {
    return ch.CurrentPosition > PositionTypes.Sleeping;
  }

  public static bool IsOutside(this CharacterInstance ch)
  {
    return !ch.CurrentRoom.Flags.IsSet(RoomFlags.Indoors) &&
           !ch.CurrentRoom.Flags.IsSet(RoomFlags.Tunnel);
  }

  public static bool IsDrunk(this CharacterInstance ch, int drunk)
  {
    if (ch.IsNpc()) return false;

    PlayerInstance pch = (PlayerInstance)ch;
    return SmaugRandom.D100() < (pch.GetCondition(ConditionTypes.Drunk) & 2 / drunk);
  }

  public static bool IsDevoted(this CharacterInstance ch)
  {
    if (ch.IsNpc()) return false;

    PlayerInstance pch = (PlayerInstance)ch;
    return pch.PlayerData.CurrentDeity != null;
  }

  public static bool IsIdle(this CharacterInstance ch)
  {
    if (ch.IsNpc()) return false;

    PlayerInstance pch = (PlayerInstance)ch;
    return pch.PlayerData != null && pch.PlayerData.Flags.IsSet(PCFlags.Idle);
  }

  public static bool IsPKill(this CharacterInstance ch)
  {
    if (ch.IsNpc()) return false;

    PlayerInstance pch = (PlayerInstance)ch;
    return pch.PlayerData != null && pch.PlayerData.Flags.IsSet(PCFlags.Deadly);
  }

  public static bool CanPKill(this CharacterInstance ch)
  {
    if (ch.IsNpc()) return false;

    PlayerInstance pch = (PlayerInstance)ch;
    return pch.IsPKill() && pch.Level >= 5 && pch.CalculateAge() >= 18;
  }

  public static bool HasBodyPart(this CharacterInstance ch, int part)
  {
    return ch.ExtraFlags == 0 || ch.ExtraFlags.IsSet(part);
  }

  [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
  public static bool IsAffectedBy(this CharacterInstance ch, int sn)
  {
    return ch.Affects.ToList().Exists(x => x.SkillNumber == sn);
  }

  public static bool IsNotAuthorized(this CharacterInstance ch)
  {
    if (ch.IsNpc())
      return false;
    bool hasAuthState = ((PlayerInstance)ch).PlayerData == null ||
                        ((PlayerInstance)ch).PlayerData.AuthState != AuthorizationStates.Authorized;
    bool isUnauthed = ((PlayerInstance)ch).PlayerData == null ||
                      ((PlayerInstance)ch).PlayerData.Flags.IsSet(PCFlags.Unauthorized);
    return hasAuthState && isUnauthed;
  }

  public static bool IsWaitingForAuthorization(this CharacterInstance ch)
  {
    if (ch.IsNpc())
      return false;
    return ((PlayerInstance)ch).Descriptor != null && ((PlayerInstance)ch).PlayerData.AuthState ==
                                                   AuthorizationStates.Waiting
                                                   && ((PlayerInstance)ch).PlayerData.Flags.IsSet(PCFlags.Unauthorized);
  }

  public static bool IsCircleFollowing(this CharacterInstance ch, CharacterInstance victim)
  {
    CharacterInstance tmp;
    do
    {
      tmp = ch.Master;
      if (tmp == ch)
        return true;
    } while (tmp != null);

    return false;
  }

  public static int GetArmorClass(this CharacterInstance ch)
  {
    return ch.ArmorClass + (ch.IsAwake()
      ? LookupConstants.dex_app[ch.GetCurrentDexterity()].defensive
      : 0) + ch.GetVampArmorClass();
  }

  public static int GetHitroll(this CharacterInstance ch)
  {
    int toHitMod = (int)Program.LookupManager.GetStatMod("Strength", ch.GetCurrentStrength(),
      StrengthModTypes.ToHit);
    return ch.HitRoll.SizeOf + toHitMod + (2 - Math.Abs(ch.MentalState) / 10);
  }

  public static int GetDamroll(this CharacterInstance ch)
  {
    int toDamMod = (int)Program.LookupManager.GetStatMod("Strength", ch.GetCurrentStrength(),
      StrengthModTypes.ToDam);
    return ch.DamageRoll.SizeOf + ch.DamageRoll.Bonus + toDamMod +
           (ch.MentalState is > 5 and < 15 ? 1 : 0);
  }

  // TODO fix
  //public static bool IsClanned(this CharacterInstance ch)
  //{
  //    if (ch.IsNpc())
  //        return false;
  //    return ((PlayerInstance)ch).PlayerData.Clan != null &&
  //        ((PlayerInstance)ch).PlayerData.Clan.ClanType != ClanTypes.Order &&
  //        ((PlayerInstance)ch).PlayerData.Clan.ClanType != ClanTypes.Guild;
  //}

  // TODO Fix
  //public static bool IsOrdered(this CharacterInstance ch)
  //{
  //    if (ch.IsNpc())
  //        return false;
  //    return ((PlayerInstance)ch).PlayerData.Clan != null &&
  //           ((PlayerInstance)ch).PlayerData.Clan.ClanType == ClanTypes.Order;
  //}

  // TODO Fix
  //public static bool IsGuilded(this CharacterInstance ch)
  //{
  //    if (ch.IsNpc())
  //        return false;
  //    return ((PlayerInstance)ch).PlayerData.Clan != null &&
  //           ((PlayerInstance)ch).PlayerData.Clan.ClanType == ClanTypes.Guild;
  //}

  // TODO fix
  //public static bool IsDeadlyClan(this CharacterInstance ch)
  //{
  //    if (ch.IsNpc())
  //        return false;
  //    return ((PlayerInstance)ch).PlayerData.Clan != null &&
  //           ((PlayerInstance)ch).PlayerData.Clan.ClanType != ClanTypes.NoKill &&
  //           ((PlayerInstance)ch).PlayerData.Clan.ClanType != ClanTypes.Order &&
  //           ((PlayerInstance)ch).PlayerData.Clan.ClanType != ClanTypes.Guild;
  //}

  public static bool CanTakePrototype(this CharacterInstance ch)
  {
    if (ch.IsImmortal())
      return true;
    return ch.IsNpc()
           && ch.Act.IsSet((int)ActFlags.Prototype);
  }

  public static void ModifySkill(this CharacterInstance ch, int sn, int mod, bool add)
  {
    if (ch.IsNpc())
      return;

    SkillData skill = Program.RepositoryManager.SKILLS.Get(sn);
    if (add)
      ((PlayerInstance)ch).PlayerData.UpdateSkillMastery(sn, mod, true);
    else
    {
      int mastery = skill.GetMasteryLevel((PlayerInstance)ch);
      int current = ((PlayerInstance)ch).PlayerData.GetSkillMastery(sn);
      if (current + mod > mastery)
        current = mastery;

      ((PlayerInstance)ch).PlayerData.UpdateSkillMastery(sn, current);
    }
  }

  public static ObjectInstance GetEquippedItem(this CharacterInstance ch, WearLocations location)
  {
    ObjectInstance maxObj = null;
    foreach (ObjectInstance obj in ch.Carrying.Where(x => x.WearLocation == location))
    {
      if (obj.ObjectIndex.Layers == 0)
        return obj;
      if (maxObj == null || obj.ObjectIndex.Layers > maxObj.ObjectIndex.Layers)
        maxObj = obj;
    }

    return maxObj;
  }

  public static int GetEncumberedMove(this CharacterInstance ch, int movement)
  {
    checked
    {
      int max = ch.CanCarryMaxWeight();

      if (ch.CarryWeight >= max)
        return Convert.ToInt16(movement * 4);
      if (ch.CarryWeight >= max * 0.95)
        return Convert.ToInt16(movement * 3.5);
      if (ch.CarryWeight >= max * 0.90)
        return Convert.ToInt16(movement * 3);
      if (ch.CarryWeight >= max * 0.85)
        return Convert.ToInt16(movement * 2.5);
      if (ch.CarryWeight >= max * 0.80)
        return Convert.ToInt16(movement * 2);
      return ch.CarryWeight >= max * 0.75 ? Convert.ToInt16(movement * 1.5) : movement;
    }
  }

  public static void AdvanceLevel(this PlayerInstance ch, IRepositoryManager databaseManager = null)
  {
    string buffer = $"the {tables.GetTitle(ch.CurrentClass, ch.Level, ch.Gender)}";
    player.set_title(ch, buffer);

    ClassData myClass = (databaseManager ?? Program.RepositoryManager).GetClass(ch.CurrentClass);

    int add_hp = LookupConstants.con_app[ch.GetCurrentConstitution()].hitp +
                 SmaugRandom.Between(myClass.MinimumHealthGain, myClass.MaximumHealthGain);
    int add_mana = myClass.UseMana
      ? SmaugRandom.Between(2, (2 * ch.GetCurrentIntelligence() + ch.GetCurrentWisdom()) / 8)
      : 0;
    int add_move = SmaugRandom.Between(5, (ch.GetCurrentConstitution() + ch.GetCurrentDexterity()) / 4);
    int add_prac = LookupConstants.wis_app[ch.GetCurrentWisdom()].practice;

    add_hp = 1.GetHighestOfTwoNumbers(add_hp);
    add_mana = 0.GetHighestOfTwoNumbers(add_mana);
    add_move = 10.GetHighestOfTwoNumbers(add_move);

    if (ch.IsPKill())
    {
      add_mana = (int)(add_mana + add_mana * 0.3f);
      add_move = (int)(add_move + add_move * 0.3f);
      add_hp += 1;
      ch.SendTo("Gravoc's Pandect steels your sinews.");
    }

    ch.MaximumHealth += add_hp;
    ch.MaximumMana += add_mana;
    ch.MaximumMovement += add_move;
    ch.Practice += add_prac;

    if (!ch.IsNpc())
      ch.Act.RemoveBit((int)PlayerFlags.BoughtPet);

    if (ch.Level == LevelConstants.AvatarLevel)
      ch.AdvanceLevelAvatar();
    if (ch.Level < LevelConstants.ImmortalLevel)
    {
      buffer = ch.IsVampire()
        ? $"Your gain is: {add_hp}/{ch.MaximumHealth} hp, {1}/{ch.Level + 10} bp, {add_move}/{ch.MaximumMovement} mv, {add_prac}/{ch.Practice} prac.\r\n"
        : $"Your gain is: {add_hp}/{ch.MaximumHealth} hp, {add_mana}/{ch.MaximumMana} mana, {add_move}/{ch.MaximumMovement} mv, {add_prac}/{ch.Practice} prac.\r\n";

      ch.SetColor(ATTypes.AT_WHITE);
      ch.SendTo(buffer);
    }
  }

  private static void AdvanceLevelAvatar(this CharacterInstance ch)
  {
    IEnumerable<DescriptorData> descriptors = db.DESCRIPTORS.Where(d => d.ConnectionStatus == ConnectionTypes.Playing
                                                                        && d.Character != ch);
    foreach (DescriptorData d in descriptors)
    {
      d.Character.SetColor(ATTypes.AT_WHITE);
      d.Character.Printf("%s has just achieved Avatarhood!", ch.Name);
    }

    ch.SetColor(ATTypes.AT_WHITE);
    Help.do_help(ch, "M_ADVHERO_");
  }

  public static void GainXP(this PlayerInstance ch, int gain)
  {
    if (ch.IsNpc() || ch.Level >= LevelConstants.AvatarLevel)
      return;

    double modgain = gain;
    if (modgain > 0 && ch.IsPKill() && ch.Level < 17)
    {
      if (ch.Level <= 6)
      {
        ch.SendTo("The Favor of Gravoc fosters your learning.");
        modgain *= 2;
      }

      if (ch.Level <= 10 && ch.Level >= 7)
      {
        ch.SendTo("The Hand of Gravoc hastens your learning.");
        modgain *= 1.75f;
      }

      if (ch.Level <= 13 && ch.Level >= 11)
      {
        ch.SendTo("The Cunning of Gravoc succors your learning.");
        modgain *= 1.5f;
      }

      if (ch.Level <= 16 && ch.Level >= 14)
      {
        ch.SendTo("THe Patronage of Gravoc reinforces your learning.");
        modgain *= 1.25f;
      }
    }

    RaceData myRace = Program.RepositoryManager.GetRace(ch.CurrentRace);
    modgain *= myRace.ExperienceMultiplier / 100.0f;

    if (ch.IsPKill() && modgain < 0)
    {
      if (ch.Experience + modgain < ch.GetExperienceLevel(ch.Level))
      {
        modgain = ch.GetExperienceLevel(ch.Level) - ch.Experience;
        ch.SendTo("Gravoc's Pandect protects your insight.");
      }
    }

    modgain = ((int)modgain).GetLowestOfTwoNumbers(ch.GetExperienceLevel(ch.Level + 2) -
                                                   ch.GetExperienceLevel(ch.Level + 1));
    ch.Experience = 0.GetHighestOfTwoNumbers(ch.Experience + (int)modgain);

    if (ch.IsNotAuthorized() && ch.Experience >= ch.GetExperienceLevel(ch.Level + 1))
    {
      ch.SendTo("You cannot ascend to a higher level until you are authorized.");
      ch.Experience = ch.GetExperienceLevel(ch.Level + 1) - 1;
      return;
    }

    while (ch.Level < LevelConstants.AvatarLevel && ch.Experience >= ch.GetExperienceLevel(ch.Level + 1))
    {
      ch.SetColor(ATTypes.AT_WHITE | ATTypes.AT_BLINK);
      ch.Level += 1;
      ch.Printf("You have not obtained experience level %d!", ch.Level);
      ch.AdvanceLevel();
    }
  }

  public static int HealthGain(this CharacterInstance ch)
  {
    int gain;
    if (ch.IsNpc())
    {
      gain = ch.Level * 3 / 2;
      if (ch.IsAffected(AffectedByTypes.Poison))
        gain /= 4;
      return gain.GetLowestOfTwoNumbers(ch.MaximumHealth - ch.CurrentHealth);
    }

    gain = 5.GetLowestOfTwoNumbers(ch.Level);
    switch (ch.CurrentPosition)
    {
      case PositionTypes.Dead:
        return 0;
      case PositionTypes.Mortal:
      case PositionTypes.Incapacitated:
        return -1;
      case PositionTypes.Stunned:
        return 1;
      case PositionTypes.Sleeping:
        gain += (int)(ch.GetCurrentConstitution() * 2.0f);
        break;
      case PositionTypes.Resting:
        gain += (int)(ch.GetCurrentConstitution() * 1.25f);
        break;
    }

    if (ch.IsVampire())
      gain = ch.GetModifiedStatGainForVampire(gain);

    if (((PlayerInstance)ch).PlayerData.ConditionTable[ConditionTypes.Full] == 0)
      gain /= 2;
    if (((PlayerInstance)ch).PlayerData.ConditionTable[ConditionTypes.Thirsty] == 0)
      gain /= 2;

    if (ch.IsAffected(AffectedByTypes.Poison))
      gain /= 4;
    return gain.GetLowestOfTwoNumbers(ch.MaximumHealth - ch.CurrentHealth);
  }

  public static int ManaGain(this CharacterInstance ch)
  {
    int gain;

    if (ch.IsNpc())
    {
      gain = ch.Level;

      if (ch.IsAffected(AffectedByTypes.Poison))
        gain /= 4;
      return gain.GetLowestOfTwoNumbers(ch.MaximumMana - ch.CurrentMana);
    }

    gain = 5.GetLowestOfTwoNumbers(ch.Level / 2);
    if (ch.CurrentPosition < PositionTypes.Sleeping)
      return 0;
    switch (ch.CurrentPosition)
    {
      case PositionTypes.Sleeping:
        gain += (int)(ch.GetCurrentIntelligence() * 3.25f);
        break;
      case PositionTypes.Resting:
        gain += (int)(ch.GetCurrentIntelligence() * 1.75f);
        break;
    }

    if (((PlayerInstance)ch).PlayerData.GetConditionValue(ConditionTypes.Full) == 0)
      gain /= 2;
    if (((PlayerInstance)ch).PlayerData.GetConditionValue(ConditionTypes.Thirsty) == 0)
      gain /= 2;

    if (ch.IsAffected(AffectedByTypes.Poison))
      gain /= 4;
    return gain.GetLowestOfTwoNumbers(ch.MaximumMana - ch.CurrentMana);
  }

  public static int MovementGain(this CharacterInstance ch)
  {
    int gain;

    if (ch.IsNpc())
    {
      gain = ch.Level;
      if (ch.IsAffected(AffectedByTypes.Poison))
        gain /= 4;
      return gain.GetLowestOfTwoNumbers(ch.MaximumMovement - ch.CurrentMovement);
    }

    gain = 15.GetHighestOfTwoNumbers(2 * ch.Level);

    switch (ch.CurrentPosition)
    {
      case PositionTypes.Dead:
        return 0;
      case PositionTypes.Mortal:
      case PositionTypes.Incapacitated:
        return -1;
      case PositionTypes.Stunned:
        return 1;
      case PositionTypes.Sleeping:
        gain += (int)(ch.GetCurrentDexterity() * 4.5f);
        break;
      case PositionTypes.Resting:
        gain += (int)(ch.GetCurrentDexterity() * 2.5f);
        break;
    }

    if (ch.IsVampire())
      gain = ch.GetModifiedStatGainForVampire(gain);

    if (((PlayerInstance)ch).PlayerData.ConditionTable[ConditionTypes.Full] == 0)
      gain /= 2;
    if (((PlayerInstance)ch).PlayerData.ConditionTable[ConditionTypes.Thirsty] == 0)
      gain /= 2;

    if (ch.IsAffected(AffectedByTypes.Poison))
      gain /= 4;

    return gain.GetLowestOfTwoNumbers(ch.MaximumMovement - ch.CurrentMovement);
  }

  private static int GetModifiedStatGainForVampire(this CharacterInstance ch, int gain)
  {
    int modGain = gain;
    if (((PlayerInstance)ch).PlayerData.ConditionTable[ConditionTypes.Bloodthirsty] <= 1)
      modGain /= 2;
    else if (((PlayerInstance)ch).PlayerData.ConditionTable[ConditionTypes.Bloodthirsty] >= 8 + ch.Level)
      modGain *= 2;

    if (ch.IsOutside())
    {
      switch (Program.GameManager.GameTime.Sunlight)
      {
        case SunPositionTypes.Sunset:
        case SunPositionTypes.Sunrise:
          modGain /= 2;
          break;
        case SunPositionTypes.Light:
          modGain /= 4;
          break;
      }
    }

    return modGain;
  }

  public static void CheckAlignment(this CharacterInstance ch)
  {
    if (ch.CurrentAlignment < Program.RepositoryManager.GetRace(ch.CurrentRace).MinimumAlignment
        || ch.CurrentAlignment > Program.RepositoryManager.GetRace(ch.CurrentRace).MaximumAlignment)
    {
      ch.SetColor(ATTypes.AT_BLOOD);
      ch.SendTo("Your actions have been incompatible with the ideals of your race. This troubles you.");
    }

    if (ch.CurrentClass == ClassTypes.Paladin)
    {
      if (ch.CurrentAlignment < 250)
      {
        ch.SetColor(ATTypes.AT_BLOOD);
        ch.SendTo("You are wracked with guilt and remorse for your craven actions!");
        comm.act(ATTypes.AT_BLOOD, "$n prostrates $mself, seeking forgiveness from $s Lord.", ch, null, null,
          ToTypes.Room);
        ((PlayerInstance)ch).WorsenMentalState(15);
        return;
      }

      if (ch.CurrentAlignment < 500)
      {
        ch.SetColor(ATTypes.AT_BLOOD);
        ch.SendTo("As you betray your faith, your mind begins to betray you.");
        comm.act(ATTypes.AT_BLOOD, "$n shudders, judging $s actions unworthy of a Paladin.", ch, null, null,
          ToTypes.Room);
        ((PlayerInstance)ch).WorsenMentalState(6);
      }
    }
  }

  public static bool WillFall(this CharacterInstance ch, int fall)
  {
    if (ch.CurrentRoom.Flags.IsSet(RoomFlags.NoFloor)
        && ch.CanGo(DirectionTypes.Down)
        && (!ch.IsAffected(AffectedByTypes.Flying)
            || (ch.CurrentMount != null && !ch.CurrentMount.IsAffected(AffectedByTypes.Flying))))
    {
      if (fall > 80)
      {
        Program.LogManager.Bug("Falling (in a loop?) more than 80 rooms: vnum {0}", ch.CurrentRoom.Id);
        ch.CurrentRoom.RemoveFrom(ch);
        Program.RepositoryManager.ROOMS.CastAs<Repository<long, RoomTemplate>>().Get(VnumConstants.ROOM_VNUM_TEMPLE)
          .AddTo(ch);
        return true;
      }

      ch.SetColor(ATTypes.AT_FALLING);
      ch.SendTo("You're falling down...");
      Move.move_char(ch, ch.CurrentRoom.GetExit(DirectionTypes.Down), ++fall);
      return true;
    }

    return false;
  }

  public static void Equip(this CharacterInstance ch, ObjectInstance obj, WearLocations wearLoc,
    IGameManager gameManager = null)
  {
    if (obj.CarriedBy != ch)
      throw new ObjectNotCarriedByCharacterException("Object {0} is not carried by Character {1}", obj.Id,
        ch.Id);

    ObjectInstance temp = ch.GetEquippedItem(wearLoc);
    if (temp != null && (temp.ObjectIndex.Layers == 0 || obj.ObjectIndex.Layers == 0))
      throw new ObjectAlreadyEquippedException("Object {0} is already equipped at {1} by Character {2}",
        obj.Id, wearLoc, ch.Id);

    obj.Split();
    if ((obj.ExtraFlags.IsSet((int)ItemExtraFlags.AntiEvil) && ch.IsEvil())
        || (obj.ExtraFlags.IsSet((int)ItemExtraFlags.AntiGood) && ch.IsGood())
        || (obj.ExtraFlags.IsSet((int)ItemExtraFlags.AntiNeutral) && ch.IsNeutral()))
    {
      if (handler.LoadingCharacter != ch)
      {
        comm.act(ATTypes.AT_MAGIC, "You are zapped by $p and drop it.", ch, obj, null, ToTypes.Character);
        comm.act(ATTypes.AT_MAGIC, "$n is zapped by $p and drops it.", ch, obj, null, ToTypes.Room);
      }

      if (obj.CarriedBy != null)
        obj.RemoveFrom();

      ch.CurrentRoom.AddTo(obj);
      MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Zap, ch, obj);

      if ((gameManager ?? Program.GameManager).GetSaveFlags().IsSet(AutoSaveFlags.ZapDrop) && !ch.CharDied())
        save.save_char_obj(ch);
      return;
    }

    ch.ArmorClass -= obj.ApplyArmorClass;
    obj.WearLocation = wearLoc;
    ch.CarryNumber -= obj.ObjectNumber;
    if (obj.ExtraFlags.IsSet((int)ItemExtraFlags.Magical))
      ch.CarryWeight -= obj.GetWeight();

    foreach (AffectData affect in obj.ObjectIndex.Affects)
      ch.AddAffect(affect);
    foreach (AffectData affect in obj.Affects)
      ch.AddAffect(affect);
  }

  public static void Unequip(this CharacterInstance ch, ObjectInstance obj)
  {
    if (obj.WearLocation == WearLocations.None)
      return;

    ch.CarryNumber += obj.ObjectNumber;
    if (obj.ExtraFlags.IsSet((int)ItemExtraFlags.Magical))
      ch.CarryWeight += obj.GetWeight();

    ch.ArmorClass += obj.ApplyArmorClass;
    obj.WearLocation = WearLocations.None;

    foreach (AffectData paf in obj.ObjectIndex.Affects)
      ch.RemoveAffect(paf);
    if (obj.CarriedBy != null)
    {
      foreach (AffectData paf in obj.Affects)
        ch.RemoveAffect(paf);
    }

    ch.update_aris();
  }

  /// <summary>
  /// Expand the name of a character into a string that identifies THAT 
  /// character within a room. E.g. the second 'guard' -> 2. guard
  /// </summary>
  /// <returns></returns>
  public static string GetExpandedName(this CharacterInstance ch)
  {
    if (!ch.IsNpc())
      return ch.Name;

    string name = ch.Name.FirstWord();
    return string.IsNullOrEmpty(name)
      ? string.Empty
      : $"{1 + ch.CurrentRoom.Persons.Count(rch => name.IsEqual(rch.Name))}.{name}";
  }

  public static bool CharDied(this CharacterInstance ch)
  {
    if (ch == handler.CurrentCharacter && handler.CurrentDeadCharacter != null)
      return true;

    return handler.ExtractedCharacterQueue.Any(ccd => ccd.Character == ch);
  }
}