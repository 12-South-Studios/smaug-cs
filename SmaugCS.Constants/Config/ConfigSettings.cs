using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace SmaugCS.Constants.Config
{
    public class ConfigSettings
    {
        public ConstantConfigurationSection ConstantsConfiguration
        {
            get { return (ConstantConfigurationSection)ConfigurationManager.GetSection("Constants"); }
        }

        public ConstantCollection ConstantsCollection
        {
            get { return ConstantsConfiguration.Constants; }
        }

        public IEnumerable<Constant> Constants
        {
            get { return ConstantsCollection.Cast<Constant>().Where(constant => constant != null); }
        }
    }
}
