using System.Linq;
using Realm.Library.Common;
using Realm.Library.Patterns.Repository;
using SmaugCS.Common;
using SmaugCS.Data;
using SmaugCS.Data;
using SmaugCS.Managers;

// ReSharper disable CheckNamespace
namespace SmaugCS
// ReSharper restore CheckNamespace
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

        public static void SetBExitFlag(this ExitData exit, int flag)
        {
            exit.Flags.SetBit(flag);

            ExitData reverseExit = exit.GetReverseExit();
            if (reverseExit != null && reverseExit != exit)
                reverseExit.Flags.SetBit(flag);
        }

        public static void RemoveBExitFlag(this ExitData exit, int flag)
        {
            exit.Flags.RemoveBit(flag);

            ExitData reverseExit = exit.GetReverseExit();
            if (reverseExit != null && reverseExit != exit)
                reverseExit.Flags.RemoveBit(flag);
        }

        public static void ToggleBExitFlag(this ExitData exit, int flag)
        {
            exit.Flags.ToggleBit(flag);

            ExitData reverseExit = exit.GetReverseExit();
            if (reverseExit != null && reverseExit != exit)
                reverseExit.Flags.ToggleBit(flag);
        }
    }
}
