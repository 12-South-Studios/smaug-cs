using Realm.Library.Lua;
using SmaugCS.Data.Interfaces;
using SmaugCS.Exceptions;
using SmaugCS.Logging;
using SmaugCS.Managers;

namespace SmaugCS.LuaHelpers
{
    public static class LuaLookupFunctions
    {
        private static ILookupManager _lookupManager;
        private static ILogManager _logManager;

        public static void InitializeReferences(ILookupManager lookupManager, ILogManager logManager)
        {
            _lookupManager = lookupManager;
            _logManager = logManager;
        }

        [LuaFunction("LAddLookup", "Adds a new lookup entry", "Lookup table", "Lookup entry")]
        public static void LuaAddLookup(string lookupTable, string lookupEntry)
        {
            if (_lookupManager.HasLookup(lookupTable, lookupEntry))
            {
                _logManager.BootLog(new DuplicateEntryException("Lookup {0}:{1} already exists.", lookupTable, lookupEntry));
                return;
            }

            _lookupManager.AddLookup(lookupTable, lookupEntry);
            _logManager.BootLog("Lookup {0}:{1} added.", lookupTable, lookupEntry);
        }
    }
}
