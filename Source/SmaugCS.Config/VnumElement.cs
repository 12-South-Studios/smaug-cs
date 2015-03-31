using System.Configuration;

namespace SmaugCS.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class VnumElement : ConfigurationElement
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
        [ConfigurationProperty("Value", IsRequired = true)]
        public string Value
        {
            get { return (string)this["Value"]; }
        }
    }
}
