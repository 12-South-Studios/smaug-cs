using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Spells.Electrical;

class GalvanicWhip
{
  public static ReturnTypes spell_galvanic_whip(int sn, int level, CharacterInstance ch, object vo)
  {
    CharacterInstance victim = (CharacterInstance)vo;

    level = 0.GetHighestOfTwoNumbers(level);
    level = 10.GetLowestOfTwoNumbers(level);

    int dam = (int)(1.3 * (level * SmaugRandom.Between(1, 6) + 5));

    if (victim.SavingThrows.CheckSaveVsSpellStaff(level, victim))
      dam /= 2;
    
    return ch.CauseDamageTo(victim, dam, sn);
  }
}