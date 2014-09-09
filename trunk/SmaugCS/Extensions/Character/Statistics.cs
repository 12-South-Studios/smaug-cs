using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Managers;

namespace SmaugCS.Extensions
{
    public static class Statistics
    {
        public static int GetCurrentStat(this CharacterInstance ch, StatisticTypes statistic)
        {
            ClassData currentClass = DatabaseManager.Instance.GetClass(ch.CurrentClass);
            int max = 20;

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
                ch.GetCurrentStat(StatisticTypes.Strength));
        }

        public static int GetCurrentIntelligence(this CharacterInstance ch)
        {
            return (ch.PermanentIntelligence + ch.ModIntelligence).GetNumberThatIsBetween(3,
                ch.GetCurrentStat(StatisticTypes.Intelligence));
        }

        public static int GetCurrentWisdom(this CharacterInstance ch)
        {
            return (ch.PermanentWisdom + ch.ModWisdom).GetNumberThatIsBetween(3,
                ch.GetCurrentStat(StatisticTypes.Wisdom));
        }

        public static int GetCurrentDexterity(this CharacterInstance ch)
        {
            return (ch.PermanentDexterity + ch.ModDexterity).GetNumberThatIsBetween(3,
                ch.GetCurrentStat(StatisticTypes.Dexterity));
        }

        public static int GetCurrentConstitution(this CharacterInstance ch)
        {
            return (ch.PermanentConstitution + ch.ModConstitution).GetNumberThatIsBetween(3,
                ch.GetCurrentStat(StatisticTypes.Constitution));
        }

        public static int GetCurrentCharisma(this CharacterInstance ch)
        {
            return (ch.PermanentCharisma + ch.ModCharisma).GetNumberThatIsBetween(3,
                ch.GetCurrentStat(StatisticTypes.Charisma));
        }

        public static int GetCurrentLuck(this CharacterInstance ch)
        {
            return (ch.PermanentLuck + ch.ModLuck).GetNumberThatIsBetween(3, ch.GetCurrentStat(StatisticTypes.Luck));
        }

        public static int CanCarryN(this CharacterInstance ch)
        {
            int penalty = 0;

            if (!ch.IsNpc() && ch.Level >= LevelConstants.ImmortalLevel)
                return ch.Trust*200;
            if (ch.IsNpc() && ch.Act.IsSet(ActFlags.Immortal))
                return ch.Level*200;
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
            return ((ch.Level + 15)/5 + ch.GetCurrentDexterity() - 13 - penalty).GetNumberThatIsBetween(5, 20);
        }

        public static int CanCarryMaxWeight(this CharacterInstance ch)
        {
            if (!ch.IsNpc() && ch.Level >= LevelConstants.ImmortalLevel)
                return 1000000;
            if (ch.IsNpc() && ch.Act.IsSet(ActFlags.Immortal))
                return 1000000;
            return LookupConstants.str_app[ch.GetCurrentStrength()].Carry;
        }
    }
}
