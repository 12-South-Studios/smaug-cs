using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.Extensions
{
    public static class HuntHateFear
    {
        public static bool IsHunting(this CharacterInstance ch, CharacterInstance victim)
        {
            return ch.CurrentHunting != null
                && ch.CurrentHunting.Who == victim;
        }

        public static bool IsHating(this CharacterInstance ch, CharacterInstance victim)
        {
            return ch.CurrentHating != null
                && ch.CurrentHating.Who == victim;
        }

        public static bool IsFearing(this CharacterInstance ch, CharacterInstance victim)
        {
            return ch.CurrentFearing != null
                   && ch.CurrentFearing.Who == victim;
        }

        public static bool IsBlind(this CharacterInstance ch)
        {
            if (!ch.IsNpc() && ch.Act.IsSet(PlayerFlags.HolyLight))
                return true;
            if (ch.IsAffected(AffectedByTypes.TrueSight))
                return true;
            if (!ch.IsAffected(AffectedByTypes.Blind))
                return true;

            return false;
        }

        public static void StopHunting(this CharacterInstance ch)
        {
            ch.CurrentHunting = null;
        }

        public static void StopHating(this CharacterInstance ch)
        {
            ch.CurrentHating = null;
        }

        public static void StopFearing(this CharacterInstance ch)
        {
            ch.CurrentFearing = null;
        }

        public static void StartHunting(this CharacterInstance ch, CharacterInstance victim)
        {
            if (ch.CurrentHunting != null)
                ch.StopHunting();

            ch.CurrentHunting = new HuntHateFearData
            {
                Name = victim.Name,
                Who = victim
            };
        }

        public static void StartFearing(this CharacterInstance ch, CharacterInstance victim)
        {
            if (ch.CurrentFearing != null)
                ch.StopFearing();

            ch.CurrentFearing = new HuntHateFearData
            {
                Name = victim.Name,
                Who = victim
            };
        }

        public static void StartHating(this CharacterInstance ch, CharacterInstance victim)
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
