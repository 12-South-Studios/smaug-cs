using System.Configuration;

namespace SmaugCS.Config
{
    public class VnumConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("RoomVnums")]
        public VnumElementCollection RoomVnums
        {
            get { return (VnumElementCollection)this["RoomVnums"]; }
        }

        [ConfigurationProperty("MobileVnums")]
        public VnumElementCollection MobileVnums
        {
            get { return (VnumElementCollection)this["MobileVnums"]; }
        }

        [ConfigurationProperty("ObjectVnums")]
        public VnumElementCollection ObjectVnums
        {
            get { return (VnumElementCollection)this["ObjectVnums"]; }
        }
    }
}
