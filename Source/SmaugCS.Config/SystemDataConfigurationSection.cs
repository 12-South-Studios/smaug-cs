using System.Configuration;

namespace SmaugCS.Config
{
    public class SystemDataConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("SystemDirectories")]
        public SystemDirectoryElementCollection SystemDirectories
        {
            get { return (SystemDirectoryElementCollection)this["SystemDirectories"]; }
        }

        [ConfigurationProperty("SystemFiles")]
        public SystemFileElementCollectionn SystemFiles
        {
            get { return (SystemFileElementCollectionn) this["SystemFiles"]; }
        }

        [ConfigurationProperty("SystemValues")]
        public SystemValueElementCollection SystemValues
        {
            get { return (SystemValueElementCollection)this["SystemValues"]; }
        }
    }
}
