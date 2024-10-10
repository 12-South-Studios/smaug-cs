using System.Configuration;

namespace SmaugCS.Config;

public class MayorTextStringElement : ConfigurationElement
{
    [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
    public string Name => this["name"] as string;

    [ConfigurationProperty("value", IsRequired = true)]
    public string Value => this["value"] as string;
}