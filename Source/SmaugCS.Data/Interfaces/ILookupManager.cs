using System;
using System.Collections.Generic;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Data.Interfaces
{
    public interface ILookupManager
    {
        void AddLookup(string table, string entry);
        void RemoveLookup(string table, string entry);
        bool HasLookup(string table, string entry);
        IEnumerable<string> GetLookups(string table);
        string GetLookup(string table, int index);
        ResistanceTypes GetResistanceType(SpellDamageTypes type);

        LookupBase<CommandData, DoFunction> CommandLookup { get; }
        LookupBase<SkillData, DoFunction> SkillLookup { get; }
        LookupBase<SkillData, SpellFunction> SpellLookup { get; }

        Dictionary<string, List<StatModLookup>> StatModLookup { get; }

        object GetStatMod(string category, int level, Enum name);
    }
}
