using System;
using Realm.Library.Common.Extensions;
using Realm.Library.Common.Objects;
using Realm.Library.Lua;
using Realm.Standard.Patterns.Repository;
using SmaugCS.Data.Interfaces;
using SmaugCS.Data.Templates;
using SmaugCS.Logging;
using SmaugCS.Repository;

namespace SmaugCS.LuaHelpers
{
    public static class LuaRoomFunctions
    {
        private static ILuaManager _luaManager;
        private static IRepositoryManager _dbManager;
        private static ILogManager _logManager;

        public static object LastObject { get; private set; }

        public static void InitializeReferences(ILuaManager luaManager, IRepositoryManager dbManager, 
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
            var roomId = Convert.ToInt64(id);
            var newRoom = new RoomTemplate(roomId, name);

            _luaManager.Proxy.CreateTable("room");
            _dbManager.ROOMS.CastAs<Repository<long, RoomTemplate>>().Add(roomId, newRoom);
            LastObject = newRoom;

            _logManager.Boot("Room Template (id={0}, name={1}) created", id, name);
            return newRoom;
        }
    }
}
