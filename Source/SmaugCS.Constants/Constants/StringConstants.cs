using System.Linq;
using Realm.Library.Common;
using SmaugCS.Config;

namespace SmaugCS.Constants
{
    public static class StringConstants
    {
        private static StaticStringConfigurationSection _section;

        private static MayorTextStringElement GetTextString(string elementName)
        {
            if (_section == null)
                _section = ConfigurationManagerFunctions.GetSection<StaticStringConfigurationSection>("StaticStringSection");
            return _section.MayorTextStrings.Cast<MayorTextStringElement>()
                .FirstOrDefault(element => element.Name.EqualsIgnoreCase(elementName));
        }

        public static string GetMayorText(string name)
        {
            var element = GetTextString(name);
            return element != null ? element.Value : string.Empty;
        }
    }
}
