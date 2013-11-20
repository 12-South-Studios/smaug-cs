using System;
using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common.Extensions;
using SmaugCS.Commands;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions;
using SmaugCS.Managers;

namespace SmaugCS
{
    public static class act_wiz
    {
        private static int NumberOfHits { get; set; }

        /// <summary>
        /// Check if the name prefix uniquely identifies a char descriptor
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static CharacterInstance get_waiting_desc(CharacterInstance ch, string name)
        {
            CharacterInstance retChar = null;
            NumberOfHits = 0;

            foreach (DescriptorData d in db.DESCRIPTORS.Where(d => d.Character != null
                                                                  && d.Character.Name.EqualsIgnoreCase(name)
                                                                  && d.Character.IsWaitingForAuthorization()))
            {
                if (++NumberOfHits > 1)
                {
                    color.ch_printf(ch, "%s does not uniquely identify a char.\r\n", name);
                    return null;
                }

                retChar = d.Character;
            }

            if (NumberOfHits == 1)
                return retChar;

            color.send_to_char("No one like that waiting for authorization.\r\n", ch);
            return null;
        }

        public static void echo_to_all(ATTypes atColor, string argument, int tar)
        {
            if (string.IsNullOrWhiteSpace(argument))
                return;

            foreach (DescriptorData d in db.DESCRIPTORS
                .Where(x => x.ConnectionStatus == ConnectionTypes.Playing
                    || x.ConnectionStatus == ConnectionTypes.Editing))
            {
                if (tar == (int)EchoTypes.All && d.Character.IsNpc())
                    continue;
                if (tar == (int)EchoTypes.Immortal && !d.Character.IsImmortal())
                    continue;

                color.set_char_color(atColor, d.Character);
                color.send_to_char(argument, d.Character);
                color.send_to_char("\r\n", d.Character);
            }
        }

        public static void echo_to_room(ATTypes atcolor, RoomTemplate room, string argument)
        {
            foreach (CharacterInstance ch in room.Persons)
            {
                color.set_char_color(atcolor, ch);
                color.send_to_char(argument, ch);
                color.send_to_char("\r\n", ch);
            }
        }

        public static RoomTemplate find_location(CharacterInstance ch, string arg)
        {
            if (arg.IsNumber())
                return DatabaseManager.Instance.ROOMS.Get(arg.ToInt32());
            if (arg.Equals("pk"))
                return DatabaseManager.Instance.ROOMS.Get(db.LastPKRoom);

            CharacterInstance victim = handler.get_char_world(ch, arg);
            if (victim != null)
                return victim.CurrentRoom;

            ObjectInstance obj = handler.get_obj_world(ch, arg);
            if (obj != null)
                return obj.InRoom;

            return null;
        }

        /// <summary>
        ///This function shared by do_transfer and do_mptransfer 
        /// Immortals bypass most restrictions on where to transfer victims.
        /// NPCs cannot transfer victims who are:
        /// 1. Not authorized yet.
        /// 2. Outside of the level range for the target room's area.
        /// 3. Being sent to private rooms.
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="victim"></param>
        /// <param name="location"></param>
        public static void transfer_char(CharacterInstance ch, CharacterInstance victim, RoomTemplate location)
        {
            if (victim.CurrentRoom == null)
            {
                LogManager.Bug("Victim {0} in null room", victim.Name);
                return;
            }

            if (ch.IsNpc() && location.IsPrivate())
            {
                //progbug("Mptransfer - Private room", ch);
                return;
            }

            if (!handler.can_see(ch, victim))
                return;

            if (ch.IsNpc() && victim.IsNotAuthorized()
                && location.Area != victim.CurrentRoom.Area)
            {
                string buffer = string.Format("Mptransfer - unauthed char ({0})", victim.Name);
                //progbug(buffer, ch);
                return;
            }

            // if victim not in area's level range, do not transfer
            if (ch.IsNpc() && !handler.in_hard_range(victim, location.Area)
                && !location.Flags.IsSet((int)RoomFlags.Prototype))
                return;

            if (victim.CurrentFighting != null)
                fight.stop_fighting(victim, true);

            if (!ch.IsNpc())
            {
                comm.act(ATTypes.AT_MAGIC, "$n disappears in a cloud of swirling colors.", victim, null, null, ToTypes.Room);
                victim.retran = (int)victim.CurrentRoom.Vnum;
            }

            victim.CurrentRoom.FromRoom(victim);
            location.ToRoom(victim);

            if (!ch.IsNpc())
            {
                comm.act(ATTypes.AT_MAGIC, "$n arrives from a puff of smoke.", victim, null, null, ToTypes.Room);
                if (ch != victim)
                    comm.act(ATTypes.AT_IMMORT, "$n has transferred you.", ch, null, victim, ToTypes.Victim);
                Look.do_look(victim, "auto");
                if (!victim.IsImmortal()
                    && !victim.IsNpc()
                    && !handler.in_hard_range(victim, location.Area))
                    comm.act(ATTypes.AT_DANGER, "Warning: this player's level is not within the area's level range.", ch, null, null, ToTypes.Character);
            }
        }

