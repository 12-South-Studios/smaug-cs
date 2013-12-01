using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Realm.Library.Common.Extensions;
using Realm.Library.Lua;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Game.Common;

namespace SmaugCS.Lua
{
    public static class LuaGetFunctions
    {
        private static ILuaManager _luaManager;
        private static IDatabaseManager _dbManager;

        public static void InitializeReferences(ILuaManager luaManager, IDatabaseManager dbManager)
        {
            _luaManager = luaManager;
            _dbManager = dbManager;
        }

        [LuaFunction("LGetLastEntity", "Retrieves the Last Entity")]
        public static object LuaGetLastEntity()
        {
            return LuaCreateFunctions.LastObject;
        }

        [LuaFunction("LGetRoom", "Retrieves a room with a given ID", "ID of the room")]
        public static RoomTemplate LuaGetRoom(long id)
        {
            //return _dbManager.ROOMS.Get(id);
            return null;
        }

        [LuaFunction("LGetMobile", "Retrieves a mob with a given ID", "ID of the mobile")]
        public static MobTemplate LuaGetMobile(long id)
        {
           // return _dbManager.MOBILE_INDEXES.Get(id);
            return null;
        }

        [LuaFunction("LGetObject", "Retrieves an object with a given ID", "ID of the object")]
        public static ObjectTemplate LuaGetObject(long id)
        {
           // return _dbManager.OBJECT_INDEXES.Get(id);
            return null;
        }

        [ExcludeFromCodeCoverage]
        [LuaFunction("CheckNumber", "Validates a vnum for range", "Number to check")]
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
            //return Program.GetDataPath();
            return string.Empty;
        }

        [LuaFunction("FindInstance", "Locates a character matching the given name", "Instance executing this search", "String argument")]
        public static CharacterInstance LuaFindCharacter(CharacterInstance instance, string arg)
        {
            if (arg.EqualsIgnoreCase("self"))
                return instance;

            /*return
                instance.CurrentRoom.Persons.FirstOrDefault(
                    vch => handler.can_see(instance, vch) && !vch.IsNpc() && vch.Name.EqualsIgnoreCase(arg)) ??
                _dbManager.CHARACTERS.Values.FirstOrDefault(
                    vch => handler.can_see(instance, vch) && !vch.IsNpc() && vch.Name.EqualsIgnoreCase(arg));*/
            return null;
        }
    }
}
