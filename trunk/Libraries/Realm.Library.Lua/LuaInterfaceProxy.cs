using System.Linq;
using System.Reflection;
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

        public void Close()
        {
            _lua.Close();
        }

        public LuaTable CreateTable(string name)
        {
            _lua.NewTable(name);
            return _lua.GetTable(name);
        }

        public object[] DoFile(string fileName)
        {
            return _lua.DoFile(fileName);
        }

        public object[] DoFileWithLoad(string fileName)
        {
            var luaFunc = _lua.LoadFile(fileName);
            return luaFunc.Call(null);
        }

        public object[] DoString(string chunk)
        {
            return _lua.DoString(chunk);
        }

        public object[] DoStringWithLoad(string chunk, string func)
        {
            var luaFunc = _lua.LoadString(chunk, func);
            return luaFunc.Call(null);
        }

        public LuaFunction GetFunction(string fullPath)
        {
            return _lua.GetFunction(fullPath);
        }

        public void RegisterFunctions(LuaFunctionRepository repository)
        {
            repository.Values.ToList().ForEach(x => RegisterFunction(x.Name, null, x.Info));
        }

        public LuaFunction RegisterFunction(string path, object target, MethodBase function)
        {
            return _lua.RegisterFunction(path, target, function);
        }
    }
}
