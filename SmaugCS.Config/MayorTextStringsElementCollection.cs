using System.Configuration;

namespace SmaugCS.Config
{
    public class MayorTextStringsElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new MayorTextStringElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MayorTextStringElement)element).Name;
        }
    }
}
