using System;
using System.Collections.Generic;
using System.IO;
using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Language;

namespace SmaugCS
{
    public static class tables
    {
        public static List<Tuple<string, string>> SyllableTable = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>(" ", " "),
                new Tuple<string, string>("ar", "abra"),
                new Tuple<string, string>("au", "kada"),
                new Tuple<string, string>("bless", "fido"),
                new Tuple<string, string>("blind", "nose"),
                new Tuple<string, string>("bur", "mosa"),
                new Tuple<string, string>("cu", "judi"),
                new Tuple<string, string>("de", "oculo"),
                new Tuple<string, string>("en", "unso"),
                new Tuple<string, string>("light", "dies"),
                new Tuple<string, string>("lo", "hi"),
                new Tuple<string, string>("mor", "zak"),
                new Tuple<string, string>("move", "sido"),
                new Tuple<string, string>("ness", "lacri"),
                new Tuple<string, string>("ning", "illa"),
                new Tuple<string, string>("per", "duda"),
                new Tuple<string, string>("polymorph", "iaddahs"),
                new Tuple<string, string>("ra", "gru"),
                new Tuple<string, string>("re", "candus"),
                new Tuple<string, string>("son", "sabru"),
                new Tuple<string, string>("tect", "infra"),
                new Tuple<string, string>("tri", "cula"),
                new Tuple<string, string>("ven", "nofo"),
                new Tuple<string, string>("a", "a"),
                new Tuple<string, string>("b", "b"),
                new Tuple<string, string>("c", "q"),
                new Tuple<string, string>("d", "e"),
                new Tuple<string, string>("e", "z"),
                new Tuple<string, string>("f", "y"),
                new Tuple<string, string>("g", "o"),
                new Tuple<string, string>("h", "p"),
                new Tuple<string, string>("i", "u"),
                new Tuple<string, string>("j", "y"),
                new Tuple<string, string>("k", "t"),
                new Tuple<string, string>("l", "r"),
                new Tuple<string, string>("m", "w"),
                new Tuple<string, string>("n", "i"),
                new Tuple<string, string>("o", "a"),
                new Tuple<string, string>("p", "s"),
                new Tuple<string, string>("q", "d"),
                new Tuple<string, string>("r", "f"),
                new Tuple<string, string>("s", "g"),
                new Tuple<string, string>("t", "h"),
                new Tuple<string, string>("u", "j"),
                new Tuple<string, string>("v", "z"),
                new Tuple<string, string>("w", "x"),
                new Tuple<string, string>("x", "n"),
                new Tuple<string, string>("y", "l"),
                new Tuple<string, string>("z", "k"),
                new Tuple<string, string>("", "")
            };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        public static string ConvertStringSyllables(string sourceString)
        {
            string buffer = string.Empty;
            string oldStr = sourceString;

            for (int i = 0; i < sourceString.Length; i++)
            {
                foreach (Tuple<string, string> tuple in SyllableTable)
                {
                    if (oldStr.StartsWithIgnoreCase(tuple.Item1))
                    {
                        buffer += tuple.Item2;
                        oldStr = oldStr.Remove(0, tuple.Item1.Length);
                        break;
                    }
                }

                if (oldStr.IsNullOrWhitespace())
                    break;
            }

            return buffer;
        }

        private static readonly Dictionary<string, Func<int, int, CharacterInstance, object, ReturnTypes>>
            SpellFunctions = new Dictionary<string, Func<int, int, CharacterInstance, object, ReturnTypes>>()
                {
                    {"spell_smaug", Spells.Smaug.Smaug.spell_smaug}
                };
        private static readonly Dictionary<string, Action<CharacterInstance, string>> SkillFunctions = new Dictionary<string, Action<CharacterInstance, string>>();

        public static Func<int, int, CharacterInstance, object, ReturnTypes> GetSpellFunction(string name)
        {
            return SpellFunctions.ContainsKey(name.ToLower())
                       ? SpellFunctions[name.ToLower()]
                       : SpellNotfound;
        }
        public static ReturnTypes SpellNotfound(int sn, int level, CharacterInstance ch, object vo)
        {
            // TODO send_to_char("That's not a spell!\r\n", ch);
            return ReturnTypes.None;
        }

        public static Action<CharacterInstance, string> GetSkillFunction(string name)
        {
            return SkillFunctions.ContainsKey(name.ToLower())
                       ? SkillFunctions[name.ToLower()]
                       : SkillNotfound;
        }
        public static void SkillNotfound(CharacterInstance ch, string argument)
        {
            // TODO: send_to_char("Huh?\r\n", ch);
        }

        #region Skills

