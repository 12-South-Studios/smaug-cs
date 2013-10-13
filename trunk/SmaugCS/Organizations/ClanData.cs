using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Enums;
using SmaugCS.Managers;
using SmaugCS.Objects;

namespace SmaugCS.Organizations
{
    [XmlRoot("Clan")]
    public class ClanData : OrganizationData
    {
        [XmlArray]
        public List<RosterData> Members { get; set; }

        public string Motto { get; set; }
        public string Deity { get; set; }
        public string NumberOne { get; set; }
        public string NumberTwo { get; set; }

        [XmlElement]
        public string Badge { get; set; }

        public string LeaderRank { get; set; }
        public string NumberOneRank { get; set; }
        public string NumberTwoRank { get; set; }
        public int[] PKillTable { get; set; }
        public int[] PDeathTable { get; set; }
        public int MobileKills { get; set; }
        public int MobileDeaths { get; set; }
        public int IllegalPK { get; set; }
        public int Score { get; set; }

        [XmlElement]
        public ClanTypes ClanType { get; set; }

        public int Favour { get; set; }
        public int Strikes { get; set; }
        public int MemberLimit { get; set; }
        public int Alignment { get; set; }
        public int[] ClanObjects { get; set; }

        [XmlElement]
        public int RecallRoom { get; set; }

        public int StoreRoom { get; set; }
        public int GuardOne { get; set; }
        public int GuardTwo { get; set; }
        public int Class { get; set; }

        public ClanData()
        {
            PKillTable = new int[7];
            PDeathTable = new int[7];
            ClanObjects = new int[5];
        }

        public ClanData(string filename)
        {
            Filename = filename;
            PKillTable = new int[7];
            PDeathTable = new int[7];
            ClanObjects = new int[5];
        }

        public bool CanOutcast(CharacterInstance ch, CharacterInstance victim)
        {
            if (ch == null || victim == null)
                return false;
            if (ch.Name.EqualsIgnoreCase(Deity))
                return true;
            if (victim.Name.EqualsIgnoreCase(Deity))
                return false;
            if (ch.Name.EqualsIgnoreCase(Leader))
                return true;
            if (victim.Name.EqualsIgnoreCase(Leader))
                return false;
            if (ch.Name.EqualsIgnoreCase(NumberOne))
                return true;
            if (victim.Name.EqualsIgnoreCase(NumberOne))
                return false;
            if (ch.Name.EqualsIgnoreCase(NumberTwo))
                return true;
            if (victim.Name.EqualsIgnoreCase(NumberTwo))
                return false;
            return true;
        }

        public void UpdateRoster(CharacterInstance ch)
        {
            RosterData roster = Members.Find(x => x.Name.Equals(ch.Name, StringComparison.OrdinalIgnoreCase));
            if (roster != null)
            {
                roster.Level = ch.Level;
                roster.Kills = ch.PlayerData.mkills;
                roster.Deaths = ch.PlayerData.mdeaths;
            }
            else 
                AddToRoster(ch.Name, (int)ch.CurrentClass, ch.Level, ch.PlayerData.mkills, ch.PlayerData.mdeaths);
            Save();
        }

        public void AddToRoster(string name, int Class, int level, int kills, int deaths)
        {
            Members.Add(new RosterData
                            {
                                Name = name,
                                Class = Class,
                                Level = level,
                                Kills = kills,
                                Deaths = deaths,
                                Joined = DateTime.Now
                            });
        }

