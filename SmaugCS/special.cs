using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmaugCS.Enums;
using SmaugCS.Objects;

namespace SmaugCS
{
    public static class special
    {
        public static void free_specfuns()
        {
            db.SPEC_LIST.Clear();
        }

        public static void load_specfuns()
        {
            // TODO
        }

        public static bool validate_spec_fun(string name)
        {
            return db.SPEC_LIST.Any(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public static SpecialFunction spec_lookup(string name)
        {
            return db.SPEC_LIST.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public static void summon_if_hating(CharacterInstance ch)
        {
            // TODO
        }

        public static bool dragon(CharacterInstance ch, string fspell_name)
        {
            // TODO
            return false;
        }

        public static bool spec_breath_any(CharacterInstance ch)
        {
            if (ch.Position != PositionTypes.Fighting
                && ch.Position != PositionTypes.Evasive
                && ch.Position != PositionTypes.Defensive
                && ch.Position != PositionTypes.Aggressive
                && ch.Position != PositionTypes.Berserk)
                return false;

            switch (SmaugCS.Common.SmaugRandom.Bits(3))
            {
                case 0:
                    return dragon(ch, "fire breath");
                case 1:
                case 2:
                    return dragon(ch, "lightning breath");
                case 3:
                    return spec_breath_gas(ch);
                case 4:
                    return dragon(ch, "acid breath");
                default:
                    return dragon(ch, "frost breath");
            }
        }

        public static bool spec_breath_gas(CharacterInstance ch)
        {
            // TODO
            return false;
        }

        public static bool spec_cast_adept(CharacterInstance ch)
        {
            // tODO
            return false;
        }

        public static bool spec_cast_cleric(CharacterInstance ch)
        {
            // tODO
            return false;
        }

        public static bool spec_cast_mage(CharacterInstance ch)
        {
            // tODO
            return false;
        }

        public static bool spec_cast_undead(CharacterInstance ch)
        {
            // tODO
            return false;
        }

        public static bool spec_executioner(CharacterInstance ch)
        {
            // TODO
            return false;
        }

        public static bool spec_fido(CharacterInstance ch)
        {
            // TODO
            return false;
        }

        public static bool spec_guard(CharacterInstance ch)
        {
            // TODO
            return false;
        }

        public static bool spec_janitor(CharacterInstance ch)
        {
            // TODO
            return false;
        }

        public static bool spec_mayor(CharacterInstance ch)
        {
            // TODO
            return false;
        }

        public static bool spec_poison(CharacterInstance ch)
        {
            // TODO
            return false;
        }

        public static bool spec_thief(CharacterInstance ch)
        {
            // TODO
            return false;
        }

        public static bool spec_wanderer(CharacterInstance ch)
        {
            // TODO
            return false;
        }
    }
}
