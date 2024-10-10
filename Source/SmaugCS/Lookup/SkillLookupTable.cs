using System.Collections.Generic;
using System.Linq;
using Library.Common.Extensions;
using SmaugCS.Data;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Lookup;

public class SkillLookupTable : LookupBase<SkillData, DoFunction>
{
  public SkillLookupTable()
    : base(new DoFunction { Value = (ch, arg) => ch.SendTo("Huh?") })
  {
    // TODO Add skills here
  }

  public override void UpdateFunctionReferences(IEnumerable<SkillData> values)
  {
    foreach (SkillData skill in values.Where(x => !x.SkillFunctionName.IsNullOrEmpty()))
    {
      skill.SkillFunction ??= new DoFunction();

      skill.SkillFunction = GetFunction(skill.SkillFunctionName);
    }
  }
}