using System.Linq;
using Realm.Library.Common;
using Realm.Library.Patterns.Repository;
using SmaugCS.Commands;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Logging;
using SmaugCS.Repository;

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

            foreach (var d in db.DESCRIPTORS.Where(d => d.Character != null
                                                                  && d.Character.Name.EqualsIgnoreCase(name)
                                                                  && d.Character.IsWaitingForAuthorization()))
            {
                if (++NumberOfHits > 1)
                {
                    ch.Printf("%s does not uniquely identify a char.\r\n", name);
                    return null;
                }

                retChar = d.Character;
            }

            if (NumberOfHits == 1)
                return retChar;

            ch.SendTo("No one like that waiting for authorization.");
            return null;
        }

        public static void echo_to_all(ATTypes atColor, string argument, int tar)
        {
            if (string.IsNullOrWhiteSpace(argument))
                return;

            foreach (var d in db.DESCRIPTORS
                .Where(x => x.ConnectionStatus == ConnectionTypes.Playing
                    || x.ConnectionStatus == ConnectionTypes.Editing))
            {
                if (tar == (int)EchoTypes.All && d.Character.IsNpc())
                    continue;
                if (tar == (int)EchoTypes.Immortal && !d.Character.IsImmortal())
                    continue;

               d.Character.SetColor(atColor);
               d.Character.SendTo(argument);
               d.Character.SendTo("\r\n");
            }
        }

        public static void echo_to_room(ATTypes atcolor, RoomTemplate room, string argument)
        {
            foreach (var ch in room.Persons)
            {
               ch.SetColor(atcolor);
               ch.SendTo(argument);
               ch.SendTo("\r\n");
            }
        }

        public static RoomTemplate find_location(CharacterInstance ch, string arg)
        {
            if (arg.IsNumber())
                return RepositoryManager.Instance.ROOMS.CastAs<Repository<long, RoomTemplate>>().Get(arg.ToInt32());
            if (arg.Equals("pk"))
                return RepositoryManager.Instance.ROOMS.CastAs<Repository<long, RoomTemplate>>().Get(db.LastPKRoom);

            var victim = ch.GetCharacterInWorld(arg);
            if (victim != null)
                return victim.CurrentRoom;

            var obj = ch.GetObjectInWorld(arg);
            return obj?.InRoom;
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
                var buffer = $"Mptransfer - unauthed char ({victim.Name})";
                //progbug(buffer, ch);
                return;
            }

            // if victim not in area's level range, do not transfer
            if (ch.IsNpc() && !location.Area.IsInHardRange(victim)
                && !location.Flags.IsSet((int)RoomFlags.Prototype))
                return;

            if (victim.CurrentFighting != null)
                victim.StopFighting(true);

            if (!ch.IsNpc())
            {
                comm.act(ATTypes.AT_MAGIC, "$n disappears in a cloud of swirling colors.", victim, null, null, ToTypes.Room);
                victim.retran = (int)victim.CurrentRoom.ID;
            }

            victim.CurrentRoom.RemoveFrom(victim);
            location.AddTo(victim);

            if (!ch.IsNpc())
            {
                comm.act(ATTypes.AT_MAGIC, "$n arrives from a puff of smoke.", victim, null, null, ToTypes.Room);
                if (ch != victim)
                    comm.act(ATTypes.AT_IMMORT, "$n has transferred you.", ch, null, victim, ToTypes.Victim);
                Look.do_look(victim, "auto");
                if (!victim.IsImmortal()
                    && !victim.IsNpc()
                    && !location.Area.IsInHardRange(victim))
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

            return (hi <= area.HighRoomNumber && hi >= area.LowRoomNumber)
                   || (hi <= area.HighMobNumber && hi >= area.LowMobNumber)
                   || (hi <= area.HighObjectNumber && hi >= area.LowObjectNumber);
        }

        public static bool check_area_conflicts(int lo, int hi)
        {
            return (RepositoryManager.Instance.AREAS.Values.Any(area => check_area_conflict(area, lo, hi))
                    || RepositoryManager.Instance.AREAS.Values.Any(area => check_area_conflict(area, lo, hi)));
        }
    }
}
