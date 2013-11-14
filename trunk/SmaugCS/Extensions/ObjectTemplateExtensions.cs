using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Templates;
using Realm.Library.Common.Extensions;

namespace SmaugCS.Extensions
{
    public static class ObjectTemplateExtensions
    {
        public static IEnumerable<WearLocations> GetWearFlags(this ObjectTemplate template)
        {
            if (template.WearFlags.IsNullOrEmpty())
                return new List<WearLocations>();

            string[] words = template.WearFlags.Split(new[] {' '});
            return words.Select(EnumerationExtensions.GetEnum<WearLocations>).ToList();
        }

        public static void SetExtraFlags(this ObjectTemplate template, string extraFlags)
        {
            string[] words = template.Flags.Split(new[] {' '});
            foreach (string word in words)
            {
                template.ExtraFlags.SetBit((int) EnumerationExtensions.GetEnum<ItemExtraFlags>(word));
            }
        }
    }
}
