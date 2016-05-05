using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common;
using Realm.Library.Patterns.Repository;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Objects;
using SmaugCS.Managers;
using SmaugCS.Repository;

namespace SmaugCS
{
    public static class ObjectFactory
    {
        public static void CreateFire(RoomTemplate inRoom, int duration, IRepositoryManager dbManager = null)
        {
            var databaseMgr = dbManager ?? RepositoryManager.Instance;
            var fire = databaseMgr.OBJECTS.Create(databaseMgr.OBJECTTEMPLATES.CastAs<Repository<long, ObjectTemplate>>()
                                   .Get(VnumConstants.OBJ_VNUM_FIRE), 0);
            fire.Timer = SmaugRandom.Fuzzy(duration);
            inRoom.AddTo(fire);
        }

        public static ObjectInstance CreateTrap(IEnumerable<int> values, IRepositoryManager dbManager = null)
        {
            var databaseMgr = dbManager ?? RepositoryManager.Instance;
            var trap = databaseMgr.OBJECTS.Create(databaseMgr.OBJECTTEMPLATES.CastAs<Repository<long, ObjectTemplate>>()
                                                       .Get(VnumConstants.OBJ_VNUM_TRAP), 0);
            trap.Timer = 0;
            trap.Value = values.ToArray();
            return trap;
        }

