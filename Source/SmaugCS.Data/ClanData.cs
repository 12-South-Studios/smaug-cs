using Library.Common.Extensions;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace SmaugCS.Data;

[XmlRoot("Clan")]
public class ClanData(long id, string name) : OrganizationData(id, name)
{
  [XmlElement] public ClanTypes ClanType { get; set; }

  [XmlArray] public IEnumerable<RosterData> Members { get; set; }

  public string Motto { get; set; }
  public string Deity { get; set; }
  public string NumberOne { get; set; }
  public string NumberTwo { get; set; }

  [XmlElement] public string Badge { get; set; }

  public string LeaderRank { get; set; }
  public string NumberOneRank { get; set; }
  public string NumberTwoRank { get; set; }
  public IEnumerable<int> PvPKillTable { get; private set; } = new List<int>(7);
  public IEnumerable<int> PvPDeathTable { get; private set; } = new List<int>(7);
  public int PvEKills { get; set; }
  public int PvEDeaths { get; set; }
  public int IllegalPvPKill { get; set; }
  public int Score { get; set; }

  public int Favour { get; set; }
  public int Strikes { get; set; }
  public int MemberLimit { get; set; }
  public int Alignment { get; set; }
  public IEnumerable<int> ClanObjects { get; set; } = new List<int>(5);

  [XmlElement] public int RecallRoom { get; set; }

  public int StoreRoom { get; set; }
  public int GuardOne { get; set; }
  public int GuardTwo { get; set; }
  public int Class { get; set; }
  public bool Saved { get; set; }

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

  public void SetTypeByValue(int type) =>
    ClanType = EnumerationExtensions.GetEnum<ClanTypes>(type);
}