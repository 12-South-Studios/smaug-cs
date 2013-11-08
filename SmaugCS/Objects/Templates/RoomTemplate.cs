using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Enums;
using SmaugCS.Interfaces;
using SmaugCS.Managers;

// ReSharper disable CheckNamespace
namespace SmaugCS.Objects
// ReSharper restore CheckNamespace
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
        public plane_data plane { get; set; }
        public List<MudProgActData> MudProgActs { get; set; }
        public ExtendedBitvector Flags { get; set; }

        public int mpactnum { get; set; }
        public int TeleportToVnum { get; set; }
        public int Light { get; set; }
        public SectorTypes SectorType { get; set; }
        public SectorTypes WinterSector { get; set; }
        public int mpscriptpos { get; set; }
        public int TeleportDelay { get; set; }
        public int Tunnel { get; set; }

        public RoomTemplate()
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

        public ExitData GetExitTo(int dir, int vnumTo)
        {
            return Exits.FirstOrDefault(exit => exit.vdir == dir && exit.vnum == vnumTo);
        }

        public ExitData GetExitNumber(int count)
        {
            int x = 0;
            return Exits.FirstOrDefault(exit => ++x == count);
        }

        #endregion

        public bool IsDark()
        {
            if (Light > 0)
                return false;

            if (Flags.IsSet((int)RoomFlags.Dark))
                return true;

            if (SectorType == SectorTypes.Inside
                || SectorType == SectorTypes.City)
                return false;

            return db.GameTime.Sunlight == SunPositionTypes.Sunset
                   || db.GameTime.Sunlight == SunPositionTypes.Dark;
        }

        public bool IsPrivate()
        {
            int count = Persons.Count();

            return (Flags.IsSet((int)RoomFlags.Private) && count >= 2)
                   || (Flags.IsSet((int)RoomFlags.Solitary) && count >= 1);
        }

        public void FromRoom(CharacterInstance ch)
        {
            if (ch.CurrentRoom != this)
            {
                LogManager.Bug("Character {0} is not in Room {1}", ch.Name, Vnum);
                return;
            }

            if (!ch.IsNpc())
                --Area.NumberOfPlayers;

            ObjectInstance obj = ch.GetEquippedItem((int)WearLocations.Light);
            if (obj != null
                && obj.ItemType == ItemTypes.Light
                && obj.Value[2] != 0
                && Light > 0)
                --Light;

            foreach (AffectData affect in ch.Affects)
                RemoveAffect(affect);

            foreach (AffectData affect in Affects
                .Where(affect => affect.Location != ApplyTypes.WearSpell
                    && affect.Location != ApplyTypes.RemoveSpell
                    && affect.Location != ApplyTypes.StripSN))
                ch.RemoveAffect(affect);

            Persons.Remove(ch);
            ch.PreviousRoom = this;
            ch.CurrentRoom = null;

            if (!ch.IsNpc() && handler.get_timer(ch, (int)TimerTypes.ShoveDrag) > 0)
                handler.remove_timer(ch, (int)TimerTypes.ShoveDrag);
        }

        public void ToRoom(CharacterInstance ch)
        {
            RoomTemplate localRoom = this;

            if (ch == null)
            {
                LogManager.Bug("%s: NULL ch!", "char_to_room");
                return;
            }

            if (DatabaseManager.Instance.ROOMS.Get(Vnum) == null)
            {
                LogManager.Bug("%s: %s -> NULL room! Putting char in limbo (%d)",
                    "char_to_room", ch.Name, Program.ROOM_VNUM_LIMBO);
                localRoom = DatabaseManager.Instance.ROOMS.Get(Program.ROOM_VNUM_LIMBO);
            }

            ch.CurrentRoom = localRoom;
            if (ch.HomeVNum < 1)
                ch.HomeVNum = localRoom.Vnum;
            localRoom.Persons.Add(ch);

            if (!ch.IsNpc())
            {
                localRoom.Area.NumberOfPlayers += 1;
                if (localRoom.Area.NumberOfPlayers > localRoom.Area.MaximumPlayers)
                    localRoom.Area.MaximumPlayers = localRoom.Area.NumberOfPlayers;
            }

            ObjectInstance light = ch.GetEquippedItem(WearLocations.Light);
            if (light != null && light.ItemType == ItemTypes.Light
                && light.Value[2] > 0)
                localRoom.Light++;

            foreach (AffectData affect in localRoom.Affects
                .Where(affect => affect.Location != ApplyTypes.WearSpell
                    && affect.Location != ApplyTypes.RemoveSpell
                    && affect.Location != ApplyTypes.StripSN))
                ch.AddAffect(affect);

            foreach (AffectData affect in localRoom.PermanentAffects
                .Where(affect => affect.Location != ApplyTypes.WearSpell
                    && affect.Location != ApplyTypes.RemoveSpell
                    && affect.Location != ApplyTypes.StripSN))
                ch.AddAffect(affect);

            foreach (AffectData affect in ch.Affects)
                localRoom.AddAffect(affect);

            if (!ch.IsNpc()
                && localRoom.Flags.IsSet((int)RoomFlags.Safe)
                && handler.get_timer(ch, (int)TimerTypes.ShoveDrag) <= 0)
                handler.add_timer(ch, (int)TimerTypes.ShoveDrag, 10, null, 0);

            if (localRoom.Flags.IsSet((int)RoomFlags.Teleport)
                && localRoom.TeleportDelay > 0)
            {
                if (db.TELEPORT.Exists(x => x.Room == localRoom))
                    return;

                db.TELEPORT.Add(new TeleportData
                                    {
                                        Room = localRoom,
                                        Timer = (short)localRoom.TeleportDelay
                                    });
            }

            if (ch.PreviousRoom == null)
                ch.PreviousRoom = ch.CurrentRoom;
        }

        public CharacterInstance IsDoNotDisturb(CharacterInstance ch)
        {
            if (Flags.IsSet((int)RoomFlags.DoNotDisturb))
                return null;

            return Persons.FirstOrDefault(rch => !rch.IsNpc()
                && rch.PlayerData != null
                && rch.IsImmortal()
                && rch.PlayerData.Flags.IsSet((int)PCFlags.DoNotDisturb)
                && ch.Trust < rch.Trust
                && handler.can_see(ch, rch));
        }

        public void FromRoom(ObjectInstance obj)
        {
            if (obj.InRoom != this)
            {
                LogManager.Bug("Object {0} is not in Room {1}", obj.Name, Vnum);
                return;
            }

            foreach (AffectData paf in obj.Affects)
                RemoveAffect(paf);
            foreach (AffectData paf in obj.ObjectIndex.Affects)
                RemoveAffect(paf);

            Contents.Remove(obj);

            if (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Covering)
                && obj.Contents != null)
                handler.empty_obj(obj, null, this);

            if (obj.ItemType == ItemTypes.Fire)
                obj.InRoom.Light -= obj.Count;

            obj.CarriedBy = null;
            obj.InObject = null;
            obj.InRoom = null;

            if (obj.ObjectIndex.Vnum == Program.OBJ_VNUM_CORPSE_PC && handler.falling > 0)
                save.write_corpses(null, obj.ShortDescription, obj);
        }

        public ObjectInstance ToRoom(ObjectInstance obj)
        {
            foreach (AffectData paf in obj.Affects)
                AddAffect(paf);
            foreach (AffectData paf in obj.ObjectIndex.Affects)
                AddAffect(paf);

            int count = obj.Count;
            ItemTypes itemType = obj.ItemType;

            foreach (ObjectInstance otmp in Contents)
            {
                ObjectInstance oret = handler.group_object(otmp, obj);
                if (oret == otmp)
                {
                    if (itemType == ItemTypes.Fire)
                        Light += count;
                    return oret;
                }
            }

            Contents.Add(obj);
            obj.InRoom = this;
            obj.CarriedBy = null;
            obj.InObject = null;
            obj.room_vnum = Vnum;
            if (itemType == ItemTypes.Fire)
                Light += count;
            handler.falling++;
            act_obj.obj_fall(obj, false);
            handler.falling--;
            if (obj.ObjectIndex.Vnum == Program.OBJ_VNUM_CORPSE_PC && handler.falling < 1)
                save.write_corpses(null, obj.ShortDescription, null);
            return obj;
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

        public void SaveFUSS(TextWriterProxy proxy, bool install)
        {
            if (install)
            {
                Flags.RemoveBit((int) RoomFlags.Prototype);
                foreach (CharacterInstance victim in Persons.Where(x => x.IsNpc()))
                {
                    handler.extract_char(victim, true);
                }

                foreach (ObjectInstance obj in Contents)
                {
                    handler.extract_obj(obj);
                }
            }

            Flags.RemoveBit((int) RoomFlags.BfsMark);

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

            foreach(AffectData af in PermanentAffects)
                af.SaveFUSS(proxy);

            foreach(ExtraDescriptionData ed in ExtraDescriptions)
                ed.SaveFUSS(proxy);

            foreach (MudProgData mp in MudProgs)
                mp.Save(proxy);

            proxy.Write("#ENDROOM\n\n");
        }
    }
}
