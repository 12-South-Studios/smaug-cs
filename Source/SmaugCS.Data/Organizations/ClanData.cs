using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Realm.Library.Common;
using SmaugCS.Data.Instances;

namespace SmaugCS.Data.Organizations
{
    [XmlRoot("Clan")]
    public class ClanData : OrganizationData
    {
        [XmlArray]
        public IEnumerable<RosterData> Members { get; set; }

        public string Motto { get; set; }
        public string Deity { get; set; }
        public string NumberOne { get; set; }
        public string NumberTwo { get; set; }

        [XmlElement]
        public string Badge { get; set; }

        public string LeaderRank { get; set; }
        public string NumberOneRank { get; set; }
        public string NumberTwoRank { get; set; }
        public IEnumerable<int> PvPKillTable { get; private set; }
        public IEnumerable<int> PvPDeathTable { get; private set; }
        public int PvEKills { get; set; }
        public int PvEDeaths { get; set; }
        public int IllegalPvPKill { get; set; }
        public int Score { get; set; }

        [XmlElement]
        public ClanTypes ClanType { get; set; }

        public int Favour { get; set; }
        public int Strikes { get; set; }
        public int MemberLimit { get; set; }
        public int Alignment { get; set; }
        public IEnumerable<int> ClanObjects { get; set; }

        [XmlElement]
        public int RecallRoom { get; set; }

        public int StoreRoom { get; set; }
        public int GuardOne { get; set; }
        public int GuardTwo { get; set; }
        public int Class { get; set; }

        public ClanData(long id, string name) : base(id, name)
        {
            PvPKillTable = new List<int>(7);
            PvPDeathTable = new List<int>(7);
            ClanObjects = new List<int>(5);
        }

        public bool CanOutcast(CharacterInstance ch, CharacterInstance victim)
        {
            if (ch == null || victim == null) return false;
            if (ch.Name.EqualsIgnoreCase(Deity)) return true;
            if (victim.Name.EqualsIgnoreCase(Deity)) return false;
            if (ch.Name.EqualsIgnoreCase(Leader)) return true;
            if (victim.Name.EqualsIgnoreCase(Leader)) return false;
            if (ch.Name.EqualsIgnoreCase(NumberOne)) return true;
            if (victim.Name.EqualsIgnoreCase(NumberOne)) return false;
            if (ch.Name.EqualsIgnoreCase(NumberTwo)) return true;
            return !victim.Name.EqualsIgnoreCase(NumberTwo);
        }

        public void UpdateRoster(PlayerInstance ch)
        {
            RosterData roster = Members.ToList().Find(x => x.Name.EqualsIgnoreCase(ch.Name));
            if (roster != null)
            {
                roster.Level = ch.Level;
                roster.Kills = ch.PlayerData.PvEKills;
                roster.Deaths = ch.PlayerData.PvEDeaths;
            }
            else
                AddToRoster(ch.Name, (int)ch.CurrentClass, ch.Level, ch.PlayerData.PvEKills, ch.PlayerData.PvEDeaths);
            //Save();
        }

