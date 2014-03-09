using System.Configuration;

namespace SmaugCS.Constants.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class VnumElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new VnumElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((VnumElement)element).Name;
        }
    }
}
