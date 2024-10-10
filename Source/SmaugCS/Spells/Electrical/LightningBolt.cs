using System.Collections.Generic;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Spells.Electrical;

class LightningBolt
{
  private static readonly List<int> DamageValues = 
  [
    0,
    0, 0, 0, 0, 0, 0, 0, 0, 25, 28,
    31, 34, 37, 40, 40, 41, 42, 42, 43, 44,
    44, 45, 46, 46, 47, 48, 48, 49, 50, 50,
    51, 52, 52, 53, 54, 54, 55, 56, 56, 57,
    58, 58, 59, 60, 60, 61, 62, 62, 63, 64,
    64, 65, 65, 66, 66, 67, 68, 68, 69, 69,
    70, 71, 71, 72, 72, 73, 73, 74, 75, 75
  ];
  
  public static ReturnTypes spell_lightning_bolt(int sn, int level, CharacterInstance ch, object vo)
  {
    CharacterInstance victim = (CharacterInstance)vo;
    
    level = level.GetHighestOfTwoNumbers(DamageValues.Count - 1);
    level = 0.GetLowestOfTwoNumbers(level);

    int dam = SmaugRandom.Between(DamageValues[level] / 2, DamageValues[level] * 2);
    
    if (victim.SavingThrows.CheckSaveVsSpellStaff(level, victim))
      dam /= 2;
    
    return ch.CauseDamageTo(victim, dam, sn);
  }
}