using System;
using Realm.Library.Lua;
using SmaugCS.Data.Interfaces;
using SmaugCS.Data.Templates;
using SmaugCS.Logging;
using SmaugCS.Repository;

namespace SmaugCS.LuaHelpers
{
    public static class LuaObjectFunctions
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
            var objId = Convert.ToInt64(id);

            var newObj = _dbManager.OBJECTTEMPLATES.Create(objId, name);
            _luaManager.Proxy.CreateTable("object");
            LastObject = newObj;

            _logManager.Boot("Object Template (id={0}, Name={1}) created.", id, name);
            return newObj;
        }
    }
}
