using System.Configuration;

namespace SmaugCS.Config
{
    public class ConstantCollection : ConfigurationElementCollection
    {
        public Constant this[int index]
        {
            get
            {
                return base.BaseGet(index) as Constant;
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        public new Constant this[string responseString]
        {
            get { return (Constant)BaseGet(responseString); }
            set
            {
                if (BaseGet(responseString) != null)
                {
                    BaseRemoveAt(BaseIndexOf(BaseGet(responseString)));
                }
                BaseAdd(value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new Constant();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Constant)element).Key;
        }
    }
}
