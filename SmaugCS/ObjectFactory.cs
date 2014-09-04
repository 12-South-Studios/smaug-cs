using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common;
using Realm.Library.Patterns.Repository;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Extensions;
using SmaugCS.Interfaces;
using SmaugCS.Managers;

namespace SmaugCS
{
    public static class ObjectFactory
    {
        public static void CreateFire(RoomTemplate inRoom, int duration, IDatabaseManager dbManager = null)
        {
            ObjectInstance fire =
                (dbManager ?? DatabaseManager.Instance).OBJECTS.Create(
                    (dbManager ?? DatabaseManager.Instance).OBJECT_INDEXES.CastAs<Repository<long, ObjectTemplate>>()
                                   .Get(VnumConstants.OBJ_VNUM_FIRE), 0);
            fire.Timer = (short)SmaugRandom.Fuzzy(duration);
            inRoom.ToRoom(fire);
        }

        public static ObjectInstance CreateTrap(IEnumerable<int> values, IDatabaseManager dbManager = null)
        {
            ObjectInstance trap = (dbManager ?? DatabaseManager.Instance).OBJECTS.Create(
                (dbManager ?? DatabaseManager.Instance).OBJECT_INDEXES.CastAs<Repository<long, ObjectTemplate>>()
                                                       .Get(VnumConstants.OBJ_VNUM_TRAP), 0);
            trap.Timer = 0;
            trap.Value = values.ToArray();
            return trap;
        }

        public static void CreateScraps(ObjectInstance obj, IDatabaseManager dbManager = null)
        {
            obj.Split();
            ObjectInstance scraps =
                (dbManager ?? DatabaseManager.Instance).OBJECTS.Create(
                    (dbManager ?? DatabaseManager.Instance).OBJECT_INDEXES.CastAs<Repository<long, ObjectTemplate>>()
                                                           .Get(VnumConstants.OBJ_VNUM_SCRAPS), 0);

            scraps.Timer = SmaugRandom.Between(5, 15);

            scraps.ShortDescription = (obj.ObjectIndex.Vnum == VnumConstants.OBJ_VNUM_SCRAPS)
                                          ? "some debris"
                                          : obj.ShortDescription;
            scraps.Description = (obj.ObjectIndex.Vnum == VnumConstants.OBJ_VNUM_SCRAPS)
                                     ? "Bits of debris lie on the ground here."
                                     : obj.Description;

            CharacterInstance ch = null;
            if (obj.CarriedBy != null)
            {
                comm.act(ATTypes.AT_OBJECT, "$p falls to the ground in scraps!", obj.CarriedBy, obj, null, ToTypes.Character);
                if (obj == obj.CarriedBy.GetEquippedItem(WearLocations.Wield)
                    && obj.CarriedBy.GetEquippedItem(WearLocations.DualWield) != null)
                {
                    ObjectInstance tmpObj = obj.CarriedBy.GetEquippedItem(WearLocations.DualWield);
                    tmpObj.WearLocation = WearLocations.Wield;
                }

                obj.CarriedBy.CurrentRoom.ToRoom(scraps);
            }
            else if (obj.InRoom != null)
            {
                if ((ch = obj.InRoom.Persons.First()) != null)
                {
                    comm.act(ATTypes.AT_OBJECT, "$p is reduced to little more than scraps.", ch, obj, null, ToTypes.Room);
                    comm.act(ATTypes.AT_OBJECT, "$p is reduced to little more than scraps.", ch, obj, null, ToTypes.Character);
                }

                obj.InRoom.ToRoom(scraps);
            }

            if (obj.ItemType == ItemTypes.Container
                || obj.ItemType == ItemTypes.KeyRing
                || obj.ItemType == ItemTypes.Quiver
                || obj.ItemType == ItemTypes.PlayerCorpse)
            {
                if (ch != null && ch.CurrentRoom != null)
                {
                    comm.act(ATTypes.AT_OBJECT, "The contents of $p fall to the ground.", ch, obj, null, ToTypes.Room);
                    comm.act(ATTypes.AT_OBJECT, "The contents of $p fall to the ground.", ch, obj, null, ToTypes.Character);
                }

                if (obj.CarriedBy != null)
                    obj.Empty(null, obj.CarriedBy.CurrentRoom);
                else if (obj.InRoom != null)
                    obj.Empty(null, obj.InRoom);
                else if (obj.InObject != null)
                    obj.Empty(obj.InObject);
            }

            obj.Extract();
        }

