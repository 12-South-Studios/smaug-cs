using System;
using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Organizations;
using SmaugCS.Logging;
using SmaugCS.Managers;

namespace SmaugCS
{
    public static class act_obj
    {
        public static void get_obj(CharacterInstance ch, ObjectInstance obj, ObjectInstance container)
        {
            if (!obj.WearFlags.IsSet(ItemWearFlags.Take)
                && (ch.Level < GameManager.Instance.SystemData.GetMinimumLevel(PlayerPermissionTypes.LevelGetObjectNoTake)))
            {
                color.send_to_char("You can't take that.\r\n", ch);
                return;
            }

            if (obj.magic_flags.IsSet(ItemMagicFlags.PKDisarmed) && !ch.IsNpc())
            {
                TimerData timer = ch.GetTimer(TimerTypes.PKilled);
                if (ch.CanPKill() && timer == null)
                {
                    if (ch.Level - obj.Value[5] > 5 || obj.Value[5] - ch.Level > 5)
                    {
                        color.send_to_char_color("\r\n&bA godly force freezes your outstretched hand.\r\n", ch);
                        return;
                    }

                    obj.magic_flags.RemoveBit(ItemMagicFlags.PKDisarmed);
                    obj.Value[5] = 0;
                }
            }
            else
            {
                color.send_to_char_color("\r\n&BA godly force freezes your outstretched hand.\r\n", ch);
                return;
            }

            if (obj.ExtraFlags.IsSet(ItemExtraFlags.Prototype) && !ch.CanTakePrototype())
            {
                color.send_to_char("A godly force prevents you from getting close to it.\r\n", ch);
                return;
            }

            if (ch.CarryNumber + obj.GetObjectNumber() > ch.CanCarryN())
            {
                comm.act(ATTypes.AT_PLAIN, "$d: you can't carry that many items.", ch, null, obj.ShortDescription, ToTypes.Character);
                return;
            }

            int weight = obj.ExtraFlags.IsSet(ItemExtraFlags.Covering)
                             ? obj.Weight
                             : obj.GetObjectWeight();

            if (obj.ItemType != ItemTypes.Money)
            {
                if (obj.InObject != null)
                {
                    ObjectInstance tObject = obj.InObject;
                    int inobj = 1;
                    bool checkweight = tObject.ItemType == ItemTypes.Container
                                       && tObject.ExtraFlags.IsSet(ItemExtraFlags.Magical);

                    while (tObject.InObject != null)
                    {
                        tObject = tObject.InObject;
                        inobj++;

                        checkweight = tObject.ItemType == ItemTypes.Container
                                      && tObject.ExtraFlags.IsSet(ItemExtraFlags.Magical);
                    }

                    if (tObject.CarriedBy == null || tObject.CarriedBy != ch || checkweight)
                    {
                        if ((ch.CarryWeight + weight) > ch.CanCarryMaxWeight())
                        {
                            comm.act(ATTypes.AT_PLAIN, "$d: you can't carry that much weight.", ch, null, obj.ShortDescription, ToTypes.Character);
                            return;
                        }
                    }
                }
                else if ((ch.CarryWeight + weight) > ch.CanCarryMaxWeight())
                {
                    comm.act(ATTypes.AT_PLAIN, "$d: you can't carry that much weight.", ch, null, obj.ShortDescription, ToTypes.Character);
                    return;
                }
            }

            if (container != null)
            {
                if (container.ItemType == ItemTypes.KeyRing
                    && !container.ExtraFlags.IsSet(ItemExtraFlags.Covering))
                {
                    comm.act(ATTypes.AT_ACTION, "You remove $p from $P", ch, obj, container, ToTypes.Character);
                    comm.act(ATTypes.AT_ACTION, "$n removes $p from $P", ch, obj, container, ToTypes.Room);
                }
                else
                {
                    comm.act(ATTypes.AT_ACTION, container.ExtraFlags.IsSet(ItemExtraFlags.Covering)
                                                    ? "You get $p from beneath $P."
                                                    : "You get $p from $P", ch, obj, container, ToTypes.Character);
                    comm.act(ATTypes.AT_ACTION, container.ExtraFlags.IsSet(ItemExtraFlags.Covering)
                                                    ? "$n gets $p from beneath $P."
                                                    : "$n gets $p from $P", ch, obj, container, ToTypes.Room);
                }

                if (container.ExtraFlags.IsSet(ItemExtraFlags.ClanCorpse)
                    && !ch.IsNpc() && container.Name.Contains(ch.Name))
                    container.Value[5]++;
                obj.InObject.FromObject(obj);
            }
            else
            {
                comm.act(ATTypes.AT_ACTION, "You get $p.", ch, obj, null, ToTypes.Character);
                comm.act(ATTypes.AT_ACTION, "$n gets $p.", ch, obj, null, ToTypes.Room);
                obj.InRoom.FromRoom(obj);
            }

            if (ch.CurrentRoom.Flags.IsSet((int)RoomFlags.ClanStoreroom)
                && (container == null || container.CarriedBy == null))
            {
                foreach (ClanData clan in DatabaseManager.Instance.CLANS.Values)
                {
                    //if (clan.StoreRoom == ch.CurrentRoom.Vnum)
                    //     clan.SaveStoreroom(ch);
                }
            }

            if (obj.ItemType != ItemTypes.Container)
                handler.check_for_trap(ch, obj, TrapTriggerTypes.Get);
            if (ch.CharDied())
                return;

            if (obj.ItemType == ItemTypes.Money)
            {
                int amt = obj.Value[0] * obj.Count;
                ch.CurrentCoin += amt;
                handler.extract_obj(obj);
            }
            else
                obj = obj.ToCharacter(ch);

            if (ch.CharDied() || handler.obj_extracted(obj))
                return;

            mud_prog.oprog_get_trigger(ch, obj);
        }

