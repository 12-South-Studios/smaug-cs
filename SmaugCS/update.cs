using System;
using System.Linq;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Extensions;
using SmaugCS.Logging;
using SmaugCS.Managers;

namespace SmaugCS
{
    public static class update
    {
        public static void mobile_update()
        {
            // TODO
        }

        public static void char_calendar_update()
        {
            // TODO
        }

        public static void char_update()
        {
            // TODO
        }

        public static void obj_update()
        {
            // TODO
        }

        public static void char_check()
        {
            // TODO
        }

        public static void aggr_update()
        {
            // TODO
        }

        public static void drunk_randoms(CharacterInstance ch)
        {
            // TODO
        }

        public static void hallucinations(CharacterInstance ch)
        {
            // TODO
        }

        public static void tele_update()
        {
            // TODO
        }

        public static void auth_update()
        {
            // TODO
        }

        public static void update_handler()
        {
            // TODO
        }

        public static void remove_portal(ObjectInstance portal)
        {
            if (portal == null)
            {
                // TODO Exeption, log it
                return;
            }

            RoomTemplate fromRoom = portal.InRoom;
            if (fromRoom == null)
            {
                // TODO Exception log it
                return;
            }

            ExitData exit = fromRoom.Exits.FirstOrDefault(xit => xit.Flags.IsSet(ExitFlags.Portal));
            if (exit == null)
            {
                // TODO Exception, log it
                return;
            }

            if (exit.Direction != DirectionTypes.Portal)
            {
                // TODO Exception, log it
            }

            if (exit.GetDestination(DatabaseManager.Instance) == null)
            {
                // TODO Exception, log it
            }

            handler.extract_exit(fromRoom, exit);
        }

        public static void reboot_check(DateTime reset)
        {
            // TODO
        }

        public static void auction_update()
        {
            // TODO
        }

        public static void subtract_times(DateTime etime, DateTime sttime)
        {
            // TODO
        }

        public static void time_update()
        {
            // TODO
        }

        public static void hint_update()
        {
            // TODO
        }
    }
}
