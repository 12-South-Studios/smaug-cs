using System.Configuration;

namespace SmaugCS.Config;

public class StaticStringConfigurationSection : ConfigurationSection
{
    [ConfigurationProperty("MayorTextStrings")]
    public MayorTextStringsElementCollection MayorTextStrings => (MayorTextStringsElementCollection)this["MayorTextStrings"];
}