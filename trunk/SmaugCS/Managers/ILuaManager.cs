using Realm.Library.Lua;

namespace SmaugCS.Managers
{
    public interface ILuaManager
    {
        LuaVirtualMachine LUA { get; }
        LuaInterfaceProxy Proxy { get; }
        void InitializeManager(ILogManager logManager, string path);
        void InitializeLuaProxy(LuaInterfaceProxy proxy);
        void InitVirtualMachine();
        void DoLuaScript(string file);
    }
}