        public void AddToRoster(string name, int Class, int level, int kills, int deaths)
        {
            Members.ToList().Add(new RosterData
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
            RosterData roster = Members.ToList().Find(x => x.Name.EqualsIgnoreCase(name));
            if (roster != null)
                Members.ToList().Remove(roster);
        }

        public void RemoveAllRosters() => Members.ToList().Clear();

        public void SetTypeByValue(int type) => ClanType = EnumerationExtensions.GetEnum<ClanTypes>(type);

        /* public override void Save()
         {
             if (string.IsNullOrWhiteSpace(Filename))
             {
                 LogManager.Instance.Bug("Clan {0} has no filename", Name);
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
                             string.Format("{0} {1} {2} {3} {4} {5}", PvPKillTable[0], PvPKillTable[1], PvPKillTable[2],
                                           PvPKillTable[3], PvPKillTable[4], PvPKillTable[5]));
                 proxy.Write("PDeathRangeNew   {0}\n",
                             string.Format("{0} {1} {2} {3} {4} {5}", PvPDeathTable[0], PvPDeathTable[1], PvPDeathTable[2],
                                           PvPDeathTable[3], PvPDeathTable[4], PvPDeathTable[5]));
                 proxy.Write("MKills       {0}~\n", PvEKills);
                 proxy.Write("MDeaths      {0}~\n", PvEDeaths);
                 proxy.Write("IllegalPK    {0}~\n", IllegalPvPKill);
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
         */
        /*public override void Load()
        {
            if (string.IsNullOrWhiteSpace(Filename))
            {
                LogManager.Instance.Bug("Clan {0} has no filename", Name);
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
        }*/

        /* private bool ReadClan(Tuple<string, string> tuple, string path)
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
                     IllegalPvPKill = tuple.Item2.ToInt32();
                     break;
                 case "leader":
                     Leader = tuple.Item2.TrimHash();
                     break;
                 case "leadrank":
                     LeaderRank = tuple.Item2.TrimHash();
                     break;
                 case "mdeaths":
                     PvEDeaths = tuple.Item2.ToInt32();
                     break;
                 case "members":
                     // Do nothing here as we don't maintain an external count any longer
                     break;
                 case "memlimit":
                     MemberLimit = tuple.Item2.ToInt32();
                     break;
                 case "mkills":
                     PvEKills = tuple.Item2.ToInt32();
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
                     PvPDeathTable[6] = tuple.Item2.ToInt32();
                     break;
                 case "pkills":
                     PvPKillTable[6] = tuple.Item2.ToInt32();
                     break;
                 case "pdeathrange":
                     // TODO What to do here?  Original file doesn't assign the values to anything
                     break;
                 case "pdeathrangenew":
                     words = tuple.Item2.Split(' ');
                     if (words.Length < 7)
                     {
                         LogManager.Instance.Bug("Not enough death range values {0}", path);
                         break;
                     }

                     PvPDeathTable[0] = words[0].ToInt32();
                     PvPDeathTable[1] = words[1].ToInt32();
                     PvPDeathTable[2] = words[2].ToInt32();
                     PvPDeathTable[3] = words[3].ToInt32();
                     PvPDeathTable[4] = words[4].ToInt32();
                     PvPDeathTable[5] = words[5].ToInt32();
                     PvPDeathTable[6] = words[6].ToInt32();
                     break;
                 case "pkillrangenew":
                     words = tuple.Item2.Split(' ');
                     if (words.Length < 7)
                     {
                         LogManager.Instance.Bug("Not enough kill range values {0}", path);
                         break;
                     }

                     PvPKillTable[0] = words[0].ToInt32();
                     PvPKillTable[1] = words[1].ToInt32();
                     PvPKillTable[2] = words[2].ToInt32();
                     PvPKillTable[3] = words[3].ToInt32();
                     PvPKillTable[4] = words[4].ToInt32();
                     PvPKillTable[5] = words[5].ToInt32();
                     PvPKillTable[6] = words[6].ToInt32();
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
                     LogManager.Instance.Bug("Unknown clan parameter {0} for {1}", tuple.Item1, path);
                     break;
             }

             return false;
         }*/

        /*private void SaveRoster(TextWriterProxy proxy)
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
        }*/

        /*public void SaveStoreroom(CharacterInstance ch)
        {
            if (ch == null)
                return;

            string path = SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Clan) + Filename;
            using (TextWriterProxy proxy = new TextWriterProxy(new StreamWriter(path)))
            {
                int templvl = ch.Level;
                ch.Level = Program.GetLevel("hero");
                List<ObjectInstance> contents = ch.CurrentRoom.Contents;

                if (contents.Any())
                {
                    foreach (ObjectInstance obj in contents)
                        save.fwrite_obj(ch, obj, null, 0, 0, false);
                }

                proxy.Write("#END\n");
                ch.Level = templvl;
            }
        }*/
    }
}
