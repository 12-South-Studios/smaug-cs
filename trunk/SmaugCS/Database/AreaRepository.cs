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
    }
}
