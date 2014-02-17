﻿using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using Realm.Library.Lua;
using Realm.Library.Patterns.Repository;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions;
using SmaugCS.Managers;

namespace SmaugCS.LuaHelpers
{
    public static class LuaGetFunctions
    {
        private static ILuaManager _luaManager;
        private static IDatabaseManager _dbManager;
        private static string _dataPath;

        public static void InitializeReferences(ILuaManager luaManager, IDatabaseManager dbManager, string dataPath)
        {
            _luaManager = luaManager;
            _dbManager = dbManager;
            _dataPath = dataPath;
        }

        [LuaFunction("LGetLastEntity", "Retrieves the Last Entity")]
        public static object LuaGetLastEntity()
        {
            return LuaCreateFunctions.LastObject;
        }

        [LuaFunction("LGetRoom", "Retrieves a room with a given ID", "ID of the room")]
        public static RoomTemplate LuaGetRoom(long id)
        {
            return _dbManager.ROOMS.CastAs<Repository<long, RoomTemplate>>().Get(id);
        }

        [LuaFunction("LGetMobile", "Retrieves a mob with a given ID", "ID of the mobile")]
        public static MobTemplate LuaGetMobile(long id)
        {
            return _dbManager.MOBILE_INDEXES.CastAs<Repository<long, MobTemplate>>().Get(id);
        }

        [LuaFunction("LGetObject", "Retrieves an object with a given ID", "ID of the object")]
        public static ObjectTemplate LuaGetObject(long id)
        {
            return _dbManager.OBJECT_INDEXES.CastAs<Repository<long, ObjectTemplate>>().Get(id);
        }

        [ExcludeFromCodeCoverage]
        [LuaFunction("CheckNumber", "Validates a vnum for range", "Number to check", "Int maximum")]
        public static bool LuaCheckNumber(int vnum, int max)
        {
            if (vnum < 1 || vnum > max)
            {
                //.Bug("Vnum {0} is out of range (1 to {1})", vnum, Program.MAX_VNUM);
                return false;
            }

            return true;
        }

        [ExcludeFromCodeCoverage]
        [LuaFunction("LDataPath", "Retrieves the game's data path")]
        public static string LuaGetDataPath()
        {
            return _dataPath.IsNullOrEmpty() ? Program.GetDataPath() : _dataPath;
        }

        [LuaFunction("FindInstance", "Locates a character matching the given name", "Instance executing this search", "String argument")]
        public static CharacterInstance LuaFindCharacter(CharacterInstance instance, string arg)
        {
            if (arg.EqualsIgnoreCase("self"))
                return instance;

            return
                instance.CurrentRoom.Persons.FirstOrDefault(
                    vch => handler.can_see(instance, vch) && !vch.IsNpc() && vch.Name.EqualsIgnoreCase(arg)) ??
                _dbManager.CHARACTERS.CastAs<Repository<long, CharacterInstance>>().Values.FirstOrDefault(
                    vch => handler.can_see(instance, vch) && !vch.IsNpc() && vch.Name.EqualsIgnoreCase(arg));
        }
    }
}