        /*public static List<string> skill_tname = new List<string>
                                                     {
                                                         "unknown",
                                                         "Spell",
                                                         "Skill",
                                                         "Weapon",
                                                         "Tongue",
                                                         "Herb",
                                                         "Racial",
                                                         "Disease"
                                                     };
        private static readonly Dictionary<string, SkillTypes> skilltypes = new Dictionary<string, SkillTypes>()
            {
                {"racial", SkillTypes.Racial},
                {"spell", SkillTypes.Spell},
                {"skill", SkillTypes.Skill},
                {"weapon", SkillTypes.Weapon},
                {"tongue", SkillTypes.Tongue},
                {"herb", SkillTypes.Herb},
                {"disease", SkillTypes.Disease}
            };

        private static readonly List<SkillData> skill_table = new List<SkillData>();

        private static readonly List<SkillData> skill_table_bytype = new List<SkillData>();

        public static int get_skill(string skilltype)
        {
            return skilltypes.ContainsKey(skilltype.ToLower())
                       ? (int)skilltypes[skilltype.ToLower()]
                       : (int)SkillTypes.Unknown;
        }

        /// <summary>
        /// Sorts the skill tables
        /// </summary>
        public static void sort_skill_table()
        {
            List<SkillData> orderedList = skill_table;
            orderedList.Sort(skill_comp);

            // Refresh the bytype table with the newly sorted list
            skill_table_bytype.Clear();
            skill_table_bytype.AddRange(skill_table);

            List<SkillData> orderedListByType = skill_table_bytype;
            orderedListByType.Sort(skill_comp_bytype);
        }*/


        #endregion

        #region Languages
        //new StreamReader(Program.TONGUE_FILE)
        private enum LanguageSection
        {
            Name, PreCNV, Alphabet, CNV
        }
        public static List<LanguageData> load_tongues()
        {
            List<LanguageData> languages = new List<LanguageData>();
            string path = SystemConstants.GetSystemFile(SystemFileTypes.Tongues);

            using (TextReaderProxy sr = new TextReaderProxy(new StreamReader(path)))
            {
                List<TextSection> sections = sr.ReadSections(new[] { "#" }, new[] { "*" }, null, "#end");

                foreach (TextSection section in sections)
                {
                    LanguageData ld = new LanguageData { Name = section.Header };
                    languages.Add(ld);
                    LanguageSection lngSection = LanguageSection.PreCNV;

                    foreach (string line in section.Lines)
                    {
                        if (lngSection == LanguageSection.PreCNV)
                        {
                            ld = ReadPreCnv(line, ld);
                            lngSection = LanguageSection.Alphabet;
                            continue;
                        }
                        if (lngSection == LanguageSection.Alphabet)
                        {
                            ld = ReadAlphabet(line, ld);
                            lngSection = LanguageSection.CNV;
                            continue;
                        }
                        if (lngSection == LanguageSection.CNV)
                        {
                            ld = ReadCnv(line, ld);
                            lngSection = LanguageSection.Name;
                        }
                    }
                }
            }
            return languages;
        }

        private static LanguageData ReadAlphabet(string line, LanguageData ld)
        {
            if (!line.Equals("~"))
            {
                string tempLine = line;
                if (line.EndsWith("~"))
                    tempLine = tempLine.Substring(0, tempLine.Length - 1);
                ld.Alphabet = tempLine;
            }
            return ld;
        }

        private static LanguageData ReadPreCnv(string line, LanguageData ld)
        {
            if (line.Equals("~"))
                return ld;

            string tempLine = line;
            if (tempLine.EndsWith("~"))
                tempLine = tempLine.Substring(0, tempLine.Length - 1);


            ld.PreConversion.Add(new LanguageConversionData(tempLine));
            return ld;
        }

        private static LanguageData ReadCnv(string line, LanguageData ld)
        {
            if (line.Equals("~"))
                return ld;

            string tempLine = line;
            if (tempLine.EndsWith("~"))
                tempLine = tempLine.Substring(0, tempLine.Length - 1);

            ld.Conversion.Add(new LanguageConversionData(tempLine));
            return ld;
        }

        #endregion

        //public static readonly string[, ,] TitleTable = new string[Program.MAX_CLASS, Program.MAX_LEVEL + 1, 2];
        public static readonly string[, ,] TitleTable;
        public static string GetTitle(ClassTypes type, int level, GenderTypes gender)
        {
            return TitleTable[(int)type, level, gender == GenderTypes.Female ? 1 : 0];
        }

        public static bool load_class_file(string fname)
        {
            // TODO
            return false;
        }

        public static void load_classes()
        {
            // TODO
        }

        public static void write_class_file(int cl)
        {
            // TODO
        }

        public static void load_races()
        {
            // TODO
        }

        public static void write_race_file(int ra)
        {
            // TODO
        }

        public static bool load_race_file(string fname)
        {
            // TODO
            return false;
        }

        public static void remap_slot_numbers()
        {
            // TODO
        }

        public static void fwrite_skill(TextReaderProxy proxy, SkillData skill)
        {
            // TODO
        }

        public static void save_skill_table()
        {
            // TODO
        }

        public static void save_herb_table()
        {
            // TODO
        }

        public static void save_socials()
        {
            // TODO
        }

        public static void save_commands()
        {
            // TODO
        }

        public static SkillData fread_skill(TextReaderProxy proxy)
        {
            // TODO
            return null;
        }

        public static void load_skill_table()
        {
            // TODO
        }

        public static void load_herb_table()
        {
            // TODO
        }

        public static void fread_social(TextReaderProxy proxy)
        {
            // TODO
        }

        public static void load_socials()
        {
            // TODO
        }

        public static void fread_command(TextReaderProxy proxy)
        {
            // TODO
        }

        public static void load_commands()
        {
            // TODO
        }

        public static void save_classes()
        {
            foreach (ClassData data in db.CLASSES)
                write_class_file((int)data.Type);
        }

        public static void save_races()
        {
            foreach (RaceData data in db.RACES)
                write_race_file((int)data.Type);
        }

        public static void free_tongues()
        {
            // TODO
        }

        public static void fwrite_langs()
        {
            // TODO
        }
    }
}
