using System.Collections.Generic;
using System.Linq;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using Realm.Library.Common;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Lookup
{
    public class SpellLookupTable : LookupBase<SkillData, SpellFunction>
    {
        public SpellLookupTable()
            : base(new SpellFunction {Value = (id, level, ch, vo) =>
                {
                    ch.SendTo("That's not a spell!");
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
