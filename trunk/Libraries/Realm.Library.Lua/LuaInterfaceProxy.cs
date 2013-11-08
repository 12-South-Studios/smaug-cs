using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using LuaInterface;
using Realm.Library.Common;
using Realm.Library.Lua.Properties;

namespace Realm.Library.Lua
{
    public class LuaInterfaceProxy
    {
        private readonly LuaInterface.Lua _lua;

        public LuaInterfaceProxy()
        {
            _lua = new LuaInterface.Lua();    
            if (_lua.IsNull())
                throw new LuaException(Resources.ERR_UNABLE_INITIALIZE);
        }

        public object[] DoFile(string fileName)
        {
            return _lua.DoFile(fileName);
        }

        public object[] DoString(string chunk)
        {
            return _lua.DoString(chunk);
        }

        public LuaFunction GetFunction(string fullPath)
        {
            return _lua.GetFunction(fullPath);
        }

        public LuaFunction RegisterFunction(string path, object target, MethodBase function)
        {
            return _lua.RegisterFunction(path, target, function);
        }
    }
}
