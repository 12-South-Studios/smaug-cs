using Realm.Library.Common.Objects;
using Realm.Library.Lua;
using Realm.Library.Common.Extensions;

namespace SmaugCS.Managers
{
    public sealed class LuaManager : GameSingleton
    {
        private static LuaManager _instance;
        private static readonly object Padlock = new object();

        private string _dataPath;

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
        /// <param name="path"></param>
        public void InitDataPath(string path)
        {
            _dataPath = path;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxy"></param>
        public void InitProxy(LuaInterfaceProxy proxy)
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
    }
}
