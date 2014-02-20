using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common.Objects;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Interfaces;

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

        public static Dictionary<SpellDamageTypes, ResistanceTypes> SpellDamageToResistanceTypeMap = new Dictionary
            <SpellDamageTypes, ResistanceTypes>()
            {
                {SpellDamageTypes.Fire, ResistanceTypes.Fire},
                {SpellDamageTypes.Cold, ResistanceTypes.Cold},
                {SpellDamageTypes.Electricty, ResistanceTypes.Electricity},
                {SpellDamageTypes.Energy, ResistanceTypes.Energy},
                {SpellDamageTypes.Acid, ResistanceTypes.Acid},
                {SpellDamageTypes.Poison, ResistanceTypes.Poison},
                {SpellDamageTypes.Drain, ResistanceTypes.Drain}
            };

        public ResistanceTypes GetResistanceType(SpellDamageTypes type)
        {
            return SpellDamageToResistanceTypeMap.ContainsKey(type)
                       ? SpellDamageToResistanceTypeMap[type]
                       : ResistanceTypes.Unknown;
        }

        public void Initialize()
        {
            LookupTable = new Dictionary<string, List<string>>();
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
