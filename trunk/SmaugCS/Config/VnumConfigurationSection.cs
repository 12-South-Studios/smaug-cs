using System.Configuration;

namespace SmaugCS.Config
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
