using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Spells.Energy;

class Disruption
{
  public static ReturnTypes spell_disruption(int sn, int level, CharacterInstance ch, object vo)
  {
    CharacterInstance victim = (CharacterInstance)vo;

    level = 0.GetHighestOfTwoNumbers(level);
    level = 14.GetLowestOfTwoNumbers(level);

    int dam = (int)(1.3 * (level * SmaugRandom.Between(1, 6) + 8));

    if (victim.SavingThrows.CheckSaveVsSpellStaff(level, victim))
      dam = 0;
    
    return ch.CauseDamageTo(victim, dam, sn);
  }
}