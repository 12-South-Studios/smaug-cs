using System;
using Realm.Library.Common;
using Realm.Library.Lua;
using Realm.Library.Patterns.Repository;

using SmaugCS.Data;
using SmaugCS.Data.Templates;
using SmaugCS.Interfaces;
using SmaugCS.Logging;

namespace SmaugCS.LuaHelpers
{
    public static class LuaRoomFunctions
    {
        private static ILuaManager _luaManager;
        private static IDatabaseManager _dbManager;
        private static ILogManager _logManager;

        public static object LastObject { get; private set; }

        public static void InitializeReferences(ILuaManager luaManager, IDatabaseManager dbManager, 
            ILogManager logManager)
        {
            _luaManager = luaManager;
            _dbManager = dbManager;
            _logManager = logManager;
        }

        [LuaFunction("LGetLastRoom", "Retrieves the Last Room")]
        public static RoomTemplate LuaGetLastRoom()
        {
            return (RoomTemplate) LastObject;
        }

        [LuaFunction("LProcessRoom", "Processes a room script", "script text")]
        public static RoomTemplate LuaProcessRoom(string text)
        {
            _luaManager.Proxy.DoString(text);
            return LuaGetLastRoom();
        }

        [LuaFunction("LCreateRoom", "Creates a new room", "Id of the Room", "Name of the Room")]
        public static RoomTemplate LuaCreateRoom(string id, string name)
        {
            long roomId = Convert.ToInt64(id);
            RoomTemplate newRoom = new RoomTemplate(roomId, name);

            _luaManager.Proxy.CreateTable("room");
            _dbManager.ROOMS.CastAs<Repository<long, RoomTemplate>>().Add(roomId, newRoom);
            LastObject = newRoom;

            _logManager.Boot("Room Template (id={0}, name={1}) created", id, name);
            return newRoom;
        }
    }
}
