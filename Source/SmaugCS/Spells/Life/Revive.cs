using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Spells.Life;

class Revive
{
  public static ReturnTypes spell_revive(int sn, int level, CharacterInstance ch, object vo)
  {
    CharacterInstance victim = (CharacterInstance)vo;

    level = 0.GetHighestOfTwoNumbers(level);
    int dam = SmaugRandom.Between(0, 0);

    if (victim.SavingThrows.CheckSaveVsSpellStaff(level, victim))
      dam /= 2;
    
    return ch.CauseDamageTo(victim, dam, sn);
  }
}