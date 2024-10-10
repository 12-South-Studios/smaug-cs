using Library.Lua;

namespace SmaugCS.Data.Interfaces;

public interface ILuaManager
{
    LuaVirtualMachine LUA { get; }
    LuaInterfaceProxy Proxy { get; }
    void InitializeLuaProxy(LuaInterfaceProxy proxy);
    void InitVirtualMachine();

    void DoLuaScript(string file);
}