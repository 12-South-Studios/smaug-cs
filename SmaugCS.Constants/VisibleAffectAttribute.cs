﻿using System;

namespace SmaugCS.Constants
{
    public class VisibleAffectAttribute : Attribute
    {
        public Enums.ATTypes ATType { get; private set; }
        public string Description { get; private set; }

        public VisibleAffectAttribute(Enums.ATTypes atType, string description) 
        {
            ATType = atType;
            Description = description;
        }
    }
}