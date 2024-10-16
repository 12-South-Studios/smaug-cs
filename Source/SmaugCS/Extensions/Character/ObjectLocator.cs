﻿using System.Collections.Generic;
using System.Linq;
using Library.Common.Extensions;
using Library.Common.Objects;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Helpers;
using SmaugCS.Repository;

namespace SmaugCS.Extensions.Character;

public static class ObjectLocator
{
  private static readonly Dictionary<string, int> DoorDirectionMap = new()
  {
    { "n;north", 0 },
    { "e;east", 1 },
    { "s;south", 2 },
    { "w;west", 3 },
    { "u;up", 4 },
    { "d;down", 5 },
    { "ne;northeast", 6 },
    { "nw;northwest", 7 },
    { "se;southeast", 8 },
    { "sw;southwest", 9 }
  };

  public static ExitData FindExit(this CharacterInstance ch, string arg, bool quiet = false)
  {
    if (string.IsNullOrEmpty(arg))
      return null;

    int door = (from key in DoorDirectionMap.Keys
      let words = key.Split(';')
      where words.Any(x => x.EqualsIgnoreCase(arg))
      select DoorDirectionMap[key]).FirstOrDefault();

    if (door == 0)
    {
      foreach (ExitData pexit in ch.CurrentRoom.Exits)
      {
        if ((quiet || pexit.Flags.IsSet(ExitFlags.IsDoor))
            && !string.IsNullOrEmpty(pexit.Keywords)
            && arg.IsAnyEqual(pexit.Keywords))
          return pexit;
      }

      if (!quiet)
        comm.act(ATTypes.AT_PLAIN, "You see no $T here.", ch, null, arg, ToTypes.Character);
      return null;
    }

    ExitData exit = ch.CurrentRoom.GetExit(door);
    if (exit == null)
    {
      if (!quiet)
        comm.act(ATTypes.AT_PLAIN, "You see no $T here.", ch, null, arg, ToTypes.Character);
      return null;
    }

    if (quiet)
      return exit;

    if (!exit.Flags.IsSet(ExitFlags.Secret))
      return CheckFunctions.CheckIfNotSet(ch, exit.Flags, ExitFlags.IsDoor, "You can't do that.") ? null : exit;
    comm.act(ATTypes.AT_PLAIN, "You see no $T here.", ch, null, arg, ToTypes.Character);
    return null;
  }

  public static ObjectInstance GetObjectOfType(this CharacterInstance ch, ItemTypes type)
  {
    return ch.Carrying.FirstOrDefault(obj => obj.ItemType == type);
  }

  public static ObjectInstance GetCarriedObject(this CharacterInstance ch, string argument)
  {
    int number = argument.IsNumberArgument() ? argument.GetNumberArgument() : 1;
    string arg = argument.IsNumberArgument() ? argument.StripNumberArgument() : argument;

    IEnumerable<ObjectInstance> items =
      ch.Carrying.Where(obj => obj.WearLocation == WearLocations.None && ch.CanSee(obj));

    int vnum = GetVnumFromArgumentIfImmortal(ch, arg);
    return vnum > 0
      ? GetObjectInList(items.Where(obj => obj.ObjectIndex.Id == vnum), number)
      : GetObjectInList(items.Where(obj => arg.IsAnyEqual(obj.Name) || arg.IsAnyEqualPrefix(obj.Name)), number);
  }

  public static ObjectInstance GetObjectOnMeOrInRoom(this CharacterInstance ch, string argument)
  {
    return GetObjectInReversedList(ch.CurrentRoom.Contents, argument)
           ?? ch.GetCarriedObject(argument)
           ?? GetWornObject(ch, argument);
  }

  public static ObjectInstance GetWornObject(this CharacterInstance ch, string argument)
  {
    int number = argument.IsNumberArgument() ? argument.GetNumberArgument() : 1;
    string arg = argument.IsNumberArgument() ? argument.StripNumberArgument() : argument;

    IEnumerable<ObjectInstance> items =
      ch.Carrying.Where(obj => obj.WearLocation != WearLocations.None && ch.CanSee(obj));

    int vnum = GetVnumFromArgumentIfImmortal(ch, arg);
    return vnum > 0
      ? GetObjectInList(items.Where(obj => obj.ObjectIndex.Id == vnum), number)
      : GetObjectInList(items.Where(obj => arg.IsAnyEqual(obj.Name) || arg.IsAnyEqualPrefix(obj.Name)), number);
  }

