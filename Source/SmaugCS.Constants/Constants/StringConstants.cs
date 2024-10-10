using Library.Common.Extensions;
using SmaugCS.Config;
using System.Linq;

namespace SmaugCS.Constants.Constants;

public static class StringConstants
{
  private static StaticStringConfigurationSection _section;

  private static MayorTextStringElement GetTextString(string elementName)
  {
    _section ??= ConfigurationManagerFunctions.GetSection<StaticStringConfigurationSection>("StaticStringSection");
    return _section.MayorTextStrings.Cast<MayorTextStringElement>()
      .FirstOrDefault(element => element.Name.EqualsIgnoreCase(elementName));
  }

  public static string GetMayorText(string name)
  {
    MayorTextStringElement element = GetTextString(name);
    return element != null ? element.Value : string.Empty;
  }
}