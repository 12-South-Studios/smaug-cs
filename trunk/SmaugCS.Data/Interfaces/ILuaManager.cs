using Realm.Library.Lua;
using SmaugCS.Logging;

namespace SmaugCS.Data.Interfaces
{
    public interface ILuaManager
    {
        LuaVirtualMachine LUA { get; }
        LuaInterfaceProxy Proxy { get; }
        void Initialize(ILogManager logManager, string path);
        void InitializeLuaProxy(LuaInterfaceProxy proxy);
        void InitVirtualMachine();
        void DoLuaScript(string file);
    }
}
