using SmaugCS.Common;
using SmaugCS.Common.Enumerations;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Repository;

namespace SmaugCS
{
    public static class Statistics
    {
        public static int GetCurrentStat(this CharacterInstance ch, StatisticTypes statistic)
        {
            var currentClass = RepositoryManager.Instance.GetClass(ch.CurrentClass);
            var max = 20;

            if (ch.IsNpc() || currentClass.PrimaryAttribute == statistic)
                max = 25;
            if (currentClass.SecondaryAttribute == statistic)
                max = 22;
            if (currentClass.DeficientAttribute == statistic)
                max = 16;

            return max;
        }

        public static int GetCurrentStrength(this CharacterInstance ch)
        {
            return (ch.PermanentStrength + ch.ModStrength).GetNumberThatIsBetween(3,
                ch.GetCurrentStat(StatisticTypes.PermanentStrength));
        }

        public static int GetCurrentIntelligence(this CharacterInstance ch)
        {
            return (ch.PermanentIntelligence + ch.ModIntelligence).GetNumberThatIsBetween(3,
                ch.GetCurrentStat(StatisticTypes.PermanentIntelligence));
        }

        public static int GetCurrentWisdom(this CharacterInstance ch)
        {
            return (ch.PermanentWisdom + ch.ModWisdom).GetNumberThatIsBetween(3,
                ch.GetCurrentStat(StatisticTypes.PermanentWisdom));
        }

        public static int GetCurrentDexterity(this CharacterInstance ch)
        {
            return (ch.PermanentDexterity + ch.ModDexterity).GetNumberThatIsBetween(3,
                ch.GetCurrentStat(StatisticTypes.PermanentDexterity));
        }

        public static int GetCurrentConstitution(this CharacterInstance ch)
        {
            return (ch.PermanentConstitution + ch.ModConstitution).GetNumberThatIsBetween(3,
                ch.GetCurrentStat(StatisticTypes.PermanentConstitution));
        }

        public static int GetCurrentCharisma(this CharacterInstance ch)
        {
            return (ch.PermanentCharisma + ch.ModCharisma).GetNumberThatIsBetween(3,
                ch.GetCurrentStat(StatisticTypes.PermanentCharisma));
        }

        public static int GetCurrentLuck(this CharacterInstance ch)
        {
            return (ch.PermanentLuck + ch.ModLuck).GetNumberThatIsBetween(3, ch.GetCurrentStat(StatisticTypes.PermanentLuck));
        }

        public static int CanCarryN(this CharacterInstance ch)
        {
            var penalty = 0;

            if (!ch.IsNpc() && ch.Level >= LevelConstants.ImmortalLevel)
                return ch.Trust * 200;
            if (ch.IsNpc() && ch.Act.IsSet((int)ActFlags.Immortal))
                return ch.Level * 200;
            if (ch.GetEquippedItem(WearLocations.Wield) != null)
                ++penalty;
            if (ch.GetEquippedItem(WearLocations.DualWield) != null)
                ++penalty;
            if (ch.GetEquippedItem(WearLocations.WieldMissile) != null)
                ++penalty;
            if (ch.GetEquippedItem(WearLocations.Hold) != null)
                ++penalty;
            if (ch.GetEquippedItem(WearLocations.Shield) != null)
                ++penalty;
            return ((ch.Level + 15) / 5 + ch.GetCurrentDexterity() - 13 - penalty).GetNumberThatIsBetween(5, 20);
        }

        public static int CanCarryMaxWeight(this CharacterInstance ch)
        {
            if (!ch.IsNpc() && ch.Level >= LevelConstants.ImmortalLevel)
                return 1000000;
            if (ch.IsNpc() && ch.Act.IsSet((int)ActFlags.Immortal))
                return 1000000;

            return (int)LookupManager.Instance.GetStatMod("Strength", ch.GetCurrentStrength(),
                StrengthModTypes.Carry);
        }
    }
}
