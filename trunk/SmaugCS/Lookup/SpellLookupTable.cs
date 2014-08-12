using System.Collections.Generic;
using System.Linq;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using Realm.Library.Common;

namespace SmaugCS.Lookup
{
    public class SpellLookupTable : LookupBase<SkillData, SpellFunction>
    {
        public SpellLookupTable()
            : base(new SpellFunction {Value = (id, level, ch, vo) =>
                {
                    color.send_to_char("That's not a spell!", ch);
                    return ReturnTypes.None;
                }})
        {
            LookupTable.Add("spell_smaug", new SpellFunction {Value = Spells.Smaug.Smaug.spell_smaug});

            // TODO Add spells here
        }

        public override void UpdateFunctionReferences(IEnumerable<SkillData> values)
        {
            foreach (SkillData skill in values.Where(x => !x.SpellFunctionName.IsNullOrEmpty()))
            {
                if (skill.SpellFunction == null)
                    skill.SpellFunction = new SpellFunction();

                skill.SpellFunction = GetFunction(skill.SpellFunctionName);
            }
        }
    }
}
