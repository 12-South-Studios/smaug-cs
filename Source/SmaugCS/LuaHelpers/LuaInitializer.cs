using Library.Lua;
using SmaugCS.Common;
using SmaugCS.Data.Interfaces;
using SmaugCS.Logging;
using SmaugCS.Repository;

namespace SmaugCS.LuaHelpers;

public class LuaInitializer(ILuaManager luaManager, IRepositoryManager dbManager, ILogManager logManager,
  ILookupManager lookupManager) : IInitializer
{
  public void Initialize()
  {
  }

  public void InitializeLuaInjections(string dataPath)
  {
    LuaAreaFunctions.InitializeReferences(luaManager, dbManager, logManager);
    LuaCreateFunctions.InitializeReferences(luaManager, dbManager, logManager);
    LuaGetFunctions.InitializeReferences(luaManager, dbManager, dataPath);
    LuaMobFunctions.InitializeReferences(luaManager, dbManager, logManager);
    LuaObjectFunctions.InitializeReferences(luaManager, dbManager, logManager);
    LuaRoomFunctions.InitializeReferences(luaManager, dbManager, logManager);
    LuaLookupFunctions.InitializeReferences(lookupManager, logManager);
    LuaManagerFunctions.InitializeReferences(logManager);
  }

  public void InitializeLuaFunctions()
  {
    LuaInterfaceProxy proxy = new();
    LuaFunctionRepository luaFuncRepo = new();
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
    luaManager.InitializeLuaProxy(proxy);
  }
}