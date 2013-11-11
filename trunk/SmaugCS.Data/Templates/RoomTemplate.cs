using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Interfaces;

namespace SmaugCS.Data.Templates
{
    public class RoomTemplate : Template, IHasExtraDescriptions
    {
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
        public ExtendedBitvector Flags { get; set; }

        public int mpactnum { get; set; }
        public long TeleportToVnum { get; set; }
        public int Light { get; set; }
        public SectorTypes SectorType { get; set; }
        public SectorTypes WinterSector { get; set; }
        public int mpscriptpos { get; set; }
        public int TeleportDelay { get; set; }
        public int Tunnel { get; set; }

        public RoomTemplate(long id, string name)
            : base(id, name)
        {
            Flags = new ExtendedBitvector();
            Resets = new List<ResetData>();
            Persons = new List<CharacterInstance>();
            Contents = new List<ObjectInstance>();
            ExtraDescriptions = new List<ExtraDescriptionData>();
            Exits = new List<ExitData>();
            PermanentAffects = new List<AffectData>();
            Affects = new List<AffectData>();
            MudProgActs = new List<MudProgActData>();
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
            return Exits.FirstOrDefault(exit => exit.vdir == dir);
        }

        public ExitData GetExit(DirectionTypes dir)
        {
            return Exits.FirstOrDefault(exit => exit.vdir == (int)dir);
        }

        public ExitData GetExitTo(int dir, long vnumTo)
        {
            return Exits.FirstOrDefault(exit => exit.vdir == dir && exit.vnum == vnumTo);
        }

        public ExitData GetExitNumber(int count)
        {
            int x = 0;
            return Exits.FirstOrDefault(exit => ++x == count);
        }

        #endregion

        public bool IsPrivate()
        {
            int count = Persons.Count();

            return (Flags.IsSet((int)RoomFlags.Private) && count >= 2)
                   || (Flags.IsSet((int)RoomFlags.Solitary) && count >= 1);
        }

        #region IHasExtraDescriptions Implementation
        public ExtraDescriptionData Add(string keywords)
        {
            ExtraDescriptionData foundEd = ExtraDescriptions.FirstOrDefault(ed => ed.Keyword.IsEqual(keywords));
            if (foundEd == null)
            {
                foundEd = new ExtraDescriptionData { Keyword = keywords, Description = "" };
                ExtraDescriptions.Add(foundEd);
            }

            return foundEd;
        }

        public bool Delete(string keywords)
        {
            ExtraDescriptionData foundEd = ExtraDescriptions.FirstOrDefault(ed => ed.Keyword.EqualsIgnoreCase(keywords));
            if (foundEd == null)
                return false;

            ExtraDescriptions.Remove(foundEd);
            return true;
        }
        #endregion

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
    }
}
