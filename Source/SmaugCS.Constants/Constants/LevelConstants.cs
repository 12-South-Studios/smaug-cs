using Realm.Library.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Constants
{
    public static class LevelConstants
    {
        private static int? _maxLevel;

        public static int MaxLevel
        {
            get
            {
                if (_maxLevel == null)
                    _maxLevel = GameConstants.GetConstant<int>("MaximumLevel");
                return _maxLevel.Value;
            }
            internal set { _maxLevel = value; }
        }

        public static int GetLevel(ImmortalTypes type)
        {
            return MaxLevel - type.GetValue();
        }

        public static int ImmortalLevel => GetLevel(ImmortalTypes.Immortal);
        public static int AvatarLevel => GetLevel(ImmortalTypes.Avatar);
    }
}
