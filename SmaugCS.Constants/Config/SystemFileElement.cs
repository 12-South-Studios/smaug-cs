using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmaugCS.Constants.Config
{
    public class SystemFileElement : ConfigurationElement
    {
        [ConfigurationProperty("Name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["Name"]; }
        }

        [ConfigurationProperty("Filename", IsRequired = true)]
        public string Filename
        {
            get { return (string)this["Filename"]; }
        }
        
        [ConfigurationProperty("UseSystemFolder", IsRequired = false)]
        public bool UseSystemFolder
        {
            get { return (bool)this["UseSystemFolder"]; }
        }
    }
}
