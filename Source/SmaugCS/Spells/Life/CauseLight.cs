using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Spells.Life;

class CauseLight
{
  public static ReturnTypes spell_cause_light(int sn, int level, CharacterInstance ch, object vo)
  {
    CharacterInstance victim = (CharacterInstance)vo;
    int damage = SmaugRandom.D8(1) + level / 3;

    return ch.CauseDamageTo(victim, damage, sn);
  }
}