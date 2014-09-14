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
            LuaInterfaceProxy proxy = LuaInterfaceProxy.Create();
            LuaFunctionRepository luaFuncRepo = new LuaFunctionRepository();
            LuaHelper.RegisterFunctionTypes(luaFuncRepo, typeof(LuaAreaFunctions));
            LuaHelper.RegisterFunctionTypes(luaFuncRepo, typeof(LuaCreateFunctions));
            LuaHelper.RegisterFunctionTypes(luaFuncRepo, typeof(LuaGetFunctions));
            LuaHelper.RegisterFunctionTypes(luaFuncRepo, typeof(LuaMobFunctions));
            LuaHelper.RegisterFunctionTypes(luaFuncRepo, typeof(LuaObjectFunctions));
            LuaHelper.RegisterFunctionTypes(luaFuncRepo, typeof(LuaRoomFunctions));
            LuaHelper.RegisterFunctionTypes(luaFuncRepo, typeof(LuaLookupFunctions));
            LuaHelper.RegisterFunctionTypes(luaFuncRepo, typeof(LuaManagerFunctions));
            LuaHelper.RegisterFunctionTypes(luaFuncRepo, typeof (LuaMudProgFunctions));
            proxy.RegisterFunctions(luaFuncRepo);
            _luaManager.InitializeLuaProxy(proxy);
        }
    }
}
