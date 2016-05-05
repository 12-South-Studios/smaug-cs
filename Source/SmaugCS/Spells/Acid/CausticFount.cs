using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Spells.Acid
{
    public static class CausticFount
    {
        public static ReturnTypes spell_caustic_fount(int sn, int level, CharacterInstance ch, object vo)
        {
            var victim = (CharacterInstance)vo;

            var lvl = 0.GetHighestOfTwoNumbers(level);
            lvl = 42.GetLowestOfTwoNumbers(lvl);
            var damage = (int) (1.3f*(2*lvl*SmaugRandom.D6() - 31));
            damage = 0.GetHighestOfTwoNumbers(damage);

            if (victim.SavingThrows.CheckSaveVsSpellStaff(lvl, victim))
                damage = damage*1/2;
            return ch.CauseDamageTo(victim, damage, sn);
        }
    }
}
