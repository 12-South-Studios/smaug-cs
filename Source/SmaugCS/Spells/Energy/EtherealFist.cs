using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Spells.Energy;

class EtherealFist
{
  public static ReturnTypes spell_ethereal_fist(int sn, int level, CharacterInstance ch, object vo)
  {
    CharacterInstance victim = (CharacterInstance)vo;

    level = 0.GetHighestOfTwoNumbers(level);
    level = 35.GetLowestOfTwoNumbers(level);

    int dam = (int)(1.3 * (level * SmaugRandom.Between(1, 6) - 31));

    if (victim.SavingThrows.CheckSaveVsSpellStaff(level, victim))
      dam /= 4;
    
    return ch.CauseDamageTo(victim, dam, sn);
  }
}