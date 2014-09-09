using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;

namespace SmaugCS.Spells
{
    public static class SulfurousSpray
    {
        public static ReturnTypes spell_sulfurous_spray(int sn, int level, CharacterInstance ch, object vo)
        {
            CharacterInstance victim = (CharacterInstance)vo;

            int lvl = 0.GetHighestOfTwoNumbers(level);
            lvl = 19.GetLowestOfTwoNumbers(lvl);
            int damage = (int) (1.3f*(2*lvl*SmaugRandom.Roll(7, 1) + 11));

            if (victim.SavingThrows.CheckSaveVsSpellStaff(lvl, victim))
                damage /= 4;
            return ch.CauseDamageTo(victim, damage, sn);
        }
    }
}
