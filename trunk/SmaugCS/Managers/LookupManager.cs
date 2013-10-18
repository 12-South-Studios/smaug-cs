using System.Collections.Generic;
using Realm.Library.Common.Objects;
using SmaugCS.Enums;

namespace SmaugCS.Managers
{
    public sealed class LookupManager : GameSingleton
    {
        private static LookupManager _instance;
        private static readonly object Padlock = new object();

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
    }
}
