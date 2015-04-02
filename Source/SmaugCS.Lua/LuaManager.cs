using Ninject;
using Realm.Library.Common.Logging;
using Realm.Library.Lua;
using SmaugCS.Data;

namespace SmaugCS.Lua
{
    public sealed class LuaManager : ILuaManager
    {
        private string _dataPath;
        private static ILogWrapper _logWrapper;
        private static IKernel _kernel;

        public LuaVirtualMachine LUA { get; private set; }
        public LuaInterfaceProxy Proxy { get; private set; }

        public LuaManager(IKernel kernel, ILogWrapper logWrapper, string path)
        {
            _kernel = kernel;
            _logWrapper = logWrapper;
            _dataPath = path;
        }

        public static ILuaManager Instance
        {
            get { return _kernel.Get<ILuaManager>(); }
        }

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
            Proxy.DoFile(file);
        }
    }
}
