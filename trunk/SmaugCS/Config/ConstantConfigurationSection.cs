using System.Configuration;

namespace SmaugCS.Config
{
    public class ConstantConfigurationSection : ConfigurationSection
    {
        public static ConstantConfigurationSection GetConfig()
        {
            return (ConstantConfigurationSection)ConfigurationManager.GetSection("ConstantConfigurationSection") ??
                   new ConstantConfigurationSection();
        }

        [ConfigurationProperty("ConstantCollection")]
        [ConfigurationCollection(typeof(ConstantCollection), AddItemName = "Constant")]
        public ConstantCollection Constants
        {
            get
            {
                return this["ConstantCollection"] as ConstantCollection;
            }
        }
    }
}
