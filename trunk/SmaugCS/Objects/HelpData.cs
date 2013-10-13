﻿using System.Xml.Serialization;

namespace SmaugCS.Objects
{
    [XmlRoot("Help")]
    public class HelpData
    {
        [XmlElement]
        public int Level { get; set; }

        [XmlElement]
        public string Keyword { get; set; }

        [XmlElement]
        public string Text { get; set; }
    }
}
