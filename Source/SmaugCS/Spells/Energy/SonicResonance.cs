using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Spells.Energy;

class SonicResonance
{
  public static ReturnTypes spell_sonic_resonance(int sn, int level, CharacterInstance ch, object vo)
  {
    CharacterInstance victim = (CharacterInstance)vo;

    level = 0.GetHighestOfTwoNumbers(level);
    level = 23.GetLowestOfTwoNumbers(level);

    int dam = (int)(1.3 * (level * SmaugRandom.Between(1, 8)));

    if (victim.SavingThrows.CheckSaveVsSpellStaff(level, victim))
      dam = dam * 3/4;
    
    return ch.CauseDamageTo(victim, dam, sn);
  }
}