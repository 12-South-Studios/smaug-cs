using System.Configuration;

namespace SmaugCS.Config
{
    public class SystemValueElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SystemValueElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SystemValueElement)element).Name;
        }
    }
}
