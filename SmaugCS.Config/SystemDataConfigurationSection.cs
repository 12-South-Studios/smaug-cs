using System.Configuration;

namespace SmaugCS.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemDataConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("SystemDirectories")]
        public SystemDirectoryElementCollection SystemDirectories
        {
            get { return (SystemDirectoryElementCollection)this["SystemDirectories"]; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("SystemFiles")]
        public SystemFileElementCollectionn SystemFiles
        {
            get { return (SystemFileElementCollectionn) this["SystemFiles"]; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("SystemValues")]
        public SystemValueElementCollection SystemValues
        {
            get { return (SystemValueElementCollection)this["SystemValues"]; }
        }
    }
}
