using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Spells.Death
{
    public static class BlackFist
    {
        public static ReturnTypes spell_black_fist(int sn, int level, CharacterInstance ch, object vo)
        {
            var lvl = 0.GetHighestOfTwoNumbers(level);
            lvl = 30.GetLowestOfTwoNumbers(lvl);

            var dam = (int)(1.3f * (lvl * SmaugRandom.Between(1, 9) + 4));

            var victim = (CharacterInstance)vo;
            if (victim.SavingThrows.CheckSaveVsSpellStaff(lvl, victim))
                dam /= 4;

            return ch.CauseDamageTo(victim, dam, sn);
        }
    }
}
