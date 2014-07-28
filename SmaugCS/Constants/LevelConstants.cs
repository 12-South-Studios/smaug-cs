using SmaugCS.Constants.Enums;
using System;
using System.Collections.Generic;

namespace SmaugCS.Constants
{
    public static class LevelConstants
    {
        private static int? _maxLevel;
        /// <summary>
        /// 
        /// </summary>
        public static int MaxLevel
        {
            get
            {
                return _maxLevel == null
                           ? _maxLevel.GetValueOrDefault(GameConstants.GetIntegerConstant("MaximumLevel"))
                           : _maxLevel.Value;
            }
            set { _maxLevel = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int GetLevel(string type)
        {
            Enum enumType = Realm.Library.Common.EnumerationFunctions.GetEnumByName<ImmortalTypes>(type);
            int value = Realm.Library.Common.EnumerationExtensions.GetValue(enumType);
            return MaxLevel - value;
        }

        public static int LEVEL_LOG
        {
            get { return GetLevel("lesser"); }
        }

        public static int LEVEL_HIGOD
        {
            get { return GetLevel("god"); }
        }
    }
}
