using System;
using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common.Extensions;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Managers;

namespace SmaugCS
{
    public static class special
    {
        public static Dictionary<string, Func<CharacterInstance, bool>> SpecialFuncLookupTable =
            new Dictionary<string, Func<CharacterInstance, bool>>()
                {
                    {"spec_breath_any", SpecFuns.BreathAny.spec_breath_any},
                    {"spec_breath_acid", SpecFuns.BreathAcid.spec_breath_acid},
                    {"spec_breath_fire", SpecFuns.BreathFire.spec_breath_fire},
                    {"spec_breath_frost", SpecFuns.BreathFrost.spec_breath_frost},
                    {"spec_breath_gas", SpecFuns.BreathGas.spec_breath_gas},
                    {"spec_breath_lightning", SpecFuns.BreathLightning.spec_breath_lightning},
                    {"spec_cast_adept", SpecFuns.CastAdept.spec_cast_adept},
                    {"spec_cast_cleric", SpecFuns.CastCleric.spec_cast_cleric},
                    {"spec_cast_mage", SpecFuns.CastMage.spec_cast_mage},
                    {"spec_cast_undead", SpecFuns.CastUndead.spec_cast_undead},
                    {"spec_executioner", SpecFuns.Executioner.spec_executioner},
                    {"spec_fido", SpecFuns.Fido.spec_fido},
                    {"spec_guard", SpecFuns.Guard.spec_guard},
                    {"spec_janitor", SpecFuns.Janitor.spec_janitor},
                    {"spec_mayor", SpecFuns.Mayor.spec_mayor},
                    {"spec_poison", SpecFuns.Poison.spec_poison},
                    {"spec_thief", SpecFuns.Thief.spec_thief},
                    {"spec_wanderer", SpecFuns.Wanderer.spec_wanderer}
                };

        public static Func<CharacterInstance, bool> GetSpecFunReference(string name)
        {
            return SpecialFuncLookupTable.ContainsKey(name.ToLower())
                       ? SpecialFuncLookupTable[name.ToLower()]
                       : null;
        }

        public static bool IsValidSpecFun(string name)
        {
            return DatabaseManager.Instance.SPEC_FUNS.Any(s => s.Name.EqualsIgnoreCase(name));
        }

        public static SpecialFunction GetSpecFun(string name)
        {
            return DatabaseManager.Instance.SPEC_FUNS.FirstOrDefault(s => s.Name.EqualsIgnoreCase(name));
        }

        public static void summon_if_hating(CharacterInstance ch)
        {
            // TODO
        }
    }
}
