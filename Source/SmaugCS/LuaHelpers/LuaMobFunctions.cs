using System;
using Library.Lua;
using SmaugCS.Data.Interfaces;
using SmaugCS.Data.Templates;
using SmaugCS.Logging;
using SmaugCS.Repository;

namespace SmaugCS.LuaHelpers;

public static class LuaMobFunctions
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

  [LuaFunction("LGetLastMob", "Retrieves the Last Mob")]
  public static MobileTemplate LuaGetLastMob()
  {
    return (MobileTemplate)LastObject;
  }

  [LuaFunction("LProcessMob", "Processes a mob script", "script text")]
  public static MobileTemplate LuaProcessMob(string text)
  {
    _luaManager.Proxy.DoString(text);
    return LuaGetLastMob();
  }

  [LuaFunction("LCreateMobile", "Creates a new mob", "Id of the Mobile", "Name of the Mobile")]
  public static MobileTemplate LuaCreateMob(string id, string name)
  {
    long mobId = Convert.ToInt64(id);

    MobileTemplate newMob = _dbManager.MOBILETEMPLATES.Create(mobId, name);
    _luaManager.Proxy.CreateTable("mobile");
    LastObject = newMob;

    _logManager.Boot("Mob Template (id={0}, Name={1}) created.", id, name);
    return newMob;
  }
}