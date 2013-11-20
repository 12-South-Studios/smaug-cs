using System;
using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using Realm.Library.Lua;
using Realm.Library.Patterns.Repository;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Templates;
using SmaugCS.Exceptions;
using SmaugCS.Managers;

namespace SmaugCS.Database
{
    /// <summary>
    /// 
    /// </summary>
    public class RoomRepository : Repository<long, RoomTemplate>, ITemplateRepository<RoomTemplate>
    {
        private RoomTemplate LastRoom { get; set; }

        [LuaFunction("LGetLastRoom", "Retrieves the Last Room")]
        public static RoomTemplate LuaGetLastRoom()
        {
            return DatabaseManager.Instance.ROOMS.LastRoom;
        }

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

        public RoomTemplate Create(long vnum, long cvnum, string name)
        {
            throw new NotImplementedException();
        }

        public RoomTemplate Create(long vnum, string name)
        {
            Validation.Validate(vnum >= 1);
            Validation.Validate(() =>
                {
                    if (Contains(vnum))
                        throw new DuplicateIndexException("Invalid vnum {0}, Index already exists", vnum);
                });

            RoomTemplate newRoom = new RoomTemplate(vnum, name.IsNullOrEmpty() ? "Floating in a Void" : name)
                                       {
                                           SectorType = SectorTypes.Inside,
                                       };
            newRoom.Flags.SetBit((int)RoomFlags.Prototype);

            Add(vnum, newRoom);
            return newRoom;
        }
    }
}