        /// <summary>
        /// Damage an object, effect player's AC if necessary, make object 
        /// into scraps if necessary, send message about damaged object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ReturnTypes damage_obj(ObjectInstance obj)
        {
            CharacterInstance ch = obj.CarriedBy;
            handler.separate_obj(obj);

            if (!ch.IsNpc() && (!ch.IsPKill() || (ch.IsPKill() && !ch.PlayerData.Flags.IsSet((int)PCFlags.Gag))))
                comm.act(ATTypes.AT_OBJECT, "($p gets damaged)", ch, obj, null, ToTypes.Character);
            else if (obj.InRoom != null && obj.InRoom.Persons.First() != null)
            {
                ch = obj.InRoom.Persons.First();
                comm.act(ATTypes.AT_OBJECT, "($p gets damaged)", ch, obj, null, ToTypes.Room);
                comm.act(ATTypes.AT_OBJECT, "($p gets damaged)", ch, obj, null, ToTypes.Character);
                ch = null;
            }

            if (obj.ItemType != ItemTypes.Light)
                mud_prog.oprog_damage_trigger(ch, obj);
            else if (!ch.IsInArena())
                mud_prog.oprog_damage_trigger(ch, obj);

            if (handler.obj_extracted(obj))
                return db.GlobalObjectCode;

            ReturnTypes returnVal = ReturnTypes.None;

            switch (obj.ItemType)
            {
                default:
                    ObjectFactory.CreateScraps(obj);
                    returnVal = ReturnTypes.ObjectScrapped;
                    break;
                case ItemTypes.Container:
                case ItemTypes.KeyRing:
                case ItemTypes.Quiver:
                    if (--obj.Value[3] <= 0)
                    {
                        if (!ch.IsInArena())
                        {
                            ObjectFactory.CreateScraps(obj);
                            returnVal = ReturnTypes.ObjectScrapped;
                        }
                        else
                            obj.Value[3] = 1;
                    }
                    break;
                case ItemTypes.Light:
                    if (--obj.Value[0] <= 0)
                    {
                        if (!ch.IsInArena())
                        {
                            ObjectFactory.CreateScraps(obj);
                            returnVal = ReturnTypes.ObjectScrapped;
                        }
                        else
                            obj.Value[0] = 1;
                    }
                    break;
                case ItemTypes.Armor:
                    if (ch != null && obj.Value[0] >= 1)
                        ch.ArmorClass += obj.ApplyArmorClass;
                    if (--obj.Value[0] <= 0)
                    {
                        if (!ch.IsPKill() && !ch.IsInArena())
                        {
                            ObjectFactory.CreateScraps(obj);
                            returnVal = ReturnTypes.ObjectScrapped;
                        }
                        else
                        {
                            obj.Value[0] = 1;
                            ch.ArmorClass -= obj.ApplyArmorClass;
                        }
                    }
                    else if (ch != null && obj.Value[0] >= 1)
                        ch.ArmorClass -= obj.ApplyArmorClass;
                    break;
                case ItemTypes.Weapon:
                    if (--obj.Value[0] <= 0)
                    {
                        if (!ch.IsPKill() && !ch.IsInArena())
                        {
                            ObjectFactory.CreateScraps(obj);
                            returnVal = ReturnTypes.ObjectScrapped;
                        }
                        else
                            obj.Value[0] = 1;
                    }
                    break;
            }

            if (ch != null)
                save.save_char_obj(ch);

            return returnVal;
        }

        public static bool remove_obj(CharacterInstance ch, int iWear, bool fReplace)
        {
            ObjectInstance obj = ch.GetEquippedItem(iWear);
            if (obj == null)
                return true;

            if (!fReplace && ch.CarryNumber + obj.GetObjectNumber() > ch.CanCarryN())
            {
                comm.act(ATTypes.AT_PLAIN, "$d: you can't carry that many items.", ch, null, obj.ShortDescription, ToTypes.Character);
                return false;
            }

            if (!fReplace)
                return false;

            if (obj.ExtraFlags.IsSet(ItemExtraFlags.NoRemove))
            {
                comm.act(ATTypes.AT_PLAIN, "You can't remove $p.", ch, obj, null, ToTypes.Character);
                return false;
            }

            ObjectInstance tObj = ch.GetEquippedItem(WearLocations.DualWield);
            if (obj == ch.GetEquippedItem(WearLocations.Wield)
                && tObj != null)
                tObj.WearLocation = WearLocations.Wield;

            ch.Unequip(obj);

            comm.act(ATTypes.AT_ACTION, "$n stop using $p.", ch, obj, null, ToTypes.Room);
            comm.act(ATTypes.AT_ACTION, "You stop using $p.", ch, obj, null, ToTypes.Character);
            mud_prog.oprog_remove_trigger(ch, obj);

            // Check in case, the trigger forces them to rewear the item
            return ch.GetEquippedItem(iWear) == null;
        }

