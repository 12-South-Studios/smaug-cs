using System.Configuration;

namespace SmaugCS.Config
{
    public class ConstantConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("Constants")]
        public ConstantElementCollection Constants => (ConstantElementCollection)this["Constants"];
    }
}
