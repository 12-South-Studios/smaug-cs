using System.Configuration;

namespace SmaugCS.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class VnumConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("RoomVnums")]
        public VnumElementCollection RoomVnums
        {
            get { return (VnumElementCollection)this["RoomVnums"]; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("MobileVnums")]
        public VnumElementCollection MobileVnums
        {
            get { return (VnumElementCollection)this["MobileVnums"]; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("ObjectVnums")]
        public VnumElementCollection ObjectVnums
        {
            get { return (VnumElementCollection)this["ObjectVnums"]; }
        }
    }
}
