﻿using System.Configuration;

namespace SmaugCS.Config;

public class VnumElement : ConfigurationElement
{
    [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
    public string Name => (string)this["name"];

    [ConfigurationProperty("value", IsRequired = true)]
    public string Value => (string)this["value"];
}