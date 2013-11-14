using System;
using Realm.Library.Common;
using Realm.Library.Lua;
using Realm.Library.Patterns.Repository;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Templates;
using SmaugCS.Exceptions;
using SmaugCS.Managers;

namespace SmaugCS.Database
{
    /// <summary>
    /// 
    /// </summary>
    public class RoomRepository : Repository<long, RoomTemplate>
    {
        private RoomTemplate LastRoom { get; set; }

        [LuaFunction("LProcessRoom", "Processes a room script", "script text")]
        public static RoomTemplate LuaProcessRoom(string text)
        {
            LuaManager.Instance.Proxy.DoString(text);
            return DatabaseManager.Instance.ROOMS.LastRoom;
        }

        [LuaFunction("LCreateRoom", "Creates a new room", "Id of the Room", "Name of the Room")]
        public static RoomTemplate LuaCreateRoom(string id, string name)
        {
            long roomId = Convert.ToInt64(id);
            RoomTemplate newRoom = new RoomTemplate(roomId, name);
            if (DatabaseManager.Instance.ROOMS.Contains(roomId))
                throw new DuplicateEntryException("Repository contains Room with Id {0}", roomId);

            LuaManager.Instance.Proxy.CreateTable("room");
            DatabaseManager.Instance.ROOMS.Add(roomId, newRoom);
            DatabaseManager.Instance.ROOMS.LastRoom = newRoom;
            return newRoom;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vnum"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        public RoomTemplate Create(long vnum, AreaData area)
        {
            Validation.Validate(vnum >= 1);
            Validation.IsNotNull(area, "area");
            Validation.Validate(() =>
                {
                    if (Contains(vnum))
                        throw new DuplicateIndexException("Invalid vnum {0}, Index already exists", vnum);
                });

            RoomTemplate newRoom = new RoomTemplate(vnum, "Floating in a void")
                                       {
                                           SectorType = SectorTypes.Inside,
                                           Area = area
                                       };
            newRoom.Flags.SetBit((int)RoomFlags.Prototype);

            Add(vnum, newRoom);
            area.Rooms.Add(newRoom);
            return newRoom;
        }
    }
}
