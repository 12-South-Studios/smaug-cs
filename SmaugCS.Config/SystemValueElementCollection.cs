using System.Configuration;

namespace SmaugCS.Config
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface")]
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
