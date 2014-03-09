using System.Collections.Generic;
using Realm.Library.Common;

namespace SmaugCS.Lookup
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TK"></typeparam>
    public abstract class LookupBase<T, TK> 
        where T : Entity
        where TK : class, new()
    {
        protected readonly Dictionary<string, TK> LookupTable;
        private readonly TK _defaultFunc;

        protected LookupBase(TK defaultFunc)
        {
            _defaultFunc = defaultFunc;
            LookupTable = new Dictionary<string, TK>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected TK GetFunction(string name)
        {
            return LookupTable.ContainsKey(name.ToLower())
                       ? LookupTable[name.ToLower()]
                       : _defaultFunc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        public abstract void UpdateFunctionReferences(IEnumerable<T> values);
    }
}
