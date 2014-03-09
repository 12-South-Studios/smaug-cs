using System.Collections.Generic;

// ReSharper disable CheckNamespace
namespace SmaugCS
// ReSharper restore CheckNamespace
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

        private static readonly Dictionary<string, int> LevelMap = new Dictionary<string, int>()
            {
                 {"hero", -15},
                 {"immortal", -14},
                 {"supreme", 0},
                 {"infinite", -1},
                 {"eternal", -2},
                 {"implementor", -3},
                 {"sub implementor", -4},
                 {"ascendant", -5},
                 {"greater", -6},
                 {"god", -7},
                 {"lesser", -8},
                 {"true immortal", -9},
                 {"demi", -10},
                 {"savior", -11},
                 {"creator", -12},
                 {"acolyte", -13},
                 {"neophyte", -14},
                 {"avatar", -15}

            };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int GetLevel(string type)
        {
            return LevelMap.ContainsKey(type.ToLower()) ? MaxLevel + LevelMap[type.ToLower()] : MaxLevel;
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
