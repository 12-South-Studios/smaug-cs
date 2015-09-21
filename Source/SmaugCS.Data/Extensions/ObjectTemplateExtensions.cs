using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Templates;
using NumberExtensions = SmaugCS.Common.NumberExtensions;

namespace SmaugCS.Data.Extensions
{
    public static class ObjectTemplateExtensions
    {
        public static IEnumerable<WearLocations> GetWearFlags(this ObjectTemplate template)
        {
            if (template.WearFlags.IsNullOrEmpty())
                return new List<WearLocations>();

            var words = template.WearFlags.Split(' ');
            return words.Select(EnumerationExtensions.GetEnum<WearLocations>).ToList();
        }

        public static void SetExtraFlags(this ObjectTemplate template, string extraFlags)
        {
            var words = template.Flags.Split(' ');
            foreach (var word in words)
            {
                NumberExtensions.SetBit(template.ExtraFlags, EnumerationExtensions.GetEnum<ItemExtraFlags>(word));
            }
        }
    }
}
