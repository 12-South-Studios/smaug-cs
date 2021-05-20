namespace Realm.Library.Lua
{
    /// <summary>
    ///
    /// </summary>
    public interface ILuaLoadBalancer
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="script"></param>
        void ExecuteScript(LuaScript script);
    }
}