using System.Xml.Serialization;

namespace SmaugCS.Language
{
    [XmlRoot("LanguageConversion")]
    public class LanguageConversionData
    {
        [XmlElement]
        public string OldValue { get; set; }

        [XmlElement]
        public string NewValue { get; set; }

        public LanguageConversionData() { }

        public LanguageConversionData(string line)
        {
            string[] words = line.Split(' ');
            OldValue = words[0].Trim('\'');
            NewValue = words[1].Trim('\'');
        }
    }
}
