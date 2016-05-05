using System;
using Ninject;
using Realm.Library.Common.Logging;
using Realm.Library.Lua;
using SmaugCS.Data.Interfaces;

namespace SmaugCS.Lua
{
    public sealed class LuaManager : ILuaManager
    {
        private static ILogWrapper _logWrapper;
        private static IKernel _kernel;

        public LuaVirtualMachine LUA { get; private set; }
        public LuaInterfaceProxy Proxy { get; private set; }

        public LuaManager(IKernel kernel, ILogWrapper logWrapper)
        {
            _kernel = kernel;
            _logWrapper = logWrapper;
        }

        public static ILuaManager Instance => _kernel.Get<ILuaManager>();

        public void InitializeLuaProxy(LuaInterfaceProxy proxy)
        {
            Proxy = proxy ?? new LuaInterfaceProxy();
            var luaFuncRepo = LuaHelper.Register(typeof(LuaManager), null);
            Proxy.RegisterFunctions(luaFuncRepo);
        }

        public void InitVirtualMachine()
        {
            LUA = new LuaVirtualMachine(1, null, new LuaFunctionRepository(), Proxy);
        }

        public void DoLuaScript(string file)
        {
            try
            {
                Proxy.DoFile(file);
            }
            catch (Exception ex)
            {
                _logWrapper.Error(ex.ToString(), ex);
            }
        }
    }
}
