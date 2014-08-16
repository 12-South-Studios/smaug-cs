using System.Configuration;

namespace SmaugCS.Config
{
    public class ConstantConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("Constants")]
        public ConstantElementCollection Constants
        {
            get { return (ConstantElementCollection)this["Constants"]; }
        }
    }
}
