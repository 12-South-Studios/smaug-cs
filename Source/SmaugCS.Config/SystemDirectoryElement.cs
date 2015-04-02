using System.Configuration;

namespace SmaugCS.Config
{
    public class SystemDirectoryElement : ConfigurationElement
    {
        [ConfigurationProperty("Name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return this["Name"] as string; }
        }

        [ConfigurationProperty("Path", IsRequired = true)]
        public string Path
        {
            get { return this["Path"] as string; }
        }
    }
}
