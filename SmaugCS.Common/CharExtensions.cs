using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmaugCS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class CharExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDigit(this char value)
        {
            return Char.IsDigit(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsSpace(this char value)
        {
            return value == '\0';
        }
    }
}
