using System.Configuration;

namespace SmaugCS.Config;

public static class ConfigurationManagerFunctions
{
  public static T GetSection<T>(string sectionName) where T : ConfigurationSection
  {
    object section = ConfigurationManager.GetSection(sectionName);
    if (section != null)
      return (T)section;

    throw new ConfigurationSectionNotFoundException("Section {0} was not found", sectionName);
  }
}