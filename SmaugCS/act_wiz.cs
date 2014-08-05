using System.Linq;
using Realm.Library.Common;
using Realm.Library.Patterns.Repository;
using SmaugCS.Commands;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Logging;
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
                return DatabaseManager.Instance.ROOMS.CastAs<Repository<long, RoomTemplate>>().Get(arg.ToInt32());
            if (arg.Equals("pk"))
                return DatabaseManager.Instance.ROOMS.CastAs<Repository<long, RoomTemplate>>().Get(db.LastPKRoom);

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
                LogManager.Instance.Bug("Victim {0} in null room", victim.Name);
                return;
            }

            if (ch.IsNpc() && location.IsPrivate())
            {
                //progbug("Mptransfer - Private room", ch);
                return;
            }

            if (!ch.CanSee(victim))
                return;

            if (ch.IsNpc() && victim.IsNotAuthorized()
                && location.Area != victim.CurrentRoom.Area)
            {
                string buffer = string.Format("Mptransfer - unauthed char ({0})", victim.Name);
                //progbug(buffer, ch);
                return;
            }

            // if victim not in area's level range, do not transfer
            if (ch.IsNpc() && !location.Area.InHardRange(victim)
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
                    && !location.Area.InHardRange(victim))
                    comm.act(ATTypes.AT_DANGER, "Warning: this player's level is not within the area's level range.", ch, null, null, ToTypes.Character);
            }
        }


        public static void update_calendar()
        {
            /*GameConstants.GetSystemValue<int>("DaysPerYear") = GameConstants.GetSystemValue<int>("DaysPerMonth * GameConstants.GetSystemValue<int>("MonthsPerYear;
            GameConstants.GetSystemValue<int>("HourOfSunrise = GameConstants.GetSystemValue<int>("HoursPerDay / 4;
            GameConstants.GetSystemValue<int>("HourOfDayBegin = GameConstants.GetSystemValue<int>("HourOfSunrise + 1;
            GameConstants.GetSystemValue<int>("HourOfNoon = GameConstants.GetSystemValue<int>("HoursPerDay / 2;
            GameConstants.GetSystemValue<int>("HourOfSunset = ((GameConstants.GetSystemValue<int>("HoursPerDay / 4) * 3);
            GameConstants.GetSystemValue<int>("HourOfNightBegin = GameConstants.GetSystemValue<int>("HourOfSunset + 1;
            GameConstants.GetSystemValue<int>("HourOfMidnight = GameConstants.GetSystemValue<int>("HoursPerDay;
            CalendarManager.Instance.CalculateSeason(GameManager.Instance.GameTime);*/
        }

        public static void update_timers()
        {
            /*GameConstants.GetSystemValue<int>("PulseTick = GameConstants.GetSystemValue<int>("SecondsPerTick * GameConstants.GetSystemValue<int>("PulsesPerSecond;
            GameConstants.GetSystemValue<int>("PulseViolence = 3 * GameConstants.GetSystemValue<int>("PulsesPerSecond;
            GameConstants.GetSystemValue<int>("PulseMobile = 4 * GameConstants.GetSystemValue<int>("PulsesPerSecond;
            GameConstants.GetSystemValue<int>("PulseCalendar = 4 * GameConstants.GetSystemValue<int>("PulseTick;*/
        }

        public static void get_reboot_string()
        {
            // TODO 
            // snprintf(reboot_time, 50, "%s", asctime(new_boot_time));
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
            return (DatabaseManager.Instance.AREAS.Values.Any(area => check_area_conflict(area, lo, hi))
                    || DatabaseManager.Instance.AREAS.Values.Any(area => check_area_conflict(area, lo, hi)));
        }
    }
}
