using System.Configuration;

namespace SmaugCS.Constants.Config
{
    public class ConstantConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("Constants")]
        public ConstantCollection Constants
        {
            get { return ((ConstantCollection)(base["Constants"])); }
            set { base["Constants"] = value; }
        }
    }
}
