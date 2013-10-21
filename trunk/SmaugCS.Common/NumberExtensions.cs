using System;

namespace SmaugCS.Common
{
    public static class NumberExtensions
    {
        /// <summary>
        /// Converts the given value to a percentage
        /// </summary>
        /// <param name="value"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static string ToPercent(this int value, int total)
        {
            return ((double)value).ToPercent(total);
        }

        /// <summary>
        /// Converts the given value to a percentage
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
        /// <param name="bitvector"></param>
        /// <param name="flagarray"></param>
        /// <returns></returns>
        public static string GetFlagString(this int bitvector, string[] flagarray)
        {
            string flags = string.Empty;

            for (int x = 0; x < 32; ++x)
            {
                if (bitvector.IsSet(1 << x))
                {
                    if (string.IsNullOrWhiteSpace(flagarray[x]))
                        continue;

                    flags += flagarray[x];
                }
            }

            return flags;
        }

        /// <summary>
        /// Gets if the integer field contains the given bit
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
        /// Sets a given bit on an integer field
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bit"></param>
        /// <returns></returns>
        public static int SetBit(this int value, int bit)
        {
            return value | bit;
        }


        /// <summary>
        /// Sets a given bit on an integer field
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bit"></param>
        /// <returns></returns>
        public static uint SetBit(this uint value, int bit)
        {
            return value | (uint)bit;
        }

        /// <summary>
        /// Removes a given bit from an integer field
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bit"></param>
        /// <returns></returns>
        public static int RemoveBit(this int value, int bit)
        {
            return value & ~bit;
        }

        /// <summary>
        /// Removes a given bit from an integer field
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bit"></param>
        /// <returns></returns>
        public static uint RemoveBit(this uint value, int bit)
        {
            return value & ~(uint)bit;
        }

        /// <summary>
        /// Toggles the given bit on the integer field
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bit"></param>
        /// <returns></returns>
        public static int ToggleBit(this int value, int bit)
        {
            return value ^ bit;
        }

        /// <summary>
        /// Toggles the given bit on the integer field
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bit"></param>
        /// <returns></returns>
        public static uint ToggleBit(this uint value, int bit)
        {
            return value ^ (uint)bit;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <param name="value_00"></param>
        /// <param name="value_32"></param>
        /// <returns></returns>
        public static int Interpolate(this int level, int value_00, int value_32)
        {
            return value_00 + level * (value_32 - value_00) / 32;
        }
    }
}
