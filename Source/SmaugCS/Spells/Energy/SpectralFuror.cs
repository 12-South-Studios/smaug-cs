using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Spells.Energy;

class SpectralFuror
{
  public static ReturnTypes spell_spectral_furor(int sn, int level, CharacterInstance ch, object vo)
  {
    CharacterInstance victim = (CharacterInstance)vo;

    level = 0.GetHighestOfTwoNumbers(level);
    level = 16.GetLowestOfTwoNumbers(level);

    int dam = (int)(1.3 * (level * SmaugRandom.Between(1, 7) + 7));

    if (victim.SavingThrows.CheckSaveVsSpellStaff(level, victim))
      dam /= 2;
    
    return ch.CauseDamageTo(victim, dam, sn);
  }
}