using System.Configuration;

namespace SmaugCS.Constants.Config
{
    public class Constant : ConfigurationElement
    {
        private const string ATTRIBUTE_KEY = "key";
        private const string ATTRIBUTE_VALUE = "value";

        [ConfigurationProperty(ATTRIBUTE_KEY, IsRequired = true, IsKey = true)]
        public string Key
        {
            get { return (string)this[ATTRIBUTE_KEY]; }
            set { this[ATTRIBUTE_KEY] = value; }
        }

        [ConfigurationProperty(ATTRIBUTE_VALUE, IsRequired = true, IsKey = false)]
        public string Value
        {
            get { return (string)this[ATTRIBUTE_VALUE]; }
            set { this[ATTRIBUTE_VALUE] = value; }
        }
    }
}
