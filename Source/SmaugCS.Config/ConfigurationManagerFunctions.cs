using System.Configuration;

namespace SmaugCS.Config
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConfigurationManagerFunctions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public static T GetSection<T>(string sectionName) where T : ConfigurationSection
        {
            var section = ConfigurationManager.GetSection(sectionName);
            if (section != null)
                return (T) section;

            throw new ConfigurationSectionNotFoundException("Section {0} was not found", sectionName);
        }
    }
}
