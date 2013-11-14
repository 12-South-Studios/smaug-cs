using System;
using Realm.Library.Lua;
using Realm.Library.Patterns.Repository;
using SmaugCS.Data;
using SmaugCS.Exceptions;
using SmaugCS.Managers;

namespace SmaugCS.Database
{
    /// <summary>
    /// 
    /// </summary>
    public class AreaRepository : Repository<long, AreaData>
    {
        private AreaData LastArea { get; set; }

        [LuaFunction("LProcessArea", "Processes an area script", "script text")]
        public static AreaData LuaProcessArea(string text)
        {
            LuaManager.Instance.Proxy.DoString(text);
            return DatabaseManager.Instance.AREAS.LastArea;
        }

        [LuaFunction("LCreateArea", "Creates a new Area", "Id of the Area", "Name of the Area")]
        public static AreaData LuaCreateArea(string id, string name)
        {
            long areaId = Convert.ToInt64(id);
            if (DatabaseManager.Instance.AREAS.Contains(areaId))
                throw new DuplicateEntryException("Repository contains Area with Id {0}", areaId);

            AreaData newArea = new AreaData(areaId, name);
            LuaManager.Instance.Proxy.CreateTable("area");
            DatabaseManager.Instance.AREAS.Add(areaId, newArea);
            DatabaseManager.Instance.AREAS.LastArea = newArea;
            return newArea;
        }
    }
}
