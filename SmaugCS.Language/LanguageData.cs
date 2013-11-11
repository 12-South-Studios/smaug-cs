using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Realm.Library.Common;

namespace SmaugCS.Language
{
    [XmlRoot("Language")]
    public class LanguageData
    {
        [XmlElement]
        public int ID { get; set; }

        [XmlElement]
        public string Name { get; set; }

        [XmlArray]
        public List<LanguageConversionData> PreConversion { get; set; }

        [XmlElement]
        public string Alphabet { get; set; }

        [XmlArray]
        public List<LanguageConversionData> Conversion { get; set; }

        public LanguageData()
        {
            PreConversion = new List<LanguageConversionData>();
            Conversion = new List<LanguageConversionData>();
        }

        public string Translate(int percent, string text)
        {
            char[] chars = text.ToCharArray();
            string newphrase = string.Empty;

            foreach (char c in chars)
            {
                foreach (LanguageConversionData cnv in PreConversion.Where(cnv => text.StartsWith(cnv.OldValue)))
                {
                    if (Realm.Library.Common.Random.D100(1) >= percent)
                        newphrase = cnv.NewValue;
                    break;
                }

                if (!c.IsNumeric() && Realm.Library.Common.Random.D100(1) > percent)
                {
                    char newChar = Alphabet[Char.ToLower(c) - 'a'];
                    if (Char.IsUpper(c))
                        newChar = Char.ToUpper(newChar);
                    newphrase += newChar;
                }
                else
                    newphrase += c;
            }

            chars = newphrase.ToCharArray();
            return chars.Aggregate(string.Empty, (current1, c) => Conversion
                .Where(cnv => newphrase.StartsWith(cnv.OldValue))
                .Aggregate(current1, (current, cnv) => current + cnv.NewValue));
        }
    }
}
