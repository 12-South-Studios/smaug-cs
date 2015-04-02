using System.Configuration;

namespace SmaugCS.Config
{
    public class MayorTextStringElement : ConfigurationElement
    {
        [ConfigurationProperty("Name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return this["Name"] as string; }
        }

        [ConfigurationProperty("Value", IsRequired = true)]
        public string Value
        {
            get { return this["Value"] as string; }
        }
    }
}
