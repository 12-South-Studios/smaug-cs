using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Spells.Electrical;

class MagneticThrust
{
  public static ReturnTypes spell_magnetic_thrust(int sn, int level, CharacterInstance ch, object vo)
  {
    CharacterInstance victim = (CharacterInstance)vo;

    level = 0.GetHighestOfTwoNumbers(level);
    level = 29.GetLowestOfTwoNumbers(level);

    int dam = (int)(0.65 * (5 * level * SmaugRandom.Between(1, 6) + 16));

    if (victim.SavingThrows.CheckSaveVsSpellStaff(level, victim))
      dam /= 3;
    
    return ch.CauseDamageTo(victim, dam, sn);
  }
}