using System.Configuration;

namespace SmaugCS.Config
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface")]
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
