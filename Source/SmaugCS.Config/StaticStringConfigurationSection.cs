using System.Configuration;

namespace SmaugCS.Config
{
    public class StaticStringConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("MayorTextStrings")]
        public MayorTextStringsElementCollection MayorTextStrings
        {
            get { return (MayorTextStringsElementCollection)this["MayorTextStrings"]; }
        }
    }
}
