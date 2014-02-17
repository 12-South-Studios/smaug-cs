using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace SmaugCS.Constants.Config
{
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
