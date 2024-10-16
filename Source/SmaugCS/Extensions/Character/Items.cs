﻿using System;
using System.Collections.Generic;
using Autofac;
using Library.Common.Extensions;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Objects;
using SmaugCS.MudProgs;
using CheckFunctions = SmaugCS.Helpers.CheckFunctions;

namespace SmaugCS.Extensions.Character;

public static class Objects
{
  public static bool RemoveFrom(this CharacterInstance ch, WearLocations location, bool replace)
  {
    ObjectInstance obj = ch.GetEquippedItem(location);
    if (obj == null) return true;

    switch (replace)
    {
      case false when ch.CarryNumber + obj.ObjectNumber > ch.CanCarryN():
        comm.act(ATTypes.AT_PLAIN, "$d: you can't carry that many items.", ch, null, obj.ShortDescription,
          ToTypes.Character);
        return false;
      case false:
        return false;
    }

    if (obj.ExtraFlags.IsSet((int)ItemExtraFlags.NoRemove))
    {
      comm.act(ATTypes.AT_PLAIN, "You can't remove $p.", ch, obj, null, ToTypes.Character);
      return false;
    }

    ObjectInstance tObj = ch.GetEquippedItem(WearLocations.DualWield);
    if (obj == ch.GetEquippedItem(WearLocations.Wield) && tObj != null)
      tObj.WearLocation = WearLocations.Wield;

    ch.Unequip(obj);

    comm.act(ATTypes.AT_ACTION, "$n stop using $p.", ch, obj, null, ToTypes.Room);
    comm.act(ATTypes.AT_ACTION, "You stop using $p.", ch, obj, null, ToTypes.Character);
    MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Remove, ch, obj);