        public static ObjectInstance CreateCorpse(CharacterInstance ch, CharacterInstance killer, IDatabaseManager dbManager = null)
        {
            string name;
            ObjectInstance corpse;

            if (ch.IsNpc())
            {
                name = ch.ShortDescription;
                corpse =
                    (dbManager ?? DatabaseManager.Instance).OBJECTS.Create(
                        (dbManager ?? DatabaseManager.Instance).OBJECT_INDEXES.CastAs<Repository<long, ObjectTemplate>>()
                                                               .Get(VnumConstants.OBJ_VNUM_CORPSE_NPC), 0);

                corpse.Timer = 6;
                if (ch.CurrentCoin > 0)
                {
                    if (ch.CurrentRoom != null)
                    {
                        ch.CurrentRoom.Area.gold_looted += ch.CurrentCoin;
                        GameManager.Instance.SystemData.global_looted += ch.CurrentCoin / 100;
                    }

                    ObjectInstance money = CreateMoney(ch.CurrentCoin);
                    money.ToObject(corpse);
                    ch.CurrentCoin = 0;
                }

                corpse.Cost = -1 * (int)ch.MobIndex.Vnum;
                corpse.Value[2] = corpse.Timer;
            }
            else
            {
                name = ch.Name;
                corpse =
                    (dbManager ?? DatabaseManager.Instance).OBJECTS.Create(
                        (dbManager ?? DatabaseManager.Instance).OBJECT_INDEXES.CastAs<Repository<long, ObjectTemplate>>()
                                                               .Get(VnumConstants.OBJ_VNUM_CORPSE_PC), 0);

                corpse.Timer = ch.IsInArena() ? 0 : 40;
                corpse.Value[2] = corpse.Timer / 8;
                corpse.Value[4] = ch.Level;
                if (ch.CanPKill() && GameManager.Instance.SystemData.PlayerKillLoot > 0)
                    corpse.ExtraFlags.SetBit((int)ItemExtraFlags.ClanCorpse);

                corpse.Value[3] = (!ch.IsNpc() && !killer.IsNpc()) ? 1 : 0;
            }

            if (ch.CanPKill() && killer.CanPKill() && ch != killer)
                corpse.Action = killer.Name;

            corpse.Name = string.Format("corpse {0}", name);
            corpse.ShortDescription = name;
            corpse.Description = name;

            foreach (ObjectInstance obj in ch.Carrying)
            {
                obj.FromCharacter();
                if (obj.ExtraFlags.IsSet(ItemExtraFlags.Inventory)
                    || obj.ExtraFlags.IsSet(ItemExtraFlags.DeathRot))
                    obj.Extract();
                else
                    obj.ToObject(corpse);
            }

            return ch.CurrentRoom.ToRoom(corpse);
        }

        public static void CreateBlood(CharacterInstance ch, IDatabaseManager dbManager = null)
        {
            ObjectInstance obj = (dbManager ?? DatabaseManager.Instance).OBJECTS.Create(
                (dbManager ?? DatabaseManager.Instance).OBJECT_INDEXES.CastAs<Repository<long, ObjectTemplate>>()
                                                       .Get(VnumConstants.OBJ_VNUM_BLOOD), 0);

            obj.Timer = (short)SmaugRandom.Between(2, 4);
            obj.Value[1] = SmaugRandom.Between(3, 5.GetLowestOfTwoNumbers(ch.Level));
            ch.CurrentRoom.ToRoom(obj);
        }

        public static void CreateBloodstain(CharacterInstance ch, IDatabaseManager dbManager = null)
        {
            ObjectInstance obj = (dbManager ?? DatabaseManager.Instance).OBJECTS.Create(
                (dbManager ?? DatabaseManager.Instance).OBJECT_INDEXES.CastAs<Repository<long, ObjectTemplate>>()
                                                       .Get(VnumConstants.OBJ_VNUM_BLOODSTAIN), 0);

            obj.Timer = (short)SmaugRandom.Between(1, 2);
            ch.CurrentRoom.ToRoom(obj);
        }

        public static ObjectInstance CreateMoney(int amount, IDatabaseManager dbManager = null)
        {
            int coinAmt = amount <= 0 ? 1 : amount;

            ObjectInstance obj = (dbManager ?? DatabaseManager.Instance).OBJECTS.Create(
                (dbManager ?? DatabaseManager.Instance).OBJECT_INDEXES.CastAs<Repository<long, ObjectTemplate>>()
                                                       .Get(coinAmt == 1
                                                                ? VnumConstants.OBJ_VNUM_MONEY_ONE
                                                                : VnumConstants.OBJ_VNUM_MONEY_SOME), 0);
           
            if (coinAmt > 1)
            {
                obj.ShortDescription = string.Format(obj.ShortDescription, coinAmt);
                obj.Value[0] = coinAmt;
            }

            return obj;
        }
    }
}
