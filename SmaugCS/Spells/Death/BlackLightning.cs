using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;

namespace SmaugCS.Spells
{
    public static class BlackLightning
    {
        public static ReturnTypes spell_black_lightning(int sn, int level, CharacterInstance ch, object vo)
        {
            int lvl = 0.GetHighestOfTwoNumbers(level);
            lvl = 10.GetLowestOfTwoNumbers(lvl);

            int dam = (int)(1.3f * (lvl * SmaugRandom.Roll(1, 50) + 135));

            CharacterInstance victim = (CharacterInstance)vo;
            if (victim.SavingThrows.CheckSaveVsSpellStaff(lvl, victim))
                dam /= 4;

            return ch.CauseDamageTo(victim, dam, sn);
        }
    }
}
