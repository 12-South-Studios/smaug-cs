using System.Linq;
using Realm.Library.Common;
using Realm.Library.Patterns.Repository;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Logging;
using SmaugCS.Managers;
using SmaugCS.Data.Instances;
using SmaugCS.Objects;

namespace SmaugCS.Extensions
{
    public static class RoomTemplateExtensions
    {
        public static void FromRoom(this RoomTemplate room, CharacterInstance ch)
        {
            if (ch.CurrentRoom != room)
            {
                LogManager.Instance.Bug("Character {0} is not in Room {1}", ch.Name, room.Vnum);
                return;
            }

            if (!ch.IsNpc())
                --room.Area.NumberOfPlayers;

            ObjectInstance obj = ch.GetEquippedItem((int)WearLocations.Light);
            if (obj != null
                && obj.ItemType == ItemTypes.Light
                && obj.Value[2] != 0
                && room.Light > 0)
                --room.Light;

            foreach (AffectData affect in ch.Affects)
                room.RemoveAffect(affect);

            foreach (AffectData affect in room.Affects
                .Where(affect => affect.Location != ApplyTypes.WearSpell
                    && affect.Location != ApplyTypes.RemoveSpell
                    && affect.Location != ApplyTypes.StripSN))
                ch.RemoveAffect(affect);

            room.Persons.Remove(ch);
            ch.PreviousRoom = room;
            ch.CurrentRoom = null;

            if (!ch.IsNpc() && handler.get_timer(ch, (int)TimerTypes.ShoveDrag) > 0)
                handler.remove_timer(ch, (int)TimerTypes.ShoveDrag);
        }

        public static void ToRoom(this RoomTemplate room, CharacterInstance ch)
        {
            RoomTemplate localRoom = room;

            if (ch == null)
            {
                LogManager.Instance.Bug("%s: NULL ch!", "char_to_room");
                return;
            }

            if (DatabaseManager.Instance.ROOMS.CastAs<Repository<long, RoomTemplate>>().Get(room.ID) == null)
            {
                LogManager.Instance.Bug("%s: %s -> NULL room! Putting char in limbo (%d)",
                    "char_to_room", ch.Name, VnumConstants.ROOM_VNUM_LIMBO);
                localRoom = DatabaseManager.Instance.ROOMS.CastAs<Repository<long, RoomTemplate>>().Get(VnumConstants.ROOM_VNUM_LIMBO);
            }

            ch.CurrentRoom = localRoom;
            if (ch.HomeVNum < 1)
                ch.HomeVNum = localRoom.Vnum;
            localRoom.Persons.Add(ch);

            if (!ch.IsNpc())
            {
                localRoom.Area.NumberOfPlayers += 1;
                if (localRoom.Area.NumberOfPlayers > localRoom.Area.MaximumPlayers)
                    localRoom.Area.MaximumPlayers = localRoom.Area.NumberOfPlayers;
            }

            ObjectInstance light = ch.GetEquippedItem(WearLocations.Light);
            if (light != null && light.ItemType == ItemTypes.Light
                && light.Value[2] > 0)
                localRoom.Light++;

            foreach (AffectData affect in localRoom.Affects
                .Where(affect => affect.Location != ApplyTypes.WearSpell
                    && affect.Location != ApplyTypes.RemoveSpell
                    && affect.Location != ApplyTypes.StripSN))
                ch.AddAffect(affect);

            foreach (AffectData affect in localRoom.PermanentAffects
                .Where(affect => affect.Location != ApplyTypes.WearSpell
                    && affect.Location != ApplyTypes.RemoveSpell
                    && affect.Location != ApplyTypes.StripSN))
                ch.AddAffect(affect);

            foreach (AffectData affect in ch.Affects)
                localRoom.AddAffect(affect);

            if (!ch.IsNpc()
                && localRoom.Flags.IsSet((int)RoomFlags.Safe)
                && handler.get_timer(ch, (int)TimerTypes.ShoveDrag) <= 0)
                handler.add_timer(ch, (int)TimerTypes.ShoveDrag, 10, null, 0);

            if (localRoom.Flags.IsSet((int)RoomFlags.Teleport)
                && localRoom.TeleportDelay > 0)
            {
                if (db.TELEPORT.Exists(x => x.Room == localRoom))
                    return;

                db.TELEPORT.Add(new TeleportData
                {
                    Room = localRoom,
                    Timer = (short)localRoom.TeleportDelay
                });
            }

            if (ch.PreviousRoom == null)
                ch.PreviousRoom = ch.CurrentRoom;
        }

        public static CharacterInstance IsDoNotDisturb(this RoomTemplate room, CharacterInstance ch)
        {
            if (room.Flags.IsSet((int)RoomFlags.DoNotDisturb))
                return null;

            return room.Persons.FirstOrDefault(rch => !rch.IsNpc()
                && rch.PlayerData != null
                && rch.IsImmortal()
                && rch.PlayerData.Flags.IsSet((int)PCFlags.DoNotDisturb)
                && ch.Trust < rch.Trust
                && ch.CanSee(rch));
        }

        public static void FromRoom(this RoomTemplate room, ObjectInstance obj)
        {
            if (obj.InRoom != room)
            {
                LogManager.Instance.Bug("Object {0} is not in Room {1}", obj.Name, room.ID);
                return;
            }

            foreach (AffectData paf in obj.Affects)
                room.RemoveAffect(paf);
            foreach (AffectData paf in obj.ObjectIndex.Affects)
                room.RemoveAffect(paf);

            room.Contents.Remove(obj);

            if (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Covering)
                && obj.Contents != null)
                handler.empty_obj(obj, null, room);

            if (obj.ItemType == ItemTypes.Fire)
                obj.InRoom.Light -= obj.Count;

            obj.CarriedBy = null;
            obj.InObject = null;
            obj.InRoom = null;

            if (obj.ObjectIndex.Vnum == VnumConstants.OBJ_VNUM_CORPSE_PC && handler.falling > 0)
                save.write_corpses(null, obj.ShortDescription, obj);
        }

        public static ObjectInstance ToRoom(this RoomTemplate room, ObjectInstance obj)
        {
            foreach (AffectData paf in obj.Affects)
                room.AddAffect(paf);
            foreach (AffectData paf in obj.ObjectIndex.Affects)
                room.AddAffect(paf);

            int count = obj.Count;
            ItemTypes itemType = obj.ItemType;

            foreach (ObjectInstance otmp in room.Contents)
            {
                ObjectInstance oret = handler.group_object(otmp, obj);
                if (oret == otmp)
                {
                    if (itemType == ItemTypes.Fire)
                        room.Light += count;
                    return oret;
                }
            }

            room.Contents.Add(obj);
            obj.InRoom = room;
            obj.CarriedBy = null;
            obj.InObject = null;
            obj.room_vnum = room.ID;
            if (itemType == ItemTypes.Fire)
                room.Light += count;
            handler.falling++;
            act_obj.obj_fall(obj, false);
            handler.falling--;
            if (obj.ObjectIndex.Vnum == VnumConstants.OBJ_VNUM_CORPSE_PC && handler.falling < 1)
                save.write_corpses(null, obj.ShortDescription, null);
            return obj;
        }

        public static bool IsDark(this RoomTemplate room)
        {
            if (room.Light > 0)
                return false;

            if (room.Flags.IsSet((int)RoomFlags.Dark))
                return true;

            if (room.SectorType == SectorTypes.Inside
                || room.SectorType == SectorTypes.City)
                return false;

            return db.GameTime.Sunlight == SunPositionTypes.Sunset
                   || db.GameTime.Sunlight == SunPositionTypes.Dark;
        }
    }
}
