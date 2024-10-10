using System;
using System.Collections.Generic;
using System.Linq;
using Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Objects;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.Deity;

public static class Supplicate
{
  private static readonly Dictionary<string, Action<PlayerInstance, string>> SupplicateTable
    = new()
    {
      { "corpse", SupplicateForCorpse },
      { "avatar", SupplicateForAvatar },
      { "object", SupplicateForObject },
      { "recall", SupplicateForRecall }
    };

  public static void do_supplicate(CharacterInstance ch, string argument)
  {
    if (CheckFunctions.CheckIfTrue(ch, ch.IsNpc() || ((PlayerInstance)ch).PlayerData.CurrentDeity == null,
          "You have no deity to supplicate to.")) return;

    string firstArg = argument.FirstWord();
    if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Supplicate for what?")) return;

    if (SupplicateTable.ContainsKey(firstArg.ToLower()))
      SupplicateTable[firstArg.ToLower()].Invoke((PlayerInstance)ch, argument);
    else
      ch.SendTo("You cannot supplicate for that.");
  }

  private static void SupplicateForCorpse(PlayerInstance ch, string argument)
  {
    int oldfavor = ch.PlayerData.Favor;

    if (CheckFunctions.CheckIfTrue(ch, ch.PlayerData.Favor < ch.PlayerData.CurrentDeity.SupplicateCorpseCost,
          "You are not favored enough for a corpse retrieval.")) return;
    if (CheckFunctions.CheckIfSet(ch, ch.CurrentRoom.Flags, RoomFlags.ClanStoreroom,
          "You cannot supplicate in a storage room.")) return;

    ObjectInstance corpse =
      ch.CurrentRoom.Contents.FirstOrDefault(
        x => x.ShortDescription.Equals($"the corpse of {ch.Name}"));
    if (CheckFunctions.CheckIfNullObject(ch, corpse, "No corpse of yours litters the world...")) return;
    if (CheckFunctions.CheckIfSet(ch, corpse.InRoom.Flags, RoomFlags.NoSupplicate,
          "The image of your corpse appears, but suddenly fades away.")) return;

    comm.act(ATTypes.AT_MAGIC, "Your corpse appears suddenly, surrounded by a divine presence...", ch, null, null,
      ToTypes.Character);
    comm.act(ATTypes.AT_MAGIC, "$n's corpse appears suddenly, surrounded by a divine force...", ch, null, null,
      ToTypes.Room);
    corpse.InRoom.RemoveFrom(corpse);
    ch.CurrentRoom.AddTo(corpse);
    corpse.ExtraFlags.RemoveBit((int)ItemExtraFlags.Buried);

    ch.PlayerData.Favor -= ch.PlayerData.CurrentDeity.SupplicateCorpseCost;

    if (ch.PlayerData.Favor < ch.PlayerData.CurrentDeity.SusceptNum)
      ch.Susceptibility.SetBit(ch.PlayerData.CurrentDeity.Suscept);

    if ((oldfavor > ch.PlayerData.CurrentDeity.AffectedNum
         && ch.PlayerData.Favor <= ch.PlayerData.CurrentDeity.AffectedNum)
        || (oldfavor > ch.PlayerData.CurrentDeity.ElementNum
            && ch.PlayerData.Favor <= ch.PlayerData.CurrentDeity.ElementNum)
        || (oldfavor < ch.PlayerData.CurrentDeity.SusceptNum
            && ch.PlayerData.Favor >= ch.PlayerData.CurrentDeity.SusceptNum))
    {
      ch.update_aris();
    }
  }

