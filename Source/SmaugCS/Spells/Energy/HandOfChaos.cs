using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Spells.Energy;

class HandOfChaos
{
  public static ReturnTypes spell_hand_of_chaos(int sn, int level, CharacterInstance ch, object vo)
  {
    CharacterInstance victim = (CharacterInstance)vo;

    level = 0.GetHighestOfTwoNumbers(level);
    level = 18.GetLowestOfTwoNumbers(level);

    int dam = (int)(1.3 * (level * SmaugRandom.Between(1, 7) + 9));

    if (victim.SavingThrows.CheckSaveVsSpellStaff(level, victim))
      dam /= 4;
    
    return ch.CauseDamageTo(victim, dam, sn);
  }
}