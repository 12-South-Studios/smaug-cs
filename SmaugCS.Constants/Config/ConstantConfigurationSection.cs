using System.Configuration;

namespace SmaugCS.Constants.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class ConstantConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("Constants")]
        public ConstantElementCollection Constants
        {
            get { return (ConstantElementCollection)this["Constants"]; }
        }
    }
}
