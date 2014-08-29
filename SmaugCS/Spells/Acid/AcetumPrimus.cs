using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Extensions;

namespace SmaugCS.Spells
{
    public static class AcetumPrimus
    {
        public static ReturnTypes spell_acetum_primus(int sn, int level, CharacterInstance ch, object vo)
        {
            CharacterInstance victim = (CharacterInstance) vo;

            int lvl = 0.GetHighestOfTwoNumbers(level);
            int damage = (int) (1.3f*(2*lvl*SmaugRandom.D4() + 7));

            if (victim.SavingThrows.CheckSaveVsSpellStaff(lvl, victim))
                damage = 3*damage/4;
            return ch.CauseDamageTo(victim, damage, sn);
        }
    }
}
