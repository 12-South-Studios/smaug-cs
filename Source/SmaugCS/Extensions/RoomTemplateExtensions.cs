using System;
using System.Linq;
using Library.Common.Objects;
using Patterns.Repository;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Exceptions;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Objects;
using SmaugCS.Repository;

namespace SmaugCS.Extensions;

public static class RoomTemplateExtensions
{
  public static void RemoveFrom(this RoomTemplate room, CharacterInstance ch)
  {
    if (ch.CurrentRoom != room)
      throw new RoomMismatchException("Character {0} is not in Room {1}", ch.Name, room.Id);

    ch.Affects.ToList().ForEach(room.RemoveAffect);
    foreach (AffectData affect in room.Affects.Where(af => (af.Location & ApplyTypes.IsNotRemovable) > 0))
      ch.RemoveAffect(affect);

    room.Persons.Remove(ch);
    ch.PreviousRoom = room;
    ch.CurrentRoom = null;

    if (!ch.IsNpc() && ch.GetTimer(TimerTypes.ShoveDrag) != null)
      ch.RemoveTimer(TimerTypes.ShoveDrag);
  }

  public static void AddTo(this RoomTemplate room, CharacterInstance ch, IRepositoryManager dbManager = null)
  {
    ArgumentNullException.ThrowIfNull(ch);

    RoomTemplate localRoom = room;
    IRepositoryManager databaseMgr = dbManager ?? Program.RepositoryManager;
    if (databaseMgr.ROOMS.CastAs<Repository<long, RoomTemplate>>().Get(room.Id) == null)
    {
      Program.LogManager.Bug("{0} -> NULL room! Putting char in limbo ({1})",
        ch.Name, VnumConstants.ROOM_VNUM_LIMBO);
      localRoom = databaseMgr.ROOMS.CastAs<Repository<long, RoomTemplate>>().Get(VnumConstants.ROOM_VNUM_LIMBO);
    }

    ch.CurrentRoom = localRoom;
    if (ch.HomeVNum < 1)
      ch.HomeVNum = localRoom.Id;
    localRoom.Persons.Add(ch);

    if (!ch.IsNpc())
    {
      if (localRoom.Area.NumberOfPlayers > localRoom.Area.MaximumPlayers)
        localRoom.Area.MaximumPlayers = localRoom.Area.NumberOfPlayers;
    }

    foreach (AffectData affect in localRoom.Affects.Where(x => (x.Location & ApplyTypes.IsNotRemovable) > 0))
      ch.AddAffect(affect);
    foreach (AffectData affect in localRoom.PermanentAffects.Where(x => (x.Location & ApplyTypes.IsNotRemovable) > 0))
      ch.AddAffect(affect);
    foreach (AffectData affect in ch.Affects)
      localRoom.AddAffect(affect);

    if (!ch.IsNpc() && localRoom.Flags.IsSet(RoomFlags.Safe)
                    && ch.GetTimer(TimerTypes.ShoveDrag) == null)
      ch.AddTimer(TimerTypes.ShoveDrag, 10);

    if (localRoom.Flags.IsSet(RoomFlags.Teleport) && localRoom.TeleportDelay > 0)
    {
      if (db.TELEPORT.Exists(x => x.Room == localRoom))
        return;

      db.TELEPORT.Add(new TeleportData
      {
        Room = localRoom,
        Timer = (short)localRoom.TeleportDelay
      });
    }

    if (ch.PreviousRoom == null)
      ch.PreviousRoom = ch.CurrentRoom;
  }

  public static CharacterInstance IsDoNotDisturb(this RoomTemplate room, CharacterInstance ch)
  {
    if (room.Flags.IsSet(RoomFlags.DoNotDisturb)) return null;

    return room.Persons.FirstOrDefault(rch => !rch.IsNpc() && ((PlayerInstance)rch).PlayerData != null &&
                                              rch.IsImmortal()
                                              && ((PlayerInstance)rch).PlayerData.Flags.IsSet(PCFlags.DoNotDisturb) &&
                                              ch.Trust < rch.Trust && ch.CanSee(rch));
  }

  public static void RemoveFrom(this RoomTemplate room, ObjectInstance obj)
  {
    if (obj.InRoom != room)
      throw new RoomMismatchException("Object {0} is not in Room {1}", obj.Name, room.Id);

    obj.Affects.ToList().ForEach(room.RemoveAffect);
    obj.ObjectIndex.Affects.ToList().ForEach(room.RemoveAffect);
    room.Contents.Remove(obj);

    if (obj.ExtraFlags.IsSet((int)ItemExtraFlags.Covering) && obj.Contents != null)
      obj.Empty(null, room);

    obj.CarriedBy = null;
    obj.InObject = null;
    obj.InRoom = null;

    if (obj.ObjectIndex.Id == VnumConstants.OBJ_VNUM_CORPSE_PC && handler.falling > 0)
      save.write_corpses(null, obj.ShortDescription, obj);
  }

  public static ObjectInstance AddTo(this RoomTemplate room, ObjectInstance obj)
  {
    obj.Affects.ToList().ForEach(room.AddAffect);
    obj.ObjectIndex.Affects.ToList().ForEach(room.AddAffect);

    foreach (ObjectInstance objInstance in room.Contents)
    {
      ObjectInstance groupedObject = objInstance.GroupWith(obj);
      if (groupedObject == objInstance)
        return groupedObject;
    }

    room.Contents.Add(obj);
    obj.InRoom = room;
    obj.CarriedBy = null;
    obj.InObject = null;

    handler.falling++;
    act_obj.obj_fall(obj, false);
    handler.falling--;

    if (obj.ObjectIndex.Id == VnumConstants.OBJ_VNUM_CORPSE_PC && handler.falling < 1)
      save.write_corpses(null, obj.ShortDescription, null);
    return obj;
  }

  public static bool IsDark(this RoomTemplate room)
  {
    if (room.Light > 0) return false;
    if (room.Flags.IsSet(RoomFlags.Dark)) return true;
    if (room.SectorType is SectorTypes.Inside or SectorTypes.City)
      return false;

    return Program.GameManager.GameTime.Sunlight == SunPositionTypes.Sunset
           || Program.GameManager.GameTime.Sunlight == SunPositionTypes.Dark;
  }
}