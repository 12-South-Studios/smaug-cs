﻿using Library.Common.Extensions;
using Library.Common.Objects;
using Patterns.Repository;
using SmaugCS.Commands;
using SmaugCS.Common;
using SmaugCS.Communication;
using SmaugCS.Constants;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using SmaugCS.Commands.Movement;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Objects;
using SmaugCS.Managers;

namespace SmaugCS;

public static class act_move
{
  /// <summary>
  /// https://gist.github.com/anderssonjohan/660952
  /// </summary>
  public static string wordwrap(string text, int maxLineLength)
  {
    List<string> list = [];

    int currentIndex;
    int lastWrap = 0;
    char[] whitespace = [' ', '\r', '\n', '\t'];
    do
    {
      currentIndex = lastWrap + maxLineLength > text.Length
        ? text.Length
        : text.LastIndexOfAny([' ', ',', '.', '?', '!', ':', ';', '-', '\n', '\r', '\t'],
          Math.Min(text.Length - 1, lastWrap + maxLineLength)) + 1;
      if (currentIndex <= lastWrap)
        currentIndex = Math.Min(lastWrap + maxLineLength, text.Length);
      list.Add(text.Substring(lastWrap, currentIndex - lastWrap).Trim(whitespace));
      lastWrap = currentIndex;
    } while (currentIndex < text.Length);

    return string.Join("\n\r", list);
  }

  private static string GetDecorateRoom_PreAndPost_1(int iRand, int nRand, SectorTypes sector, int x)
  {
    string pre, post;
    int result = SmaugRandom.Between(1, 2 * (iRand == nRand - 1 ? 1 : 2));
    if (result <= 2)
    {
      post = ".";
      pre = result == 1 ? "You notice " : "You see ";
    }
    else
    {
      post = ", and ";
      pre = result == 3 ? "You see " : "You notice ";
    }

    return $"{pre}{LookupConstants.RoomSents[(int)sector][x]}{post}";
  }

  private static string GetDecorateRoom_PreAndPost_2(SectorTypes sector, int x)
  {
    string pre, post;
    int random = SmaugRandom.Between(0, 3);
    if (random is 0 or 2)
    {
      post = ".";
      pre = random == 0 ? "you notice " : "you see ";
    }
    else
    {
      post = ", and ";
      pre = random == 1 ? "you see " : "over yonder ";
    }

    return $"{pre}{LookupConstants.RoomSents[(int)sector][x]}{post}";
  }

  private static string GetDecorateRoom_PreAndPost_3(SectorTypes sector, int x)
  {
    string[] outputArray = [".", " not too far away.", ", and ", " nearby."];

    return $"{string.Empty}{LookupConstants.RoomSents[(int)sector][x]}{outputArray[SmaugRandom.Between(0, 3)]}";
  }

  public static void decorate_room(RoomTemplate room)
  {
    string buf = string.Empty;
    string buf2 = string.Empty;
    int[] previous = new int[8];

    room.Description = "You're on a pathway.\r\n";

    SectorTypes sector = room.SectorType;
    int nRand = SmaugRandom.Between(1, 8.GetLowestOfTwoNumbers(LookupConstants.SentTotals[(int)sector]));

    for (int iRand = 0; iRand < nRand; iRand++)
      previous[iRand] = -1;

    for (int iRand = 0; iRand < nRand; iRand++)
    {
      while (previous[iRand] == -1)
      {
        int x = SmaugRandom.Between(0, LookupConstants.SentTotals[(int)sector] - 1);

        int z;
        for (z = 0; z < iRand; z++)
          if (previous[z] == x)
            break;

        if (z < iRand)
          continue;

        previous[iRand] = x;
        int len = buf.Length;
        if (len == 0)
          buf2 = GetDecorateRoom_PreAndPost_1(iRand, nRand, sector, x);
        else if (iRand != nRand - 1)
          buf2 = buf.EndsWith(".")
            ? GetDecorateRoom_PreAndPost_2(sector, x)
            : GetDecorateRoom_PreAndPost_3(sector, x);
        else
          buf2 = $"{LookupConstants.RoomSents[(int)sector][x]}.";

        if (len > 5 && buf2.EndsWith("."))
        {
          buf += "  ";
          buf2 = buf2.CapitalizeFirst();
        }
        else if (len == 0)
          buf2 = buf2.CapitalizeFirst();

        buf += buf2;
      }
    }

    room.Description = $"{wordwrap(buf, 78)}\r\n";
  }

  public static string rev_exit(DirectionTypes vdir)
  {
    return Program.LookupManager.GetLookup("ReverseDirectionNames", (int)vdir);
  }

