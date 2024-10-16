﻿using System.Linq;
using Autofac;
using Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Objects;
using SmaugCS.MudProgs;
using CheckFunctions = SmaugCS.Helpers.CheckFunctions;

namespace SmaugCS.Commands.Objects;

public static class Drop
{
  public static void do_drop(CharacterInstance ch, string argument)
  {
    string firstArg = argument.FirstWord();
    if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Drop what?")) return;
    if (handler.FindObject_CheckMentalState(ch)) return;
    if (!ch.IsNpc() && ch.Act.IsSet((int)PlayerFlags.Litterbug))
    {
      ch.SetColor(ATTypes.AT_YELLOW);
      ch.SendTo("A godly force prevents you from dropping anything...");
      return;
    }

    if (ch.CurrentRoom.Flags.IsSet(RoomFlags.NoDrop) && ch != db.Supermob)
    {
      ch.SetColor(ATTypes.AT_MAGIC);
      ch.SendTo("A magical force stops you!");
      ch.SetColor(ATTypes.AT_TELL);
      ch.SendTo("Someone tells you, 'No littering here!'");
      return;
    }

    int number = 0;
    string qty = firstArg.ParseWord(1, ".");
    if (!qty.IsNullOrEmpty() && qty.IsNumber())
      number = qty.ToInt32();

    if (CheckFunctions.CheckIfTrue(ch, number < 1, "That was easy...")) return;

    firstArg = firstArg.ParseWord(2, ".");

    switch (number)
    {
      case > 0 when (firstArg.EqualsIgnoreCase("coins") || firstArg.EqualsIgnoreCase("coin")):
        DropCoins(ch, number);
        break;
      case <= 1 when !firstArg.EqualsIgnoreCase("all") && !firstArg.StartsWithIgnoreCase("all."):
        DropObject(ch, firstArg);
        break;
      default:
        DropAllOrSome(ch, firstArg);
        break;
    }

    if (Program.GameManager.SystemData.SaveFlags.IsSet(AutoSaveFlags.Drop))
      save.save_char_obj(ch);
  }

  private static void DropCoins(CharacterInstance ch, int number)
  {
    if (CheckFunctions.CheckIfTrue(ch, ch.CurrentCoin < number, "You haven't got that many coins.")) return;

    ch.CurrentCoin -= number;

    int num = number;
    ObjectInstance obj = ch.CurrentRoom.Contents.FirstOrDefault(x => x.Id == VnumConstants.OBJ_VNUM_MONEY_ONE);
    if (obj != null)
    {
      num += 1;
      obj.Extract();
    }
    else
    {
      obj = ch.CurrentRoom.Contents.FirstOrDefault(x => x.Id == VnumConstants.OBJ_VNUM_MONEY_SOME);
      if (obj != null)
      {
        num += obj.Value.ToList()[0];
        obj.Extract();
      }
    }

    comm.act(ATTypes.AT_ACTION, "$n drops some coin.", ch, null, null, ToTypes.Room);
    ch.CurrentRoom.AddTo(ObjectFactory.CreateMoney(num));
    ch.SendTo("You let the coin slip from your hand.");

    if (Program.GameManager.SystemData.SaveFlags.IsSet(AutoSaveFlags.Drop))
      save.save_char_obj(ch);
  }

  private static void DropObject(CharacterInstance ch, string firstArg)
  {
    ObjectInstance obj = ch.GetCarriedObject(firstArg);
    if (CheckFunctions.CheckIfNullObject(ch, obj, "You do not have that item.")) return;
    if (CheckFunctions.CheckIfTrue(ch, !ch.CanDrop(obj), "You can't let go of it.")) return;

    obj.Split();
    comm.act(ATTypes.AT_ACTION, "$n drops $p.", ch, obj, null, ToTypes.Room);
    comm.act(ATTypes.AT_ACTION, "You drop $p.", ch, obj, null, ToTypes.Character);

    obj.RemoveFrom();
    obj = ch.CurrentRoom.AddTo(obj);
    MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Drop, ch, obj);

    if (ch.CharDied() || handler.obj_extracted(obj))
      return;

    if (ch.CurrentRoom.Flags.IsSet(RoomFlags.ClanStoreroom))
    {
      foreach (ClanData clan in Program.RepositoryManager.CLANS.Values)
      {
        // TODO Fix
        //if (clan.StoreRoom == ch.CurrentRoom.ID)
        //    act_obj.save_clan_storeroom(ch, clan);
      }
    }

    if (Program.GameManager.SystemData.SaveFlags.IsSet(AutoSaveFlags.Drop))
      save.save_char_obj(ch);
  }

  private static void DropAllOrSome(CharacterInstance ch, string firstArg)
  {
    bool all = firstArg.EqualsIgnoreCase("all");
    string arg = all ? firstArg.Substring(4) : firstArg;

    if (CheckFunctions.CheckIfTrue(ch,
          ch.CurrentRoom.Flags.IsSet(RoomFlags.NoDropAll) || ch.CurrentRoom.Flags.IsSet(RoomFlags.ClanStoreroom),
          "You can't seem to do that.")) return;

    bool found = false;
    foreach (ObjectInstance obj in ch.Carrying)
    {
      if ((all || obj.Name.IsAnyEqual(arg)) && ch.CanSee(obj) && obj.WearLocation == WearLocations.None &&
          ch.CanDrop(obj))
      {
        // TODO
      }
    }
  }
}