        public void RemoveFromRoster(string name)
        {
            RosterData roster = Members.Find(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (roster != null)
                Members.Remove(roster);
        }

        public void RemoveAllRosters()
        {
            Members.Clear();
        }

        public override void Save()
        {
            if (string.IsNullOrWhiteSpace(Filename))
            {
                LogManager.Bug("Clan {0} has no filename", Name);
                return;
            }

            string path = SystemConstants.GetSystemFile(SystemFileTypes.Clans);
            using (TextWriterProxy proxy = new TextWriterProxy(new StreamWriter(path)))
            {
                proxy.Write("#CLAN\n");
                proxy.Write("Name         {0}~\n", Name);
                proxy.Write("Filename     {0}~\n", Filename);
                proxy.Write("Motto        {0}~\n", Motto);
                proxy.Write("Description  {0}~\n", Description);
                proxy.Write("Deity        {0}~\n", Deity);
                proxy.Write("Leader       {0}~\n", Leader);
                proxy.Write("NumberOne    {0}~\n", NumberOne);
                proxy.Write("NumberTwo    {0}~\n", NumberTwo);
                proxy.Write("Badge        {0}~\n", Badge);
                proxy.Write("Leadrank     {0}~\n", LeaderRank);
                proxy.Write("Onerank      {0}~\n", NumberOneRank);
                proxy.Write("Tworank      {0}~\n", NumberTwoRank);
                proxy.Write("PKillRangeNew   {0}\n",
                            string.Format("{0} {1} {2} {3} {4} {5}", PKillTable[0], PKillTable[1], PKillTable[2],
                                          PKillTable[3], PKillTable[4], PKillTable[5]));
                proxy.Write("PDeathRangeNew   {0}\n",
                            string.Format("{0} {1} {2} {3} {4} {5}", PDeathTable[0], PDeathTable[1], PDeathTable[2],
                                          PDeathTable[3], PDeathTable[4], PDeathTable[5]));
                proxy.Write("MKills       {0}~\n", MobileKills);
                proxy.Write("MDeaths      {0}~\n", MobileDeaths);
                proxy.Write("IllegalPK    {0}~\n", IllegalPK);
                proxy.Write("Score        {0}~\n", Score);
                proxy.Write("Type         {0}~\n", ClanType);
                proxy.Write("Class        {0}~\n", Class);
                proxy.Write("Favour       {0}~\n", Favour);
                proxy.Write("Strikes      {0}~\n", Strikes);
                proxy.Write("Members      {0}\n", Members.Count);
                proxy.Write("MemLimit     {0}\n", MemberLimit);
                proxy.Write("Alignment    {0}\n", Alignment);
                proxy.Write("Board        {0}\n", Board);
                proxy.Write("ClanObjOne   {0}\n", ClanObjects[0]);
                proxy.Write("ClanObjTwo   {0}\n", ClanObjects[1]);
                proxy.Write("ClanObjThree {0}\n", ClanObjects[2]);
                proxy.Write("ClanObjFour  {0}\n", ClanObjects[3]);
                proxy.Write("ClanObjFive  {0}\n", ClanObjects[4]);
                proxy.Write("Recall       {0}\n", RecallRoom);
                proxy.Write("Storeroom    {0}\n", StoreRoom);
                proxy.Write("GuardOne     {0}\n", GuardOne);
                proxy.Write("GuardTwo     {0}\n", GuardTwo);
                proxy.Write("End\n\n");
                SaveRoster(proxy);
                proxy.Write("#END\n");
            }
        }

        public override void Load()
        {
            if (string.IsNullOrWhiteSpace(Filename))
            {
                LogManager.Bug("Clan {0} has no filename", Name);
                return;
            }

            string path = SystemConstants.GetSystemFile(SystemFileTypes.Clans);
            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(path)))
            {
                bool inClan = false;
                bool inRoster = false;

                List<string> lines = proxy.ReadIntoList();
                foreach (string line in lines.Where(x => !x.StartsWith("*")))
                {
                    Tuple<string, string> tuple = line.FirstArgument();
                    if (tuple.Item1.EqualsIgnoreCase("#clan") || (inClan && !inRoster))
                        inClan = ReadClan(tuple, path);
                    else if (tuple.Item1.EqualsIgnoreCase("#roster") || (inRoster && !inClan))
                        inRoster = ReadRoster(tuple, path);
                    else if (tuple.Item1.EqualsIgnoreCase("#end"))
                        return;
                }
            }

            // TODO: Read the Clan storeroom here (line 841 clans.c)
        }

        private bool ReadRoster(Tuple<string, string> tuple, string path)
        {
            RosterData roster = new RosterData();
            switch (tuple.Item1.ToLower())
            {
                case "name":
                    roster.Name = tuple.Item2.TrimHash();
                    break;
                case "joined":
                    DateTime joined;
                    DateTime.TryParse(tuple.Item2.TrimHash(), out joined);
                    roster.Joined = joined;
                    break;
                case "class":
                    roster.Class = (int)EnumerationExtensions.GetEnum<ClassTypes>(tuple.Item2.TrimHash());
                    break;
                case "level":
                    roster.Level = tuple.Item2.ToInt32();
                    break;
                case "kills":
                    roster.Kills = tuple.Item2.ToInt32();
                    break;
                case "deaths":
                    roster.Deaths = tuple.Item2.ToInt32();
                    break;
                case "end":
                    return true;
            }
            return false;
        }