    return ch.GetEquippedItem(location) == null;
  }

  /// <summary>
  /// Wear one object. Optional replacement of existing objects.
  /// Restructured a bit to allow for specifying body location and 
  /// added support for layering on certain body locations
  /// </summary>
  public static void WearItem(this CharacterInstance ch, ObjectInstance obj, bool replace, ItemWearFlags wearFlag)
  {
    obj.Split();
    if (ch.Trust < obj.Level)
    {
      ch.Printf("You must be level %d to use this object.", obj.Level);
      comm.act(ATTypes.AT_ACTION, "$n tries to use $p, but is too inexperienced.", ch, obj, null, ToTypes.Room);
      return;
    }

    if (!ch.IsImmortal() && !ch.IsAllowedToUseObject(obj))
    {
      comm.act(ATTypes.AT_MAGIC, "You are forbidden to use that item.", ch, null, null, ToTypes.Character);
      comm.act(ATTypes.AT_ACTION, "$n tries to use $p, but is forbidden to do so.", ch, obj, null, ToTypes.Room);
      return;
    }

    if (obj.ExtraFlags.IsSet((int)ItemExtraFlags.Personal) && ch.Name.EqualsIgnoreCase(obj.Owner))
    {
      ch.SendTo("That item is personalized and belongs to someone else.");
      if (obj.CarriedBy != null)
        obj.RemoveFrom();
      ch.CurrentRoom.AddTo(obj);
      return;
    }

    // TODO Is this going to replace an item already equipped?
    /*int bit;

     f (wear_bit > -1)



         it = wear_bit;

         f (!obj.WearFlags.IsSet(1 << bit))



             f (replace)



                 witch (1 << bit)



                     ase (int)ItemWearFlags.Hold:

                         h.SetColor("You cannot hold that.\r\n", ch);

                         reak;

                     ase (int)ItemWearFlags.Wield:

                     ase (int)ItemWearFlags.MissileWield:

                         h.SetColor("You cannot wield that.\r\n", ch);

                         reak;

                     efault:

                         olor.ch_printf(ch, "You cannot wear that on your %s.\r\n", BuilderConstants.w_flags[bit]);

                         reak;





             eturn;





     lse



         it = -1;

         or (int x = 1; x < 31; x++)



             f (obj.WearFlags.IsSet(1 << x))



                 it = x;

                 reak;





     */

    if (obj.ItemType == ItemTypes.Light)
    {
      if (!ch.RemoveFrom(WearLocations.Light, replace))
        return;
      if (!MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Use, ch, obj,
            null, null))
      {
        comm.act(ATTypes.AT_ACTION, "$n holds $p as a light.", ch, obj, null, ToTypes.Room);
        comm.act(ATTypes.AT_ACTION, "You hold $p as your light.", ch, obj, null, ToTypes.Character);
      }

      ch.Equip(obj, WearLocations.Light);
      MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Wear, ch, obj);
      return;
    }

    /*if (bit == -1)



         f (replace)

             h.SetColor("You can't wear, wield, or hold that.\r\n", ch);

         eturn;

     */

    if (ItemWearMap.TryGetValue(wearFlag, out Action<ObjectInstance, CharacterInstance, bool> value))
      value.Invoke(obj, ch, replace);
    else
    {
      if (wearFlag is ItemWearFlags.Wield or ItemWearFlags.MissileWield)
        ItemWearWield(obj, ch, replace, wearFlag);
      else
      {
        Program.LogManager.Bug("Unknown/Unused ItemWearFlag {0}", wearFlag);
        if (replace)
          ch.SendTo("You can't wear, wield, or hold that.");
      }
    }
  }

  private static readonly Dictionary<ItemWearFlags, Action<ObjectInstance, CharacterInstance, bool>> ItemWearMap =
    new()
    {
      { ItemWearFlags.Finger, ItemWearFinger },
      { ItemWearFlags.Neck, ItemWearNeck },
      { ItemWearFlags.Body, ItemWearBody },
      { ItemWearFlags.Head, ItemWearHead },
      { ItemWearFlags.Eyes, ItemWearEyes },
      { ItemWearFlags.Face, ItemWearFace },
      { ItemWearFlags.Ears, ItemWearEars },
      { ItemWearFlags.Legs, ItemWearLegs },
      { ItemWearFlags.Feet, ItemWearFeet },
      { ItemWearFlags.Hands, ItemWearHands },
      { ItemWearFlags.Arms, ItemWearArms },
      { ItemWearFlags.About, ItemWearAbout },
      { ItemWearFlags.Back, ItemWearBack },
      { ItemWearFlags.Waist, ItemWearWaist },
      { ItemWearFlags.Wrist, ItemWearWrist },
      { ItemWearFlags.Ankle, ItemWearAnkle },
      { ItemWearFlags.Shield, ItemWearShield },
      { ItemWearFlags.Hold, ItemWearHold }
    };

  private static void ItemWearFinger(ObjectInstance obj, CharacterInstance ch, bool replace)
  {
    if (ch.GetEquippedItem(WearLocations.LeftFinger) != null
        && ch.GetEquippedItem(WearLocations.RightFinger) != null
        && !ch.RemoveFrom(WearLocations.LeftFinger, replace)
        && !ch.RemoveFrom(WearLocations.RightFinger, replace))
      return;

    if (ch.GetEquippedItem(WearLocations.LeftFinger) != null)
    {
      if (!MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Use, ch, obj,
            null, null))
      {
        comm.act(ATTypes.AT_ACTION, "$n slips $s left finger into $p.", ch, obj, null, ToTypes.Room);
        comm.act(ATTypes.AT_ACTION, "You slip your left finger into $p.", ch, obj, null, ToTypes.Character);
      }

      ch.Equip(obj, WearLocations.LeftFinger);
      MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Wear, ch, obj);
      return;
    }

    if (ch.GetEquippedItem(WearLocations.RightFinger) != null)
    {
      if (!MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Use, ch, obj,
            null, null))
      {
        comm.act(ATTypes.AT_ACTION, "$n slips $s right finger into $p.", ch, obj, null, ToTypes.Room);
        comm.act(ATTypes.AT_ACTION, "You slip your right finger into $p.", ch, obj, null, ToTypes.Character);
      }

      ch.Equip(obj, WearLocations.RightFinger);
      MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Wear, ch, obj);
      return;
    }

    ch.SendTo("You already wear something on both fingers.");
  }

  private static void ItemWearNeck(ObjectInstance obj, CharacterInstance ch, bool replace)
  {
    if (ch.GetEquippedItem(WearLocations.Neck_1) != null
        && ch.GetEquippedItem(WearLocations.Neck_2) != null
        && !ch.RemoveFrom(WearLocations.Neck_1, replace)
        && !ch.RemoveFrom(WearLocations.Neck_2, replace))
      return;

    if (ch.GetEquippedItem(WearLocations.Neck_1) != null)
    {
      if (!MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Use, ch, obj,
            null, null))
      {
        comm.act(ATTypes.AT_ACTION, "$n wears $p around $s neck.", ch, obj, null, ToTypes.Room);
        comm.act(ATTypes.AT_ACTION, "You wear $p around your neck.", ch, obj, null, ToTypes.Character);
      }

      ch.Equip(obj, WearLocations.Neck_1);
      MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Wear, ch, obj);
      return;
    }

    if (ch.GetEquippedItem(WearLocations.Neck_2) != null)
    {
      if (!MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Use, ch, obj,
            null, null))
      {
        comm.act(ATTypes.AT_ACTION, "$n wears $p around $s neck.", ch, obj, null, ToTypes.Room);
        comm.act(ATTypes.AT_ACTION, "You wear $p around your neck.", ch, obj, null, ToTypes.Character);
      }

      ch.Equip(obj, WearLocations.Neck_2);
      MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Wear, ch, obj);
      return;
    }

    ch.SendTo("You already wear two neck items.");
  }

  private static void ItemWearBody(ObjectInstance obj, CharacterInstance ch, bool replace)
  {
    if (CheckFunctions.CheckIfTrue(ch, !ch.CanWearLayer(obj, WearLocations.Body),
          "It won't fit overtop of what you're already wearing.")) return;

    if (!MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Use, ch, obj, null,
          null))
    {
      comm.act(ATTypes.AT_ACTION, "$n fits $p on $s body.", ch, obj, null, ToTypes.Room);
      comm.act(ATTypes.AT_ACTION, "You fit $p on your body.", ch, obj, null, ToTypes.Character);
    }

    ch.Equip(obj, WearLocations.Body);
    MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Wear, ch, obj);
  }

  private static void ItemWearHead(ObjectInstance obj, CharacterInstance ch, bool replace)
  {
    if (!ch.RemoveFrom(WearLocations.Head, replace))
      return;

    if (!MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Use, ch, obj, null,
          null))
    {
      comm.act(ATTypes.AT_ACTION, "$n dons $p upon $s body.", ch, obj, null, ToTypes.Room);
      comm.act(ATTypes.AT_ACTION, "You don $p upon your head.", ch, obj, null, ToTypes.Character);
    }

    ch.Equip(obj, WearLocations.Head);
    MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Wear, ch, obj);
  }

  private static void ItemWearEyes(ObjectInstance obj, CharacterInstance ch, bool replace)
  {
    if (!ch.RemoveFrom(WearLocations.Eyes, replace))
      return;

    if (!MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Use, ch, obj, null,
          null))
    {
      comm.act(ATTypes.AT_ACTION, "$n places $p on $s eyes.", ch, obj, null, ToTypes.Room);
      comm.act(ATTypes.AT_ACTION, "You place $p on your eyes.", ch, obj, null, ToTypes.Character);
    }

    ch.Equip(obj, WearLocations.Eyes);
    MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Wear, ch, obj);
  }

  private static void ItemWearFace(ObjectInstance obj, CharacterInstance ch, bool replace)
  {
    if (!ch.RemoveFrom(WearLocations.Face, replace))
      return;

    if (!MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Use, ch, obj, null,
          null))
    {
      comm.act(ATTypes.AT_ACTION, "$n places $p on $s face.", ch, obj, null, ToTypes.Room);
      comm.act(ATTypes.AT_ACTION, "You place $p on your face.", ch, obj, null, ToTypes.Character);
    }

    ch.Equip(obj, WearLocations.Face);
    MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Wear, ch, obj);
  }

  private static void ItemWearEars(ObjectInstance obj, CharacterInstance ch, bool replace)
  {
    if (!ch.RemoveFrom(WearLocations.Ears, replace))
      return;

    if (!MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Use, ch, obj, null,
          null))
    {
      comm.act(ATTypes.AT_ACTION, "$n wears $p on $s ears.", ch, obj, null, ToTypes.Room);
      comm.act(ATTypes.AT_ACTION, "You wear $p on your ears.", ch, obj, null, ToTypes.Character);
    }

    ch.Equip(obj, WearLocations.Ears);
    MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Wear, ch, obj);
  }

  private static void ItemWearLegs(ObjectInstance obj, CharacterInstance ch, bool replace)
  {
    if (CheckFunctions.CheckIfTrue(ch, !ch.CanWearLayer(obj, WearLocations.Legs),
          "It won't fit overtop of what you're already wearing.")) return;

    if (!MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Use, ch, obj, null,
          null))
    {
      comm.act(ATTypes.AT_ACTION, "$n slips into $p.", ch, obj, null, ToTypes.Room);
      comm.act(ATTypes.AT_ACTION, "You slip into $p.", ch, obj, null, ToTypes.Character);
    }

    ch.Equip(obj, WearLocations.Legs);
    MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Wear, ch, obj);
  }

  private static void ItemWearFeet(ObjectInstance obj, CharacterInstance ch, bool replace)
  {
    if (CheckFunctions.CheckIfTrue(ch, !ch.CanWearLayer(obj, WearLocations.Feet),
          "It won't fit overtop of what you're already wearing.")) return;

    if (!MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Use, ch, obj, null,
          null))
    {
      comm.act(ATTypes.AT_ACTION, "$n wears $p on $s feet.", ch, obj, null, ToTypes.Room);
      comm.act(ATTypes.AT_ACTION, "You wear $p on your feet.", ch, obj, null, ToTypes.Character);
    }

    ch.Equip(obj, WearLocations.Feet);
    MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Wear, ch, obj);
  }

  private static void ItemWearHands(ObjectInstance obj, CharacterInstance ch, bool replace)
  {
    if (CheckFunctions.CheckIfTrue(ch, !ch.CanWearLayer(obj, WearLocations.Hands),
          "It won't fit overtop of what you're already wearing.")) return;

    if (!MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Use, ch, obj, null,
          null))
    {
      comm.act(ATTypes.AT_ACTION, "$n wears $p on $s hands.", ch, obj, null, ToTypes.Room);
      comm.act(ATTypes.AT_ACTION, "You wear $p on your hands.", ch, obj, null, ToTypes.Character);
    }

    ch.Equip(obj, WearLocations.Hands);
    MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Wear, ch, obj);
  }

  private static void ItemWearArms(ObjectInstance obj, CharacterInstance ch, bool replace)
  {
    if (CheckFunctions.CheckIfTrue(ch, !ch.CanWearLayer(obj, WearLocations.Arms),
          "It won't fit overtop of what you're already wearing.")) return;

    if (!MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Use, ch, obj, null,
          null))
    {
      comm.act(ATTypes.AT_ACTION, "$n wears $p on $s arms.", ch, obj, null, ToTypes.Room);
      comm.act(ATTypes.AT_ACTION, "You wear $p on your arms.", ch, obj, null, ToTypes.Character);
    }

    ch.Equip(obj, WearLocations.Arms);
    MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Wear, ch, obj);
  }

  private static void ItemWearAbout(ObjectInstance obj, CharacterInstance ch, bool replace)
  {
    if (CheckFunctions.CheckIfTrue(ch, !ch.CanWearLayer(obj, WearLocations.About),
          "It won't fit overtop of what you're already wearing.")) return;

    if (!MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Use, ch, obj, null,
          null))
    {
      comm.act(ATTypes.AT_ACTION, "$n wears $p about $s arms.", ch, obj, null, ToTypes.Room);
      comm.act(ATTypes.AT_ACTION, "You wear $p about your arms.", ch, obj, null, ToTypes.Character);
    }

    ch.Equip(obj, WearLocations.About);
    MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Wear, ch, obj);
  }

  private static void ItemWearBack(ObjectInstance obj, CharacterInstance ch, bool replace)
  {
    if (!ch.RemoveFrom(WearLocations.Back, replace))
      return;

    if (!MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Use, ch, obj, null,
          null))
    {
      comm.act(ATTypes.AT_ACTION, "$n slings $p on $s back.", ch, obj, null, ToTypes.Room);
      comm.act(ATTypes.AT_ACTION, "You sling $p on your back.", ch, obj, null, ToTypes.Character);
    }

    ch.Equip(obj, WearLocations.Back);
    MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Wear, ch, obj);
  }

  private static void ItemWearWaist(ObjectInstance obj, CharacterInstance ch, bool replace)
  {
    if (CheckFunctions.CheckIfTrue(ch, !ch.CanWearLayer(obj, WearLocations.Waist),
          "It won't fit overtop of what you're already wearing.")) return;

    if (!MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Use, ch, obj, null,
          null))
    {
      comm.act(ATTypes.AT_ACTION, "$n wears $p about $s waist.", ch, obj, null, ToTypes.Room);
      comm.act(ATTypes.AT_ACTION, "You wear $p about your waist.", ch, obj, null, ToTypes.Character);
    }

    ch.Equip(obj, WearLocations.Waist);
    MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Wear, ch, obj);
  }

  private static void ItemWearWrist(ObjectInstance obj, CharacterInstance ch, bool replace)
  {
    if (ch.GetEquippedItem(WearLocations.LeftWrist) != null
        && ch.GetEquippedItem(WearLocations.RightWrist) != null
        && !ch.RemoveFrom(WearLocations.LeftWrist, replace)
        && !ch.RemoveFrom(WearLocations.RightWrist, replace))
      return;

    if (ch.GetEquippedItem(WearLocations.LeftWrist) != null)
    {
      if (!MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Use, ch, obj,
            null, null))
      {
        comm.act(ATTypes.AT_ACTION, "$n fit $p around $s left wrist.", ch, obj, null, ToTypes.Room);
        comm.act(ATTypes.AT_ACTION, "You fit $p around your left wrist.", ch, obj, null, ToTypes.Character);
      }

      ch.Equip(obj, WearLocations.LeftWrist);
      MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Wear, ch, obj);
      return;
    }

    if (ch.GetEquippedItem(WearLocations.RightWrist) != null)
    {
      if (!MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Use, ch, obj,
            null, null))
      {
        comm.act(ATTypes.AT_ACTION, "$n fit $p around $s right wrist.", ch, obj, null, ToTypes.Room);
        comm.act(ATTypes.AT_ACTION, "You fit $p around your right wrist.", ch, obj, null, ToTypes.Character);
      }

      ch.Equip(obj, WearLocations.RightWrist);
      MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Wear, ch, obj);
      return;
    }

    ch.SendTo("You already wear two wrist items.");
  }

  private static void ItemWearAnkle(ObjectInstance obj, CharacterInstance ch, bool replace)
  {
    if (ch.GetEquippedItem(WearLocations.LeftAnkle) != null
        && ch.GetEquippedItem(WearLocations.RightAnkle) != null
        && !ch.RemoveFrom(WearLocations.LeftAnkle, replace)
        && !ch.RemoveFrom(WearLocations.RightAnkle, replace))
      return;

    if (ch.GetEquippedItem(WearLocations.LeftAnkle) != null)
    {
      if (!MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Use, ch, obj,
            null, null))
      {
        comm.act(ATTypes.AT_ACTION, "$n fit $p around $s left ankle.", ch, obj, null, ToTypes.Room);
        comm.act(ATTypes.AT_ACTION, "You fit $p around your left ankle.", ch, obj, null, ToTypes.Character);
      }

      ch.Equip(obj, WearLocations.LeftAnkle);
      MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Wear, ch, obj);
      return;
    }

    if (ch.GetEquippedItem(WearLocations.RightAnkle) != null)
    {
      if (!MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Use, ch, obj,
            null, null))
      {
        comm.act(ATTypes.AT_ACTION, "$n fit $p around $s right ankle.", ch, obj, null, ToTypes.Room);
        comm.act(ATTypes.AT_ACTION, "You fit $p around your right ankle.", ch, obj, null, ToTypes.Character);
      }

      ch.Equip(obj, WearLocations.RightAnkle);
      MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Wear, ch, obj);
      return;
    }

    ch.SendTo("You already wear two ankle items.");
  }

  private static void ItemWearShield(ObjectInstance obj, CharacterInstance ch, bool replace)
  {
    if (ch.GetEquippedItem(WearLocations.DualWield) != null
        || (ch.GetEquippedItem(WearLocations.Wield) != null
            && ch.GetEquippedItem(WearLocations.WieldMissile) != null)
        || (ch.GetEquippedItem(WearLocations.Wield) != null
            && ch.GetEquippedItem(WearLocations.Hold) != null))
    {
      ch.SendTo(
        ch.GetEquippedItem(WearLocations.Hold) != null
          ? "You can't use a shield while using a weapon and holding something!"
          : "You can't use a shield AND two weapons!");
      return;
    }

    if (!ch.RemoveFrom(WearLocations.Shield, replace))
      return;
    if (!MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Use, ch, obj, null,
          null))
    {
      comm.act(ATTypes.AT_ACTION, "$n uses $p as a shield.", ch, obj, null, ToTypes.Room);
      comm.act(ATTypes.AT_ACTION, "You use $p as a shield.", ch, obj, null, ToTypes.Character);
    }

    ch.Equip(obj, WearLocations.Shield);
    MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Wear, ch, obj);
  }

  private static void ItemWearWield(ObjectInstance obj, CharacterInstance ch, bool replace, ItemWearFlags wearFlag)
  {
    int strWieldMod = (int)Program.LookupManager.GetStatMod("Strength", ch.GetCurrentStrength(),
      StrengthModTypes.Wield);

    if (!ch.CouldDualWield())
    {
      if (!ch.RemoveFrom(WearLocations.WieldMissile, replace)
          || !ch.RemoveFrom(WearLocations.Wield, replace))
        return;
    }
    else
    {
      ObjectInstance tobj = ch.GetEquippedItem(WearLocations.Wield);
      ObjectInstance mw = ch.GetEquippedItem(WearLocations.WieldMissile);
      ObjectInstance dw = ch.GetEquippedItem(WearLocations.DualWield);
      ObjectInstance hd = ch.GetEquippedItem(WearLocations.Hold);
      ObjectInstance sd = ch.GetEquippedItem(WearLocations.Shield);

      if (CheckFunctions.CheckIfTrue(ch, hd != null && sd != null,
            "You are already holding something and wearing a shield.")) return;

      if (tobj != null)
      {
        if (!ch.CanDualWield()) return;

        if (CheckFunctions.CheckIfTrue(ch,
              obj.GetWeight() + tobj.GetWeight() > strWieldMod, "It is too heavy for you to wield."))
          return;

        if (CheckFunctions.CheckIfTrue(ch, hd != null || sd != null,
              "You're already wielding a weapon AND holding something.")) return;

        if (!MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Use, ch, obj,
              null, null))
        {
          comm.act(ATTypes.AT_ACTION, "$n dual-wields $p.", ch, obj, null, ToTypes.Room);
          comm.act(ATTypes.AT_ACTION, "You dual-wield $p.", ch, obj, null, ToTypes.Character);
        }

        ch.Equip(obj, wearFlag == ItemWearFlags.MissileWield
          ? WearLocations.WieldMissile
          : WearLocations.DualWield);
        MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Wear, ch, obj);
        return;
      }

      if (mw != null)
      {
        ItemEquipMissileWeapon(obj, ch, mw, dw, hd, sd);
        return;
      }
    }

    if (CheckFunctions.CheckIfTrue(ch, obj.GetWeight() > strWieldMod, "It is too heavy for you to wield."))
      return;

    if (!MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Use, ch, obj, null,
          null))
    {
      comm.act(ATTypes.AT_ACTION, "$n wields $p.", ch, obj, null, ToTypes.Room);
      comm.act(ATTypes.AT_ACTION, "You wield $p.", ch, obj, null, ToTypes.Character);
    }

    ch.Equip(obj, wearFlag == ItemWearFlags.MissileWield ? WearLocations.WieldMissile : WearLocations.Wield);
    MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Wear, ch, obj);
  }

  private static void ItemEquipMissileWeapon(ObjectInstance obj, CharacterInstance ch, ObjectInstance mw,
    ObjectInstance dw, ObjectInstance hd, ObjectInstance sd)
  {
    if (!ch.CanDualWield())
      return;

    if (CheckFunctions.CheckIfTrue(ch, obj.ItemType == ItemTypes.MissileWeapon,
          "You're already wielding a missile weapon.")) return;

    int strWieldMod = (int)Program.LookupManager.GetStatMod("Strength", ch.GetCurrentStrength(),
      StrengthModTypes.Wield);

    if (CheckFunctions.CheckIfTrue(ch,
          obj.GetWeight() + mw.GetWeight() > strWieldMod, "It is too heavy for you to wield.")) return;

    if (CheckFunctions.CheckIfNotNullObject(ch, dw, "You're already wielding two weapons.")) return;
    if (CheckFunctions.CheckIfTrue(ch, hd != null || sd != null,
          "You're already wielding a weapon AND holding something.")) return;

    if (!MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Use, ch, obj, null,
          null))
    {
      comm.act(ATTypes.AT_ACTION, "$n wields $p.", ch, obj, null, ToTypes.Room);
      comm.act(ATTypes.AT_ACTION, "You wield $p.", ch, obj, null, ToTypes.Character);
    }

    ch.Equip(obj, WearLocations.Wield);
    MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Wear, ch, obj);
  }

  private static void ItemWearHold(ObjectInstance obj, CharacterInstance ch, bool replace)
  {
    if (ch.GetEquippedItem(WearLocations.DualWield) != null
        || (ch.GetEquippedItem(WearLocations.Wield) != null
            && (ch.GetEquippedItem(WearLocations.WieldMissile) != null
                || ch.GetEquippedItem(WearLocations.Shield) != null)))
    {
      ch.SendTo(
        ch.GetEquippedItem(WearLocations.Shield) != null
          ? "You cannot hold something while using a weapon and a shield!"
          : "You cannot hold something AND two weapons!");
      return;
    }

    if (!ch.RemoveFrom(WearLocations.Hold, replace))
      return;

    if (obj.ItemType == ItemTypes.Wand
        || obj.ItemType == ItemTypes.Staff
        || obj.ItemType == ItemTypes.Food
        || obj.ItemType == ItemTypes.Cook
        || obj.ItemType == ItemTypes.Pill
        || obj.ItemType == ItemTypes.Potion
        || obj.ItemType == ItemTypes.Scroll
        || obj.ItemType == ItemTypes.DrinkContainer
        || obj.ItemType == ItemTypes.Blood
        || obj.ItemType == ItemTypes.Pipe
        || obj.ItemType == ItemTypes.Herb
        || obj.ItemType == ItemTypes.Key
        || !MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Use, ch, obj,
          null, null))
    {
      comm.act(ATTypes.AT_ACTION, "$n holds $p in $s hands.", ch, obj, null, ToTypes.Room);
      comm.act(ATTypes.AT_ACTION, "You hold $p in your hands.", ch, obj, null, ToTypes.Character);
    }

    ch.Equip(obj, WearLocations.Hold);
    MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Wear, ch, obj);
  }
}