  private static void SupplicateForAvatar(PlayerInstance ch, string argument)
  {
    int oldfavor = ch.PlayerData.Favor;

    if (CheckFunctions.CheckIfTrue(ch, ch.PlayerData.Favor < ch.PlayerData.CurrentDeity.SupplicateAvatarCost,
          "You are not favored enough for that.")) return;

    MobileTemplate template = Program.RepositoryManager.MOBILETEMPLATES.Get(VnumConstants.MOB_VNUM_DEITY);
    CharacterInstance mob = Program.RepositoryManager.CHARACTERS.Create(template);

    ch.CurrentRoom.AddTo(mob);

    comm.act(ATTypes.AT_MAGIC, "$n summons a powerful avatar!", ch, null, null, ToTypes.Room);
    comm.act(ATTypes.AT_MAGIC, "You summon a powerful avatar!", ch, null, null, ToTypes.Character);
    mob.AddFollower(ch);
    mob.AffectedBy.SetBit((int)AffectedByTypes.Charm);
    mob.Level = 10;
    mob.MaximumHealth = ch.MaximumHealth * 6 + ch.PlayerData.Favor;
    mob.CurrentAlignment = ch.PlayerData.CurrentDeity.Alignment;
    ch.PlayerData.Favor -= ch.PlayerData.CurrentDeity.SupplicateAvatarCost;

    if (ch.PlayerData.Favor < ch.PlayerData.CurrentDeity.SusceptNum)
      ch.Susceptibility.SetBit(ch.PlayerData.CurrentDeity.Suscept);

    if ((oldfavor > ch.PlayerData.CurrentDeity.AffectedNum
         && ch.PlayerData.Favor <= ch.PlayerData.CurrentDeity.AffectedNum)
        || (oldfavor > ch.PlayerData.CurrentDeity.ElementNum
            && ch.PlayerData.Favor <= ch.PlayerData.CurrentDeity.ElementNum)
        || (oldfavor < ch.PlayerData.CurrentDeity.SusceptNum
            && ch.PlayerData.Favor >= ch.PlayerData.CurrentDeity.SusceptNum))
    {
      ch.update_aris();
    }
  }

  private static void SupplicateForObject(PlayerInstance ch, string argument)
  {
    int oldfavor = ch.PlayerData.Favor;

    if (CheckFunctions.CheckIfTrue(ch, ch.PlayerData.Favor < ch.PlayerData.CurrentDeity.SupplicateDeityObjectCost,
          "You are not favored enough for that.")) return;

    ObjectTemplate template = Program.RepositoryManager.OBJECTTEMPLATES.Get(VnumConstants.OBJ_VNUM_DEITY);
    ObjectInstance obj = Program.RepositoryManager.OBJECTS.Create(template, ch.Level,
      $"sigil {ch.PlayerData.CurrentDeity.Name}");
    obj = obj.WearFlags.IsSet(ItemWearFlags.Take) ? obj.AddTo(ch) : ch.CurrentRoom.AddTo(obj);

    comm.act(ATTypes.AT_MAGIC, "$n weaves $p from divine matter!", ch, obj, null, ToTypes.Room);
    comm.act(ATTypes.AT_MAGIC, "You weave $p from divine matter!", ch, obj, null, ToTypes.Character);
    ch.PlayerData.Favor -= ch.PlayerData.CurrentDeity.SupplicateDeityObjectCost;

    if (ch.PlayerData.Favor < ch.PlayerData.CurrentDeity.SusceptNum)
      ch.Susceptibility.SetBit(ch.PlayerData.CurrentDeity.Suscept);

    if ((oldfavor > ch.PlayerData.CurrentDeity.AffectedNum
         && ch.PlayerData.Favor <= ch.PlayerData.CurrentDeity.AffectedNum)
        || (oldfavor > ch.PlayerData.CurrentDeity.ElementNum
            && ch.PlayerData.Favor <= ch.PlayerData.CurrentDeity.ElementNum)
        || (oldfavor < ch.PlayerData.CurrentDeity.SusceptNum
            && ch.PlayerData.Favor >= ch.PlayerData.CurrentDeity.SusceptNum))
    {
      ch.update_aris();
    }

    AffectData af = new()
    {
      Type = AffectedByTypes.None,
      Duration = -1,
      Location = GetApplyTypeForDeity(ch.PlayerData.CurrentDeity),
      Modifier = 1
    };
    af.BitVector.ClearBits();
    obj.Affects.Add(af);
  }

