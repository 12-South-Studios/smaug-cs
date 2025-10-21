using SmaugCS.Common.Enumerations;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmaugCS.Data;

public class PlayerData
{
  public CharacterInstance Pet { get; set; }
  public ClanData Clan { get; set; }
  public CouncilData Council { get; set; }
  public AreaData BuilderArea { get; set; }
  public DeityData CurrentDeity { get; set; }
  public nuisance_data Nuisance { get; set; }
  public ICollection<KilledData> Killed { get; private set; }
  public AuthorizationStates AuthState { get; set; }
  public string AuthorizedBy { get; set; }

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
  public List<string> Bestowments { get; set; } = [];
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
  public Dictionary<ConditionTypes, int> ConditionTable { get; private set; }
  public List<Tuple<long, int>> Learned { get; private set; }
  public int quest_number { get; set; }
  public int quest_curr { get; set; }
  public int quest_accum { get; set; }
  public int Favor { get; set; }
  public int NumberOfCharmies { get; set; }
  public DateTime release_date { get; set; }
  public string helled_by { get; set; }
  public string bio { get; set; }

  public ICollection<SkillData> SpecialSkills { get; private set; }
  public string Prompt { get; set; }
  public string FPrompt { get; set; }
  public string SubPrompt { get; set; }
  public int PagerLineCount { get; set; }
  public ICollection<IgnoreData> Ignored { get; private set; }
  public ICollection<string> TellHistory { get; private set; }

  public int LastTellIndex { get; set; }

  //public imc_char_data imcchardata { get; set; }
  public bool hotboot { get; set; }
  public int age_bonus { get; set; }
  public int age { get; set; }
  public int Day { get; set; }
  public int Month { get; set; }
  public int Year { get; set; }
  public int timezone { get; set; }

  public PlayerData(int maxSkills, int maxPersonal)
  {
    Killed = [];
    ConditionTable = [];
    Learned = new List<Tuple<long, int>>(maxSkills);
    SpecialSkills = new SkillData[maxPersonal];
    TellHistory = [];
    Ignored = [];
    SpecialSkills = [];
  }

  public int GetConditionValue(ConditionTypes condition)
  {
    return ConditionTable.GetValueOrDefault(condition, 0);
  }

  public void SetConditionValue(ConditionTypes condition, int value)
  {
    ConditionTable[condition] = value;
  }

  public void ClearLearnedSkills()
  {
    IEnumerable<long> learnedIDs = Learned.Select(x => x.Item1);
    Learned.Clear();

    foreach (long learnedId in learnedIDs)
      Learned.Add(new Tuple<long, int>(learnedId, 0));
  }

  public int GetSkillMastery(long skillID)
  {
    return Learned.All(x => x.Item1 != skillID) ? 0 : Learned.First(x => x.Item1 == skillID).Item2;
  }

  public void UpdateSkillMastery(long skillId, int value, bool additive = false)
  {
    int newValue = value;
    if (Learned.Any(x => x.Item1 == skillId))
    {
      if (additive)
      {
        Tuple<long, int> learned = Learned.First(x => x.Item1 == skillId);
        newValue += learned.Item2;
      }

      Learned.Remove(Learned.First(x => x.Item1 == skillId));
    }

    Learned.Add(new Tuple<long, int>(skillId, newValue));
  }
}