using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace SmaugCS.Constants.Config
{
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
