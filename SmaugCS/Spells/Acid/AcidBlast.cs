using Realm.Library.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;

namespace SmaugCS.Spells
{
    public static class AcidBlast
    {
        public static ReturnTypes spell_acid_blast(int sn, int level, CharacterInstance ch, object vo)
        {
            CharacterInstance victim = vo.CastAs<CharacterInstance>();

            int damage = Common.SmaugRandom.D6(level);

            if (victim.SavingThrows.CheckSaveVsSpellStaff(level, victim))
                damage /= 2;

            return ch.CauseDamageTo(victim, damage, sn);
        }
    }
}
