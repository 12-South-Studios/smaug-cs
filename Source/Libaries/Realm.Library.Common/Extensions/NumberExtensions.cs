﻿using Realm.Library.Common.Attributes;
using Realm.Library.Common.Objects;
using System;

namespace Realm.Library.Common.Extensions

{
    /// <summary>
    /// Static class for some number extension functions
    /// </summary>
    public static class NumberExtensions
    {
        private static readonly string[] UnitsMap =
        {
            "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten",
            "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen",
            "nineteen"
        };

        private static readonly string[] TensMap =
        {
            "zero", "ten", "twenty", "thirty", "fourty", "fifty", "sixty", "seventy", "eighty", "ninety"
        };

        private static readonly string[] HourMap =
        {
            "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten",
            "eleven", "twelve"
        };

        /// <summary>
        /// Verifies if the given string is an integer
        /// </summary>
        /// <param name="value">String to check</param>
        /// <returns>Returns true if the string is an integer</returns>
        public static bool IsNumeric(this object value)
        {
            return value.IsNotNull() && double.TryParse(value.ToString(), out _);
        }

        /// <summary>
        /// Converts the given number into string form (words)
        /// </summary>
        /// <param name="value">Number to convert</param>
        /// <returns>Returns a string value representing the number in words</returns>
        public static string ToWords(this int value)
        {
            string returnVal;
            if (value == 0)
                returnVal = "zero";
            else if (value < 0)
                returnVal = $"minus {ToWords(Math.Abs(value))}";
            else
            {
                var words = string.Empty;
                if (value / 1000000 > 0)
                {
                    words += $"{ToWords(value / 1000000)} million ";
                    value %= 1000000;
                }

                if (value / 1000 > 0)
                {
                    words += $"{ToWords(value / 1000)} thousand ";
                    value %= 1000;
                }

                if (value / 100 > 0)
                {
                    words += $"{ToWords(value / 100)} hundred ";
                    value %= 100;
                }

                if (value > 0)
                {
                    if (!string.IsNullOrEmpty(words))
                        words += "and ";

                    if (value < 20)
                        words += UnitsMap[value];
                    else
                    {
                        words += TensMap[value / 10];
                        if (value % 10 > 0)
                            words += "-" + UnitsMap[value % 10];
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
            Validation.Validate(hour > 0 && hour <= 24, "Invalid hour (must be between 0 and 24)");

            return hour > 12 ? HourMap[hour - 12] : HourMap[hour];
        }

        /// <summary>
        /// Converts a numerical hour to a period of the day
        /// </summary>
        /// <param name="hour"></param>
        /// <returns></returns>
        public static string ToPeriodOfDay(this int hour)
        {
            if (hour > 21 || hour < 5)
                return "at night";
            if (hour < 12)
                return "in the morning";
            return hour < 17 ? "in the afternoon" : "in the evening";
        }

        /// <summary>
        /// Appends an ordinal value to the number and convers it to a string
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string GetOrdinal(this int number)
        {
            var suf = "th";
            if (number % 100 / 10 == 1) return number + suf;
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
        public static bool InRange(this int value, int min, int max, bool inclusive = false)
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
        public static bool InRange(this long value, long min, long max, bool inclusive = false)
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
        public static bool InRange(this double value, double min, double max, bool inclusive = false)
        {
            return inclusive ? value >= min && value <= max : value > min && value < max;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public static bool IsEquivalent(this double left, double right,
            DoublePrecisionComparisonTypes comparison = DoublePrecisionComparisonTypes.ThreeDigits)
        {
            var precision = (double)comparison.GetAttributeValue<PrecisionAttribute>("Value");
            return Math.Abs(left - right) < precision;
        }
    }
}