        public static void CreateScraps(ObjectInstance obj, IRepositoryManager dbManager = null)
        {
            obj.Split();
            var databaseMgr = dbManager ?? RepositoryManager.Instance;
            var scraps = databaseMgr.OBJECTS.Create(databaseMgr.OBJECTTEMPLATES.CastAs<Repository<long, ObjectTemplate>>()
                                                           .Get(VnumConstants.OBJ_VNUM_SCRAPS), 0);

            scraps.Timer = SmaugRandom.Between(5, 15);

            scraps.ShortDescription = obj.ObjectIndex.ID == VnumConstants.OBJ_VNUM_SCRAPS
                                          ? "some debris"
                                          : obj.ShortDescription;
            scraps.Description = obj.ObjectIndex.ID == VnumConstants.OBJ_VNUM_SCRAPS
                                     ? "Bits of debris lie on the ground here."
                                     : obj.Description;

            CharacterInstance ch = null;
            if (obj.CarriedBy != null)
            {
                comm.act(ATTypes.AT_OBJECT, "$p falls to the ground in scraps!", obj.CarriedBy, obj, null, ToTypes.Character);
                if (obj == obj.CarriedBy.GetEquippedItem(WearLocations.Wield)
                    && obj.CarriedBy.GetEquippedItem(WearLocations.DualWield) != null)
                {
                    var tmpObj = obj.CarriedBy.GetEquippedItem(WearLocations.DualWield);
                    tmpObj.WearLocation = WearLocations.Wield;
                }

                obj.CarriedBy.CurrentRoom.AddTo(scraps);
            }
            else if (obj.InRoom != null)
            {
                if ((ch = obj.InRoom.Persons.First()) != null)
                {
                    comm.act(ATTypes.AT_OBJECT, "$p is reduced to little more than scraps.", ch, obj, null, ToTypes.Room);
                    comm.act(ATTypes.AT_OBJECT, "$p is reduced to little more than scraps.", ch, obj, null, ToTypes.Character);
                }

                obj.InRoom.AddTo(scraps);
            }

            if (obj.ItemType == ItemTypes.Container
                || obj.ItemType == ItemTypes.KeyRing
                || obj.ItemType == ItemTypes.Quiver
                || obj.ItemType == ItemTypes.PlayerCorpse)
            {
                if (ch?.CurrentRoom != null)
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

        public static ObjectInstance CreateCorpse(CharacterInstance ch, CharacterInstance killer, IRepositoryManager dbManager = null)
        {
            string name;
            ObjectInstance corpse;
            var databaseMgr = dbManager ?? RepositoryManager.Instance;

            if (ch.IsNpc())
            {
                name = ch.ShortDescription;
                corpse = databaseMgr.OBJECTS.Create(databaseMgr.OBJECTTEMPLATES.CastAs<Repository<long, ObjectTemplate>>()
                        .Get(VnumConstants.OBJ_VNUM_CORPSE_NPC), 0);

                corpse.Timer = 6;
                if (ch.CurrentCoin > 0)
                {
                    if (ch.CurrentRoom != null)
                    {
                        ch.CurrentRoom.Area.gold_looted += ch.CurrentCoin;
                        GameManager.Instance.SystemData.global_looted += ch.CurrentCoin / 100;
                    }

                    var money = CreateMoney(ch.CurrentCoin);
                    money.AddTo(corpse);
                    ch.CurrentCoin = 0;
                }

                corpse.Cost = -1 * (int)((MobileInstance)ch).MobIndex.ID;
                corpse.Value.ToList()[2] = corpse.Timer;
            }
            else
            {
                name = ch.Name;
                corpse = databaseMgr.OBJECTS.Create(databaseMgr.OBJECTTEMPLATES.Get(VnumConstants.OBJ_VNUM_CORPSE_PC), 0);
                corpse.Timer = ch.IsInArena() ? 0 : 40;
                corpse.Value.ToList()[2] = corpse.Timer / 8;
                corpse.Value.ToList()[4] = ch.Level;
                if (ch.CanPKill() && GameManager.Instance.SystemData.PlayerKillLoot > 0)
                    corpse.ExtraFlags.SetBit(ItemExtraFlags.ClanCorpse);

                corpse.Value.ToList()[3] = !ch.IsNpc() && !killer.IsNpc() ? 1 : 0;
            }

            if (ch.CanPKill() && killer.CanPKill() && ch != killer)
                corpse.Action = killer.Name;

            corpse.Name = $"corpse {name}";
            corpse.ShortDescription = name;
            corpse.Description = name;

            foreach (var obj in ch.Carrying)
            {
                obj.RemoveFrom();
                if (obj.ExtraFlags.IsSet(ItemExtraFlags.Inventory)
                    || obj.ExtraFlags.IsSet(ItemExtraFlags.DeathRot))
                    obj.Extract();
                else
                    obj.AddTo(corpse);
            }

            return ch.CurrentRoom.AddTo(corpse);
        }

        public static void CreateBlood(CharacterInstance ch, IRepositoryManager dbManager = null)
        {
            var databaseMgr = dbManager ?? RepositoryManager.Instance;
            var obj = databaseMgr.OBJECTS.Create(databaseMgr.OBJECTTEMPLATES.CastAs<Repository<long, 
                ObjectTemplate>>().Get(VnumConstants.OBJ_VNUM_BLOOD), 0);

            obj.Timer = SmaugRandom.Between(2, 4);
            obj.Value.ToList()[1] = SmaugRandom.Between(3, 5.GetLowestOfTwoNumbers(ch.Level));
            ch.CurrentRoom.AddTo(obj);
        }

        public static void CreateBloodstain(CharacterInstance ch, IRepositoryManager dbManager = null)
        {
            var databaseMgr = dbManager ?? RepositoryManager.Instance;
            var obj = databaseMgr.OBJECTS.Create(databaseMgr.OBJECTTEMPLATES.CastAs<Repository<long, ObjectTemplate>>()
                                                       .Get(VnumConstants.OBJ_VNUM_BLOODSTAIN), 0);

            obj.Timer = SmaugRandom.Between(1, 2);
            ch.CurrentRoom.AddTo(obj);
        }

        public static ObjectInstance CreateMoney(int amount, IRepositoryManager dbManager = null)
        {
            var coinAmt = amount <= 0 ? 1 : amount;
            var databaseMgr = dbManager ?? RepositoryManager.Instance;
            var obj = databaseMgr.OBJECTS.Create(databaseMgr.OBJECTTEMPLATES.CastAs<Repository<long, ObjectTemplate>>()
                                                       .Get(coinAmt == 1
                                                                ? VnumConstants.OBJ_VNUM_MONEY_ONE
                                                                : VnumConstants.OBJ_VNUM_MONEY_SOME), 0);

            if (coinAmt <= 1) return obj;

            obj.ShortDescription = string.Format(obj.ShortDescription, coinAmt);
            obj.Value.ToList()[0] = coinAmt;
            return obj;
        }
    }
}
