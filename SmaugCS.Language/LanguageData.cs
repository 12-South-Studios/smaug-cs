using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using SmaugCS.Common;

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

        public void AddPreConversion(string part1, string part2)
        {
            PreConversion.Add(new LanguageConversionData {OldValue = part1, NewValue = part2});
        }
        public void AddPostConversion(string part1, string part2)
        {
            Conversion.Add(new LanguageConversionData { OldValue = part1, NewValue = part2 });
        }

        public string Translate(int percent, string text)
        {
            string newPhrase = string.Empty;
            string[] words = text.Split(' ');

            foreach (string word in words)
            {
                string newWord = DoPreConversion(percent, word);
                if (!newWord.Equals(word))
                {
                    newPhrase += newWord + ' ';
                    continue;
                }

                newWord = DoCharacterConversion(percent, newWord);
                newWord = DoPostConversion(percent, newWord);

                newPhrase += newWord + ' ';
            }

            return newPhrase.TrimStart(' ').TrimEnd(' ');
        }

        internal string DoCharacterConversion(int percent, string text)
        {
            string newPhrase = string.Empty;
            char[] chars = text.ToCharArray();

            foreach (char c in chars)
            {
                if (!c.IsNumeric() && !Char.IsPunctuation(c) && !Char.IsSymbol(c) &&
                    Realm.Library.Common.Random.D100(1) > percent)
                    newPhrase += GetAlphabetEquivalent(c);
                else
                    newPhrase += c;
            }

            return newPhrase;
        }

        internal string DoPreConversion(int percent, string text)
        {
            string preConversion = text;
            if (PreConversion != null)
            {
                foreach (LanguageConversionData lcd in PreConversion)
                {
                    if (preConversion.Contains(lcd.OldValue) && Realm.Library.Common.Random.D100(1) >= percent)
                        preConversion = preConversion.Replace(lcd.OldValue, lcd.NewValue);
                }
            }
            return preConversion;
        }

        internal string DoPostConversion(int percent, string text)
        {
            string postConversion = text;
            if (Conversion != null)
            {
                foreach (LanguageConversionData lcd in Conversion)
                {
                    if (postConversion.Contains(lcd.OldValue) && Realm.Library.Common.Random.D100(1) > percent)
                        postConversion = postConversion.Replace(lcd.OldValue, lcd.NewValue);
                }
            }
            return postConversion; 
        }

        private const string EnglishAlphabet = "abcdefghijklmnopqrtsuvwxyz";
        internal char GetAlphabetEquivalent(char englishChar)
        {
            char newChar = Alphabet[EnglishAlphabet.IndexOf(Char.ToLower(englishChar))];
            return Char.IsUpper(englishChar) ? Char.ToUpper(newChar) : newChar;
        }

    }
}