  public static RoomTemplate generate_exit(RoomTemplate room, ExitData exit)
  {
    long serial;
    long roomnum;
    long brvnum;
    long distance = -1;
    RoomTemplate backroom;
    int vdir = (int)exit.Direction;

    if (room.Id > 32767)
    {
      serial = room.Id;
      roomnum = room.TeleportToVnum;
      if ((serial & 65535) == exit.vnum)
      {
        brvnum = serial >> 16;
        --roomnum;
        distance = roomnum;
      }
      else
      {
        brvnum = serial & 65535;
        ++roomnum;
        distance = exit.Distance - 1;
      }

      backroom = Program.RepositoryManager.ROOMS.CastAs<Repository<long, RoomTemplate>>().Get(brvnum);
    }
    else
    {
      long r1 = room.Id;
      long r2 = exit.vnum;

      brvnum = r1;
      backroom = room;
      serial = (r1.GetHighestOfTwoNumbers(r2) << 16) | r1.GetLowestOfTwoNumbers(r2);
      distance = exit.Distance - 1;
      roomnum = r1 < r2 ? 1 : distance;
    }

    bool found = false;

    RoomTemplate foundRoom =
      Program.RepositoryManager.ROOMS.CastAs<Repository<long, RoomTemplate>>().Values.FirstOrDefault(
        x => x.Id == serial && x.TeleportToVnum == roomnum);
    if (foundRoom != null)
      found = true;

    // Create room in direction
    RoomTemplate newRoom = null;
    if (!found)
    {
      newRoom = new RoomTemplate(serial, "New room")
      {
        Area = room.Area,
        TeleportToVnum = roomnum,
        SectorType = room.SectorType,
        Flags = room.Flags
      };
      decorate_room(newRoom);
      Program.RepositoryManager.ROOMS.CastAs<Repository<long, RoomTemplate>>().Add(newRoom.Id, newRoom);
    }

    ExitData xit = newRoom.GetExit(vdir);
    if (!found || xit == null)
    {
      xit = db.make_exit(newRoom, exit.GetDestination(), vdir);
      xit.Key = -1;
      xit.Distance = (int)distance;
    }

    if (!found)
    {
      ExitData bxit = db.make_exit(newRoom, backroom, LookupConstants.rev_dir[vdir]);
      bxit.Key = -1;
      if ((serial & 65536) != exit.vnum)
        bxit.Distance = (int)roomnum;
      else
      {
        ExitData tmp = backroom.GetExit(vdir);
        bxit.Distance = tmp.Distance - (int)distance;
      }
    }

    return newRoom;
  }

  public static void teleportch(CharacterInstance ch, RoomTemplate room, bool show)
  {
    if (room.IsPrivate())
      return;

    comm.act(ATTypes.AT_ACTION, "$n disappears suddenly!", ch, null, null, ToTypes.Room);
    ch.CurrentRoom.RemoveFrom(ch);
    room.AddTo(ch);
    comm.act(ATTypes.AT_ACTION, "$n arrives suddenly!", ch, null, null, ToTypes.Room);

    if (show)
      Look.do_look(ch, "auto");

    if (!ch.CurrentRoom.Flags.IsSet(RoomFlags.Death)
        || ch.IsImmortal()) return;
    comm.act(ATTypes.AT_DEAD, "$n falls prey to a terrible death!", ch, null, null, ToTypes.Room);
    ch.SetColor(ATTypes.AT_DEAD);
    ch.SendTo("Oopsie... you're dead!");

    string buffer = $"{ch.Name} hit a DEATH TRAP in room {ch.CurrentRoom.Id}!";
    ChatManager.to_channel(buffer, ChannelTypes.Monitor, "Monitor", (short)LevelConstants.ImmortalLevel);
    ch.Extract(false);
  }

  public static void teleport(CharacterInstance ch, int room, int flags)
  {
    RoomTemplate dest = Program.RepositoryManager.ROOMS.CastAs<Repository<long, RoomTemplate>>().Get(room);
    if (dest == null)
    {
      Program.LogManager.Bug("bad room vnum {0}", room);
      return;
    }

    bool show = flags.IsSet(TeleportTriggerFlags.ShowDescription);

    if (!flags.IsSet(TeleportTriggerFlags.TransportAll))
    {
      teleportch(ch, dest, show);
      return;
    }

    RoomTemplate start = ch.CurrentRoom;
    foreach (CharacterInstance nch in start.Persons)
      teleportch(nch, dest, show);

    if (!flags.IsSet(TeleportTriggerFlags.TransportAllPlus)) return;
    foreach (ObjectInstance obj in start.Contents)
    {
      obj.InRoom.RemoveFrom(obj);
      dest.AddTo(obj);
    }
  }