        private bool ReadClan(Tuple<string, string> tuple, string path)
        {
            string[] words;
            switch (tuple.Item1.ToLower())
            {
                case "alignment":
                    Alignment = tuple.Item2.ToInt32();
                    break;
                case "badge":
                    Badge = tuple.Item2.TrimHash();
                    break;
                case "board":
                    Board = tuple.Item2.ToInt32();
                    break;
                case "clanobjone":
                    ClanObjects[0] = tuple.Item2.ToInt32();
                    break;
                case "clanobjtwo":
                    ClanObjects[1] = tuple.Item2.ToInt32();
                    break;
                case "clanobjthree":
                    ClanObjects[2] = tuple.Item2.ToInt32();
                    break;
                case "clanobjfour":
                    ClanObjects[3] = tuple.Item2.ToInt32();
                    break;
                case "clanobjfive":
                    ClanObjects[4] = tuple.Item2.ToInt32();
                    break;
                case "class":
                    Class = tuple.Item2.ToInt32();
                    break;
                case "deity":
                    Deity = tuple.Item2.TrimHash();
                    break;
                case "description":
                    Description = tuple.Item2.TrimHash();
                    break;
                case "end":
                    return true;
                case "favour":
                    Favour = tuple.Item2.ToInt32();
                    break;
                case "filename":
                    Filename = tuple.Item2.TrimHash();
                    break;
                case "guardone":
                    GuardOne = tuple.Item2.ToInt32();
                    break;
                case "guardtwo":
                    GuardTwo = tuple.Item2.ToInt32();
                    break;
                case "illegalpk":
                    IllegalPK = tuple.Item2.ToInt32();
                    break;
                case "leader":
                    Leader = tuple.Item2.TrimHash();
                    break;
                case "leadrank":
                    LeaderRank = tuple.Item2.TrimHash();
                    break;
                case "mdeaths":
                    MobileDeaths = tuple.Item2.ToInt32();
                    break;
                case "members":
                    // Do nothing here as we don't maintain an external count any longer
                    break;
                case "memlimit":
                    MemberLimit = tuple.Item2.ToInt32();
                    break;
                case "mkills":
                    MobileKills = tuple.Item2.ToInt32();
                    break;
                case "motto":
                    Motto = tuple.Item2.TrimHash();
                    break;
                case "name":
                    Name = tuple.Item2.TrimHash();
                    break;
                case "numberone":
                    NumberOne = tuple.Item2.TrimHash();
                    break;
                case "numbertwo":
                    NumberTwo = tuple.Item2.TrimHash();
                    break;
                case "onerank":
                    NumberOneRank = tuple.Item2.TrimHash();
                    break;
                case "pdeaths":
                    PDeathTable[6] = tuple.Item2.ToInt32();
                    break;
                case "pkills":
                    PKillTable[6] = tuple.Item2.ToInt32();
                    break;
                case "pdeathrange":
                    // TODO What to do here?  Original file doesn't assign the values to anything
                    break;
                case "pdeathrangenew":
                    words = tuple.Item2.Split(' ');
                    if (words.Length < 7)
                    {
                        LogManager.Bug("Not enough death range values {0}", path);
                        break;
                    }

                    PDeathTable[0] = words[0].ToInt32();
                    PDeathTable[1] = words[1].ToInt32();
                    PDeathTable[2] = words[2].ToInt32();
                    PDeathTable[3] = words[3].ToInt32();
                    PDeathTable[4] = words[4].ToInt32();
                    PDeathTable[5] = words[5].ToInt32();
                    PDeathTable[6] = words[6].ToInt32();
                    break;
                case "pkillrangenew":
                    words = tuple.Item2.Split(' ');
                    if (words.Length < 7)
                    {
                        LogManager.Bug("Not enough kill range values {0}", path);
                        break;
                    }

                    PKillTable[0] = words[0].ToInt32();
                    PKillTable[1] = words[1].ToInt32();
                    PKillTable[2] = words[2].ToInt32();
                    PKillTable[3] = words[3].ToInt32();
                    PKillTable[4] = words[4].ToInt32();
                    PKillTable[5] = words[5].ToInt32();
                    PKillTable[6] = words[6].ToInt32();
                    break;
                case "pkillrange":
                    // TODO What to do here?  Original file doesn't assign the vlaues to anything
                    break;
                case "recall":
                    RecallRoom = tuple.Item2.ToInt32();
                    break;
                case "score":
                    Score = tuple.Item2.ToInt32();
                    break;
                case "strikes":
                    Strikes = tuple.Item2.ToInt32();
                    break;
                case "storeroom":
                    StoreRoom = tuple.Item2.ToInt32();
                    break;
                case "tworank":
                    NumberTwoRank = tuple.Item2.TrimHash();
                    break;
                case "type":
                    ClanType = EnumerationExtensions.GetEnum<ClanTypes>(tuple.Item2.ToInt32());
                    break;
                default:
                    LogManager.Bug("Unknown clan parameter {0} for {1}", tuple.Item1, path);
                    break;
            }

            return false;
        }

        private void SaveRoster(TextWriterProxy proxy)
        {
            foreach (RosterData roster in Members)
            {
                proxy.Write("{0}", "#ROSTER\n");
                proxy.Write("Name      {0}~\n", roster.Name);
                proxy.Write("Joined    {0}\n", roster.Joined.ToFileTimeUtc());
                proxy.Write("Class     {0}~\n", GameConstants.npc_class[roster.Class]);
                proxy.Write("Level     {0}\n", roster.Level);
                proxy.Write("Kills     {0}\n", roster.Kills);
                proxy.Write("Deaths    {0}\n", roster.Deaths);
                proxy.Write("{0}", "End\n\n");
            }
        }

        public void SaveStoreroom(CharacterInstance ch)
        {
            if (ch == null)
                return;

            string path = SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Clan) + Filename;
            using (TextWriterProxy proxy = new TextWriterProxy(new StreamWriter(path)))
            {
                int templvl = ch.Level;
                ch.Level = Program.LEVEL_HERO;
                List<ObjectInstance> contents = ch.CurrentRoom.Contents;

                if (contents.Any())
                {
                    foreach (ObjectInstance obj in contents)
                        save.fwrite_obj(ch, obj, null, 0, 0, false);
                }

                proxy.Write("#END\n");
                ch.Level = templvl;
            }
        }
    }
}
