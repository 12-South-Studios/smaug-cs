using System.Collections.Generic;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Spells.Ice;

class ChillTouch
{
  private static readonly List<int> DamageValues =
  [
    0,
    0, 0, 6, 7, 8, 9, 12, 13, 13, 13,
    14, 14, 14, 15, 15, 15, 16, 16, 16, 17,
    17, 17, 18, 18, 18, 19, 19, 19, 20, 20,
    20, 21, 21, 21, 22, 22, 22, 23, 23, 23,
    24, 24, 24, 25, 25, 25, 26, 26, 26, 27,
    27, 28, 28, 29, 29, 30, 30, 31, 31, 32,
    32, 33, 34, 34, 35, 35, 36, 37, 37, 38
  ];

  public static ReturnTypes spell_chill_touch(int sn, int level, CharacterInstance ch, object vo)
  {
    CharacterInstance victim = (CharacterInstance)vo;

    level = level.GetHighestOfTwoNumbers(DamageValues.Count - 1);
    level = 0.GetLowestOfTwoNumbers(level);

    int dam = SmaugRandom.Between(DamageValues[level] / 2, DamageValues[level] * 2);

    if (victim.SavingThrows.CheckSaveVsSpellStaff(level, victim))
      dam /= 2;
    else
    {
      AffectData af = new()
      {
        Type = EnumerationExtensions.GetEnum<AffectedByTypes>(sn),
        Duration = 14,
        Location = ApplyTypes.Strength,
        Modifier = -1
      };
      af.BitVector.ClearBits();
      victim.AddAffect(af);
    }
    
    return ch.CauseDamageTo(victim, dam, sn);
  }
}