using System;
using System.Collections.Generic;
using System.IO;
using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using SmaugCS.Constants;
using SmaugCS.Enums;
using SmaugCS.Language;
using SmaugCS.Objects;

namespace SmaugCS
{
    public static class tables
    {
        private static readonly Dictionary<string, Func<int, int, CharacterInstance, object, int>> spell_functions = new Dictionary<string, Func<int, int, CharacterInstance, object, int>>();
        private static readonly Dictionary<string, Action<CharacterInstance, string>> skill_functions = new Dictionary<string, Action<CharacterInstance, string>>();

        public static Func<int, int, CharacterInstance, object, int> spell_function(string name)
        {
            return spell_functions.ContainsKey(name.ToLower())
                       ? spell_functions[name.ToLower()]
                       : SpellNotfound;
        }
        private static int SpellNotfound(int sn, int level, CharacterInstance ch, object vo)
        {
            // TODO send_to_char("That's not a spell!\r\n", ch);
            return (int)ReturnTypes.None;
        }

        public static Action<CharacterInstance, string> skill_function(string name)
        {
            return skill_functions.ContainsKey(name.ToLower())
                       ? skill_functions[name.ToLower()]
                       : SkillNotfound;
        }
        private static void SkillNotfound(CharacterInstance ch, string argument)
        {
            // TODO: send_to_char("Huh?\r\n", ch);
        }

        #region Skills

        public static List<string> skill_tname = new List<string>
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

        public static int skill_comp(SkillData sk1, SkillData sk2)
        {
            if (sk1 == null && sk2 != null)
                return 1;
            if (sk1 != null && sk2 == null)
                return -1;
            if (sk1 == null)
                return 0;

            return (int)sk1.Name.CaseCompare(sk2.Name);
        }

        public static int skill_comp_bytype(SkillData sk1, SkillData sk2)
        {
            if (sk1 == null && sk2 != null)
                return 1;
            if (sk1 != null && sk2 == null)
                return -1;
            if (sk1 == null)
                return 0;

            if (sk1.Type < sk1.Type)
                return -1;
            if (sk1.Type > sk2.Type)
                return 1;

            return (int)sk1.Name.CaseCompare(sk2.Name);
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
        }


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

        public static readonly string[, ,] TitleTable = new string[Program.MAX_CLASS, Program.MAX_LEVEL + 1, 2];

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
