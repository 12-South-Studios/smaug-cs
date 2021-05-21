using System.Configuration;

namespace SmaugCS.Config
{
    public class SystemDataConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("SystemDirectories")]
        public SystemDirectoryElementCollection SystemDirectories => (SystemDirectoryElementCollection)this["SystemDirectories"];

        [ConfigurationProperty("SystemFiles")]
        public SystemFileElementCollectionn SystemFiles => (SystemFileElementCollectionn)this["SystemFiles"];

        [ConfigurationProperty("SystemValues")]
        public SystemValueElementCollection SystemValues => (SystemValueElementCollection)this["SystemValues"];
    }
}
