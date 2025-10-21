using Library.Common.Extensions;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Templates;
using System.Collections.Generic;
using System.Linq;

namespace SmaugCS.Data.Extensions;

public static class ObjectTemplateExtensions
{
  public static IEnumerable<WearLocations> GetWearFlags(this ObjectTemplate template)
  {
    if (template.WearFlags.IsNullOrEmpty())
      return [];

    string[] words = template.WearFlags.Split(' ');
    return words.Select(EnumerationExtensions.GetEnum<WearLocations>).ToList();
  }

  public static void SetExtraFlags(this ObjectTemplate template)
  {
    string[] words = template.Flags.Split(' ');
    foreach (string word in words)
    {
      template.ExtraFlags.SetBit((int)EnumerationExtensions.GetEnum<ItemExtraFlags>(word));
    }
  }
}