using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmaugCS.Constants.Config
{
    public class SystemDataConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("SystemDirectories")]
        public SystemDirectoryElementCollection SystemDirectories
        {
            get { return (SystemDirectoryElementCollection)this["SystemDirectories"]; }
        }

        [ConfigurationProperty("SystemFiles")]
        public SystemFileElementCollectionn SystemFiles
        {
            get { return (SystemFileElementCollectionn) this["SystemFiles"]; }
        }
    }
}
