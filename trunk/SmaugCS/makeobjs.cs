using SmaugCS.Managers;
using SmaugCS.Objects;
using SmaugCS.Common;

namespace SmaugCS
{
    public static class makeobjs
    {
        public static void make_fire(RoomTemplate in_room, short timer)
        {
            ObjectInstance fire = DatabaseManager.Instance.OBJECTS.Create(DatabaseManager.Instance.OBJECT_INDEXES.Get(Program.OBJ_VNUM_FIRE), 0);
            fire.Timer = (short)SmaugRandom.Fuzzy(timer);
            in_room.ToRoom(fire);
        }

        public static ObjectInstance make_trap(int v0, int v1, int v2, int v3)
        {
            ObjectInstance trap = DatabaseManager.Instance.OBJECTS.Create(DatabaseManager.Instance.OBJECT_INDEXES.Get(Program.OBJ_VNUM_TRAP), 0);
            trap.Timer = 0;
            trap.Value[0] = v0;
            trap.Value[1] = v1;
            trap.Value[2] = v2;
            trap.Value[3] = v3;

            return trap;
        }

        public static void make_scraps(ObjectInstance obj)
        {
            // TODO
        }

        public static ObjectInstance make_corpse(CharacterInstance ch, CharacterInstance killer)
        {
            // TODO
            return null;
        }

        public static void make_blood(CharacterInstance ch)
        {
            ObjectInstance obj = DatabaseManager.Instance.OBJECTS.Create(DatabaseManager.Instance.OBJECT_INDEXES.Get(Program.OBJ_VNUM_BLOOD), 0);
            obj.Timer = (short)SmaugRandom.Between(2, 4);
            obj.Value[1] = SmaugRandom.Between(3, Check.Minimum(5, ch.Level));
            ch.CurrentRoom.ToRoom(obj);
        }

        public static void make_bloodstain(CharacterInstance ch)
        {
            ObjectInstance obj = DatabaseManager.Instance.OBJECTS.Create(DatabaseManager.Instance.OBJECT_INDEXES.Get(Program.OBJ_VNUM_BLOODSTAIN), 0);
            obj.Timer = (short)SmaugRandom.Between(1, 2);
            ch.CurrentRoom.ToRoom(obj);
        }

        public static ObjectInstance create_money(int amount)
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
