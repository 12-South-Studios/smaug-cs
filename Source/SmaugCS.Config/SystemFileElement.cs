using System.Configuration;

namespace SmaugCS.Config;

public class SystemFileElement : ConfigurationElement
{
    [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
    public string Name => (string)this["name"];

    [ConfigurationProperty("Filename", IsRequired = true)]
    public string Filename => (string)this["Filename"];

    [ConfigurationProperty("UseSystemFolder", IsRequired = false)]
    public bool UseSystemFolder => (bool)this["UseSystemFolder"];
}