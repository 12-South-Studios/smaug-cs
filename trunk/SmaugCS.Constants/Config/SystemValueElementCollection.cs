﻿using System.Configuration;

namespace SmaugCS.Constants.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemValueElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SystemValueElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SystemValueElement)element).Name;
        }
    }
}
