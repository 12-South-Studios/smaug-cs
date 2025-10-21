using System.Collections.Generic;
using System.Linq;
using Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Objects;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.Liquids;

public static class Fill
{
  public static void do_fill(CharacterInstance ch, string argument)
  {
    string firstArg = argument.FirstWord();
    if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Fill what?")) return;
    if (handler.FindObject_CheckMentalState(ch)) return;

    ObjectInstance obj = ch.GetCarriedObject(firstArg);
    if (CheckFunctions.CheckIfNullObject(ch, obj, "You do not have that item.")) return;

    if (obj.ItemType == ItemTypes.Container)
    {
      if (obj.Values.Flags.IsSet(ContainerFlags.Closed))
      {
        comm.act(ATTypes.AT_PLAIN, "The $d is closed.", ch, null, obj.Name, ToTypes.Character);
        return;
      }

      if (CheckFunctions.CheckIfTrue(ch, obj.GetRealWeight() / obj.Count >= obj.Values.Capacity,
            "It's already full as it can be.")) return;
    }
    else
    {
      if (CheckFunctions.CheckIfTrue(ch,
            GetMaximumCondition() < 1 || obj.Values.Quantity >= obj.Values.Capacity,
            "It's already full as it can be.")) return;
    }

    if (CheckFunctions.CheckIfTrue(ch,
          obj.ItemType == ItemTypes.Pipe && obj.Values.Flags.IsSet(PipeFlags.FullOfAsh),
          "It's full of ashes, and needs to be emptied first.")) return;

    IEnumerable<ItemTypes> sourceItemTypes = ChooseSourceItemTypes(ch, obj);

    string secondArg = argument.SecondWord();
    if (secondArg.EqualsIgnoreCase("from")
        || secondArg.EqualsIgnoreCase("with"))
      secondArg = argument.ThirdWord();

    ObjectInstance source = null;
    bool all = false;

    if (!secondArg.IsNullOrEmpty())
    {
      handler.separate_obj(obj);

      switch (obj.ItemType)
      {
        case ItemTypes.Container when
          (secondArg.EqualsIgnoreCase("all") || secondArg.StartsWithIgnoreCase("all.")):
          all = true;
          break;
        case ItemTypes.Pipe:
        {
          source = ch.GetCarriedObject(secondArg);
          if (CheckFunctions.CheckIfNullObject(ch, source, "You don't have that item.")) return;
          if (sourceItemTypes.All(x => x != source.ItemType))
          {
            comm.act(ATTypes.AT_PLAIN, "You cannot fill $p with $P!", ch, obj, source, ToTypes.Character);
            return;
          }

          break;
        }
        default:
        {
          source = ch.GetObjectOnMeOrInRoom(secondArg);
          if (CheckFunctions.CheckIfNullObject(ch, source, "You cannot find that item.")) return;
          break;
        }
      }
    }
    else source = null;

    if (CheckFunctions.CheckIfTrue(ch, source == null && obj.ItemType == ItemTypes.Pipe,
          "Fill it with what?")) return;

    bool found = false;
    if (source != null)
    {
      foreach (ObjectInstance sourceObj in ch.CurrentRoom.Contents)
      {
        source = sourceObj;
        if (obj.ItemType == ItemTypes.Container)
        {
          if (ch.CanWear(source, ItemWearFlags.Take) || source.IsObjStat(ItemExtraFlags.Buried)
                                                     || (source.IsObjStat(ItemExtraFlags.Prototype) &&
                                                         ch.CanTakePrototype())
                                                     || ch.CarryWeight + source.GetWeight() > ch.CanCarryMaxWeight()
                                                     || source.GetRealWeight() + obj.GetRealWeight() / obj.Count >
                                                     obj.Value[0])
            continue;

          if (all && secondArg[3] == '.' && secondArg.Substring(4).IsAnyEqual(source.Name))
            continue;

          source.InRoom.Contents.Remove(source);
          if (source.ItemType == ItemTypes.Money)
          {
            ch.CurrentCoin += source.Value[0];
            source.Extract();
          }
          else
          {
            source.AddTo(obj);
          }

          found = true;
        }
        else if (sourceItemTypes.Any(x => x == source.ItemType))
        {
          found = true;
          break;
        }
      }

      if (!found)
      {
        ch.SendTo(SourceItemTypeNotFoundTable.GetValueOrDefault(sourceItemTypes.First(), "There is nothing appropriate here!"));
      }

      if (obj.ItemType == ItemTypes.Container)
      {
        comm.act(ATTypes.AT_ACTION, "You fill $p.", ch, obj, null, ToTypes.Character);
        comm.act(ATTypes.AT_ACTION, "$n fills $p.", ch, obj, null, ToTypes.Room);
        return;
      }
    }

    if (obj.ItemType == ItemTypes.Container)
    {
      if (source == obj)
      {
        ch.SendTo("You can't fill something with itself!");
        return;
      }

      switch (source.ItemType)
      {
        case ItemTypes.Money:
          ch.SendTo("You can't do that... yet.");
          break;

        case ItemTypes.PlayerCorpse:
          if (ch.IsNpc())
          {
            ch.SendTo("You can't do that.");
            return;
          }

          if (source.IsObjStat(ItemExtraFlags.ClanCorpse) && !ch.IsImmortal())
          {
            ch.SendTo("Your hands fumble.  Maybe you better loot a different way.");
            return;
          }

          if (!source.IsObjStat(ItemExtraFlags.ClanCorpse) ||
              !((PlayerInstance)ch).PlayerData.Flags.IsSet((int)PCFlags.Deadly))
          {
            // TODO liquids.c lines 1734 to 1758
          }

          break;

        case ItemTypes.Container:
          if (source.ItemType == ItemTypes.Container && source.Value[1].IsSet((int)ContainerFlags.Closed))
          {
            comm.act(ATTypes.AT_PLAIN, "The $d is closed.", ch, null, source.Name, ToTypes.Character);
            return;
          }

          break;

        case ItemTypes.NpcCorpse:
          if (source.Contents.Count == 0)
          {
            ch.SendTo("It's empty.");
            return;
          }

          handler.separate_obj(obj);

          bool wasFound = false;
          foreach (ObjectInstance contentObj in source.Contents)
          {
            if (!ch.CanWear(contentObj, ItemWearFlags.Take)
                || (contentObj.IsObjStat(ItemExtraFlags.Prototype) && !ch.CanTakePrototype())
                || ch.CarryNumber + contentObj.Count > ch.CanCarryN()
                || ch.CarryWeight + contentObj.GetWeight() > ch.CanCarryMaxWeight()
                || source.GetRealWeight() + obj.GetRealWeight() / obj.Count > obj.Value[0])
              continue;

            contentObj.RemoveFrom(source);
            contentObj.AddTo(obj);
            wasFound = true;
          }

          if (wasFound)
          {
            comm.act(ATTypes.AT_ACTION, "You fill $p from $P.", ch, obj, source, ToTypes.Character);
            comm.act(ATTypes.AT_ACTION, "$n fills $p from $P.", ch, obj, source, ToTypes.Room);
          }
          else
          {
            ch.SendTo("There is nothing appropriate in there.");
          }

          break;

        default:
          if (source.InRoom == null
              || ch.CanWear(source, ItemWearFlags.Take)
              || (obj.IsObjStat(ItemExtraFlags.Prototype) && !ch.CanTakePrototype())
              || ch.CarryWeight + source.GetWeight() > ch.CanCarryMaxWeight()
              || source.GetRealWeight() + obj.GetRealWeight() / obj.Count > obj.Value[0])
          {
            ch.SendTo("You can't do that.");
            return;
          }

          handler.separate_obj(obj);
          comm.act(ATTypes.AT_ACTION, "You take $P and put it inside $p.", ch, obj, source, ToTypes.Character);
          comm.act(ATTypes.AT_ACTION, "$n takes $P and puts it inside $p.", ch, obj, source, ToTypes.Room);
          source.InRoom.Contents.Remove(source);
          source.AddTo(obj);
          break;
      }

      return;
    }

    if (source.Value[1] < 1)
    {
      ch.SendTo("There's none left!");
      return;
    }

    if (source.Count > 1 && source.ItemType != ItemTypes.Fountain)
      handler.separate_obj(source);
    handler.separate_obj(obj);

    int diff = GetMaximumCondition();

    switch (obj.ItemType)
    {
      case ItemTypes.Fountain:
        if (obj.Value[1] != 0 && obj.Value[2] != 0)
        {
          ch.SendTo("There is already another liquid in it.");
          return;
        }

        obj.Value[2] = 0;
        obj.Value[1] = obj.Value[0];
        comm.act(ATTypes.AT_ACTION, "You fill $p from $P.", ch, obj, source, ToTypes.Character);
        comm.act(ATTypes.AT_ACTION, "$n fills $p from $P.", ch, obj, source, ToTypes.Room);
        return;

      case ItemTypes.Blood:
        if (obj.Value[1] != 0 && obj.Value[2] != 13)
        {
          ch.SendTo("There is already another liquid in it.");
          return;
        }

        obj.Value[2] = 13;
        if (source.Value[1] < diff)
          diff = source.Value[1];

        obj.Value[1] += diff;
        comm.act(ATTypes.AT_ACTION, "You fill $p from $p.", ch, obj, source, ToTypes.Character);
        comm.act(ATTypes.AT_ACTION, "$n fills $p from $P.", ch, obj, source, ToTypes.Room);

        source.Value[1] -= diff;
        if (source.Value[1] < 1)
        {
          source.Extract();
          ObjectFactory.CreateBloodstain(ch);
        }

        return;

      case ItemTypes.Herb:
        if (obj.Value[1] != 0 && obj.Value[2] != source.Value[2])
        {
          ch.SendTo("There is already another type of herb in it.");
          return;
        }

        obj.Value[2] = source.Value[2];
        if (source.Value[1] < diff)
          diff = source.Value[1];

        obj.Value[1] += diff;
        comm.act(ATTypes.AT_ACTION, "You fill $p from $p.", ch, obj, source, ToTypes.Character);
        comm.act(ATTypes.AT_ACTION, "$n fills $p from $P.", ch, obj, source, ToTypes.Room);

        source.Value[1] -= diff;
        if (source.Value[1] < 1)
          source.Extract();
        return;

      case ItemTypes.HerbContainer:
        if (obj.Value[1] != 0 && obj.Value[2] != source.Value[2])
        {
          ch.SendTo("There is already another type of herb in it.");
          return;
        }

        obj.Value[2] = source.Value[2];
        if (source.Value[1] < diff)
          diff = source.Value[1];

        obj.Value[1] += diff;
        comm.act(ATTypes.AT_ACTION, "You fill $p from $p.", ch, obj, source, ToTypes.Character);
        comm.act(ATTypes.AT_ACTION, "$n fills $p from $P.", ch, obj, source, ToTypes.Room);
        return;

      case ItemTypes.DrinkContainer:
        if (obj.Value[1] != 0 && obj.Value[2] != source.Value[2])
        {
          ch.SendTo("There is already another liquid in it.");
          return;
        }

        obj.Value[2] = source.Value[2];
        if (source.Value[1] < diff)
          diff = source.Value[1];

        obj.Value[1] += diff;
        source.Value[1] -= diff;

        comm.act(ATTypes.AT_ACTION, "You fill $p from $P.", ch, obj, source, ToTypes.Character);
        comm.act(ATTypes.AT_ACTION, "$n fills $p from $P.", ch, obj, source, ToTypes.Room);
        return;

      default:
        // TODO bug
        ch.SendTo("Somethign went wrong...");
        return;
    }
  }

