using System;
using System.Collections.Generic;
using System.Linq;

namespace SmaugCS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class NumberExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static string ToPercent(this int value, int total)
        {
            return ((double)value).ToPercent(total);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static string ToPercent(this double value, double total)
        {
            return String.Format("{0:0.00%}", value / total);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bit"></param>
        /// <returns></returns>
        public static bool IsSet(this int value, int bit)
        {
            return (value & bit) > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bit"></param>
        /// <returns></returns>
        public static bool IsSet(this uint value, int bit)
        {
            return (value & bit) > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bit"></param>
        /// <returns></returns>
        public static int SetBit(this int value, int bit)
        {
            return value |= bit;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bit"></param>
        /// <returns></returns>
        public static uint SetBit(this uint value, int bit)
        {
            return value |= (uint)bit;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bit"></param>
        /// <returns></returns>
        public static int RemoveBit(this int value, int bit)
        {
            return value &= ~bit;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bit"></param>
        /// <returns></returns>
        public static uint RemoveBit(this uint value, int bit)
        {
            return value &= ~(uint)bit;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bit"></param>
        /// <returns></returns>
        public static int ToggleBit(this int value, int bit)
        {
            return value ^= bit;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bit"></param>
        /// <returns></returns>
        public static uint ToggleBit(this uint value, int bit)
        {
            return value ^= (uint)bit;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <param name="value00"></param>
        /// <param name="value32"></param>
        /// <returns></returns>
        public static int Interpolate(this int level, int value00, int value32)
        {
            return value00 + level * (value32 - value00) / 32;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberToCheck"></param>
        /// <param name="numberToCompareAgainst"></param>
        /// <returns></returns>
        public static int GetLowestOfTwoNumbers(this int numberToCheck, int numberToCompareAgainst)
        {
            return numberToCheck < numberToCompareAgainst ? numberToCheck : numberToCompareAgainst;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberToCheck"></param>
        /// <param name="numberToCompareAgainst"></param>
        /// <returns></returns>
        public static long GetLowestOfTwoNumbers(this long numberToCheck, long numberToCompareAgainst)
        {
            return numberToCheck < numberToCompareAgainst ? numberToCheck : numberToCompareAgainst;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberToCheck"></param>
        /// <param name="numberToCompareAgainst"></param>
        /// <returns></returns>
        public static int GetHighestOfTwoNumbers(this int numberToCheck, int numberToCompareAgainst)
        {
            return numberToCheck > numberToCompareAgainst ? numberToCheck : numberToCompareAgainst;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberToCheck"></param>
        /// <param name="numberToCompareAgainst"></param>
        /// <returns></returns>
        public static long GetHighestOfTwoNumbers(this long numberToCheck, long numberToCompareAgainst)
        {
            return numberToCheck > numberToCompareAgainst ? numberToCheck : numberToCompareAgainst;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberToCheck"></param>
        /// <param name="numberThatIsMinimumValue"></param>
        /// <param name="numberThatIsMaximumValue"></param>
        /// <returns></returns>
        public static int GetNumberThatIsBetween(this int numberToCheck, int numberThatIsMinimumValue, int numberThatIsMaximumValue)
        {
            if (numberToCheck < numberThatIsMinimumValue) return numberThatIsMinimumValue;
            return numberToCheck > numberThatIsMaximumValue ? numberThatIsMaximumValue : numberToCheck;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberToCheck"></param>
        /// <param name="numberThatIsMinimumValue"></param>
        /// <param name="numberThatIsMaximumValue"></param>
        /// <returns></returns>
        public static long GetNumberThatIsBetween(this long numberToCheck, long numberThatIsMinimumValue, long numberThatIsMaximumValue)
        {
            if (numberToCheck < numberThatIsMinimumValue) return numberThatIsMinimumValue;
            return numberToCheck > numberThatIsMaximumValue ? numberThatIsMaximumValue : numberToCheck;
        }
    }
}
