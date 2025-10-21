using Library.Common.Extensions;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Constants.Constants;

public static class LevelConstants
{
  private static int? _maxLevel;

  public static int MaxLevel
  {
    get
    {
      _maxLevel ??= GameConstants.GetConstant<int>("MaximumLevel");
      return _maxLevel.Value;
    }
    set => _maxLevel = value;
  }

  public static int GetLevel(ImmortalTypes type)
  {
    return MaxLevel - type.GetValue();
  }

  public static int ImmortalLevel => GetLevel(ImmortalTypes.Immortal);
  public static int AvatarLevel => GetLevel(ImmortalTypes.Avatar);
}