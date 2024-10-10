using SmaugCS.Commands;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;

namespace SmaugCS.Spells.Life;

class Harm
{
  public static ReturnTypes spell_harm(int sn, int level, CharacterInstance ch, object vo)
  {
    CharacterInstance victim = (CharacterInstance)vo;
    SkillData skill = Program.RepositoryManager.SKILLS.Get(sn);

    if (CheckFunctions.CheckIfTrueCasting(victim.Immunity.IsSet(ResistanceTypes.Magic), 
          skill, ch, CastingFunctionType.Immune, victim, null))
      return ReturnTypes.SpellFailed;

    int dam = 20.GetHighestOfTwoNumbers(victim.CurrentHealth - SmaugRandom.D4(1));
    if (victim.SavingThrows.CheckSaveVsSpellStaff(level, victim))
      dam = 50.GetLowestOfTwoNumbers(dam / 4);

    dam = 100.GetLowestOfTwoNumbers(dam);
    
    return ch.CauseDamageTo(victim, dam, sn);
  }
}