        /// <summary>
        /// Wear one object. Optional replacement of existing objects.
        /// Restructured a bit to allow for specifying body location and 
        /// added support for layering on certain body locations
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="obj"></param>
        /// <param name="fReplace"></param>
        /// <param name="wear_bit"></param>
        public static void wear_obj(CharacterInstance ch, ObjectInstance obj, bool fReplace, short wear_bit)
        {
            handler.separate_obj(obj);
            if (ch.Trust < obj.Level)
            {
                color.ch_printf(ch, "You must be level %d to use this object.\r\n", obj.Level);
                comm.act(ATTypes.AT_ACTION, "$n tries to use $p, but is too inexperienced.", ch, obj, null, ToTypes.Room);
                return;
            }

            if (!ch.IsImmortal() && !ch.IsAllowedToUseObject(obj))
            {
                comm.act(ATTypes.AT_MAGIC, "You are forbidden to use that item.", ch, null, null, ToTypes.Character);
                comm.act(ATTypes.AT_ACTION, "$n tries to use $p, but is forbidden to do so.", ch, obj, null, ToTypes.Room);
                return;
            }

            if (obj.ExtraFlags.IsSet(ItemExtraFlags.Personal)
                && ch.Name.EqualsIgnoreCase(obj.Owner))
            {
                color.send_to_char("That item is personalized and belongs to someone else.\r\n", ch);
                if (obj.CarriedBy != null)
                    obj.FromCharacter();
                ch.CurrentRoom.ToRoom(obj);
                return;
            }

            int bit;
            if (wear_bit > -1)
            {
                bit = wear_bit;
                if (!obj.WearFlags.IsSet(1 << bit))
                {
                    if (fReplace)
                    {
                        switch (1 << bit)
                        {
                            case (int)ItemWearFlags.Hold:
                                color.send_to_char("You cannot hold that.\r\n", ch);
                                break;
                            case (int)ItemWearFlags.Wield:
                            case (int)ItemWearFlags.MissileWield:
                                color.send_to_char("You cannot wield that.\r\n", ch);
                                break;
                            default:
                                color.ch_printf(ch, "You cannot wear that on your %s.\r\n", BuilderConstants.w_flags[bit]);
                                break;
                        }
                    }
                    return;
                }
            }
            else
            {
                bit = -1;
                for (int x = 1; x < 31; x++)
                {
                    if (obj.WearFlags.IsSet(1 << x))
                    {
                        bit = x;
                        break;
                    }
                }
            }

            if (obj.ItemType == ItemTypes.Light)
            {
                if (!remove_obj(ch, (int)WearLocations.Light, fReplace))
                    return;
                if (!mud_prog.oprog_use_trigger(ch, obj, null, null))
                {
                    comm.act(ATTypes.AT_ACTION, "$n holds $p as a light.", ch, obj, null, ToTypes.Room);
                    comm.act(ATTypes.AT_ACTION, "You hold $p as your light.", ch, obj, null, ToTypes.Character);
                }

                ch.Equip(obj, (int)WearLocations.Light);
                mud_prog.oprog_wear_trigger(ch, obj);
                return;
            }

            if (bit == -1)
            {
                if (fReplace)
                    color.send_to_char("You can't wear, wield, or hold that.\r\n", ch);
                return;
            }

            ItemWearFlags wearLoc = Realm.Library.Common.EnumerationExtensions.GetEnum<ItemWearFlags>(1 << bit);
            if (ItemWearMap.ContainsKey(wearLoc))
                ItemWearMap[wearLoc].Invoke(obj, ch, fReplace);
            else
            {
                if (wearLoc == ItemWearFlags.Wield || wearLoc == ItemWearFlags.MissileWield)
                    ItemWearWield(obj, ch, fReplace, bit);
                else
                {
                    LogManager.Instance.Bug("Unknown/Unused ItemWearFlag {0} ({1}}", wearLoc, 1 << bit);
                    if (fReplace)
                        color.send_to_char("You can't wear, wield, or hold that.\r\n", ch);
                }
            }
        }

        private static readonly Dictionary<ItemWearFlags, Action<ObjectInstance, CharacterInstance, bool>> ItemWearMap = new Dictionary<ItemWearFlags, Action<ObjectInstance, CharacterInstance, bool>>()
            { 
                {ItemWearFlags.Finger, ItemWearFinger},
                {ItemWearFlags.Neck, ItemWearNeck},
                {ItemWearFlags.Body, ItemWearBody},
                {ItemWearFlags.Head, ItemWearHead},
                {ItemWearFlags.Eyes, ItemWearEyes},
                {ItemWearFlags.Face, ItemWearFace},
                {ItemWearFlags.Ears, ItemWearEars},
                {ItemWearFlags.Legs, ItemWearLegs},
                {ItemWearFlags.Feet, ItemWearFeet},
                {ItemWearFlags.Hands, ItemWearHands},
                {ItemWearFlags.Arms, ItemWearArms},
                {ItemWearFlags.About, ItemWearAbout},
                {ItemWearFlags.Back, ItemWearBack},
                {ItemWearFlags.Waist, ItemWearWaist},
                {ItemWearFlags.Wrist, ItemWearWrist},
                {ItemWearFlags.Ankle, ItemWearAnkle},
                {ItemWearFlags.Shield, ItemWearShield},
                {ItemWearFlags.Hold, ItemWearHold}
            };

