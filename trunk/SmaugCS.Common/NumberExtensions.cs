using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Realm.Library.Common;

namespace SmaugCS.Common
{
    public static class NumberExtensions
    {
        public static string ToPunctuation(this int value)
        {
            return value.ToString("#,#", CultureInfo.InvariantCulture);
        }

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
                if (!value.IsSet(1 << x)) continue;

                if (string.IsNullOrWhiteSpace(flagArray[x]))
                    continue;

                flagString += flagArray[x];
            }

            return flagString;
        }

        #region IsSet
        public static bool IsSet(this int value, int bit)
        {
            return (value & bit) > 0;
        }

        public static bool IsSet(this int value, Enum bit)
        {
            if (!bit.GetType().GetCustomAttributes(typeof (FlagsAttribute), true).Any()) return false;
            int bitValue = bit.GetValue();
            return (value & (bitValue == 0 ? Convert.ToInt32(bit) : bitValue)) > 0;
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
        #endregion

        #region RemoveBit
        public static int RemoveBit(this int value, int bit)
        {
            return value &= ~bit;
        }

        public static int RemoveBit(this int value, Enum bit)
        {
            if (!bit.GetType().GetCustomAttributes(typeof(FlagsAttribute), true).Any()) return 0;
            int bitValue = bit.GetValue();
            return value &= ~(bitValue == 0 ? Convert.ToInt32(bit) : bitValue); 
        }
        #endregion

        #region ToggleBit
        public static int ToggleBit(this int value, int bit)
        {
            return value ^= bit;
        }

        public static int ToggleBit(this int value, Enum bit)
        {
            if (!bit.GetType().GetCustomAttributes(typeof(FlagsAttribute), true).Any()) return 0;
            int bitValue = bit.GetValue();
            return value ^= (bitValue == 0 ? Convert.ToInt32(bit) : bitValue);  
        }
        #endregion

        public static int Interpolate(this int level, int value00, int value32)
        {
            return value00 + level * (value32 - value00) / 32;
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

        public static int GetAbsoluteDifference(this int value1, int value2)
        {
            return Math.Abs(value1 - value2);
        }
    }
}
