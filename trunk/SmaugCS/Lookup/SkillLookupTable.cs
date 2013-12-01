using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using Realm.Library.Common.Extensions;

namespace SmaugCS.Lookup
{
    public static class SkillLookupTable
    {
        private static readonly Dictionary<string, Action<CharacterInstance, string>> SkillFunctions =
            new Dictionary<string, Action<CharacterInstance, string>>();

        public static Action<CharacterInstance, string> GetSkillFunction(string name)
        {
            return SkillFunctions.ContainsKey(name.ToLower())
                       ? SkillFunctions[name.ToLower()]
                       : SkillNotfound;
        }

        public static void SkillNotfound(CharacterInstance ch, string argument)
        {
            // TODO: send_to_char("Huh?\r\n", ch);
        }

        public static void UpdateSkillFunctionReferences(IEnumerable<SkillData> skills)
        {
            foreach (SkillData skill in skills.Where(x => !x.SkillFunctionName.IsNullOrEmpty()))
            {
                if (skill.SkillFunction == null)
                    skill.SkillFunction = new DoFunction();

                skill.SkillFunction.Value = GetSkillFunction(skill.SkillFunctionName);
            }
        }
    }
}