        private static void ItemWearFinger(ObjectInstance obj, CharacterInstance ch, bool fReplace)
        {
            if (ch.GetEquippedItem(WearLocations.LeftFinger) != null
                && ch.GetEquippedItem(WearLocations.RightFinger) != null
                && !remove_obj(ch, (int)WearLocations.LeftFinger, fReplace)
                && !remove_obj(ch, (int)WearLocations.RightFinger, fReplace))
                return;

            if (ch.GetEquippedItem(WearLocations.LeftFinger) != null)
            {
                if (!mud_prog.oprog_use_trigger(ch, obj, null, null))
                {
                    comm.act(ATTypes.AT_ACTION, "$n slips $s left finger into $p.", ch, obj, null, ToTypes.Room);
                    comm.act(ATTypes.AT_ACTION, "You slip your left finger into $p.", ch, obj, null, ToTypes.Character);
                }

                ch.Equip(obj, (int)WearLocations.LeftFinger);
                mud_prog.oprog_wear_trigger(ch, obj);
                return;
            }

            if (ch.GetEquippedItem(WearLocations.RightFinger) != null)
            {
                if (!mud_prog.oprog_use_trigger(ch, obj, null, null))
                {
                    comm.act(ATTypes.AT_ACTION, "$n slips $s right finger into $p.", ch, obj, null, ToTypes.Room);
                    comm.act(ATTypes.AT_ACTION, "You slip your right finger into $p.", ch, obj, null, ToTypes.Character);
                }

                ch.Equip(obj, (int)WearLocations.RightFinger);
                mud_prog.oprog_wear_trigger(ch, obj);
                return;
            }

            color.send_to_char("You already wear something on both fingers.\r\n", ch);
        }
        private static void ItemWearNeck(ObjectInstance obj, CharacterInstance ch, bool fReplace)
        {
            if (ch.GetEquippedItem(WearLocations.Neck_1) != null
                && ch.GetEquippedItem(WearLocations.Neck_2) != null
                && !remove_obj(ch, (int)WearLocations.Neck_1, fReplace)
                && !remove_obj(ch, (int)WearLocations.Neck_2, fReplace))
                return;

            if (ch.GetEquippedItem(WearLocations.Neck_1) != null)
            {
                if (!mud_prog.oprog_use_trigger(ch, obj, null, null))
                {
                    comm.act(ATTypes.AT_ACTION, "$n wears $p around $s neck.", ch, obj, null, ToTypes.Room);
                    comm.act(ATTypes.AT_ACTION, "You wear $p around your neck.", ch, obj, null, ToTypes.Character);
                }

                ch.Equip(obj, (int)WearLocations.Neck_1);
                mud_prog.oprog_wear_trigger(ch, obj);
                return;
            }

            if (ch.GetEquippedItem(WearLocations.Neck_2) != null)
            {
                if (!mud_prog.oprog_use_trigger(ch, obj, null, null))
                {
                    comm.act(ATTypes.AT_ACTION, "$n wears $p around $s neck.", ch, obj, null, ToTypes.Room);
                    comm.act(ATTypes.AT_ACTION, "You wear $p around your neck.", ch, obj, null, ToTypes.Character);
                }

                ch.Equip(obj, (int)WearLocations.Neck_2);
                mud_prog.oprog_wear_trigger(ch, obj);
                return;
            }

            color.send_to_char("You already wear two neck items.\r\n", ch);
        }
        private static void ItemWearBody(ObjectInstance obj, CharacterInstance ch, bool fReplace)
        {
            if (!ch.CanWearLayer(obj, (int)WearLocations.Body))
            {
                color.send_to_char("It won't fit overtop of what you're already wearing.\r\n", ch);
                return;
            }

            if (!mud_prog.oprog_use_trigger(ch, obj, null, null))
            {
                comm.act(ATTypes.AT_ACTION, "$n fits $p on $s body.", ch, obj, null, ToTypes.Room);
                comm.act(ATTypes.AT_ACTION, "You fit $p on your body.", ch, obj, null, ToTypes.Character);
            }

            ch.Equip(obj, (int)WearLocations.Body);
            mud_prog.oprog_wear_trigger(ch, obj);
        }
        private static void ItemWearHead(ObjectInstance obj, CharacterInstance ch, bool fReplace)
        {
            if (!remove_obj(ch, (int)WearLocations.Head, fReplace))
                return;

            if (!mud_prog.oprog_use_trigger(ch, obj, null, null))
            {
                comm.act(ATTypes.AT_ACTION, "$n dons $p upon $s body.", ch, obj, null, ToTypes.Room);
                comm.act(ATTypes.AT_ACTION, "You don $p upon your head.", ch, obj, null, ToTypes.Character);
            }

            ch.Equip(obj, (int)WearLocations.Head);
            mud_prog.oprog_wear_trigger(ch, obj);
        }
        private static void ItemWearEyes(ObjectInstance obj, CharacterInstance ch, bool fReplace)
        {
            if (!remove_obj(ch, (int)WearLocations.Eyes, fReplace))
                return;

            if (!mud_prog.oprog_use_trigger(ch, obj, null, null))
            {
                comm.act(ATTypes.AT_ACTION, "$n places $p on $s eyes.", ch, obj, null, ToTypes.Room);
                comm.act(ATTypes.AT_ACTION, "You place $p on your eyes.", ch, obj, null, ToTypes.Character);
            }

            ch.Equip(obj, (int)WearLocations.Eyes);
            mud_prog.oprog_wear_trigger(ch, obj);
        }
        private static void ItemWearFace(ObjectInstance obj, CharacterInstance ch, bool fReplace)
        {
            if (!remove_obj(ch, (int)WearLocations.Face, fReplace))
                return;

            if (!mud_prog.oprog_use_trigger(ch, obj, null, null))
            {
                comm.act(ATTypes.AT_ACTION, "$n places $p on $s face.", ch, obj, null, ToTypes.Room);
                comm.act(ATTypes.AT_ACTION, "You place $p on your face.", ch, obj, null, ToTypes.Character);
            }

            ch.Equip(obj, (int)WearLocations.Face);
            mud_prog.oprog_wear_trigger(ch, obj);
        }
        private static void ItemWearEars(ObjectInstance obj, CharacterInstance ch, bool fReplace)
        {
            if (!remove_obj(ch, (int)WearLocations.Ears, fReplace))
                return;

            if (!mud_prog.oprog_use_trigger(ch, obj, null, null))
            {
                comm.act(ATTypes.AT_ACTION, "$n wears $p on $s ears.", ch, obj, null, ToTypes.Room);
                comm.act(ATTypes.AT_ACTION, "You wear $p on your ears.", ch, obj, null, ToTypes.Character);
            }

            ch.Equip(obj, (int)WearLocations.Ears);
            mud_prog.oprog_wear_trigger(ch, obj);
        }
        private static void ItemWearLegs(ObjectInstance obj, CharacterInstance ch, bool fReplace)
        {
            if (!ch.CanWearLayer(obj, (int)WearLocations.Legs))
            {
                color.send_to_char("It won't fit overtop of what you're already wearing.\r\n", ch);
                return;
            }

            if (!mud_prog.oprog_use_trigger(ch, obj, null, null))
            {
                comm.act(ATTypes.AT_ACTION, "$n slips into $p.", ch, obj, null, ToTypes.Room);
                comm.act(ATTypes.AT_ACTION, "You slip into $p.", ch, obj, null, ToTypes.Character);
            }

            ch.Equip(obj, (int)WearLocations.Legs);
            mud_prog.oprog_wear_trigger(ch, obj);
        }
        private static void ItemWearFeet(ObjectInstance obj, CharacterInstance ch, bool fReplace)
        {
            if (!ch.CanWearLayer(obj, (int)WearLocations.Feet))
            {
                color.send_to_char("It won't fit overtop of what you're already wearing.\r\n", ch);
                return;
            }

            if (!mud_prog.oprog_use_trigger(ch, obj, null, null))
            {
                comm.act(ATTypes.AT_ACTION, "$n wears $p on $s feet.", ch, obj, null, ToTypes.Room);
                comm.act(ATTypes.AT_ACTION, "You wear $p on your feet.", ch, obj, null, ToTypes.Character);
            }

            ch.Equip(obj, (int)WearLocations.Feet);
            mud_prog.oprog_wear_trigger(ch, obj);
        }
        private static void ItemWearHands(ObjectInstance obj, CharacterInstance ch, bool fReplace)
        {
            if (!ch.CanWearLayer(obj, (int)WearLocations.Hands))
            {
                color.send_to_char("It won't fit overtop of what you're already wearing.\r\n", ch);
                return;
            }

            if (!mud_prog.oprog_use_trigger(ch, obj, null, null))
            {
                comm.act(ATTypes.AT_ACTION, "$n wears $p on $s hands.", ch, obj, null, ToTypes.Room);
                comm.act(ATTypes.AT_ACTION, "You wear $p on your hands.", ch, obj, null, ToTypes.Character);
            }

            ch.Equip(obj, (int)WearLocations.Hands);
            mud_prog.oprog_wear_trigger(ch, obj);
        }
        private static void ItemWearArms(ObjectInstance obj, CharacterInstance ch, bool fReplace)
        {
            if (!ch.CanWearLayer(obj, (int)WearLocations.Arms))
            {
                color.send_to_char("It won't fit overtop of what you're already wearing.\r\n", ch);
                return;
            }

            if (!mud_prog.oprog_use_trigger(ch, obj, null, null))
            {
                comm.act(ATTypes.AT_ACTION, "$n wears $p on $s arms.", ch, obj, null, ToTypes.Room);
                comm.act(ATTypes.AT_ACTION, "You wear $p on your arms.", ch, obj, null, ToTypes.Character);
            }

            ch.Equip(obj, (int)WearLocations.Arms);
            mud_prog.oprog_wear_trigger(ch, obj);
        }
        private static void ItemWearAbout(ObjectInstance obj, CharacterInstance ch, bool fReplace)
        {
            if (!ch.CanWearLayer(obj, (int)WearLocations.About))
            {
                color.send_to_char("It won't fit overtop of what you're already wearing.\r\n", ch);
                return;
            }

            if (!mud_prog.oprog_use_trigger(ch, obj, null, null))
            {
                comm.act(ATTypes.AT_ACTION, "$n wears $p about $s arms.", ch, obj, null, ToTypes.Room);
                comm.act(ATTypes.AT_ACTION, "You wear $p about your arms.", ch, obj, null, ToTypes.Character);
            }

            ch.Equip(obj, (int)WearLocations.About);
            mud_prog.oprog_wear_trigger(ch, obj);
        }
        private static void ItemWearBack(ObjectInstance obj, CharacterInstance ch, bool fReplace)
        {
            if (!remove_obj(ch, (int)WearLocations.Back, fReplace))
                return;

            if (!mud_prog.oprog_use_trigger(ch, obj, null, null))
            {
                comm.act(ATTypes.AT_ACTION, "$n slings $p on $s back.", ch, obj, null, ToTypes.Room);
                comm.act(ATTypes.AT_ACTION, "You sling $p on your back.", ch, obj, null, ToTypes.Character);
            }

            ch.Equip(obj, (int)WearLocations.Back);
            mud_prog.oprog_wear_trigger(ch, obj);
        }
        private static void ItemWearWaist(ObjectInstance obj, CharacterInstance ch, bool fReplace)
        {
            if (!ch.CanWearLayer(obj, (int)WearLocations.Waist))
            {
                color.send_to_char("It won't fit overtop of what you're already wearing.\r\n", ch);
                return;
            }

            if (!mud_prog.oprog_use_trigger(ch, obj, null, null))
            {
                comm.act(ATTypes.AT_ACTION, "$n wears $p about $s waist.", ch, obj, null, ToTypes.Room);
                comm.act(ATTypes.AT_ACTION, "You wear $p about your waist.", ch, obj, null, ToTypes.Character);
            }

            ch.Equip(obj, (int)WearLocations.Waist);
            mud_prog.oprog_wear_trigger(ch, obj);
        }
        private static void ItemWearWrist(ObjectInstance obj, CharacterInstance ch, bool fReplace)
        {
            if (ch.GetEquippedItem(WearLocations.LeftWrist) != null
                && ch.GetEquippedItem(WearLocations.RightWrist) != null
                && !remove_obj(ch, (int)WearLocations.LeftWrist, fReplace)
                && !remove_obj(ch, (int)WearLocations.RightWrist, fReplace))
                return;

            if (ch.GetEquippedItem(WearLocations.LeftWrist) != null)
            {
                if (!mud_prog.oprog_use_trigger(ch, obj, null, null))
                {
                    comm.act(ATTypes.AT_ACTION, "$n fit $p around $s left wrist.", ch, obj, null, ToTypes.Room);
                    comm.act(ATTypes.AT_ACTION, "You fit $p around your left wrist.", ch, obj, null, ToTypes.Character);
                }

                ch.Equip(obj, (int)WearLocations.LeftWrist);
                mud_prog.oprog_wear_trigger(ch, obj);
                return;
            }

            if (ch.GetEquippedItem(WearLocations.RightWrist) != null)
            {
                if (!mud_prog.oprog_use_trigger(ch, obj, null, null))
                {
                    comm.act(ATTypes.AT_ACTION, "$n fit $p around $s right wrist.", ch, obj, null, ToTypes.Room);
                    comm.act(ATTypes.AT_ACTION, "You fit $p around your right wrist.", ch, obj, null, ToTypes.Character);
                }

                ch.Equip(obj, (int)WearLocations.RightWrist);
                mud_prog.oprog_wear_trigger(ch, obj);
                return;
            }

            color.send_to_char("You already wear two wrist items.\r\n", ch);
        }
        private static void ItemWearAnkle(ObjectInstance obj, CharacterInstance ch, bool fReplace)
        {
            if (ch.GetEquippedItem(WearLocations.LeftAnkle) != null
                && ch.GetEquippedItem(WearLocations.RightAnkle) != null
                && !remove_obj(ch, (int)WearLocations.LeftAnkle, fReplace)
                && !remove_obj(ch, (int)WearLocations.RightAnkle, fReplace))
                return;

            if (ch.GetEquippedItem(WearLocations.LeftAnkle) != null)
            {
                if (!mud_prog.oprog_use_trigger(ch, obj, null, null))
                {
                    comm.act(ATTypes.AT_ACTION, "$n fit $p around $s left ankle.", ch, obj, null, ToTypes.Room);
                    comm.act(ATTypes.AT_ACTION, "You fit $p around your left ankle.", ch, obj, null, ToTypes.Character);
                }

                ch.Equip(obj, (int)WearLocations.LeftAnkle);
                mud_prog.oprog_wear_trigger(ch, obj);
                return;
            }

            if (ch.GetEquippedItem(WearLocations.RightAnkle) != null)
            {
                if (!mud_prog.oprog_use_trigger(ch, obj, null, null))
                {
                    comm.act(ATTypes.AT_ACTION, "$n fit $p around $s right ankle.", ch, obj, null, ToTypes.Room);
                    comm.act(ATTypes.AT_ACTION, "You fit $p around your right ankle.", ch, obj, null, ToTypes.Character);
                }

                ch.Equip(obj, (int)WearLocations.RightAnkle);
                mud_prog.oprog_wear_trigger(ch, obj);
                return;
            }

            color.send_to_char("You already wear two ankle items.\r\n", ch);
        }
        private static void ItemWearShield(ObjectInstance obj, CharacterInstance ch, bool fReplace)
        {
            if (ch.GetEquippedItem(WearLocations.DualWield) != null
                || (ch.GetEquippedItem(WearLocations.Wield) != null
                    && ch.GetEquippedItem(WearLocations.WieldMissile) != null)
                || (ch.GetEquippedItem(WearLocations.Wield) != null
                    && ch.GetEquippedItem(WearLocations.Hold) != null))
            {
                color.send_to_char(
                    ch.GetEquippedItem(WearLocations.Hold) != null
                        ? "You can't use a shield while using a weapon and holding something!"
                        : "You can't use a shield AND two weapons!\r\n", ch);
                return;
            }
            if (!remove_obj(ch, (int)WearLocations.Shield, fReplace))
                return;
            if (!mud_prog.oprog_use_trigger(ch, obj, null, null))
            {
                comm.act(ATTypes.AT_ACTION, "$n uses $p as a shield.", ch, obj, null, ToTypes.Room);
                comm.act(ATTypes.AT_ACTION, "You use $p as a shield.", ch, obj, null, ToTypes.Character);
            }

            ch.Equip(obj, (int)WearLocations.Shield);
            mud_prog.oprog_wear_trigger(ch, obj);
        }
        private static void ItemWearWield(ObjectInstance obj, CharacterInstance ch, bool fReplace, int bit)
        {
            if (!ch.CouldDualWield())
            {
                if (!remove_obj(ch, (int)WearLocations.WieldMissile, fReplace)
                    || !remove_obj(ch, (int)WearLocations.Wield, fReplace))
                    return;
            }
            else
            {
                ObjectInstance tobj = ch.GetEquippedItem(WearLocations.Wield);
                ObjectInstance mw = ch.GetEquippedItem(WearLocations.WieldMissile);
                ObjectInstance dw = ch.GetEquippedItem(WearLocations.DualWield);
                ObjectInstance hd = ch.GetEquippedItem(WearLocations.Hold);
                ObjectInstance sd = ch.GetEquippedItem(WearLocations.Shield);

                if (hd != null && sd != null)
                {
                    color.send_to_char("You are already holding something and wearing a shield.\r\n", ch);
                    return;
                }

                if (tobj != null)
                {
                    if (!ch.CanDualWield())
                        return;

                    if ((obj.GetObjectWeight() + tobj.GetObjectWeight()) >
                        LookupConstants.str_app[ch.GetCurrentStrength()].Wield)
                    {
                        color.send_to_char("It is too heavy for you to wield.\r\n", ch);
                        return;
                    }

                    if (hd != null || sd != null)
                    {
                        color.send_to_char("You're already wielding a weapon AND holding something.\r\n", ch);
                        return;
                    }

                    if (!mud_prog.oprog_use_trigger(ch, obj, null, null))
                    {
                        comm.act(ATTypes.AT_ACTION, "$n dual-wields $p.", ch, obj, null, ToTypes.Room);
                        comm.act(ATTypes.AT_ACTION, "You dual-wield $p.", ch, obj, null, ToTypes.Character);
                    }

                    if (1 << bit == (int)ItemWearFlags.MissileWield)
                        ch.Equip(obj, (int)WearLocations.WieldMissile);
                    else
                        ch.Equip(obj, (int)WearLocations.DualWield);
                    mud_prog.oprog_wear_trigger(ch, obj);
                    return;
                }

                if (mw != null)
                {
                    if (!ch.CanDualWield())
                        return;

                    if (obj.ItemType == ItemTypes.MissileWeapon)
                    {
                        color.send_to_char("You're already wielding a missile weapon.\r\n", ch);
                        return;
                    }

                    if ((obj.GetObjectWeight() + mw.GetObjectWeight()) >
                        LookupConstants.str_app[ch.GetCurrentStrength()].Wield)
                    {
                        color.send_to_char("It is too heavy for you to wield.\r\n", ch);
                        return;
                    }

                    if (dw != null)
                    {
                        color.send_to_char("You're already wielding two weapons.\r\n", ch);
                        return;
                    }

                    if (hd != null || sd != null)
                    {
                        color.send_to_char("You're already wielding a weapon AND holding something.\r\n", ch);
                        return;
                    }

                    if (!mud_prog.oprog_use_trigger(ch, obj, null, null))
                    {
                        comm.act(ATTypes.AT_ACTION, "$n wields $p.", ch, obj, null, ToTypes.Room);
                        comm.act(ATTypes.AT_ACTION, "You wield $p.", ch, obj, null, ToTypes.Character);
                    }

                    ch.Equip(obj, (int)WearLocations.Wield);
                    mud_prog.oprog_wear_trigger(ch, obj);
                    return;
                }
            }

            if (obj.GetObjectWeight() > LookupConstants.str_app[ch.GetCurrentStrength()].Wield)
            {
                color.send_to_char("It is too heavy for you to wield.\r\n", ch);
                return;
            }

            if (!mud_prog.oprog_use_trigger(ch, obj, null, null))
            {
                comm.act(ATTypes.AT_ACTION, "$n wields $p.", ch, obj, null, ToTypes.Room);
                comm.act(ATTypes.AT_ACTION, "You wield $p.", ch, obj, null, ToTypes.Character);
            }

            if (1 << bit == (int)ItemWearFlags.MissileWield)
                ch.Equip(obj, (int)WearLocations.WieldMissile);
            else
                ch.Equip(obj, (int)WearLocations.Wield);
            mud_prog.oprog_wear_trigger(ch, obj);
        }
        private static void ItemWearHold(ObjectInstance obj, CharacterInstance ch, bool fReplace)
        {
            if (ch.GetEquippedItem(WearLocations.DualWield) != null
                || (ch.GetEquippedItem(WearLocations.Wield) != null
                    && (ch.GetEquippedItem(WearLocations.WieldMissile) != null
                        || ch.GetEquippedItem(WearLocations.Shield) != null)))
            {
                color.send_to_char(
                    ch.GetEquippedItem(WearLocations.Shield) != null
                        ? "You cannot hold something while using a weapon and a shield!\r\n"
                        : "You cannot hold something AND two weapons!\r\n", ch);
                return;
            }

            if (!remove_obj(ch, (int)WearLocations.Hold, fReplace))
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
                || !mud_prog.oprog_use_trigger(ch, obj, null, null))
            {
                comm.act(ATTypes.AT_ACTION, "$n holds $p in $s hands.", ch, obj, null, ToTypes.Room);
                comm.act(ATTypes.AT_ACTION, "You hold $p in your hands.", ch, obj, null, ToTypes.Character);
            }

