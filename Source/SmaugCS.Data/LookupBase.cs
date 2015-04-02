using System.Collections.Generic;
using Realm.Library.Common;

namespace SmaugCS.Data
{
    public abstract class LookupBase<T, TK> 
        where T : Entity
        where TK : class, new()
    {
        protected Dictionary<string, TK> LookupTable { get; private set; }
        private readonly TK _defaultFunc;

        protected LookupBase(TK defaultFunc)
        {
            _defaultFunc = defaultFunc;
            LookupTable = new Dictionary<string, TK>();
        }

        protected TK GetFunction(string name)
        {
            return LookupTable.ContainsKey(name.ToLower())
                       ? LookupTable[name.ToLower()]
                       : _defaultFunc;
        }

        public abstract void UpdateFunctionReferences(IEnumerable<T> values);
    }
}
