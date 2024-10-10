using System;
using Library.Lua;
using SmaugCS.Data;
using SmaugCS.Data.Interfaces;
using SmaugCS.Logging;
using SmaugCS.Repository;

namespace SmaugCS.LuaHelpers;

public static class LuaAreaFunctions
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

  [LuaFunction("LGetLastArea", "Retrieves the Last Area")]
  public static AreaData LuaGetLastArea()
  {
    return (AreaData)LastObject;
  }

  [LuaFunction("LProcessArea", "Processes an area script", "script text")]
  public static AreaData LuaProcessArea(string text)
  {
    _luaManager.Proxy.DoString(text);
    return LuaGetLastArea();
  }

  [LuaFunction("LCreateArea", "Creates a new Area", "Id of the Area", "Name of the Area")]
  public static AreaData LuaCreateArea(int id, string name)
  {
    long areaId = Convert.ToInt64(id);

    AreaData newArea = new(areaId, name);
    _luaManager.Proxy.CreateTable("area");
    LastObject = newArea;
    _dbManager.AREAS.Add(areaId, newArea);

    _logManager.Boot("Area (id={0}, name={1}) created.", id, name);
    return newArea;
  }
}