            ch.Equip(obj, (int)WearLocations.Hold);
            mud_prog.oprog_wear_trigger(ch, obj);
        }

        private static int fall_count;
        private static bool is_falling;

        /// stops infinite loops from the call to obj_to_room
        public static void obj_fall(ObjectInstance obj, bool through)
        {
            if (obj.InRoom == null || is_falling)
                return;

            if (fall_count > 30)
            {
                LogManager.Instance.Bug("Object falling in loop more than 30 times");
                handler.extract_obj(obj);
                fall_count = 0;
                return;
            }

            if (obj.InRoom.Flags.IsSet((int)RoomFlags.NoFloor)
                //&& Macros.CAN_GO(obj, (int) DirectionTypes.Down)
                && !obj.ExtraFlags.IsSet(ItemExtraFlags.Magical))
            {
                ExitData exit = obj.InRoom.GetExit(DirectionTypes.Down);
                RoomTemplate to_room = exit.GetDestination();

                if (through)
                    fall_count++;
                else
                    fall_count = 0;

                if (obj.InRoom == to_room)
                {
                    LogManager.Instance.Bug("Object falling into same room {0}", to_room.Vnum);
                    handler.extract_obj(obj);
                    return;
                }

                if (obj.InRoom.Persons.Any())
                {
                    comm.act(ATTypes.AT_PLAIN, "$p falls far below...", obj.InRoom.Persons.First(), obj, null, ToTypes.Room);
                    comm.act(ATTypes.AT_PLAIN, "$p falls far below...", obj.InRoom.Persons.First(), obj, null, ToTypes.Character);
                }

                obj.InRoom.FromRoom(obj);
                is_falling = true;
                to_room.ToRoom(obj);
                is_falling = false;

                if (obj.InRoom.Persons.Any())
                {
                    comm.act(ATTypes.AT_PLAIN, "$p falls from above...", obj.InRoom.Persons.First(), obj, null, ToTypes.Room);
                    comm.act(ATTypes.AT_PLAIN, "$p falls from above...", obj.InRoom.Persons.First(), obj, null, ToTypes.Character);
                }

                if (!obj.InRoom.Flags.IsSet((int)RoomFlags.NoFloor) && through)
                {
                    int dam = fall_count * obj.Weight / 2;

                    // Damage players in room
                    if (obj.InRoom.Persons.Any() && SmaugCS.Common.SmaugRandom.Percent() > 15)
                    {
                        foreach (CharacterInstance rch in obj.InRoom.Persons)
                        {
                            comm.act(ATTypes.AT_WHITE, "$p falls on $n!", rch, obj, null, ToTypes.Room);
                            comm.act(ATTypes.AT_WHITE, "$p falls on you!", rch, obj, null, ToTypes.Character);

                            if (rch.IsNpc() && rch.Act.IsSet((int)ActFlags.Hardhat))
                                comm.act(ATTypes.AT_WHITE, "$p bounces harmlessly off your head!", rch, obj, null,
                                         ToTypes.Character);
                            else
                                fight.damage(rch, rch, dam * rch.Level, -1);
                        }
                    }

                    // Damage the falling object
                    switch (obj.ItemType)
                    {
                        case ItemTypes.Weapon:
                        case ItemTypes.Armor:
                            if ((obj.Value[0] - dam) <= 0)
                            {
                                if (obj.InRoom.Persons.Any())
                                {
                                    comm.act(ATTypes.AT_PLAIN, "$p is destroyed in the fall!",
                                             obj.InRoom.Persons.First(), obj, null, ToTypes.Room);
                                    comm.act(ATTypes.AT_PLAIN, "$p is destroyed in the fall!",
                                             obj.InRoom.Persons.First(), obj, null, ToTypes.Character);
                                }
                                ObjectFactory.CreateScraps(obj);
                            }
                            else
                                obj.Value[0] -= dam;
                            break;
                        default:
                            if ((dam * 15) > obj.GetResistance())
                            {
                                if (obj.InRoom.Persons.Any())
                                {
                                    comm.act(ATTypes.AT_PLAIN, "$p is destroyed in the fall!",
                                             obj.InRoom.Persons.First(), obj, null, ToTypes.Room);
                                    comm.act(ATTypes.AT_PLAIN, "$p is destroyed in the fall!",
                                             obj.InRoom.Persons.First(), obj, null, ToTypes.Character);
                                }
                                ObjectFactory.CreateScraps(obj);
                            }
                            break;
                    }
                }
            }

            obj_fall(obj, true);
        }

