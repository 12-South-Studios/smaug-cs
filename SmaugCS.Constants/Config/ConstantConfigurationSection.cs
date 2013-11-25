using System.Configuration;

namespace SmaugCS.Constants.Config
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
