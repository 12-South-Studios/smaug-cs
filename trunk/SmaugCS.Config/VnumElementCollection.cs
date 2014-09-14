using System.Configuration;

namespace SmaugCS.Config
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface")]
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
