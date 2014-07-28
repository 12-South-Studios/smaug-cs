using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Lookup;

namespace SmaugCS.Managers
{
    public sealed class LookupManager : GameSingleton, ILookupManager
    {
        private static LookupManager _instance;
        private static readonly object Padlock = new object();

        private static Dictionary<string, List<string>> LookupTable;
         
        private LookupManager()
        {
        }

        /// <summary>
        ///
        /// </summary>
        public static LookupManager Instance
        {
            get
            {
                lock (Padlock)
                {
                    return _instance ?? (_instance = new LookupManager());
                }
            }
        }

        public CommandLookupTable CommandLookup { get; private set; }
        public SkillLookupTable SkillLookup { get; private set; }
        public SpellLookupTable SpellLookup { get; private set; }

        public ResistanceTypes GetResistanceType(SpellDamageTypes type)
        {
            DamageResistanceAttribute attrib = type.GetAttribute<DamageResistanceAttribute>();
            return attrib == null ? ResistanceTypes.Unknown : attrib.ResistanceType;
        }

        public void Initialize()
        {
            LookupTable = new Dictionary<string, List<string>>();
            CommandLookup = new CommandLookupTable();
            SkillLookup = new SkillLookupTable();
            SpellLookup = new SpellLookupTable();
        }

        public void AddLookup(string table, string entry)
        {
            if (!LookupTable.ContainsKey(table.ToLower()))
                LookupTable[table.ToLower()] = new List<string>();

            List<string> lookups = LookupTable[table.ToLower()];
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
            IEnumerable<string> lookups = GetLookups(table);
            if (lookups.Any() && lookups.Count() > index)
                return lookups.ElementAt(index);
            return string.Empty;
        }
    }
}
