using SmaugCS.Commands;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Repository;

namespace SmaugCS
{
    public static class HuntHateFear
    {
        public static void SummonIfHating(this MobileInstance ch, IRepositoryManager dbManager = null)
        {
            if ((int)ch.CurrentPosition <= (int)PositionTypes.Sleeping
                || ch.CurrentFighting != null
                || ch.CurrentFearing != null
                || ch.CurrentHating == null
                || ch.CurrentRoom.Flags.IsSet(RoomFlags.Safe)
                || ch.CurrentHunting != null)
                return;

            var victim =
                (dbManager ?? Program.RepositoryManager).GetEntity<CharacterInstance>(ch.CurrentHating.Name);
            if (victim == null || ch.CurrentRoom == victim.CurrentRoom)
                return;

            Cast.do_cast(ch, $"summon {(victim.IsNpc() ? string.Empty : "0.")}{ch.CurrentHating.Name}");
        }

        public static bool IsHunting(this MobileInstance ch, CharacterInstance victim)
        {
            return ch.CurrentHunting != null
                && ch.CurrentHunting.Who == victim;
        }

        public static bool IsHating(this MobileInstance ch, CharacterInstance victim)
        {
            return ch.CurrentHating != null
                && ch.CurrentHating.Who == victim;
        }

        public static bool IsFearing(this MobileInstance ch, CharacterInstance victim)
        {
            return ch.CurrentFearing != null
                   && ch.CurrentFearing.Who == victim;
        }

        public static void StopHunting(this MobileInstance ch)
        {
            ch.CurrentHunting = null;
        }

        public static void StopHating(this MobileInstance ch)
        {
            ch.CurrentHating = null;
        }

        public static void StopFearing(this MobileInstance ch)
        {
            ch.CurrentFearing = null;
        }

        public static void StartHunting(this MobileInstance ch, CharacterInstance victim)
        {
            if (ch.CurrentHunting != null)
                ch.StopHunting();

            ch.CurrentHunting = new HuntHateFearData
            {
                Name = victim.Name,
                Who = victim
            };
        }

        public static void StartFearing(this MobileInstance ch, CharacterInstance victim)
        {
            if (ch.CurrentFearing != null)
                ch.StopFearing();

            ch.CurrentFearing = new HuntHateFearData
            {
                Name = victim.Name,
                Who = victim
            };
        }

        public static void StartHating(this MobileInstance ch, CharacterInstance victim)
        {
            if (ch.CurrentHating != null)
                ch.StopHating();

            ch.CurrentHating = new HuntHateFearData
            {
                Name = victim.Name,
                Who = victim
            };
        }
    }
}
