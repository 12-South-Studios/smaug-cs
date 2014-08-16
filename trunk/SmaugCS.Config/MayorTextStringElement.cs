using System.Configuration;

namespace SmaugCS.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class MayorTextStringElement : ConfigurationElement
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
        [ConfigurationProperty("Value", IsRequired = true)]
        public string Value
        {
            get { return this["Value"] as string; }
        }
    }
}
