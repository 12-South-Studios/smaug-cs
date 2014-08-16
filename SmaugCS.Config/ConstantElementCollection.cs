using System.Configuration;

namespace SmaugCS.Config
{
    public class ConstantElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ConstantElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ConstantElement)element).Name;
        }
    }
}
