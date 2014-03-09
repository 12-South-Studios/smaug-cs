using System.Configuration;

namespace SmaugCS.Constants.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemFileElementCollectionn : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SystemFileElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SystemFileElement) element).Name;
        }
    }
}
