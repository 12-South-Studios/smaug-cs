using System;
using Realm.Library.Lua;

using SmaugCS.Data;
using SmaugCS.Managers;

namespace SmaugCS.LuaHelpers
{
    public static class LuaObjectFunctions
    {
        private static ILuaManager _luaManager;
        private static IDatabaseManager _dbManager;

        public static object LastObject { get; private set; }

        public static void InitializeReferences(ILuaManager luaManager, IDatabaseManager dbManager)
        {
            _luaManager = luaManager;
            _dbManager = dbManager;
        }

        [LuaFunction("LGetLastObj", "Retrieves the Last Object")]
        public static ObjectTemplate LuaGetLastObj()
        {
            return (ObjectTemplate) LastObject;
        }

        [LuaFunction("LProcessObject", "Processes an object script", "script text")]
        public static ObjectTemplate LuaProcessObject(string text)
        {
            _luaManager.Proxy.DoString(text);
            return LuaGetLastObj();
        }

        [LuaFunction("LCreateObject", "Creates a new object", "Id of the Object", "Name of the Object")]
        public static ObjectTemplate LuaCreateObject(string id, string name)
        {
            long objId = Convert.ToInt64(id);

            ObjectTemplate newObj = _dbManager.OBJECT_INDEXES.Create(objId, name);
            _luaManager.Proxy.CreateTable("object");
            LastObject = newObj;
            return newObj;
        }
    }
}
