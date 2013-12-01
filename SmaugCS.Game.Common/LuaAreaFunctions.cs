using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Realm.Library.Lua;
using SmaugCS.Data;

namespace SmaugCS.Game.Common
{
    public static class LuaAreaFunctions
    {
        private static ILuaManager _luaManager;
        private static IDatabaseManager _dbManager;

        public static object LastObject { get; private set; }

        public static void InitializeReferences(ILuaManager luaManager, IDatabaseManager dbManager)
        {
            _luaManager = luaManager;
            _dbManager = dbManager;
        }

        [LuaFunction("LGetLastArea", "Retrieves the Last Area")]
        public static AreaData LuaGetLastArea()
        {
            return (AreaData) LastObject;
        }

        [LuaFunction("LProcessArea", "Processes an area script", "script text")]
        public static AreaData LuaProcessArea(string text)
        {
            _luaManager.Proxy.DoString(text);
            return LuaGetLastArea();
        }

        [LuaFunction("LCreateArea", "Creates a new Area", "Id of the Area", "Name of the Area")]
        public static AreaData LuaCreateArea(string id, string name)
        {
            long areaId = Convert.ToInt64(id);

            AreaData newArea = new AreaData(areaId, name);
            _luaManager.Proxy.CreateTable("area");
            LastObject = newArea;
            _dbManager.AREAS.Add(areaId, newArea);
            return newArea;
        }
    }
}
