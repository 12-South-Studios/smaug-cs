using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;

namespace SmaugCS.Spells
{
    public static class BlackLightning
    {
        public static ReturnTypes spell_black_lightning(int sn, int level, CharacterInstance ch, object vo)
        {
            var lvl = 0.GetHighestOfTwoNumbers(level);
            lvl = 10.GetLowestOfTwoNumbers(lvl);

            var dam = (int)(1.3f * (lvl * SmaugRandom.Roll(1, 50) + 135));

            var victim = (CharacterInstance)vo;
            if (victim.SavingThrows.CheckSaveVsSpellStaff(lvl, victim))
                dam /= 4;

            return ch.CauseDamageTo(victim, dam, sn);
        }
    }
}
