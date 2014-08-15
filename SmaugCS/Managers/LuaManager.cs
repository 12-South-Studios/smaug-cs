using Ninject;
using Realm.Library.Common;
using Realm.Library.Common.Logging;
using Realm.Library.Lua;
using SmaugCS.Data;
using SmaugCS.Logging;

namespace SmaugCS.Managers
{
    public sealed class LuaManager : ILuaManager
    {
        private string _dataPath;
        private static ILogWrapper _logWrapper;

        public LuaVirtualMachine LUA { get; private set; }
        public LuaInterfaceProxy Proxy { get; private set; }

        public LuaManager(ILogWrapper logWrapper, string path)
        {
            _logWrapper = logWrapper;
            _dataPath = path;
        }

        public static ILuaManager Instance
        {
            get { return Program.Kernel.Get<ILuaManager>(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxy"></param>
        public void InitializeLuaProxy(LuaInterfaceProxy proxy)
        {
            Proxy = proxy ?? LuaInterfaceProxy.Create();
            var luaFuncRepo = LuaHelper.RegisterFunctionTypes(null, typeof(LuaManager));
            Proxy.RegisterFunctions(luaFuncRepo);
        }

        /// <summary>
        /// 
        /// </summary>
        public void InitVirtualMachine()
        {
            LUA = new LuaVirtualMachine(1, null, new LuaFunctionRepository(), Proxy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        public void DoLuaScript(string file)
        {
            Proxy.DoFile(file);
        }
    }
}
