using System.Configuration;

namespace SmaugCS.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemFileElement : ConfigurationElement
    {
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("Name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["Name"]; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("Filename", IsRequired = true)]
        public string Filename
        {
            get { return (string)this["Filename"]; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("UseSystemFolder", IsRequired = false)]
        public bool UseSystemFolder
        {
            get { return (bool)this["UseSystemFolder"]; }
        }
    }
}
