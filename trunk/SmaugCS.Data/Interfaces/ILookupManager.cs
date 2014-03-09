using System.Collections.Generic;
using SmaugCS.Constants.Enums;

// ReSharper disable CheckNamespace
namespace SmaugCS.Data
// ReSharper restore CheckNamespace
{
    public interface ILookupManager
    {
        void AddLookup(string table, string entry);
        void RemoveLookup(string table, string entry);
        bool HasLookup(string table, string entry);
        IEnumerable<string> GetLookups(string table);
        string GetLookup(string table, int index);
        ResistanceTypes GetResistanceType(SpellDamageTypes type);
    }
}
