using System.Configuration;

namespace SmaugCS.Config;

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