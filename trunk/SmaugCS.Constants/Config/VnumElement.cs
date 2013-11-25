using System.Configuration;

namespace SmaugCS.Constants.Config
{
    public class VnumElement : ConfigurationElement
    {
        [ConfigurationProperty("Name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["Name"]; }
        }

        [ConfigurationProperty("Value", IsRequired = true)]
        public string Value
        {
            get { return (string)this["Value"]; }
        }
    }
}
