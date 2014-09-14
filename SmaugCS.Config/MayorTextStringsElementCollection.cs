using System.Configuration;

namespace SmaugCS.Config
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface")]
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
