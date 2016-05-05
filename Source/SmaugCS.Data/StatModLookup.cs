using System.Collections.Generic;

namespace SmaugCS.Data
{
    public class StatModLookup
    {
        public Dictionary<string, object> LookupValues { get; }

        public StatModLookup()
        {
            LookupValues = new Dictionary<string, object>();
        }

        public void AddLookup(string name, object value)
        {
            LookupValues[name] = value;
        }

        public object GetLookup(string name)
        {
            return LookupValues.ContainsKey(name) ? LookupValues[name] : null;
        }
    }
}
