using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;


// ReSharper disable CheckNamespace
namespace SmaugCS.Data
// ReSharper restore CheckNamespace
{
    public class RoomTemplate : Template, IHasExtraDescriptions
    {
        public static RoomTemplate Create(long id, string name)
        {
            return new RoomTemplate(id, name);
        }

        public List<ResetData> Resets { get; set; }
        public ResetData LastMobReset { get; set; }
        public ResetData LastObjectReset { get; set; }
        public List<CharacterInstance> Persons { get; set; }
        public List<ObjectInstance> Contents { get; set; }
        public List<ExtraDescriptionData> ExtraDescriptions { get; set; }
        public AreaData Area { get; set; }
        public List<ExitData> Exits { get; set; }
        public List<AffectData> PermanentAffects { get; set; }
        public List<AffectData> Affects { get; set; }
        public PlaneData plane { get; set; }
        public List<MudProgActData> MudProgActs { get; set; }
        public int Flags { get; set; }

        public int mpactnum { get; set; }
        public long TeleportToVnum { get; set; }
        public int Light { get; set; }
        public SectorTypes SectorType { get; set; }
        public SectorTypes WinterSector { get; set; }
        public int mpscriptpos { get; set; }
        public int TeleportDelay { get; set; }
        public int Tunnel { get; set; }

        private RoomTemplate(long id, string name)
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

        public void AddAffect(AffectData affect)
        {
            if (affect.Location == ApplyTypes.RoomLight)
                Light += affect.Modifier;
        }

        public void RemoveAffect(AffectData affect)
        {
            if (affect.Location == ApplyTypes.RoomLight)
                Light -= affect.Modifier;
        }

        #endregion

        #region Exits

        public ExitData GetExit(int dir)
        {
            return Exits.FirstOrDefault(exit => (int)exit.Direction == dir);
        }

        public ExitData GetExit(DirectionTypes dir)
        {
            return Exits.FirstOrDefault(exit => exit.Direction == dir);
        }

        public ExitData GetExitTo(int dir, long vnumTo)
        {
            return Exits.FirstOrDefault(exit => (int)exit.Direction == dir && exit.vnum == vnumTo);
        }

        public ExitData GetExitNumber(int count)
        {
            int x = 0;
            return Exits.FirstOrDefault(exit => ++x == count);
        }

        public void AddExit(string direction, long destination, string description)
        {
            DirectionTypes dir = Realm.Library.Common.EnumerationExtensions.GetEnumIgnoreCase<DirectionTypes>(direction);
            if (Exits.Any(x => x.Direction == dir))
                return;

            ExitData newExit = ExitData.Create((int)dir, direction);
            newExit.Destination = destination;
            newExit.Description = description;
            newExit.Direction = dir;
            newExit.Keywords = direction;
            Exits.Add(newExit);
        }
        public void AddExitObject(ExitData exit)
        {
            if (Exits.Any(x => x.Direction == exit.Direction))
                return;

            Exits.Add(exit);
        }
        #endregion

        public bool IsPrivate()
        {
            int count = Persons.Count();

            return (Flags.IsSet(RoomFlags.Private) && count >= 2)
                   || (Flags.IsSet(RoomFlags.Solitary) && count >= 1);
        }

        #region IHasExtraDescriptions Implementation
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="description"></param>
        public void AddExtraDescription(string keywords, string description)
        {
            string[] words = keywords.Split(new[] { ' ' });
            foreach (string word in words)
            {
                ExtraDescriptionData foundEd = ExtraDescriptions.FirstOrDefault(ed => ed.Keyword.EqualsIgnoreCase(word));
                if (foundEd == null)
                {
                    foundEd = ExtraDescriptionData.Create();
                    foundEd.Keyword = keywords;
                    foundEd.Description = description;
                    ExtraDescriptions.Add(foundEd);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public bool DeleteExtraDescription(string keyword)
        {
            ExtraDescriptionData foundEd = ExtraDescriptions.FirstOrDefault(ed => ed.Keyword.EqualsIgnoreCase(keyword));
            if (foundEd == null)
                return false;

            ExtraDescriptions.Remove(foundEd);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public ExtraDescriptionData GetExtraDescription(string keyword)
        {
            return ExtraDescriptions.FirstOrDefault(ed => ed.Keyword.EqualsIgnoreCase(keyword));
        }

        #endregion

        public void AddReset(ResetData reset)
        {
            Resets.Add(reset);
        }

        public void AddReset(string type, int extra, int arg1, int arg2, int arg3)
        {
            ResetTypes resetType = Realm.Library.Common.EnumerationExtensions.GetEnumIgnoreCase<ResetTypes>(type);
            ResetData newReset = ResetData.Create();
            newReset.Type = resetType;
            newReset.Extra = extra;
            newReset.Command = type[0].ToString();
            newReset.Args[0] = arg1;
            newReset.Args[1] = arg2;
            newReset.Args[2] = arg3;
            Resets.Add(newReset);
        }

        public void SetSector(string sector)
        {
            SectorType = Realm.Library.Common.EnumerationExtensions.GetEnum<SectorTypes>(sector.CapitalizeFirst());
        }

        public void SetFlags(string flags)
        {
            string[] words = flags.Split(new[] { ' ' });
            foreach (string word in words)
            {
                Flags += (int)Realm.Library.Common.EnumerationExtensions.GetEnumIgnoreCase<RoomFlags>(word);
            }

        }
        /*public void SaveFUSS(TextWriterProxy proxy, bool install)
        {
            if (install)
            {
                Flags.RemoveBit((int)RoomFlags.Prototype);
                foreach (CharacterInstance victim in Persons.Where(x => x.IsNpc()))
                {
                    handler.extract_char(victim, true);
                }

                foreach (ObjectInstance obj in Contents)
                {
                    handler.extract_obj(obj);
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

        internal void ProcessResets()
        {
            foreach (ResetData reset in Resets)
            {
                // TODO Do resets (and sub-resets) here
            }
        }
    }
}