  public static ReturnTypes pullcheck(CharacterInstance ch, int pulse)
  {
    if (ch.CurrentRoom == null)
    {
      Program.LogManager.Bug("{0} not in a room?!?", ch.Name);
      return ReturnTypes.None;
    }

    ExitData xit = null;
    foreach (ExitData exit in ch.CurrentRoom.Exits)
    {
      if (exit.Pull > 0 && (xit == null || Math.Abs(exit.Pull) > Math.Abs(xit.Pull)))
        xit = exit;
    }

    if (xit == null)
      return ReturnTypes.None;

    int pull = xit.Pull;
    int pullfact = (20 - Math.Abs(pull) / 5).GetNumberThatIsBetween(1, 20);

    if (pulse % pullfact != 0)
    {
      foreach (ExitData exit in ch.CurrentRoom.Exits
                 .Where(exit => exit.Pull > 0))
      {
        pull = exit.Pull;
        pullfact = (20 - Math.Abs(pull) / 5).GetNumberThatIsBetween(1, 20);
        if (pulse % pullfact == 0)
          break;
      }
    }

    if (xit.Flags.IsSet(ExitFlags.Closed))
      return ReturnTypes.None;

    if (xit.GetDestination().Tunnel > 0)
    {
      int count = ch.CurrentMount != null ? 1 : 0;

      if (xit.GetDestination().Persons.Any(ctmp => ++count >= xit.GetDestination().Tunnel))
        return ReturnTypes.None;
    }

    if (pull < 0)
    {
      xit = ch.CurrentRoom.GetExit(LookupConstants.rev_dir[(int)xit.Direction]);
      if (xit == null)
        return ReturnTypes.None;
    }

    string dtxt = rev_exit(xit.Direction);

    // First determine if the player should be moved or not Check various flags, spells,
    // the players position and strength vs. the pull, etc... any kind of checks you like.
    bool move = false;
    switch (xit.PullType)
    {
      case DirectionPullTypes.Current:
      case DirectionPullTypes.Whirlpool:
        if (xit.PullType == DirectionPullTypes.Current)
          break;

        switch (ch.CurrentRoom.SectorType)
        {
          // allow whirlpool to be in any sector type
          case SectorTypes.ShallowWater:
          case SectorTypes.DeepWater:
            if ((ch.CurrentMount != null
                 && !ch.CurrentMount.IsFloating())
                || (ch.CurrentMount == null
                    && !ch.IsFloating()))
              move = true;
            break;
          case SectorTypes.Underwater:
          case SectorTypes.OceanFloor:
            move = true;
            break;
        }

        break;
      case DirectionPullTypes.Geyser:
      case DirectionPullTypes.Wave:
        move = true;
        break;
      case DirectionPullTypes.Wind:
      case DirectionPullTypes.Storm:
        // if not flying, check weight, position and strength
        move = true;
        break;
      case DirectionPullTypes.ColdWind:
        // if not flying, check weight, position and strength
        // also check for damage due to bitter cold
        move = true;
        break;
      case DirectionPullTypes.HotAir:
        // if not flying, check weight, position and strength
        // also check for damage due to heat
        move = true;
        break;
      case DirectionPullTypes.Breeze:
        move = false;
        break;
      case DirectionPullTypes.Earthquake:
      case DirectionPullTypes.Sinkhole:
      case DirectionPullTypes.Quicksand:
      case DirectionPullTypes.Landslide:
      case DirectionPullTypes.Slip:
      case DirectionPullTypes.Lava:
        if ((ch.CurrentMount != null && !ch.CurrentMount.IsFloating())
            || (ch.CurrentMount == null && !ch.IsFloating()))
          move = true;
        break;
      case DirectionPullTypes.Undefined:
        // as if player moved in that direction him/herself
        return Move.move_char(ch, xit, 0);
      default:
        move = true;
        break;
    }

    bool showroom = xit.PullType == DirectionPullTypes.Mysterious;
    PullcheckMessages msg = GetPullcheckMessages(pull, xit.PullType);

    if (move)
    {
      if (!string.IsNullOrEmpty(msg.ToChar))
      {
        comm.act(ATTypes.AT_PLAIN, msg.ToChar, ch, null,
          Program.LookupManager.GetLookup("DirectionNames", (int)xit.Direction), ToTypes.Character);
        ch.SendTo("\r\n");
      }

      if (!string.IsNullOrEmpty(msg.ToRoom))
        comm.act(ATTypes.AT_PLAIN, msg.ToRoom, ch, null,
          Program.LookupManager.GetLookup("DirectionNames", (int)xit.Direction), ToTypes.Room);

      if (!string.IsNullOrEmpty(msg.DestRoom)
          && xit.GetDestination().Persons.Any())
      {
        comm.act(ATTypes.AT_PLAIN, msg.DestRoom, xit.GetDestination().Persons.First(), null, dtxt, ToTypes.Character);
        comm.act(ATTypes.AT_PLAIN, msg.DestRoom, xit.GetDestination().Persons.First(), null, dtxt, ToTypes.Room);
      }

      if (xit.PullType == DirectionPullTypes.Slip)
        return Move.move_char(ch, xit, 1);

      ch.CurrentRoom.RemoveFrom(ch);
      xit.GetDestination().AddTo(ch);

      if (showroom)
        Look.do_look(ch, "auto");

      if (ch.CurrentMount != null)
      {
        ch.CurrentMount.CurrentRoom.RemoveFrom(ch.CurrentMount);
        xit.GetDestination().AddTo(ch.CurrentMount);
        if (showroom)
          Look.do_look(ch.CurrentMount, "auto");
      }
    }

    foreach (ObjectInstance obj in ch.CurrentRoom.Contents)
    {
      if (obj.ExtraFlags.IsSet((int)ItemExtraFlags.Buried)
          || !obj.WearFlags.IsSet(ItemWearFlags.Take))
        continue;

      int resistance = obj.GetWeight();
      if (obj.ExtraFlags.IsSet((int)ItemExtraFlags.Metallic))
        resistance = resistance * 6 / 5;

      switch (obj.ItemType)
      {
        case ItemTypes.Scroll:
        case ItemTypes.Note:
        case ItemTypes.Trash:
          resistance >>= 2;
          break;
        case ItemTypes.Scraps:
        case ItemTypes.Container:
          resistance >>= 1;
          break;
        case ItemTypes.Pen:
        case ItemTypes.Wand:
          resistance = resistance * 5 / 6;
          break;
        case ItemTypes.PlayerCorpse:
        case ItemTypes.NpcCorpse:
        case ItemTypes.Fountain:
          resistance <<= 2;
          break;
      }

      if (Math.Abs(pull) * 10 <= resistance) continue;
      if (!string.IsNullOrEmpty(msg.ObjMsg)
          && ch.CurrentRoom.Persons.Any())
      {
        comm.act(ATTypes.AT_PLAIN, msg.ObjMsg, ch.CurrentRoom.Persons.First(), obj,
          Program.LookupManager.GetLookup("DirectionNames", (int)xit.Direction),
          ToTypes.Character);
        comm.act(ATTypes.AT_PLAIN, msg.ObjMsg, ch.CurrentRoom.Persons.First(), obj,
          Program.LookupManager.GetLookup("DirectionNames", (int)xit.Direction), ToTypes.Room);
      }

      if (!string.IsNullOrEmpty(msg.DestObj) && ch.CurrentRoom.Persons.Any())
      {
        comm.act(ATTypes.AT_PLAIN, msg.DestObj, xit.GetDestination().Persons.First(), obj, dtxt, ToTypes.Character);
        comm.act(ATTypes.AT_PLAIN, msg.DestObj, xit.GetDestination().Persons.First(), obj, dtxt, ToTypes.Room);
      }

      obj.InRoom.RemoveFrom(obj);
      xit.GetDestination().AddTo(obj);
    }

    return ReturnTypes.None;
  }

  private static PullcheckMessages GetPullcheckMessages(int pull, DirectionPullTypes pulltype)
  {
    PullcheckAttribute attrib = pulltype.GetAttribute<PullcheckAttribute>();
    if (attrib != null)
      return new PullcheckMessages
      {
        ToChar = attrib.ToChar,
        ToRoom = attrib.ToRoom,
        DestRoom = attrib.DestRoom,
        ObjMsg = attrib.ObjMsg,
        DestObj = attrib.DestObj
      };

    return new PullcheckMessages
    {
      ToChar = pull > 0 ? "You are pulled $T!" : "You are pushed $T!",
      ToRoom = pull > 0 ? "$n is pulled $T." : "$n is pushed $T.",
      DestRoom = pull > 0 ? "$n is pulled in from $T." : "$n is pushed in from $T.",
      ObjMsg = pull > 0 ? "$p is pulled $T." : "$p is pushed $T.",
      DestObj = pull > 0 ? "$p is pulled in from $T." : "$p is pushed in from $T."
    };
  }

  private class PullcheckMessages
  {
    public string ToChar { get; set; }
    public string ToRoom { get; set; }
    public string DestRoom { get; set; }
    public string ObjMsg { get; set; }
    public string DestObj { get; set; }
  }
}