        /// <summary>
        /// Extract area names from "input" string and returns the output
        /// e.g. "aset joe.are sedit susan.are cset" --> "joe.are susan.are"
        /// </summary>
        /// <param name="inp"></param>
        public static string extract_area_names(string inp)
        {
            string[] words = inp.Split(' ');
            string outBuf = string.Empty;

            outBuf = words.Where(word => word.EndsWith(".are", StringComparison.OrdinalIgnoreCase))
                          .Aggregate(outBuf, (current, word) => current + (" " + word));

            return outBuf;
        }

        /// <summary>
        /// Remove area names from "input" string and returns the output
        /// e.g. "aset joe.are sedit susan.are cset" --> "aset sedit cset"
        /// </summary>
        /// <param name="inp"></param>
        public static string remove_area_names(string inp)
        {
            string[] words = inp.Split(' ');
            string outBuf = string.Empty;

            outBuf = words.Where(word => !word.EndsWith(".are", StringComparison.OrdinalIgnoreCase))
                          .Aggregate(outBuf, (current, word) => current + (" " + word));

            return outBuf;
        }

        public static void close_area(AreaData pArea)
        {
            foreach (CharacterInstance ech in DatabaseManager.Instance.CHARACTERS.Values)
            {
                if (ech.CurrentFighting != null)
                    fight.stop_fighting(ech, true);
                if (ech.IsNpc())
                {
                    if ((ech.MobIndex.Vnum.GetNumberThatIsBetween(pArea.LowMobNumber, pArea.HighMobNumber) == ech.MobIndex.Vnum)
                        || (ech.CurrentRoom != null && ech.CurrentRoom.Area == pArea))
                        handler.extract_char(ech, true);
                    continue;
                }
                if (ech.CurrentRoom != null && ech.CurrentRoom.Area == pArea)
                {
                    //Recall.do_recall(ech, "");c
                }
            }

            foreach (ObjectInstance eobj in DatabaseManager.Instance.OBJECTS.Values)
            {
                if ((eobj.ObjectIndex.Vnum.GetNumberThatIsBetween(pArea.LowObjectNumber, pArea.HighObjectNumber) == eobj.ObjectIndex.Vnum)
                    || (eobj.InRoom != null && eobj.InRoom.Area == pArea))
                    handler.extract_obj(eobj);
            }

            for (int i = 0; i < Program.MAX_KEY_HASH; i++)
            {
                foreach (RoomTemplate room in db.ROOM_INDEX_HASH
                    .Where(room => room.Area == pArea))
                {
                    db.delete_room(room);
                }

                foreach (MobTemplate mob in db.MOB_INDEX_HASH
                    .Where(mob => mob.Vnum >= pArea.LowMobNumber && mob.Vnum <= pArea.HighMobNumber))
                {
                    db.delete_mob(mob);
                }

                foreach (ObjectTemplate obj in db.OBJECT_INDEX_HASH
                    .Where(obj => obj.Vnum >= pArea.LowObjectNumber && obj.Vnum <= pArea.HighObjectNumber))
                {
                    db.delete_obj(obj);
                }
            }

            if (pArea.Flags.IsSet((int)AreaFlags.Prototype))
            {
                db.BUILD_AREAS.Remove(pArea);
                // TODO Remove from bsort - do we need that?
            }
            else
            {
                db.AREAS.Remove(pArea);
                // TODO Remove from asort - do we need that?
            }
        }

        public static void close_all_areas()
        {
            db.AREAS.ForEach(close_area);
            db.BUILD_AREAS.ForEach(close_area);
        }

        public static void update_calendar()
        {
            db.SystemData.DaysPerYear = db.SystemData.DaysPerMonth * db.SystemData.MonthsPerYear;
            db.SystemData.HourOfSunrise = db.SystemData.HoursPerDay / 4;
            db.SystemData.HourOfDayBegin = db.SystemData.HourOfSunrise + 1;
            db.SystemData.HourOfNoon = db.SystemData.HoursPerDay / 2;
            db.SystemData.HourOfSunset = ((db.SystemData.HoursPerDay / 4) * 3);
            db.SystemData.HourOfNightBegin = db.SystemData.HourOfSunset + 1;
            db.SystemData.HourOfMidnight = db.SystemData.HoursPerDay;
            calendar.calc_season();
        }

