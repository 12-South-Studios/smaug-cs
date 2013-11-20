using System;
using System.Collections.Generic;
using System.Linq;

namespace SmaugCS.Common
{
    public static class NumberExtensions
    {
        public static string ToPercent(this int value, int total)
        {
            return ((double)value).ToPercent(total);
        }

        public static string ToPercent(this double value, double total)
        {
            return String.Format("{0:0.00%}", value / total);
        }

        public static string GetFlagString(this int value, IEnumerable<string> flags)
        {
            string flagString = string.Empty;
            string[] flagArray = flags.ToArray();

            for (int x = 0; x < 32; ++x)
            {
                if (value.IsSet(1 << x))
                {
                    if (string.IsNullOrWhiteSpace(flagArray[x]))
                        continue;

                    flagString += flagArray[x];
                }
            }

            return flagString;
        }

        public static bool IsSet(this int value, int bit)
        {
            return (value & bit) > 0;
        }

        public static bool IsSet(this uint value, int bit)
        {
            return (value & bit) > 0;
        }

        public static int SetBit(this int value, int bit)
        {
            return value |= bit;
        }

        public static uint SetBit(this uint value, int bit)
        {
            return value |= (uint)bit;
        }

        public static int RemoveBit(this int value, int bit)
        {
            return value &= ~bit;
        }

        public static uint RemoveBit(this uint value, int bit)
        {
            return value &= ~(uint)bit;
        }

        public static int ToggleBit(this int value, int bit)
        {
            return value ^= bit;
        }

        public static uint ToggleBit(this uint value, int bit)
        {
            return value ^= (uint)bit;
        }

        public static int Interpolate(this int level, int value_00, int value_32)
        {
            return value_00 + level * (value_32 - value_00) / 32;
        }

        public static int GetLowestOfTwoNumbers(this int numberToCheck, int numberToCompareAgainst)
        {
            return numberToCheck < numberToCompareAgainst ? numberToCheck : numberToCompareAgainst;
        }

        public static long GetLowestOfTwoNumbers(this long numberToCheck, long numberToCompareAgainst)
        {
            return numberToCheck < numberToCompareAgainst ? numberToCheck : numberToCompareAgainst;
        }

        public static int GetHighestOfTwoNumbers(this int numberToCheck, int numberToCompareAgainst)
        {
            return numberToCheck > numberToCompareAgainst ? numberToCheck : numberToCompareAgainst;
        }

        public static long GetHighestOfTwoNumbers(this long numberToCheck, long numberToCompareAgainst)
        {
            return numberToCheck > numberToCompareAgainst ? numberToCheck : numberToCompareAgainst;
        }

        public static int GetNumberThatIsBetween(this int numberToCheck, int numberThatIsMinimumValue, int numberThatIsMaximumValue)
        {
            if (numberToCheck < numberThatIsMinimumValue) return numberThatIsMinimumValue;
            return numberToCheck > numberThatIsMaximumValue ? numberThatIsMaximumValue : numberToCheck;
        }

        public static long GetNumberThatIsBetween(this long numberToCheck, long numberThatIsMinimumValue, long numberThatIsMaximumValue)
        {
            if (numberToCheck < numberThatIsMinimumValue) return numberThatIsMinimumValue;
            return numberToCheck > numberThatIsMaximumValue ? numberThatIsMaximumValue : numberToCheck;
        }
    }
}
