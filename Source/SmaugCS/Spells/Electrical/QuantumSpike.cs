using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Spells.Electrical;

class QuantumSpike
{
  public static ReturnTypes spell_quantum_spike(int sn, int level, CharacterInstance ch, object vo)
  {
    CharacterInstance victim = (CharacterInstance)vo;

    level = 0.GetHighestOfTwoNumbers(level);
    int dam = (int)(1.3 * (2 * (level / 10) * SmaugRandom.Between(1, 40) + 145));

    if (victim.SavingThrows.CheckSaveVsBreath(level, victim))
      dam /= 2;

    return ch.CauseDamageTo(victim, dam, sn);
  }
}