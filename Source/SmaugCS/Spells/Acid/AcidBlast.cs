using Realm.Library.Common.Objects;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Spells.Acid
{
    public static class AcidBlast
    {
        public static ReturnTypes spell_acid_blast(int sn, int level, CharacterInstance ch, object vo)
        {
            var victim = vo.CastAs<CharacterInstance>();

            var damage = SmaugRandom.D6(level);

            if (victim.SavingThrows.CheckSaveVsSpellStaff(level, victim))
                damage /= 2;

            return ch.CauseDamageTo(victim, damage, sn);
        }
    }
}
