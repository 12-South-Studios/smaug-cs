using System;
using System.Collections.Generic;
using SmaugCS.Common;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Interfaces;

namespace SmaugCS.SpecFuns
{
    public sealed class SpecFunHandler : ISpecFunHandler
    {
        private readonly IDatabaseManager _dbManager;

        public SpecFunHandler(IDatabaseManager dbManager)
        {
            _dbManager = dbManager;
        }

        private static readonly Dictionary<string, Func<MobileInstance, bool>> SpecialFuncLookupTable =
            new Dictionary<string, Func<MobileInstance, bool>>
                {
                    {"spec_breath_any", BreathAny.DoSpecBreathAny},
                    {"spec_breath_acid", BreathAcid.DoSpecBreathAcid},
                    {"spec_breath_fire", BreathFire.DoSpecBreathFire},
                    {"spec_breath_frost", BreathFrost.DoSpecBreathFrost},
                    {"spec_breath_gas", BreathGas.DoSpecBreathGas},
                    {"spec_breath_lightning", BreathLightning.DoSpecBreathLightning},
                    {"spec_cast_adept", CastAdept.DoSpecCastAdept},
                    {"spec_cast_cleric", CastCleric.DoSpecCastCleric},
                    {"spec_cast_mage", CastMage.DoSpecCastMage},
                    {"spec_cast_undead", CastUndead.DoSpecCastUndead},
                    {"spec_executioner", Executioner.DoSpecExecutioner},
                    {"spec_fido", Fido.DoSpecFido},
                    {"spec_guard", Guard.DoSpecGuard},
                    {"spec_janitor", Janitor.DoSpecJanitor},
                    {"spec_mayor", Mayor.DoSpecMayor},
                    {"spec_poison", Poison.DoSpecPoison},
                    {"spec_thief", Thief.DoSpecThief},
                    {"spec_wanderer", Wanderer.DoSpecWanderer}
                };

        public static Func<MobileInstance, bool> GetSpecFunReference(string name)
        {
            return SpecialFuncLookupTable.ContainsKey(name.ToLower())
                       ? SpecialFuncLookupTable[name.ToLower()]
                       : null;
        }

        public bool IsValidSpecFun(string name)
        {
            return _dbManager.GetEntity<SpecialFunction>(name) != null &&
                   SpecialFuncLookupTable.ContainsKey(name.ToLower());
        }

        public SpecialFunction GetSpecFun(string name)
        {
            return IsValidSpecFun(name)
                       ? _dbManager.GetEntity<SpecialFunction>(name)
                       : null;
        }

        public SkillData PickSpell(Dictionary<int, Tuple<int, string>> lookupTable, int characterLevel)
        {
            int minLevel = 0;
            string spellName = string.Empty;

            while (minLevel < characterLevel)
            {
                int bits = SmaugRandom.Bits(4);
                if (lookupTable.ContainsKey(bits))
                {
                    minLevel = lookupTable[bits].Item1;
                    spellName = lookupTable[bits].Item2;
                }
                else
                {
                    minLevel = lookupTable[-1].Item1;
                    spellName = lookupTable[-1].Item2;
                }
            }

            return _dbManager.GetEntity<SkillData>(spellName);
        }
    }
}
