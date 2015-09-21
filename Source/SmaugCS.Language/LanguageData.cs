using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common;

namespace SmaugCS.Language
{
    public class LanguageData : Entity
    {
        private readonly List<LanguageConversionData> _preConversion;
        private readonly List<LanguageConversionData> _conversion;

        public IEnumerable<LanguageConversionData> PreConversion => _preConversion;

        public string Alphabet { get; set; }

        public LanguageTypes Type { get; private set; }

        public IEnumerable<LanguageConversionData> Conversion => _conversion;

        public LanguageData(long id, string name, LanguageTypes type) : base(id, name)
        {
            Type = type;
            _preConversion = new List<LanguageConversionData>();
            _conversion = new List<LanguageConversionData>();
        }

        public void AddPreConversion(string part1, string part2)
        {
            _preConversion.Add(new LanguageConversionData
            {
                OldValue = part1, 
                NewValue = part2
            });
        }

        public void AddPostConversion(string part1, string part2)
        {
            _conversion.Add(new LanguageConversionData
            {
                OldValue = part1, 
                NewValue = part2
            });
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
                if (!c.IsNumeric() && !char.IsPunctuation(c) && !char.IsSymbol(c) &&
                    Random.D100(1) > percent)
                    newPhrase += GetAlphabetEquivalent(c);
                else
                    newPhrase += c;
            }

            return newPhrase;
        }

        internal string DoPreConversion(int percent, string text)
        {
            string preConversion = text;
            if (PreConversion == null) return preConversion;
            foreach (LanguageConversionData lcd in PreConversion.Where(lcd => preConversion.Contains(lcd.OldValue) && Random.D100(1) >= percent))
            {
                preConversion = preConversion.Replace(lcd.OldValue, lcd.NewValue);
            }
            return preConversion;
        }

        internal string DoPostConversion(int percent, string text)
        {
            string postConversion = text;
            if (Conversion == null) return postConversion;
            foreach (LanguageConversionData lcd in Conversion.Where(lcd => postConversion.Contains(lcd.OldValue) && Random.D100(1) > percent))
            {
                postConversion = postConversion.Replace(lcd.OldValue, lcd.NewValue);
            }
            return postConversion; 
        }

        private const string EnglishAlphabet = "abcdefghijklmnopqrtsuvwxyz";
        internal char GetAlphabetEquivalent(char englishChar)
        {
            char newChar = Alphabet[EnglishAlphabet.IndexOf(char.ToLower(englishChar))];
            return char.IsUpper(englishChar) ? char.ToUpper(newChar) : newChar;
        }

    }
}
