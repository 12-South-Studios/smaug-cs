using System.Configuration;

namespace SmaugCS.Config
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface")]
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
