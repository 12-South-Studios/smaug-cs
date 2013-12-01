using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Realm.Library.Lua;
using SmaugCS.Data.Templates;

namespace SmaugCS.Game.Common
{
    public static class LuaMobFunctions
    {
        private static ILuaManager _luaManager;
        private static IDatabaseManager _dbManager;

        public static object LastObject { get; private set; }

        public static void InitializeReferences(ILuaManager luaManager, IDatabaseManager dbManager)
        {
            _luaManager = luaManager;
            _dbManager = dbManager;
        }

        [LuaFunction("LGetLastMob", "Retrieves the Last Mob")]
        public static MobTemplate LuaGetLastMob()
        {
            return (MobTemplate) LastObject;
        }

        [LuaFunction("LProcessMob", "Processes a mob script", "script text")]
        public static MobTemplate LuaProcessMob(string text)
        {
            _luaManager.Proxy.DoString(text);
            return LuaGetLastMob();
        }

        [LuaFunction("LCreateMobile", "Creates a new mob", "Id of the Mobile", "Name of the Mobile")]
        public static MobTemplate LuaCreateMob(string id, string name)
        {
            long mobId = Convert.ToInt64(id);

            MobTemplate newMob = _dbManager.MOBILE_INDEXES.Create(mobId, name);
            _luaManager.Proxy.CreateTable("mobile");
            LastObject = newMob;
            return newMob;
        }
    }
}
