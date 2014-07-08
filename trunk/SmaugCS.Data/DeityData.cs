using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;


namespace SmaugCS.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class DeityData : Entity, IPersistable
    {
        public string Filename { get; set; }
        public string Description { get; set; }
        public int Alignment { get; set; }
        public int Worshippers { get; set; }
        public int SCorpse { get; set; }
        public int SDeityObj { get; set; }
        public int SAvatar { get; set; }
        public int SRecall { get; set; }
        public int Flee { get; set; }
        public int Flee_NPCRace { get; set; }
        public int Flee_NPCFoe { get; set; }
        public int Kill { get; set; }
        public int Kill_Magic { get; set; }
        public int Kill_NPCRace { get; set; }
        public int Kill_NPCFoe { get; set; }
        public int Sacrifice { get; set; }
        public int BuryCorpse { get; set; }
        public int AidSpell { get; set; }
        public int Aid { get; set; }
        public int Backstab { get; set; }
        public int Steal { get; set; }
        public int Die { get; set; }
        public int Die_NPCRace { get; set; }
        public int Die_NPCFoe { get; set; }
        public int SpellAid { get; set; }
        public int DigCorpse { get; set; }
        public int Race { get; set; }
        public int Race2 { get; set; }
        public int Class { get; set; }
        public int Gender { get; set; }
        public RaceTypes NPCRace { get; set; }
        public RaceTypes NPCFoe { get; set; }
        public int Suscept { get; set; }
        public int Element { get; set; }
        public ExtendedBitvector AffectedBy { get; set; }
        public int SusceptNum { get; set; }
        public int ElementNum { get; set; }
        public int AffectedNum { get; set; }
        public int ObjStat { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        public DeityData(long id, string name) : base(id, name)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void Save()
        {
            if (string.IsNullOrWhiteSpace(Filename))
                return;

            string path = SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Deity) + Filename;

            using (TextWriterProxy proxy = new TextWriterProxy(new StreamWriter(path)))
            {
                proxy.Write("#DEITY\n");
                proxy.Write("Filename {0}~\n", Filename);
                proxy.Write("Name {0}~\n", Name);
                proxy.Write("Description {0}~\n", Description);
                proxy.Write("Alignment {0}\n", Alignment);
                proxy.Write("Worshippers {0}\n", Worshippers);
                proxy.Write("Flee {0}\n", Flee);
                proxy.Write("Flee_npcrace {0}\n", Flee_NPCRace);
                proxy.Write("Flee_npcfoe {0}\n", Flee_NPCFoe);
                proxy.Write("Kill {0}\n", Kill);
                proxy.Write("Kill_npcrace {0}\n", Kill_NPCRace);
                proxy.Write("Kill_npcfoe {0}\n", Kill_NPCFoe);
                proxy.Write("Kill_magic {0}\n", Kill_Magic);
                proxy.Write("Sac {0}\n", Sacrifice);
                proxy.Write("Bury_corpse {0}\n", BuryCorpse);
                proxy.Write("Aid_spell {0}\n", AidSpell);
                proxy.Write("Aid {0}\n", Aid);
                proxy.Write("Steal {0}\n", Steal);
                proxy.Write("Backstab {0}\n", Backstab);
                proxy.Write("Die {0}\n", Die);
                proxy.Write("Die_npcrace {0}\n", Die_NPCRace);
                proxy.Write("Die_npcfoe {0}\n", Die_NPCFoe);
                proxy.Write("Spell_aid {0}\n", SpellAid);
                proxy.Write("Dig_corpse {0}\n", DigCorpse);
                proxy.Write("Scorpse {0}\n", SCorpse);
                proxy.Write("Savatar {0}\n", SAvatar);
                proxy.Write("Sdeityobj {0}\n", SDeityObj);
                proxy.Write("Srecall {0}\n", SRecall);
                proxy.Write("Race {0}\n", Race);
                proxy.Write("Class {0}\n", Class);
                proxy.Write("Element {0}\n", Element);
                proxy.Write("Sex {0}\n", Gender);
                proxy.Write("Affected {0}\n", AffectedBy.ToString());
                proxy.Write("Npcrace {0}\n", NPCRace);
                proxy.Write("Npcfoe {0}\n", NPCFoe);
                proxy.Write("Suscept {0}\n", Suscept);
                proxy.Write("Race2 {0}\n", Race2);
                proxy.Write("Susceptnum {0}\n", SusceptNum);
                proxy.Write("Elementnum {0}\n", ElementNum);
                proxy.Write("Affectednum {0}\n", AffectedNum);
                proxy.Write("Objstat {0}\n", ObjStat);
                proxy.Write("End\r\n");
                proxy.Write("#END\n");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Load()
        {
            string path = SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Deity) + Filename;
            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(path)))
            {
                List<string> lines =
                    proxy.ReadIntoList()
                         .Where(line => !line.Equals("$") && !line.Equals("End", StringComparison.OrdinalIgnoreCase))
                         .ToList();
                foreach (string line in lines.Where(x => !x.StartsWith("*")))
                {
                    string firstWord = line.FirstWord();
                    string restOfLine = line.RemoveWord(1).TrimEnd('~');

                    switch (firstWord.ToUpper())
                    {
                        case "AFFECTED":
                            AffectedBy = restOfLine.ToBitvector();
                            break;
                        case "AFFECTEDNUM":
                            AffectedNum = restOfLine.ToInt32();
                            break;
                        case "AID":
                            Aid = restOfLine.ToInt32();
                            break;
                        case "AID_SPELL":
                            AidSpell = restOfLine.ToInt32();
                            break;
                        case "ALIGNMENT":
                            Alignment = restOfLine.ToInt32();
                            break;
                        case "BACKSTAB":
                            Backstab = restOfLine.ToInt32();
                            break;
                        case "BURY_CORPSE":
                            BuryCorpse = restOfLine.ToInt32();
                            break;
                        case "CLASS":
                            Class = restOfLine.ToInt32();
                            break;
                        case "DESCRIPTION":
                            Description = restOfLine;
                            break;
                        case "DIE":
                            Die = restOfLine.ToInt32();
                            break;
                        case "DIE_NPCRACE":
                            Die_NPCRace = restOfLine.ToInt32();
                            break;
                        case "DIE_NPCFOE":
                            Die_NPCFoe = restOfLine.ToInt32();
                            break;
                        case "DIG_CORPSE":
                            DigCorpse = restOfLine.ToInt32();
                            break;
                        case "ELEMENT":
                            Element = restOfLine.ToInt32();
                            break;
                        case "ELEMENTNUM":
                            ElementNum = restOfLine.ToInt32();
                            break;
                        case "FILENAME":
                            Filename = restOfLine;
                            break;
                        case "FLEE":
                            Flee = restOfLine.ToInt32();
                            break;
                        case "FLEE_NPCRACE":
                            Flee_NPCRace = restOfLine.ToInt32();
                            break;
                        case "FLEE_NPCFOE":
                            Flee_NPCFoe = restOfLine.ToInt32();
                            break;
                        case "KILL":
                            Kill = restOfLine.ToInt32();
                            break;
                        case "KILL_NPCRACE":
                            Kill_NPCRace = restOfLine.ToInt32();
                            break;
                        case "KILL_NPCFOE":
                            Kill_NPCFoe = restOfLine.ToInt32();
                            break;
                        case "KILL_MAGIC":
                            Kill_Magic = restOfLine.ToInt32();
                            break;
                        case "NAME":
                            Name = restOfLine;
                            break;
                        case "NPCFOE":
                            NPCFoe = Realm.Library.Common.EnumerationExtensions.GetEnum<RaceTypes>(restOfLine.ToInt32());
                            break;
                        case "NPCRACE":
                            NPCRace = Realm.Library.Common.EnumerationExtensions.GetEnum<RaceTypes>(restOfLine.ToInt32());
                            break;
                        case "OBJSTAT":
                            ObjStat = restOfLine.ToInt32();
                            break;
                        case "RACE":
                            Race = restOfLine.ToInt32();
                            break;
                        case "RACE2":
                            Race2 = restOfLine.ToInt32();
                            break;
                        case "SAC":
                            Sacrifice = restOfLine.ToInt32();
                            break;
                        case "SAVATAR":
                            SAvatar = restOfLine.ToInt32();
                            break;
                        case "SCORPSE":
                            SCorpse = restOfLine.ToInt32();
                            break;
                        case "SDEITYOBJ":
                            SDeityObj = restOfLine.ToInt32();
                            break;
                        case "SRECALL":
                            SRecall = restOfLine.ToInt32();
                            break;
                        case "SEX":
                            Gender = restOfLine.ToInt32();
                            break;
                        case "SPELL_AID":
                            SpellAid = restOfLine.ToInt32();
                            break;
                        case "STEAL":
                            Steal = restOfLine.ToInt32();
                            break;
                        case "SUSCEPT":
                            Suscept = restOfLine.ToInt32();
                            break;
                        case "SUSCEPTNUM":
                            SusceptNum = restOfLine.ToInt32();
                            break;
                        case "WORSHIPPERS":
                            Worshippers = restOfLine.ToInt32();
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="mod"></param>
        /// <returns></returns>
        public int FuzzifyFavor(int field, int mod)
        {
            int fieldvalue = 0;
            switch (field)
            {
                case 0:
                    fieldvalue = Flee;
                    break;
                case 1:
                    fieldvalue = Flee_NPCRace;
                    break;
                case 2:
                    fieldvalue = Kill;
                    break;
                case 3:
                    fieldvalue = Kill_NPCRace;
                    break;
                case 4:
                    fieldvalue = Kill_Magic;
                    break;
                case 5:
                    fieldvalue = Sacrifice;
                    break;
                case 6:
                    fieldvalue = BuryCorpse;
                    break;
                case 7:
                    fieldvalue = AidSpell;
                    break;
                case 8:
                    fieldvalue = Aid;
                    break;
                case 9:
                    fieldvalue = Steal;
                    break;
                case 10:
                    fieldvalue = Backstab;
                    break;
                case 11:
                    fieldvalue = Die;
                    break;
                case 12:
                    fieldvalue = Die_NPCRace;
                    break;
                case 13:
                    fieldvalue = SpellAid;
                    break;
                case 14:
                    fieldvalue = DigCorpse;
                    break;
                case 15:
                    fieldvalue = Die_NPCFoe;
                    break;
                case 16:
                    fieldvalue = Flee_NPCFoe;
                    break;
                case 17:
                    fieldvalue = Kill_NPCFoe;
                    break;
            }

            return SmaugRandom.Fuzzy(fieldvalue / mod);
        }
    }
}