  private static ApplyTypes GetApplyTypeForDeity(DeityData deityData)
  {
    switch (deityData.ObjStat)
    {
      case 0:
        return ApplyTypes.Strength;
      case 1:
        return ApplyTypes.Intelligence;
      case 2:
        return ApplyTypes.Wisdom;
      case 3:
        return ApplyTypes.Constitution;
      case 4:
        return ApplyTypes.Dexterity;
      case 5:
        return ApplyTypes.Charisma;
      case 6:
        return ApplyTypes.Luck;
    }

    return ApplyTypes.None;
  }

  private static void SupplicateForRecall(PlayerInstance ch, string argument)
  {
    int oldfavor = ch.PlayerData.Favor;

    if (CheckFunctions.CheckIfTrue(ch, ch.PlayerData.Favor < ch.PlayerData.CurrentDeity.SupplicateRecallCost,
          "Your favor is inadequate for such a supplication.")) return;
    if (CheckFunctions.CheckIfSet(ch, ch.CurrentRoom.Flags, RoomFlags.NoSupplicate, "You have been forsaken!"))
      return;
    if (CheckFunctions.CheckIfTrue(ch, ch.HasTimer(TimerTypes.RecentFight) && !ch.IsImmortal(),
          "You cannot supplicate recall under adrenaline!")) return;

    RoomTemplate location = null;

    // TODO Fix
    //if (!ch.IsNpc() && ch.PlayerData.Clan != null)
    //    location = Program.RepositoryManager.ROOMS.Get(ch.PlayerData.Clan.RecallRoom);

    if (!ch.IsNpc() && ch.Level >= 5 && ch.PlayerData.Flags.IsSet(PCFlags.Deadly))
      location = Program.RepositoryManager.ROOMS.Get(VnumConstants.ROOM_VNUM_DEADLY);

    if (location == null)
    {
      int raceRecallRoom = Program.RepositoryManager.RACES.Get(ch.CurrentRace.GetValue()).RaceRecallRoom;
      location = Program.RepositoryManager.ROOMS.Get(raceRecallRoom);
    }

    if (location == null)
      location = Program.RepositoryManager.ROOMS.Get(VnumConstants.ROOM_VNUM_TEMPLE);

    if (CheckFunctions.CheckIfNullObject(ch, location, "You are completely lost.")) return;

    comm.act(ATTypes.AT_MAGIC, "$n disappears in a column of divine power.", ch, null, null, ToTypes.Room);

    RoomTemplate oldRoom = ch.CurrentRoom;
    oldRoom.RemoveFrom(ch);
    location.AddTo(ch);

    if (ch.CurrentMount != null)
    {
      oldRoom.RemoveFrom(ch.CurrentMount);
      location.AddTo(ch.CurrentMount);
    }

    comm.act(ATTypes.AT_MAGIC, "$n appears in the room from a column of divine power.", ch, null, null, ToTypes.Room);

    Look.do_look(ch, "auto");
    ch.PlayerData.Favor -= ch.PlayerData.CurrentDeity.SupplicateRecallCost;

    if (ch.PlayerData.Favor < ch.PlayerData.CurrentDeity.SusceptNum)
      ch.Susceptibility.SetBit(ch.PlayerData.CurrentDeity.Suscept);

    if ((oldfavor > ch.PlayerData.CurrentDeity.AffectedNum
         && ch.PlayerData.Favor <= ch.PlayerData.CurrentDeity.AffectedNum)
        || (oldfavor > ch.PlayerData.CurrentDeity.ElementNum
            && ch.PlayerData.Favor <= ch.PlayerData.CurrentDeity.ElementNum)
        || (oldfavor < ch.PlayerData.CurrentDeity.SusceptNum
            && ch.PlayerData.Favor >= ch.PlayerData.CurrentDeity.SusceptNum))
    {
      ch.update_aris();
    }
  }
}