using System.Configuration;

namespace SmaugCS.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemDirectoryElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SystemDirectoryElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SystemDirectoryElement)element).Name;
        }
    }
}
