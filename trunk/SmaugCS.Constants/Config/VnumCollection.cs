using System.Configuration;

namespace SmaugCS.Constants.Config
{
    public class VnumCollection : ConfigurationElementCollection
    {
        public Vnum this[int index]
        {
            get
            {
                return base.BaseGet(index) as Vnum;
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

        public new Vnum this[string responseString]
        {
            get { return (Vnum)BaseGet(responseString); }
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
            return new Vnum();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Vnum)element).Key;
        }
    }
}
