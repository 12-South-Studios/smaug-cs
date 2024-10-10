using Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SmaugCS.Data.Templates;

public class RoomTemplate(long id, string name) : Template(id, name), IHasExtraDescriptions
{
  public ICollection<ResetData> Resets { get; } = new List<ResetData>();
  public ResetData LastMobReset { get; set; }
  public ResetData LastObjectReset { get; set; }
  public ICollection<CharacterInstance> Persons { get; } = new List<CharacterInstance>();
  public ICollection<ObjectInstance> Contents { get; } = new List<ObjectInstance>();
  public ICollection<ExtraDescriptionData> ExtraDescriptions { get; } = new List<ExtraDescriptionData>();
  public AreaData Area { get; set; }
  public ICollection<ExitData> Exits { get; } = new List<ExitData>();
  public ICollection<AffectData> PermanentAffects { get; private set; } = new List<AffectData>();
  public ICollection<AffectData> Affects { get; } = new List<AffectData>();
  public PlaneData plane { get; set; }
  public ICollection<MudProgActData> MudProgActs { get; private set; } = new List<MudProgActData>();
  public int Flags { get; set; }

  public int mpactnum { get; set; }
  public long TeleportToVnum { get; set; }

  public int Light
  {
    get
    {
      int total = 0;

      // Light sum of any objects in the room and any affects on those objects
      if (Contents.Count != 0)
      {
        total += Contents.Where(x => x.ItemType == ItemTypes.Light)
          .Sum(item => item.Values.HoursLeft * item.Count);

        total += Contents.Sum(item => item.Affects.Where(affect => affect.Location == ApplyTypes.RoomLight)
          .Sum(affect => affect.Modifier));
      }

      // Light sum of affects on persons in the room, any light objects worn by those persons,          // and any affects on objects being worn by those persons
      if (Persons.Count != 0)
      {
        foreach (CharacterInstance person in Persons)
        {
          total += person.Affects.Where(affect => affect.Location == ApplyTypes.RoomLight)
            .Sum(affect => affect.Modifier);

          foreach (ObjectInstance item in person.Carrying.Where(item => item.WearLocation != WearLocations.None))
          {
            total += item.Affects.Where(affect => affect.Location == ApplyTypes.RoomLight)
              .Sum(affect => affect.Modifier);
            if (item.ItemType == ItemTypes.Light)
              total += item.Values.HoursLeft * item.Count;
          }
        }
      }

      // Light sum of any affects on the room itself
      if (Affects.Count != 0)
      {
        total += Affects.Where(affect => affect.Location == ApplyTypes.RoomLight)
          .Sum(affect => affect.Modifier);
      }

      return total;
    }
  }

  public SectorTypes SectorType { get; set; } = SectorTypes.Inside;
  public SectorTypes WinterSector { get; set; }
  public int mpscriptpos { get; set; }
  public int TeleportDelay { get; set; }
  public int Tunnel { get; set; }

  #region Affects

  public void AddAffect(AffectData affect) => Affects.Add(affect);

  public void RemoveAffect(AffectData affect)
  {
    Affects.Remove(affect);
  }

  #endregion

  #region Exits

  public ExitData GetExit(string arg)
  {
    ExitData exit = Exits.FirstOrDefault(x => x.Keywords.ContainsIgnoreCase(arg));
    if (exit != null) return exit;
    DirectionTypes dir = Library.Common.Extensions.EnumerationExtensions.GetEnumByName<DirectionTypes>(arg);
    exit = GetExit(dir);
    return exit;
  }

  public ExitData GetExit(int dir) => Exits.FirstOrDefault(exit => (int)exit.Direction == dir);

  public ExitData GetExit(DirectionTypes dir) => Exits.FirstOrDefault(exit => exit.Direction == dir);

  public ExitData GetExitTo(int dir, long vnumTo)
    => Exits.FirstOrDefault(exit => (int)exit.Direction == dir && exit.vnum == vnumTo);

  public ExitData GetExitNumber(int count)
  {
    int x = 0;
    return Exits.FirstOrDefault(exit => ++x == count);
  }

  public void AddExit(string direction, long destination, string description)
  {
    DirectionTypes dir =
      Library.Common.Extensions.EnumerationExtensions.GetEnumIgnoreCase<DirectionTypes>(direction);
    if (Exits.Any(x => x.Direction == dir))
      return;

    ExitData newExit = new((int)dir, direction)
    {
      Destination = destination,
      Description = description,
      Direction = dir,
      Keywords = direction
    };
    Exits.Add(newExit);
  }

