using System.IO;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;

namespace SmaugCS.Spells
{
    public static class ColourSpray
    {
        private static readonly int[] DamageValues =
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            30, 35, 40, 45, 50, 55, 55, 55, 56, 57,
            58, 58, 59, 60, 61, 61, 62, 63, 64, 64,
            65, 66, 67, 67, 68, 69, 70, 70, 71, 72,
            73, 73, 74, 75, 76, 76, 77, 78, 79, 79,
            80, 80, 81, 82, 82, 83, 83, 84, 85, 85,
            86, 86, 87, 88, 88, 89, 89, 90, 91, 91
        };

        public static ReturnTypes spell_colour_spray(int sn, int level, CharacterInstance ch, object vo)
        {
            CharacterInstance victim = (CharacterInstance) vo;

            int modLevel = level.GetLowestOfTwoNumbers(((DamageValues.Length * 2) / 2) - 1);
            modLevel = modLevel.GetHighestOfTwoNumbers(0);

            if (DamageValues.Length >= modLevel)
                throw new InvalidDataException(string.Format(
                    "ModLevel {0} is larger than the Damage Array size of {1}", modLevel,
                    DamageValues.Length));

            int dam = SmaugRandom.Between(DamageValues[modLevel]/2, DamageValues[modLevel]*2);
            if (victim.SavingThrows.CheckSaveVsSpellStaff(modLevel, victim))
                dam /= 2;

            return ch.CauseDamageTo(victim, dam, sn);
        }
    }
}
