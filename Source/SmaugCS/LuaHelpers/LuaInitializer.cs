using Realm.Library.Lua;
using SmaugCS.Data;
using SmaugCS.Interfaces;
using SmaugCS.Logging;

namespace SmaugCS.LuaHelpers
{
    public class LuaInitializer : IInitializer
    {
        private readonly ILuaManager _luaManager;
        private readonly IDatabaseManager _dbManager;
        private readonly ILogManager _logManager;
        private readonly ILookupManager _lookupManager;

        public LuaInitializer(ILuaManager luaManager, IDatabaseManager dbManager, ILogManager logManager,
            ILookupManager lookupManager)
        {
            _luaManager = luaManager;
            _dbManager = dbManager;
            _logManager = logManager;
            _lookupManager = lookupManager;
        }

        public void InitializeLuaInjections(string dataPath)
        {
            LuaAreaFunctions.InitializeReferences(_luaManager, _dbManager, _logManager);
            LuaCreateFunctions.InitializeReferences(_luaManager, _dbManager, _logManager);
            LuaGetFunctions.InitializeReferences(_luaManager, _dbManager, dataPath);
            LuaMobFunctions.InitializeReferences(_luaManager, _dbManager, _logManager);
            LuaObjectFunctions.InitializeReferences(_luaManager, _dbManager, _logManager);
            LuaRoomFunctions.InitializeReferences(_luaManager, _dbManager, _logManager);
            LuaLookupFunctions.InitializeReferences(_lookupManager, _logManager);
            LuaManagerFunctions.InitializeReferences(_logManager);
        }

        public void InitializeLuaFunctions()
        {
            LuaInterfaceProxy proxy = new LuaInterfaceProxy();
            LuaFunctionRepository luaFuncRepo = new LuaFunctionRepository();
            LuaHelper.Register(typeof(LuaAreaFunctions), luaFuncRepo);
            LuaHelper.Register(typeof(LuaCreateFunctions), luaFuncRepo);
            LuaHelper.Register(typeof(LuaGetFunctions), luaFuncRepo);
            LuaHelper.Register(typeof(LuaMobFunctions), luaFuncRepo);
            LuaHelper.Register(typeof(LuaObjectFunctions), luaFuncRepo);
            LuaHelper.Register(typeof(LuaRoomFunctions), luaFuncRepo);
            LuaHelper.Register(typeof(LuaLookupFunctions), luaFuncRepo);
            LuaHelper.Register(typeof(LuaManagerFunctions), luaFuncRepo);
            LuaHelper.Register(typeof(LuaMudProgFunctions), luaFuncRepo);
            proxy.RegisterFunctions(luaFuncRepo);
            _luaManager.InitializeLuaProxy(proxy);
        }
    }
}
