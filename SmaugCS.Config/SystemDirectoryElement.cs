using System.Configuration;

namespace SmaugCS.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemDirectoryElement : ConfigurationElement
    {
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("Name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return this["Name"] as string; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("Path", IsRequired = true)]
        public string Path
        {
            get { return this["Path"] as string; }
        }
    }
}
