using System;
using System.Collections.Generic;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Organizations;

namespace SmaugCS.Data
{
    public class PlayerData
    {
        public static PlayerData Create(int maxSkills, int maxPersonal)
        {
            return new PlayerData(maxSkills, maxPersonal);
        }

        public CharacterInstance Pet { get; set; }
        public ClanData Clan { get; set; }
        public CouncilData Council { get; set; }
        public AreaData BuilderArea { get; set; }
        public DeityData CurrentDeity { get; set; }
        //public BoardData game_board { get; set; }
        public nuisance_data Nuisance { get; set; }
        public List<KilledData> Killed { get; private set; }

        public string homepage { get; set; }
        public string clan_name { get; set; }
        public string council_name { get; set; }
        public string deity_name { get; set; }
        public string pwd { get; set; }
        public string bamfin { get; set; }
        public string bamfout { get; set; }
        public string filename { get; set; }
        public string rank { get; set; }
        public string Title { get; set; }
        public string bestowments { get; set; }
        public DateTime outcast_time { get; set; }
        public DateTime restore_time { get; set; }
        public int Flags { get; set; }
        public int PvPKills { get; set; }
        public int PvPDeaths { get; set; }
        public int PvEKills { get; set; }
        public int PvEDeaths { get; set; }
        public int IllegalPvPKill { get; set; }
        public int r_range_lo { get; set; }
        public int r_range_hi { get; set; }
        public int m_range_lo { get; set; }
        public int m_range_hi { get; set; }
        public int o_range_lo { get; set; }
        public int o_range_hi { get; set; }
        public int WizardInvisible { get; set; }
        public int min_snoop { get; set; }
        public Dictionary<ConditionTypes, int> ConditionTable { get; set; }
        public int[] Learned { get; set; }
        public int quest_number { get; set; }
        public int quest_curr { get; set; }
        public int quest_accum { get; set; }
        public int Favor { get; set; }
        public int NumberOfCharmies { get; set; }
        public int AuthState { get; set; }
        public DateTime release_date { get; set; }
        public string helled_by { get; set; }
        public string bio { get; set; }
        public string authed_by { get; set; }
        public SkillData[] special_skills { get; set; }
        public string Prompt { get; set; }
        public string FPrompt { get; set; }
        public string SubPrompt { get; set; }
        public int PagerLineCount { get; set; }
        public List<IgnoreData> Ignored { get; set; }
        public List<string> TellHistory { get; set; }
        public int LastTellIndex { get; set; }
        //public imc_char_data imcchardata { get; set; }
        public bool hotboot { get; set; }
        public int age_bonus { get; set; }
        public int age { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int timezone { get; set; }

        private PlayerData(int maxSkills, int maxPersonal)
        {
            Killed = new List<KilledData>();
            ConditionTable = new Dictionary<ConditionTypes, int>();
            Learned = new int[maxSkills];
            special_skills = new SkillData[maxPersonal];
            TellHistory = new List<string>();
        }

        public int GetConditionValue(ConditionTypes condition)
        {
            return ConditionTable.ContainsKey(condition)
                       ? ConditionTable[condition]
                       : 0;
        }
        public void SetConditionValue(ConditionTypes condition, int value)
        {
            ConditionTable[condition] = value;
        }
    }
}
