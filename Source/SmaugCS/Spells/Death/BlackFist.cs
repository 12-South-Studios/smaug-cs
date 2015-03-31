using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Spells
{
    public static class BlackFist
    {
        public static ReturnTypes spell_black_fist(int sn, int level, CharacterInstance ch, object vo)
        {
            int lvl = 0.GetHighestOfTwoNumbers(level);
            lvl = 30.GetLowestOfTwoNumbers(lvl);

            int dam = (int) (1.3f*(lvl*SmaugRandom.Between(1, 9) + 4));

            CharacterInstance victim = (CharacterInstance) vo;
            if (victim.SavingThrows.CheckSaveVsSpellStaff(lvl, victim))
                dam /= 4;

            return ch.CauseDamageTo(victim, dam, sn);
        }
    }
}
