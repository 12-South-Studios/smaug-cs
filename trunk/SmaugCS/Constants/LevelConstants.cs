using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmaugCS
{
    public static class LevelConstants
    {
        public static int MAX_LEVEL
        {
            get { return GameConstants.GetIntegerConstant("MaximumLevel"); }
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

        public static int GetLevel(string type)
        {
            return LevelMap.ContainsKey(type.ToLower()) ? MAX_LEVEL - LevelMap[type.ToLower()] : MAX_LEVEL;
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
