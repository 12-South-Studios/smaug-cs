using System.Collections.Generic;
using System.Linq;
using Ninject;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Lookup;

namespace SmaugCS.Managers
{
    public sealed class LookupManager : ILookupManager
    {
        private static Dictionary<string, List<string>> LookupTable;
         
        public LookupManager()
        {
            LookupTable = new Dictionary<string, List<string>>();
            CommandLookup = new CommandLookupTable();
            SkillLookup = new SkillLookupTable();
            SpellLookup = new SpellLookupTable();
        }

        public static ILookupManager Instance
        {
            get { return Program.Kernel.Get<ILookupManager>(); }
        }

        public LookupBase<CommandData, DoFunction> CommandLookup { get; private set; }
        public LookupBase<SkillData, DoFunction> SkillLookup { get; private set; }
        public LookupBase<SkillData, SpellFunction> SpellLookup { get; private set; }

        public ResistanceTypes GetResistanceType(SpellDamageTypes type)
        {
            var attrib = type.GetAttribute<DamageResistanceAttribute>();
            return attrib == null ? ResistanceTypes.Unknown : attrib.ResistanceType;
        }

        public void AddLookup(string table, string entry)
        {
            if (!LookupTable.ContainsKey(table.ToLower()))
                LookupTable[table.ToLower()] = new List<string>();

            var lookups = LookupTable[table.ToLower()];
            if (!lookups.Contains(entry))
                lookups.Add(entry);
        }

        public void RemoveLookup(string table, string entry)
        {
            if (LookupTable.ContainsKey(table.ToLower()) && LookupTable[table.ToLower()].Contains(entry))
                LookupTable[table.ToLower()].Remove(entry);
        }

        public bool HasLookup(string table, string entry)
        {
            if (LookupTable.ContainsKey(table.ToLower()))
            {
                if (LookupTable[table.ToLower()].Contains(entry))
                    return true;
            }
            return false;
        }

        public IEnumerable<string> GetLookups(string table)
        {
            return LookupTable.ContainsKey(table.ToLower()) ? LookupTable[table.ToLower()] : new List<string>();
        }

        public string GetLookup(string table, int index)
        {
            var lookups = GetLookups(table);
            if (lookups.Any() && lookups.Count() > index)
                return lookups.ElementAt(index);
            return string.Empty;
        }
    }
}
