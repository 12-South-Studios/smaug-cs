using Realm.Library.Common.Objects;
using Realm.Library.Lua;
using SmaugCS.Data.Interfaces;
using SmaugCS.Logging;

namespace SmaugCS.Managers
{
    public sealed class LuaManager : GameSingleton, ILuaManager
    {
        private static LuaManager _instance;
        private static readonly object Padlock = new object();

        private string _dataPath;
        private static ILogManager _logManager;

        public LuaVirtualMachine LUA { get; private set; }
        public LuaInterfaceProxy Proxy { get; private set; }

        private LuaManager()
        {
        }

        /// <summary>
        ///
        /// </summary>
        public static LuaManager Instance
        {
            get
            {
                lock (Padlock)
                {
                    return _instance ?? (_instance = new LuaManager());
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logManager"></param>
        public void Initialize(ILogManager logManager, string path)
        {
            _logManager = logManager;
            _dataPath = path;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxy"></param>
        public void InitializeLuaProxy(LuaInterfaceProxy proxy)
        {
            Proxy = proxy ?? new LuaInterfaceProxy();
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
