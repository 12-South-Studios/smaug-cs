using System.Collections.Generic;

namespace SmaugCS.Data.Interfaces
{
    public interface ILookupManager
    {
        void AddLookup(string table, string entry);
        void RemoveLookup(string table, string entry);
        bool HasLookup(string table, string entry);
        IEnumerable<string> GetLookups(string table);
        string GetLookup(string table, int index);
    }
}
