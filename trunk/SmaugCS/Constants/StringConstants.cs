using System.Linq;
using Realm.Library.Common;
using SmaugCS.Constants.Config;

namespace SmaugCS.Constants
{
    public static class StringConstants
    {
        private static MayorTextStringElement GetTextString(string elementName)
        {
            var section =
                ConfigurationManagerFunctions.GetSection<StaticStringConfigurationSection>("StaticStringSection");
            return
                section.MayorTextStrings.Cast<MayorTextStringElement>()
                    .FirstOrDefault(element => element.Name.EqualsIgnoreCase(elementName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetMayorText(string name)
        {
            var element = GetTextString(name);
            return element != null ? element.Value : string.Empty;
        }
    }
}
