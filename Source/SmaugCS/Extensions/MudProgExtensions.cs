using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Interfaces;
using SmaugCS.Logging;
using SmaugCS.Lua;
using System;

namespace SmaugCS
{
    public static class MudProgExtensions
    {
        public static bool Execute(this MudProgData mudProg, CharacterInstance mob, ILuaManager luaManager = null,
            ILogManager logManager = null)
        {
            if (mob.IsAffected(AffectedByTypes.Charm))
                return false;

            if (mudProg.IsFileProg)
                throw new NotImplementedException("File-based MudProgs are not currently implemented!");

            try
            {
                (luaManager ?? Program.LuaManager).DoLuaScript(mudProg.Script);
            }
            catch (Exception ex)
            {
                (logManager ?? Program.LogManager).Error(ex);
                return false;
            }

            return true;
        }
    }
}
