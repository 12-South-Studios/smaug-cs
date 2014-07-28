using System.Configuration;

namespace SmaugCS.Constants.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class StaticStringConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("MayorTextStrings")]
        public MayorTextStringsElementCollection MayorTextStrings
        {
            get { return (MayorTextStringsElementCollection)this["MayorTextStrings"]; }
        }
    }
}
