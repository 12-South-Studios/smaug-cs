using System.Configuration;

namespace SmaugCS.Constants.Config
{
    public class VnumConfigurationSection : ConfigurationSection
    {
        public static VnumConfigurationSection GetConfig()
        {
            return (VnumConfigurationSection)ConfigurationManager.GetSection("VnumConfigurationSection") ??
                   new VnumConfigurationSection();
        }

        [ConfigurationProperty("VnumCollection")]
        [ConfigurationCollection(typeof(VnumCollection), AddItemName = "Vnum")]
        public VnumCollection Vnums
        {
            get
            {
                return this["VnumCollection"] as VnumCollection;
            }
        }
    }
}