        public static ObjectInstance recursive_note_find(ObjectInstance obj, string argument)
        {
            if (obj == null) return null;

            bool match = true;
            string arg = string.Empty;

            switch (obj.ItemType)
            {
                case ItemTypes.Paper:
                    string subject = db.get_extra_descr("_subject_", obj.ExtraDescriptions);
                    if (string.IsNullOrEmpty(subject))
                        break;

                    subject = subject.ToLower();

                    while (match)
                    {
                        Tuple<string, string> args = argument.FirstArgument();
                        string argcopy = args.Item2;
                        arg = args.Item1;
                        if (string.IsNullOrEmpty(argcopy))
                            break;

                        if (!subject.Contains(arg))
                            match = false;
                    }

                    if (match)
                        return obj;
                    break;
                case ItemTypes.Container:
                case ItemTypes.NpcCorpse:
                case ItemTypes.PlayerCorpse:
                    if (obj.Contents.Any())
                    {
                        ObjectInstance returnedObj = recursive_note_find(obj.Contents.First(), argument);
                        if (returnedObj != null)
                            return returnedObj;
                    }
                    break;
            }

            return recursive_note_find(obj.Contents.First(), argument);
        }

        public static string get_chance_verb(ObjectInstance obj)
        {
            return !string.IsNullOrWhiteSpace(obj.Action) ? obj.Action : "roll$q";
        }

        public static string get_ed_number(ObjectInstance obj, int number)
        {
            int count = 1;
            foreach (ExtraDescriptionData ed in obj.ExtraDescriptions)
            {
                if (count == number)
                    return ed.Description;
                count++;
            }

            return string.Empty;
        }

        internal static void save_clan_storeroom(CharacterInstance ch, ClanData clan)
        {
            throw new NotImplementedException();
        }
    }
}
