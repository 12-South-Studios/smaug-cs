using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Spells.Electrical;

class LightningBreath
{
  public static ReturnTypes spell_lightning_breath(int sn, int level, CharacterInstance ch, object vo)
  {
    CharacterInstance victim = (CharacterInstance)vo;

    int hpch = 10.GetHighestOfTwoNumbers(ch.CurrentHealth);
    int dam = SmaugRandom.Between(hpch / 16 + 1, hpch / 8);

    if (victim.SavingThrows.CheckSaveVsBreath(level, victim))
      dam /= 2;

    return ch.CauseDamageTo(victim, dam, sn);
  }
}