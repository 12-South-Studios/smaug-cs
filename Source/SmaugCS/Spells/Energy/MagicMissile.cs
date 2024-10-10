using System.Collections.Generic;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Spells.Energy;

class MagicMissile
{
  private static readonly List<int> DamageValues = 
  [
    0,
    3, 3, 4, 4, 5, 6, 6, 6, 6, 6,
    7, 7, 7, 7, 7, 8, 8, 8, 8, 8,
    9, 9, 9, 9, 9, 10, 10, 10, 10, 10,
    11, 11, 11, 11, 11, 12, 12, 12, 12, 12,
    13, 13, 13, 13, 13, 14, 14, 14, 14, 14,
    15, 15, 15, 15, 15, 16, 16, 16, 16, 16,
    17, 17, 17, 17, 17, 18, 18, 18, 18, 18
  ];

  public static ReturnTypes spell_magic_missile(int sn, int level, CharacterInstance ch, object vo)
  {
    CharacterInstance victim = (CharacterInstance)vo;
    
    level = level.GetLowestOfTwoNumbers(DamageValues.Count);
    level = 0.GetHighestOfTwoNumbers(level);

    int dam = SmaugRandom.Between(DamageValues[level] / 2, DamageValues[level] * 2);
    
    return ch.CauseDamageTo(victim, dam, sn);
  }
}