using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Realm.Library.Common;

namespace SmaugCS.Lookup
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    public abstract class LookupBase<T, K> 
        where T : Entity
        where K : class, new()
    {
        protected readonly Dictionary<string, K> _lookupTable;
        private readonly K _defaultFunc;

        protected LookupBase(K defaultFunc)
        {
            _defaultFunc = defaultFunc;
            _lookupTable = new Dictionary<string, K>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected K GetFunction(string name)
        {
            return _lookupTable.ContainsKey(name.ToLower())
                       ? _lookupTable[name.ToLower()]
                       : _defaultFunc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        public abstract void UpdateFunctionReferences(IEnumerable<T> values);
    }
}
