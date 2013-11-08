using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Realm.Library.Common.Objects;
using Realm.Library.Lua;

namespace SmaugCS.Managers
{
    public sealed class LuaManager : GameSingleton
    {
        private static LuaManager _instance;
        private static readonly object Padlock = new object();

        public LuaVirtualMachine LUA { get; private set; }

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

        public void Init()
        {
            LUA = new LuaVirtualMachine(1, null, new LuaFunctionRepository(), new LuaInterfaceProxy());
        }
    }
}
