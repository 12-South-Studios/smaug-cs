using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Realm.Library.Common;

namespace SmaugCS.Common
{
    public static class NumberExtensions
    {
        public static string ToPunctuation(this int value) => value.ToString("#,#", CultureInfo.InvariantCulture);

        public static string ToPercent(this int value, int total) => ((double)value).ToPercent(total);

        public static string ToPercent(this double value, double total) => $"{value/total:0.00%}";

        public static string GetFlagString(this int value, IEnumerable<string> flags)
        {
            string flagString = string.Empty;
            string[] flagArray = flags.ToArray();

            for (int x = 0; x < 32; ++x)
            {
                if (!value.IsSet(1 << x)) continue;

                if (string.IsNullOrWhiteSpace(flagArray[x]))
                    continue;

                flagString += flagArray[x];
            }

            return flagString;
        }

        #region IsSet
        public static bool IsSet(this int value, int bit) => (value & bit) > 0;

        public static bool IsSet(this int value, Enum bit)
        {
            if (!bit.GetType().GetCustomAttributes(typeof (FlagsAttribute), true).Any()) return false;
            int bitValue = bit.GetValue();
            return (value & (bitValue == 0 ? Convert.ToInt32(bit) : bitValue)) > 0;
        }

        public static bool IsSet(this long value, long bit) => (value & bit) > 0;

        public static bool IsSet(this long value, Enum bit)
        {
            if (!bit.GetType().GetCustomAttributes(typeof(FlagsAttribute), true).Any()) return false;
            int bitValue = bit.GetValue();
            return (value & (bitValue == 0 ? Convert.ToInt64(bit) : bitValue)) > 0;
        }
        #endregion

        #region SetBit
        public static int SetBit(this int value, int bit)
        {
            value = value | bit;
            return value;
        }

        public static int SetBit(this int value, Enum bit)
        {
            if (!bit.GetType().GetCustomAttributes(typeof(FlagsAttribute), true).Any()) return 0;
            int bitValue = bit.GetValue();
            value = value | (bitValue == 0 ? Convert.ToInt32(bit) : bitValue);
            return value;
        }

        public static long SetBit(this long value, Enum bit)
        {
            if (!bit.GetType().GetCustomAttributes(typeof(FlagsAttribute), true).Any()) return 0;
            int bitValue = bit.GetValue();
            value = value | (bitValue == 0 ? Convert.ToInt64(bit) : bitValue);
            return value;
        }

        public static long SetBit(this long value, long bit)
        {
            value = value | bit;
            return value;
        }
        #endregion

        #region RemoveBit
        public static int RemoveBit(this int value, int bit) => value &= ~bit;

        public static int RemoveBit(this int value, Enum bit)
        {
            if (!bit.GetType().GetCustomAttributes(typeof(FlagsAttribute), true).Any()) return 0;
            int bitValue = bit.GetValue();
            return value &= ~(bitValue == 0 ? Convert.ToInt32(bit) : bitValue); 
        }

        public static long RemoveBit(this long value, long bit) => value &= ~bit;

        public static long RemoveBit(this long value, Enum bit)
        {
            if (!bit.GetType().GetCustomAttributes(typeof(FlagsAttribute), true).Any()) return 0;
            long bitValue = bit.GetValue();
            return value &= ~(bitValue == 0 ? Convert.ToInt64(bit) : bitValue);
        }
        #endregion

        #region ToggleBit
        public static int ToggleBit(this int value, int bit) => value ^= bit;

        public static int ToggleBit(this int value, Enum bit)
        {
            if (!bit.GetType().GetCustomAttributes(typeof(FlagsAttribute), true).Any()) return 0;
            int bitValue = bit.GetValue();
            return value ^= bitValue == 0 ? Convert.ToInt32(bit) : bitValue;  
        }
        #endregion

        public static int Interpolate(this int level, int value00, int value32) => value00 + level * (value32 - value00) / 32;

        public static int GetLowestOfTwoNumbers(this int numberToCheck, int numberToCompareAgainst)
            => numberToCheck < numberToCompareAgainst ? numberToCheck : numberToCompareAgainst;

        public static long GetLowestOfTwoNumbers(this long numberToCheck, long numberToCompareAgainst)
            => numberToCheck < numberToCompareAgainst ? numberToCheck : numberToCompareAgainst;

        public static int GetHighestOfTwoNumbers(this int numberToCheck, int numberToCompareAgainst)
            => numberToCheck > numberToCompareAgainst ? numberToCheck : numberToCompareAgainst;

        public static long GetHighestOfTwoNumbers(this long numberToCheck, long numberToCompareAgainst)
            => numberToCheck > numberToCompareAgainst ? numberToCheck : numberToCompareAgainst;

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

        public static int GetAbsoluteDifference(this int value1, int value2) => Math.Abs(value1 - value2);
    }
}
