using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Spells.Life;

class CauseCritical
{
  public static ReturnTypes spell_cause_critical(int sn, int level, CharacterInstance ch, object vo)
  {
    CharacterInstance victim = (CharacterInstance)vo;
    int damage = SmaugRandom.D8(3) + level - 6;

    return ch.CauseDamageTo(victim, damage, sn);
  }
}