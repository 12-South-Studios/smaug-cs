using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Spells
{
    public static class BlackHand
    {
        public static ReturnTypes spell_black_hand(int sn, int level, CharacterInstance ch, object vo)
        {
            var lvl = 0.GetHighestOfTwoNumbers(level);
            lvl = 5.GetLowestOfTwoNumbers(lvl);

            var dam = (int)(1.3f * (lvl * SmaugRandom.D6() + 3));

            var victim = (CharacterInstance)vo;
            if (victim.SavingThrows.CheckSaveVsSpellStaff(lvl, victim))
                dam /= 4;

            return ch.CauseDamageTo(victim, dam, sn);
        }
    }
}
