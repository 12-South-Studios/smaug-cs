using Realm.Library.Lua;
using SmaugCS.Logging;

namespace SmaugCS.LuaHelpers
{
    public static class LuaManagerFunctions
    {
        private static ILogManager _logManager;

        public static void InitializeReferences(ILogManager logManager)
        {
            _logManager = logManager;
        }

        [LuaFunction("LBootLog", "Logs an entry to the boot log", "Text to log")]
        public static void LuaBootLog(string txt)
        {
            _logManager.Boot(txt);
        }
    }
}
