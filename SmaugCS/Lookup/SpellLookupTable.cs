using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using Realm.Library.Common.Extensions;

namespace SmaugCS.Lookup
{
    public static class SpellLookupTable
    {
        private static readonly Dictionary<string, Func<int, int, CharacterInstance, object, ReturnTypes>>
            SpellFunctions = new Dictionary<string, Func<int, int, CharacterInstance, object, ReturnTypes>>()
                {
                    {"spell_smaug", Spells.Smaug.Smaug.spell_smaug}
                };

        public static Func<int, int, CharacterInstance, object, ReturnTypes> GetSpellFunction(string name)
        {
            return SpellFunctions.ContainsKey(name.ToLower())
                       ? SpellFunctions[name.ToLower()]
                       : SpellNotfound;
        }

        public static ReturnTypes SpellNotfound(int sn, int level, CharacterInstance ch, object vo)
        {
            // TODO send_to_char("That's not a spell!\r\n", ch);
            return ReturnTypes.None;
        }

        public static void UpdateSpellFunctionReferences(IEnumerable<SkillData> skills)
        {
            foreach (SkillData skill in skills.Where(x => !x.SpellFunctionName.IsNullOrEmpty()))
            {
                if (skill.SpellFunction == null)
                    skill.SpellFunction = new SpellFunction();

                skill.SpellFunction.Value = GetSpellFunction(skill.SpellFunctionName);
            }
        }
    }
}
