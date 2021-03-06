﻿using Realm.Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SmaugCS.Data.Templates
{
    public class RoomTemplate : Template, IHasExtraDescriptions
    {
        public ICollection<ResetData> Resets { get; }
        public ResetData LastMobReset { get; set; }
        public ResetData LastObjectReset { get; set; }
        public ICollection<CharacterInstance> Persons { get; }
        public ICollection<ObjectInstance> Contents { get; }
        public ICollection<ExtraDescriptionData> ExtraDescriptions { get; }
        public AreaData Area { get; set; }
        public ICollection<ExitData> Exits { get; }
        public ICollection<AffectData> PermanentAffects { get; private set; }
        public ICollection<AffectData> Affects { get; }
        public PlaneData plane { get; set; }
        public ICollection<MudProgActData> MudProgActs { get; private set; }
        public int Flags { get; set; }

        public int mpactnum { get; set; }
        public long TeleportToVnum { get; set; }

        public int Light
        {
            get
            {
                int total = 0;

                // Light sum of any objects in the room and any affects on those objects
                if (Contents.Any())
                {
                    total += Contents.Where(x => x.ItemType == ItemTypes.Light)
                            .Sum(item => item.Values.HoursLeft * item.Count);

                    total += Contents.Sum(item => item.Affects.Where(affect => affect.Location == ApplyTypes.RoomLight)
                                    .Sum(affect => affect.Modifier));
                }

                // Light sum of affects on persons in the room, any light objects worn by those persons, 
                // and any affects on objects being worn by those persons
                if (Persons.Any())
                {
                    foreach (var person in Persons)
                    {
                        total += person.Affects.Where(affect => affect.Location == ApplyTypes.RoomLight)
                            .Sum(affect => affect.Modifier);

                        foreach (var item in person.Carrying.Where(item => item.WearLocation != WearLocations.None))
                        {
                            total += item.Affects.Where(affect => affect.Location == ApplyTypes.RoomLight)
                                .Sum(affect => affect.Modifier);
                            if (item.ItemType == ItemTypes.Light)
                                total += item.Values.HoursLeft * item.Count;
                        }
                    }
                }

                // Light sum of any affects on the room itself
                if (Affects.Any())
                {
                    total += Affects.Where(affect => affect.Location == ApplyTypes.RoomLight)
                        .Sum(affect => affect.Modifier);
                }

                return total;
            }
        }

        public SectorTypes SectorType { get; set; }
        public SectorTypes WinterSector { get; set; }
        public int mpscriptpos { get; set; }
        public int TeleportDelay { get; set; }
        public int Tunnel { get; set; }

        public RoomTemplate(long id, string name)
            : base(id, name)
        {
            Resets = new List<ResetData>();
            Persons = new List<CharacterInstance>();
            Contents = new List<ObjectInstance>();
            ExtraDescriptions = new List<ExtraDescriptionData>();
            Exits = new List<ExitData>();
            PermanentAffects = new List<AffectData>();
            Affects = new List<AffectData>();
            MudProgActs = new List<MudProgActData>();

            SectorType = SectorTypes.Inside;
        }

        #region Affects

        public void AddAffect(AffectData affect) => Affects.Add(affect);

        public void RemoveAffect(AffectData affect)
        {
            if (Affects.Contains(affect))
                Affects.Remove(affect);
        }

        #endregion

        #region Exits

        public ExitData GetExit(string arg)
        {
            var exit = Exits.FirstOrDefault(x => x.Keywords.ContainsIgnoreCase(arg));
            if (exit != null) return exit;
            var dir = Realm.Library.Common.Extensions.EnumerationExtensions.GetEnumByName<DirectionTypes>(arg);
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
            DirectionTypes dir = Realm.Library.Common.Extensions.EnumerationExtensions.GetEnumIgnoreCase<DirectionTypes>(direction);
            if (Exits.Any(x => x.Direction == dir))
                return;

            ExitData newExit = new ExitData((int)dir, direction)
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
            var newReset = new ResetData
            {
                Type = Realm.Library.Common.Extensions.EnumerationExtensions.GetEnumIgnoreCase<ResetTypes>(type),
                Extra = extra,
                Command = type[0].ToString()
            };
            newReset.SetArgs(arg1, arg2, arg3);
            Resets.Add(newReset);
        }

        public void SetSector(string sector)
            => SectorType = Realm.Library.Common.Extensions.EnumerationExtensions.GetEnum<SectorTypes>(sector.CapitalizeFirst());

        public void SetFlags(string flags)
        {
            var words = flags.Split(' ');
            foreach (var word in words)
            {
                Flags += (int)Realm.Library.Common.Extensions.EnumerationExtensions.GetEnumIgnoreCase<RoomFlags>(word);
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
}
