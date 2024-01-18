using Realm.Library.Common.Extensions;
using SmaugCS.DAL;
using SmaugCS.DAL.Models;
using SmaugCS.Data.Organizations;
using SmaugCS.Logging;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace SmaugCS.Clans
{
    public class ClanRepository : IClanRepository
    {
        public IEnumerable<ClanData> Clans { get; private set; }
        private readonly ILogManager _logManager;
        private readonly IDbContext _dbContext;

        public ClanRepository(ILogManager logManager, IDbContext dbContext)
        {
            Clans = new List<ClanData>();
            _logManager = logManager;
            _dbContext = dbContext;
        }

        public void Add(ClanData clan)
        {
            Clans.ToList().Add(clan);
        }

        public void Load()
        {
            try
            {
                if (_dbContext.Count<DAL.Models.Clan>() == 0) return;

                var clans = _dbContext.GetAll<DAL.Models.Clan>();
                foreach (DAL.Models.Clan clan in clans)
                {
                    var newClan = new ClanData(clan.Id, clan.Name);
                    newClan.Description = clan.Description;
                    newClan.Board = clan.BoardId;
                    newClan.ClanType = EnumerationExtensions.GetEnum<ClanTypes>(clan.ClanType);
                    newClan.Motto = clan.Motto;
                    newClan.Deity = clan.DeityName;
                    newClan.Badge = clan.Badge;
                    newClan.RecallRoom = clan.RecallRoomId;
                    newClan.StoreRoom = clan.StoreRoomId;

                    var clanStats = clan.Stats;
                    newClan.PvEKills = clan.Stats.PvE_Kills;
                    newClan.PvEDeaths = clan.Stats.PvE_Deaths;
                    newClan.IllegalPvPKill = clan.Stats.Illegal_PvP_Kills;
                    newClan.Score = clan.Stats.Score;
                    newClan.Favour = clan.Stats.Favour;
                    newClan.Strikes = clan.Stats.Strikes;
                    newClan.MemberLimit = clan.Stats.MemberLimit;
                    newClan.Alignment = clan.Stats.Alignment;

                    var members = new List<RosterData>();
                    foreach (DAL.Models.ClanMember clanMember in clan.Members)
                    {
                        var newMember = new RosterData();
                        newMember.Name = clanMember.Name;
                        newMember.Joined = clanMember.Joined;
                        newMember.Class = clanMember.ClassId;
                        newMember.Level = clanMember.Level;
                        newMember.Kills = clanMember.Kills;
                        newMember.Deaths = clanMember.Deaths;
                        members.Add(newMember);
                    }
                    newClan.Members = members;

                    newClan.LeaderRank = clan.Ranks.First(x => x.RankType == (int)ClanRanks.Leader).RankName;
                    newClan.Leader = clan.Members.First(x => x.ClanRank == (int)ClanRanks.Leader).Name;

                    newClan.NumberOneRank = clan.Ranks.First(x => x.RankType == (int)ClanRanks.NumberOne).RankName;
                    var member = clan.Members.FirstOrDefault(x => x.ClanRank == (int)ClanRanks.NumberOne);
                    newClan.NumberOne = member == null ? string.Empty : member.Name;

                    newClan.NumberTwoRank = clan.Ranks.First(x => x.RankType == (int)ClanRanks.NumberTwo).RankName;
                    member = clan.Members.FirstOrDefault(x => x.ClanRank == (int)ClanRanks.NumberTwo);
                    newClan.NumberTwo = member == null ? string.Empty : member.Name;

                    newClan.ClanObjects = clan.Items.Select(x => x.Id);

                    Add(newClan);
                }

                _logManager.Boot("Loaded {0} Clans", Clans.Count());
            }
            catch (DbException ex)
            {
                _logManager.Error(ex);
                throw;
            }
        }

        public void Save()
        {
            try
            {
                foreach (var clan in Clans.Where(x => !x.Saved).ToList())
                {
                    var clanToSave = _dbContext.Get<DAL.Models.Clan>(clan.ID);
                    if (clanToSave == null)
                    {
                        clanToSave = new DAL.Models.Clan();
                        clanToSave.Name = clan.Name;
                        clanToSave.ClanType = (int)clan.ClanType;
                    }

                    clanToSave.Description = clan.Description;
                    clanToSave.BoardId = clan.Board;
                    clanToSave.Motto = clan.Motto;
                    clanToSave.DeityName = clan.Deity;
                    clanToSave.Badge = clan.Badge;
                    clanToSave.RecallRoomId = clan.RecallRoom;
                    clanToSave.StoreRoomId = clan.StoreRoom;

                    // Stats
                    if (clanToSave.Stats == null)
                        clanToSave.Stats = new ClanStats();
                    clanToSave.Stats.PvE_Kills = clan.PvEKills;
                    clanToSave.Stats.PvE_Deaths = clan.PvEDeaths;
                    clanToSave.Stats.Illegal_PvP_Kills = clan.IllegalPvPKill;
                    clanToSave.Stats.Score = clan.Score;
                    clanToSave.Stats.Favour = clan.Favour;
                    clanToSave.Stats.Strikes = clan.Strikes;
                    clanToSave.Stats.MemberLimit = clan.MemberLimit;
                    clanToSave.Stats.Alignment = clan.Alignment;

                    // Members
                    foreach (var member in clan.Members)
                    {
                        var clanMember = clanToSave.Members.FirstOrDefault(x => x.Name == member.Name);
                        if (clanMember == null)
                        {
                            clanMember = new ClanMember();
                            clanToSave.Members.Add(clanMember);
                        }

                        clanMember.Name = member.Name;
                        clanMember.Joined = member.Joined;
                        clanMember.ClassId = member.Class;
                        clanMember.Level = member.Level;
                        clanMember.Kills = member.Kills;
                        clanMember.Deaths = member.Deaths;
                        // TODO Notes
                        clanMember.ClanRank = (int)GetClanRank(member.Name, clan);
                    }

                    // Ranks

                    // Items

                    _dbContext.AddOrUpdate<DAL.Models.Clan>(clanToSave);
                }
            }
            catch (DbException ex)
            {
                _logManager.Error(ex);
                throw;
            }
        }

        private ClanRanks GetClanRank(string name, ClanData clan)
        {
            if (clan.Leader == name) return ClanRanks.Leader;
            if (clan.NumberOne == name) return ClanRanks.NumberOne;
            if (clan.NumberTwo == name) return ClanRanks.NumberTwo;
            return ClanRanks.Member;
        }

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
