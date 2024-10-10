using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Spells.Life;

class CauseSerious
{
  public static ReturnTypes spell_cause_serious(int sn, int level, CharacterInstance ch, object vo)
  {
    CharacterInstance victim = (CharacterInstance)vo;
    int damage = SmaugRandom.D8(2) + level / 2;

    return ch.CauseDamageTo(victim, damage, sn);
  }
}