  public static ObjectInstance GetObjectInWorld(this CharacterInstance ch, string argument,
    IRepositoryManager dbManager = null)
  {
    int number = argument.IsNumberArgument() ? argument.GetNumberArgument() : 1;
    string arg = argument.IsNumberArgument() ? argument.StripNumberArgument() : argument;

    IEnumerable<ObjectInstance> items = (dbManager ?? Program.RepositoryManager).OBJECTS.Values.Where(ch.CanSee);

    int vnum = GetVnumFromArgumentIfImmortal(ch, arg);
    return vnum > 0
      ? GetObjectInList(items.Where(obj => obj.ObjectIndex.Id == vnum), number)
      : GetObjectInList(items.Where(obj => arg.IsAnyEqual(obj.Name) || arg.IsAnyEqualPrefix(obj.Name)), number);
  }

  public static CharacterInstance GetCharacterInRoom(this CharacterInstance ch, string argument)
  {
    int number = argument.IsNumberArgument() ? argument.GetNumberArgument() : 1;
    string arg = argument.IsNumberArgument() ? argument.StripNumberArgument() : argument;

    if (arg.EqualsIgnoreCase("self"))
      return ch;

    IEnumerable<CharacterInstance> chars = ch.CurrentRoom.Persons.Where(ch.CanSee);

    int vnum = GetVnumFromArgumentIfImmortal(ch, arg);
    return vnum > 0
      ? GetObjectInList(chars.Where(vch => vch.IsNpc() && ((MobileInstance)vch).MobIndex.Id == vnum), number)
      : GetObjectInList(chars.Where(vch => arg.IsAnyEqual(vch.Name) || arg.IsAnyEqualPrefix(vch.Name)),
        number);
  }

  public static CharacterInstance GetCharacterInWorld(this CharacterInstance ch, string argument,
    IRepositoryManager dbManager = null)
  {
    int number = argument.IsNumberArgument() ? argument.GetNumberArgument() : 1;
    string arg = argument.IsNumberArgument() ? argument.StripNumberArgument() : argument;

    if (arg.EqualsIgnoreCase("self"))
      return ch;

    IEnumerable<CharacterInstance> chars =
      (dbManager ?? Program.RepositoryManager).CHARACTERS.Values.Where(ch.CanSee);

    int vnum = GetVnumFromArgumentIfImmortal(ch, arg);
    return vnum > 0
      ? GetObjectInList(chars.Where(vch => vch.IsNpc() && ((MobileInstance)vch).MobIndex.Id == vnum), number)
      : GetObjectInList(chars.Where(vch => arg.IsAnyEqual(vch.Name) || arg.IsAnyEqualPrefix(vch.Name)),
        number);
  }

  private static T GetObjectInReversedList<T>(IEnumerable<T> items, string argument) where T : Cell
  {
    List<T> reversedList = new();
    reversedList.AddRange(items);
    reversedList.Reverse();

    return GetObjectInList(reversedList
      .Where(obj => argument.IsAnyEqual(obj.Name) || argument.IsAnyEqualPrefix(obj.Name)), 1);
  }

  public static ObjectInstance GetObjectInList(this CharacterInstance ch, IEnumerable<ObjectInstance> objects,
    string argument)
  {
    int number = argument.IsNumberArgument() ? argument.GetNumberArgument() : 1;
    string arg = argument.IsNumberArgument() ? argument.StripNumberArgument() : argument;

    int vnum = GetVnumFromArgumentIfImmortal(ch, arg);
    return vnum > 0
      ? GetObjectInList(objects.Where(obj => obj.ObjectIndex.Id == vnum), number)
      : GetObjectInList(objects.Where(obj => arg.IsAnyEqual(obj.Name) || arg.IsAnyEqualPrefix(obj.Name)),
        number);
  }

  private static T GetObjectInList<T>(IEnumerable<T> objects, int number) where T : Cell
  {
    int count = 0;
    foreach (T obj in objects)
    {
      count++;
      if (count >= number)
        return obj;
    }

    return null;
  }

  private static int GetVnumFromArgumentIfImmortal(CharacterInstance ch, string arg)
  {
    return ch.Trust >= LevelConstants.GetLevel(ImmortalTypes.Savior) && arg.IsNumber() ? arg.ToInt32() : -1;
  }
}