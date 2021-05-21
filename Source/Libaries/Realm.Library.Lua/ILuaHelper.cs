using System;
using System.Reflection;

namespace Realm.Library.Lua
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILuaHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="functionRepository"></param>
        /// <returns></returns>
        LuaFunctionRepository Register(Type type, LuaFunctionRepository functionRepository);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        LuaFunctionDescriptor Create(MethodInfo info, Attribute attr);
    }
}
