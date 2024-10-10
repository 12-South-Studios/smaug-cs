using System.Configuration;

namespace SmaugCS.Config;

public class VnumConfigurationSection : ConfigurationSection
{
    [ConfigurationProperty("RoomVnums")]
    public VnumElementCollection RoomVnums => (VnumElementCollection)this["RoomVnums"];

    [ConfigurationProperty("MobileVnums")]
    public VnumElementCollection MobileVnums => (VnumElementCollection)this["MobileVnums"];

    [ConfigurationProperty("ObjectVnums")]
    public VnumElementCollection ObjectVnums => (VnumElementCollection)this["ObjectVnums"];
}