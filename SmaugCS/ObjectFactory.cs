using System.Linq;
using SmaugCS.Enums;
using SmaugCS.Managers;
using SmaugCS.Objects;
using SmaugCS.Common;

namespace SmaugCS
{
    public static class ObjectFactory
    {
        public static void CreateFire(RoomTemplate in_room, short timer)
        {
            ObjectInstance fire = DatabaseManager.Instance.OBJECTS.Create(DatabaseManager.Instance.OBJECT_INDEXES.Get(Program.OBJ_VNUM_FIRE), 0);
            fire.Timer = (short)SmaugRandom.Fuzzy(timer);
            in_room.ToRoom(fire);
        }

        public static ObjectInstance CreateTrap(int v0, int v1, int v2, int v3)
        {
            ObjectInstance trap = DatabaseManager.Instance.OBJECTS.Create(DatabaseManager.Instance.OBJECT_INDEXES.Get(Program.OBJ_VNUM_TRAP), 0);
            trap.Timer = 0;
            trap.Value[0] = v0;
            trap.Value[1] = v1;
            trap.Value[2] = v2;
            trap.Value[3] = v3;

            return trap;
        }

        public static void CreateScraps(ObjectInstance obj)
        {
            handler.separate_obj(obj);
            ObjectInstance scraps =
                DatabaseManager.Instance.OBJECTS.Create(
                    DatabaseManager.Instance.OBJECT_INDEXES.Get(Program.OBJ_VNUM_SCRAPS), 0);
            scraps.Timer = SmaugRandom.Between(5, 15);

            // Don't make scraps of scraps of...
            if (obj.ObjectIndex.Vnum == Program.OBJ_VNUM_SCRAPS)
            {
                scraps.ShortDescription = "some debris";
                scraps.Description = "Bits of debris lie on the ground here.";
            }
            else
            {
                scraps.ShortDescription = obj.ShortDescription;
                scraps.Description = obj.Description;
            }

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
                    handler.empty_obj(obj, null, obj.CarriedBy.CurrentRoom);
                else if (obj.InRoom != null)
                    handler.empty_obj(obj, null, obj.InRoom);
                else if (obj.InObject != null)
                    handler.empty_obj(obj, obj.InObject, null);
            }

            handler.extract_obj(obj);
        }

        public static ObjectInstance CreateCorpse(CharacterInstance ch, CharacterInstance killer)
        {
            string name;
            ObjectInstance corpse;

            if (ch.IsNpc())
            {
                name = ch.ShortDescription;
                corpse =
                    DatabaseManager.Instance.OBJECTS.Create(
                        DatabaseManager.Instance.OBJECT_INDEXES.Get(Program.OBJ_VNUM_CORPSE_NPC), 0);
                corpse.Timer = 6;
                if (ch.CurrentCoin > 0)
                {
                    if (ch.CurrentRoom != null)
                    {
                        ch.CurrentRoom.Area.gold_looted += ch.CurrentCoin;
                        db.SystemData.global_looted += ch.CurrentCoin/100;
                    }

                    ObjectInstance money = CreateMoney(ch.CurrentCoin);
                    money.ToObject(corpse);
                    ch.CurrentCoin = 0;
                }

                corpse.Cost = -1*ch.MobIndex.Vnum;
                corpse.Value[2] = corpse.Timer;
            }
            else
            {
                name = ch.Name;
                corpse =
                    DatabaseManager.Instance.OBJECTS.Create(
                        DatabaseManager.Instance.OBJECT_INDEXES.Get(Program.OBJ_VNUM_CORPSE_PC), 0);
                corpse.Timer = fight.in_arena(ch) ? 0 : 40;
                corpse.Value[2] = corpse.Timer/8;
                corpse.Value[4] = ch.Level;
                if (ch.CanPKill() && db.SystemData.PlayerKillLoot > 0)
                    corpse.ExtraFlags.SetBit((int) ItemExtraFlags.ClanCorpse);

                // Pkill corpses get save timers in tickets (approx 70 seconds)
                corpse.Value[3] = (!ch.IsNpc() && !killer.IsNpc()) ? 1 : 0;
            }

            if (ch.CanPKill() && killer.CanPKill() && ch != killer)
                corpse.ActionDescription = killer.Name;
            
            // Added corpse name, makes locate easier
            corpse.Name = string.Format("corpse {0}", name);
            corpse.ShortDescription = name;
            corpse.Description = name;

            foreach (ObjectInstance obj in ch.Carrying)
            {
                obj.FromCharacter();
                if (Macros.IS_OBJ_STAT(obj, (int) ItemExtraFlags.Inventory)
                    || Macros.IS_OBJ_STAT(obj, (int) ItemExtraFlags.DeathRot))
                    handler.extract_obj(obj);
                else
                    obj.ToObject(corpse);
            }

            return ch.CurrentRoom.ToRoom(corpse);
        }

        public static void CreateBlood(CharacterInstance ch)
        {
            ObjectInstance obj = DatabaseManager.Instance.OBJECTS.Create(DatabaseManager.Instance.OBJECT_INDEXES.Get(Program.OBJ_VNUM_BLOOD), 0);
            obj.Timer = (short)SmaugRandom.Between(2, 4);
            obj.Value[1] = SmaugRandom.Between(3, Check.Minimum(5, ch.Level));
            ch.CurrentRoom.ToRoom(obj);
        }

        public static void CreateBloodstain(CharacterInstance ch)
        {
            ObjectInstance obj = DatabaseManager.Instance.OBJECTS.Create(DatabaseManager.Instance.OBJECT_INDEXES.Get(Program.OBJ_VNUM_BLOODSTAIN), 0);
            obj.Timer = (short)SmaugRandom.Between(1, 2);
            ch.CurrentRoom.ToRoom(obj);
        }

        public static ObjectInstance CreateMoney(int amount)
        {
            int coinAmt = amount <= 0 ? 1 : amount;

            ObjectInstance obj;
            if (coinAmt == 1)
                obj = DatabaseManager.Instance.OBJECTS.Create(DatabaseManager.Instance.OBJECT_INDEXES.Get(Program.OBJ_VNUM_MONEY_ONE), 0);
            else
            {
                obj = DatabaseManager.Instance.OBJECTS.Create(DatabaseManager.Instance.OBJECT_INDEXES.Get(Program.OBJ_VNUM_MONEY_SOME), 0);
                obj.ShortDescription = string.Format(obj.ShortDescription, coinAmt);
                obj.Value[0] = coinAmt;
            }

            return obj;
        }
    }
}
