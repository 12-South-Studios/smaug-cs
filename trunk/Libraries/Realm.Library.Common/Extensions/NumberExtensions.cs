using System;
using Realm.Library.Common.Properties;

// ReSharper disable CheckNamespace
namespace Realm.Library.Common
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Static class for some number extension functions
    /// </summary>
    public static class NumberExtensions
    {
        private static readonly string[] unitsMap = new[]
            {
                Resources.MSG_ZERO, Resources.MSG_ONE, Resources.MSG_TWO, Resources.MSG_THREE,
                Resources.MSG_FOUR, Resources.MSG_FIVE, Resources.MSG_SIX, Resources.MSG_SEVEN, Resources.MSG_EIGHT,
                Resources.MSG_NINE, Resources.MSG_TEN, Resources.MSG_ELEVEN, Resources.MSG_TWELVE,
                Resources.MSG_THIRTEEN,
                Resources.MSG_FOURTEEN, Resources.MSG_FIFTEEN, Resources.MSG_SIXTEEN, Resources.MSG_SEVENTEEN,
                Resources.MSG_EIGHTEEN, Resources.MSG_NINETEEN
            };

        private static readonly string[] tensMap = new[]
            {
                Resources.MSG_ZERO, Resources.MSG_TEN, Resources.MSG_TWENTY, Resources.MSG_THIRTY,
                Resources.MSG_FORTY, Resources.MSG_FIFTY, Resources.MSG_SIXTY, Resources.MSG_SEVENTY,
                Resources.MSG_EIGHTY, Resources.MSG_NINETY
            };

        private static readonly string[] hourMap = new[]
            {
                Resources.MSG_ZERO, Resources.MSG_ONE, Resources.MSG_TWO, Resources.MSG_THREE,
                Resources.MSG_FOUR, Resources.MSG_FIVE, Resources.MSG_SIX, Resources.MSG_SEVEN,
                Resources.MSG_EIGHT, Resources.MSG_NINE, Resources.MSG_TEN, Resources.MSG_ELEVEN,
                Resources.MSG_TWELVE
            };

        /// <summary>
        /// Verifies if the given string is an integer
        /// </summary>
        /// <param name="value">String to check</param>
        /// <returns>Returns true if the string is an integer</returns>
        public static bool IsNumeric(this object value)
        {
            double result;
            return value.IsNotNull() && Double.TryParse(value.ToString(), out result);
        }

        /// <summary>
        /// Converts the given number into string form (words)
        /// </summary>
        /// <param name="value">Number to convert</param>
        /// <returns>Returns a string value representing the number in words</returns>
        public static string ToWords(this int value)
        {
            Validation.Validate<ArgumentOutOfRangeException>(value >= Int32.MinValue && value <= Int32.MaxValue);

            string returnVal;
            if (value == 0)
            {
                returnVal = Resources.MSG_ZERO;
            }
            else if (value < 0)
            {
                returnVal = string.Format("{0} {1}", Resources.MSG_MINUS, ToWords(Math.Abs(value)));
            }
            else
            {
                var words = string.Empty;
                if ((value / 1000000) > 0)
                {
                    words += String.Format("{0} {1} ", ToWords(value / 1000000), Resources.MSG_MILLION);
                    value %= 1000000;
                }

                if ((value / 1000) > 0)
                {
                    words += String.Format("{0} {1} ", ToWords(value / 1000), Resources.MSG_THOUSAND);
                    value %= 1000;
                }

                if ((value / 100) > 0)
                {
                    words += String.Format("{0} {1} ", ToWords(value / 100), Resources.MSG_HUNDRED);
                    value %= 100;
                }

                if (value > 0)
                {
                    if (!string.IsNullOrEmpty(words))
                        words += Resources.MSG_AND + " ";

                    if (value < 20)
                        words += unitsMap[value];
                    else
                    {
                        words += tensMap[value / 10];
                        if ((value % 10) > 0)
                            words += "-" + unitsMap[value % 10];
                    }
                }

                returnVal = words;
            }

            return returnVal;
        }

        /// <summary>
        /// Converts the numerical hour to a string
        /// </summary>
        /// <param name="hour"></param>
        /// <returns></returns>
        public static string ConvertHour(this int hour)
        {
            Validation.Validate(hour > 0 && hour <= 24, Resources.ERR_INVALID_HOUR);

            return (hour > 12) ? hourMap[hour - 12] : hourMap[hour];
        }

        /// <summary>
        /// Converts a numerical hour to a period of the day
        /// </summary>
        /// <param name="hour"></param>
        /// <returns></returns>
        public static string ToPeriodOfDay(this int hour)
        {
            Validation.Validate<ArgumentOutOfRangeException>(hour >= Int32.MinValue && hour <= Int32.MaxValue);

            if (hour > 21 || hour < 5)
                return Resources.MSG_NIGHT;
            if (hour < 12)
                return Resources.MSG_MORNING;
            return hour < 17 ? Resources.MSG_AFTERNOON : Resources.MSG_EVENING;
        }

        /// <summary>
        /// Appends an ordinal value to the number and convers it to a string
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string GetOrdinal(this int number)
        {
            Validation.Validate<ArgumentOutOfRangeException>(number >= Int32.MinValue && number <= Int32.MaxValue);

            var suf = "th";
            if (((number % 100) / 10) != 1)
            {
                switch (number % 10)
                {
                    case 1:
                        suf = "st";
                        break;

                    case 2:
                        suf = "nd";
                        break;

                    case 3:
                        suf = "rd";
                        break;
                }
            }
            return number + suf;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="inclusive"></param>
        /// <returns></returns>
        public static bool InRange(this Int32 value, Int32 min, Int32 max, bool inclusive = false)
        {
            return inclusive ? value >= min && value <= max : value > min && value < max;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="inclusive"></param>
        /// <returns></returns>
        public static bool InRange(this Int64 value, Int64 min, Int64 max, bool inclusive = false)
        {
            return inclusive ? value >= min && value <= max : value > min && value < max;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="inclusive"></param>
        /// <returns></returns>
        public static bool InRange(this Double value, Double min, Double max, bool inclusive = false)
        {
            return inclusive ? value >= min && value <= max : value > min && value < max;
        }
    }
}