  public void AddExitObject(ExitData exit)
  {
    if (Exits.Any(x => x.Direction == exit.Direction))
      return;

    Exits.Add(exit);
  }

  #endregion

  public bool IsPrivate() => (Flags.IsSet(RoomFlags.Private) && Persons.Count >= 2)
                             || (Flags.IsSet(RoomFlags.Solitary) && Persons.Count >= 1);

  #region IHasExtraDescriptions Implementation

  public void AddExtraDescription(string keywords, string description)
  {
    string[] words = keywords.Split(' ');
    foreach (string word in words)
    {
      ExtraDescriptionData foundEd = ExtraDescriptions.FirstOrDefault(ed => ed.Keyword.EqualsIgnoreCase(word));
      if (foundEd != null) continue;

      foundEd = new ExtraDescriptionData
      {
        Keyword = keywords,
        Description = description
      };
      ExtraDescriptions.Add(foundEd);
    }
  }

  public bool DeleteExtraDescription(string keyword)
  {
    ExtraDescriptionData foundEd = ExtraDescriptions.FirstOrDefault(ed => ed.Keyword.EqualsIgnoreCase(keyword));
    if (foundEd == null)
      return false;

    ExtraDescriptions.Remove(foundEd);
    return true;
  }

  public ExtraDescriptionData GetExtraDescription(string keyword)
    => ExtraDescriptions.FirstOrDefault(ed => ed.Keyword.EqualsIgnoreCase(keyword));

  #endregion

  public void AddReset(ResetData reset) => Resets.Add(reset);

  public void AddReset(string type, int extra, int arg1, int arg2, int arg3)
  {
    ResetData newReset = new()
    {
      Type = Library.Common.Extensions.EnumerationExtensions.GetEnumIgnoreCase<ResetTypes>(type),
      Extra = extra,
      Command = type[0].ToString()
    };
    newReset.SetArgs(arg1, arg2, arg3);
    Resets.Add(newReset);
  }

  public void SetSector(string sector)
    => SectorType =
      Library.Common.Extensions.EnumerationExtensions.GetEnum<SectorTypes>(sector.CapitalizeFirst());

  public void SetFlags(string flags)
  {
    string[] words = flags.Split(' ');
    foreach (string word in words)
    {
      Flags += (int)Library.Common.Extensions.EnumerationExtensions.GetEnumIgnoreCase<RoomFlags>(word);
    }
  }
  /*public void SaveFUSS(TextWriterProxy proxy, bool install)
  {
      if (install)
      {
          Flags.RemoveBit((int)RoomFlags.Prototype);
          foreach (CharacterInstance victim in Persons.Where(x => x.IsNpc()))
          {
              CharacterInstanceExtensions.Extract(victim, true);
          }

          foreach (ObjectInstance obj in Contents)
          {
              ObjectInstanceExtensions.Extract(obj);
          }
      }

      Flags.RemoveBit((int)RoomFlags.BfsMark);

      proxy.Write("#ROOM\n");
      proxy.Write("Vnum     {0}\n", Vnum);
      proxy.Write("Name     {0}~\n", Name);
      proxy.Write("Sector   {0}~\n", BuilderConstants.sec_flags[(int)SectorType]);
      if (!Flags.IsEmpty())
          proxy.Write("Flags    {0}~\n", Flags.GetFlagString(BuilderConstants.r_flags));
      if (TeleportDelay > 0 || TeleportToVnum > 0 || Tunnel > 0)
          proxy.Write("STats    {0} {1} {2}\n", TeleportDelay, TeleportToVnum, Tunnel);
      if (!Description.IsNullOrEmpty())
          proxy.Write("Desc     {0}~\n", Description);

      foreach (ExitData exit in Exits.Where(x => !x.Flags.IsSet((int)ExitFlags.Portal)))
      {
          exit.SaveFUSS(proxy);
      }

      build.save_reset_level(proxy, Resets, 0);

      foreach (AffectData af in PermanentAffects)
          af.SaveFUSS(proxy);

      foreach (ExtraDescriptionData ed in ExtraDescriptions)
          ed.SaveFUSS(proxy);

      foreach (MudProgData mp in MudProgs)
          mp.Save(proxy);

      proxy.Write("#ENDROOM\n\n");
  }*/
}