        public static void update_timers()
        {
            db.SystemData.PulseTick = db.SystemData.SecondsPerTick * db.SystemData.PulsesPerSecond;
            db.SystemData.PulseViolence = 3 * db.SystemData.PulsesPerSecond;
            db.SystemData.PulseMobile = 4 * db.SystemData.PulsesPerSecond;
            db.SystemData.PulseCalendar = 4 * db.SystemData.PulseTick;
        }

        public static void get_reboot_string()
        {
            // TODO 
            // snprintf(reboot_time, 50, "%s", asctime(new_boot_time));
        }

        public static bool create_new_class(int rcindex, string argument)
        {
            if (rcindex >= db.CLASSES.Count || db.CLASSES[rcindex] == null)
                return false;

            ClassData cls = db.CLASSES[rcindex];

            cls.AffectedBy.ClearBits();
            cls.PrimaryAttribute = 0;
            cls.SecondaryAttribute = 0;
            cls.DeficientAttribute = 0;
            cls.Resistance = 0;
            cls.Susceptibility = 0;
            cls.Weapon = 0;
            cls.Guild = 0;
            cls.SkillAdept = 0;
            cls.ToHitArmorClass0 = 0;
            cls.ToHitArmorClass32 = 0;
            cls.MaximumHealth = 0;
            cls.MinimumHealth = 0;
            cls.UseMana = false;
            cls.BaseExperience = 1000;

            for (int i = 0; i < Program.MAX_LEVEL; i++)
            {
                Dictionary<string, string> titleMap = db.TITLES[rcindex.ToString()];
                titleMap[i.ToString()] = "Not set.";
            }

            return true;
        }

        public static bool create_new_race(int rcindex, string argument)
        {
            if (rcindex >= db.RACES.Count || db.RACES[rcindex] == null)
                return false;

            // snprintf?

            RaceData race = db.RACES[rcindex];
            race.ClassRestriction = 0;
            race.StrengthBonus = 0;
            race.DexterityBonus = 0;
            race.WisdomBonus = 0;
            race.IntelligenceBonus = 0;
            race.ConstitutionBonus = 0;
            race.CharismaBonus = 0;
            race.LuckBonus = 0;
            race.Health = 0;
            race.Mana = 0;
            race.AffectedBy.ClearBits();
            race.Resistance = 0;
            race.Susceptibility = 0;
            race.Language = 0;
            race.Alignment = 0;
            race.MinimumAlignment = 0;
            race.MaximumAlignment = 0;
            race.ArmorClassBonus = 0;
            race.ExperienceMultiplier = 100;
            race.Attacks.ClearBits();
            race.Defenses.ClearBits();
            race.Height = 0;
            race.Weight = 0;
            race.HungerMod = 0;
            race.ThirstMod = 0;
            race.ManaRegenRate = 0;
            race.HealthRegenRate = 0;
            race.RaceRecallRoom = 0;
            return true;
        }

        public static bool check_area_conflict(AreaData area, int lo, int hi)
        {
            if ((lo < area.LowRoomNumber && area.LowRoomNumber < hi)
                || (lo < area.LowMobNumber && area.LowMobNumber < hi)
                || (lo < area.LowObjectNumber && area.LowObjectNumber < hi))
                return true;

            if ((lo < area.HighRoomNumber && area.HighRoomNumber < hi)
                || (lo < area.HighMobNumber && area.HighMobNumber < hi)
                || (lo < area.HighObjectNumber && area.HighObjectNumber < hi))
                return true;

            if ((lo >= area.LowRoomNumber && lo <= area.HighRoomNumber)
                || (lo >= area.LowMobNumber && lo <= area.HighMobNumber)
                || (lo >= area.LowObjectNumber && lo <= area.HighObjectNumber))
                return true;

            if ((hi <= area.HighRoomNumber && hi >= area.LowRoomNumber)
                || (hi <= area.HighMobNumber && hi >= area.LowMobNumber)
                || (hi <= area.HighObjectNumber && hi >= area.LowObjectNumber))
                return true;

            return false;
        }

        public static bool check_area_conflicts(int lo, int hi)
        {
            return (db.AREAS.Any(area => check_area_conflict(area, lo, hi))
                    || db.AREAS.Any(area => check_area_conflict(area, lo, hi)));
        }
    }
}
