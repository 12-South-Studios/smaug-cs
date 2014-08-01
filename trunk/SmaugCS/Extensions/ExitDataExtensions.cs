using System.Linq;
using Realm.Library.Common;
using Realm.Library.Patterns.Repository;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Interfaces;
using SmaugCS.Managers;

// ReSharper disable once CheckNamespace
namespace SmaugCS
{
    public static class ExitDataExtensions
    {
        public static ExitData GetReverseExit(this ExitData exit)
        {
            return exit.GetDestination().GetExit((int)exit.Reverse);
        }

        public static RoomTemplate GetDestination(this ExitData exit, IDatabaseManager dbManager = null)
        {
            return (dbManager ?? DatabaseManager.Instance).ROOMS.CastAs<Repository<long, RoomTemplate>>()
                                                          .Values.ToList()
                                                          .Find(x => x.Vnum == exit.Destination);
        }

        /// <summary>
        /// Was formerly set_bexit_flag
        /// </summary>
        /// <param name="exit"></param>
        /// <param name="flag"></param>
        public static void SetFlagOnSelfAndReverseExit(this ExitData exit, ExitFlags flag)
        {
            exit.Flags = exit.Flags.SetBit(flag);

            ExitData reverseExit = exit.GetReverseExit();
            if (reverseExit != null && reverseExit != exit)
                reverseExit.Flags = reverseExit.Flags.SetBit(flag);
        }

        /// <summary>
        /// Was formerly remove_bexit_flag
        /// </summary>
        /// <param name="exit"></param>
        /// <param name="flag"></param>
        public static void RemoveFlagFromSelfAndReverseExit(this ExitData exit, ExitFlags flag)
        {
            exit.Flags = exit.Flags.RemoveBit(flag);

            ExitData reverseExit = exit.GetReverseExit();
            if (reverseExit != null && reverseExit != exit)
                reverseExit.Flags = reverseExit.Flags.RemoveBit(flag);
        }

        /// <summary>
        /// Was formerly toggle_bexit_flag
        /// </summary>
        /// <param name="exit"></param>
        /// <param name="flag"></param>
        public static void ToggleFlagOnSelfAndReverseExit(this ExitData exit, ExitFlags flag)
        {
            exit.Flags = exit.Flags.ToggleBit(flag);

            ExitData reverseExit = exit.GetReverseExit();
            if (reverseExit != null && reverseExit != exit)
                reverseExit.Flags = reverseExit.Flags.ToggleBit(flag);
        }
    }
}
