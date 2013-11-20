using System;
using System.Configuration;

namespace SmaugCS.Constants.Config
{
    [ConfigurationCollection(typeof(Constant))]
    public class ConstantCollection : ConfigurationElementCollection
    {
        private const string PropertyName = "Constant";

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMapAlternate; }
        }

        protected override string ElementName
        {
            get { return PropertyName; }
        }

        protected override bool IsElementName(string elementName)
        {
            return elementName.Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool IsReadOnly()
        {
            return false;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new Constant();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Constant)(element)).Key;
        }

        public Constant this[int idx]
        {
            get { return (Constant)BaseGet(idx); }
        }

        public Constant this[string key]
        {
            get { return (Constant)BaseGet(key); }
        }
    }
}
