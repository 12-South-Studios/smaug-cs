using Realm.Library.Lua;
using SmaugCS.Logging;

// ReSharper disable CheckNamespace
namespace SmaugCS.Data
// ReSharper restore CheckNamespace
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
