using System.Configuration;

namespace SmaugCS.Config
{
    public class SystemDirectoryElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name => this["name"] as string;

        [ConfigurationProperty("Path", IsRequired = true)]
        public string Path => this["Path"] as string;
    }
}
