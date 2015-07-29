using System.Configuration;

namespace SmaugCS.Config
{
    public class SystemFileElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
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