  public const int MAX_COND_VALUE = 100;

  private static Dictionary<ItemTypes, string> SourceItemTypeNotFoundTable =
    new()
    {
      { ItemTypes.Fountain, "There is no fountain or pool here!" },
      { ItemTypes.Blood, "There is no blood pool here!" },
      { ItemTypes.HerbContainer, "There are no herbs here!" },
      { ItemTypes.Herb, "You cannot find any smoking herbs." }
    };

  private static int GetMaximumCondition()
  {
    return GameConstants.GetConstant<int>("MaximumConditionValue");
  }

  private static IEnumerable<ItemTypes> ChooseSourceItemTypes(CharacterInstance ch, ObjectInstance obj)
  {
    switch (obj.ItemType)
    {
      case ItemTypes.Container:
        return [ItemTypes.Container, ItemTypes.NpcCorpse, ItemTypes.PlayerCorpse];
      case ItemTypes.DrinkContainer:
        return [ItemTypes.Fountain, ItemTypes.Blood];
      case ItemTypes.HerbContainer:
        return [ItemTypes.Herb, ItemTypes.HerbContainer];
      case ItemTypes.Pipe:
        return [ItemTypes.Herb, ItemTypes.HerbContainer];
    }

    comm.act(ATTypes.AT_ACTION, "$n tries to fill $p... (Don't ask me how)", ch, obj, null, ToTypes.Room);
    ch.SendTo("You cannot fill that.");
    return [];
  }
}