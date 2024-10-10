using System.Collections.Generic;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Spells.Electrical;

class ShockingGrasp
{
  private static readonly List<int> DamageValues = 
  [
    0,
    0, 0, 0, 0, 0, 0, 20, 25, 29, 33,
    36, 39, 39, 39, 40, 40, 41, 41, 42, 42,
    43, 43, 44, 44, 45, 45, 46, 46, 47, 47,
    48, 48, 49, 49, 50, 50, 51, 51, 52, 52,
    53, 53, 54, 54, 55, 55, 56, 56, 57, 57,
    58, 58, 59, 59, 60, 60, 61, 61, 62, 62,
    63, 63, 64, 64, 65, 65, 66, 66, 67, 67
  ];
  
  public static ReturnTypes spell_shocking_grasp(int sn, int level, CharacterInstance ch, object vo)
  {
    CharacterInstance victim = (CharacterInstance)vo;
    
    level = level.GetLowestOfTwoNumbers(DamageValues.Count - 1);
    level = 0.GetHighestOfTwoNumbers(level);

    int dam = SmaugRandom.Between(DamageValues[level] / 2, DamageValues[level] * 2);
    
    if (victim.SavingThrows.CheckSaveVsSpellStaff(level, victim))
      dam /= 2;
    
    return ch.CauseDamageTo(victim, dam, sn);
  }
}