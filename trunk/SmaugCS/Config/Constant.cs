using System.Configuration;

namespace SmaugCS.Config
{
    public class Constant : ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get { return this["key"] as string; }
        }

        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get { return this["value"] as string; }
        }
    }
}
