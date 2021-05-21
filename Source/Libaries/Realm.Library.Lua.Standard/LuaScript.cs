using LuaInterface;

namespace Realm.Library.Lua
{
    /// <summary>
    ///
    /// </summary>
    public class LuaScript
    {
        /// <summary>
        ///
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Script { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Function { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool IsFile { get; set; }

        /// <summary>
        ///
        /// </summary>
        public LuaTable Table { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Path { get; set; }
    }
}