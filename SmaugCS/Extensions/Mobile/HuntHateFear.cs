using SmaugCS.Data;
using SmaugCS.Data.Instances;

namespace SmaugCS.Extensions
{
    public static class HuntHateFear
    {
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
