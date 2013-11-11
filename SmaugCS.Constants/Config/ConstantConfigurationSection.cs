using System.Configuration;

namespace SmaugCS.Constants.Config
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
