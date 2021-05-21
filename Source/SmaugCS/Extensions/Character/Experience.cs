using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Repository;
using System;
using System.Linq;

namespace SmaugCS.Extensions.Character
{
    public static class Experience
    {
        public static int GetExperienceWorth(this CharacterInstance ch)
        {
            var wexp = (int)Math.Pow(ch.Level, 3) * 5 + ch.MaximumHealth;
            wexp -= (ch.ArmorClass - 50) * 2;
            wexp += (ch.BareDice.NumberOf * ch.BareDice.SizeOf + ch.GetDamroll()) * 50;
            wexp += ch.GetHitroll() * ch.Level * 10;
            if (ch.IsAffected(AffectedByTypes.Sanctuary))
                wexp += (int)(wexp * 1.5);
            if (ch.IsAffected(AffectedByTypes.FireShield))
                wexp += (int)(wexp * 1.2);
            if (ch.IsAffected(AffectedByTypes.ShockShield))
                wexp += (int)(wexp * 1.2);
            return wexp.GetNumberThatIsBetween(GameConstants.MinimumExperienceWorth, GameConstants.MaximumExperienceWorth);
        }

        public static int GetExperienceBase(this CharacterInstance ch, IRepositoryManager databaseManager = null)
        {
            return ch.IsNpc()
                ? 1000
                : (databaseManager ?? RepositoryManager.Instance).CLASSES.Values.First(
                    x => x.Type == ch.CurrentClass).BaseExperience;
        }

        public static int GetExperienceLevel(this CharacterInstance ch, int level)
        {
            return (int)Math.Pow(0.GetHighestOfTwoNumbers(level - 1), 3) * ch.GetExperienceBase();
        }

        public static int GetLevelExperience(this CharacterInstance ch, int cexp)
        {
            var x = LevelConstants.GetLevel(ImmortalTypes.Supreme);
            var lastx = x;
            var y = 0;

            while (y == 0)
            {
                var tmp = ch.GetExperienceLevel(x);
                lastx = x;
                if (tmp > cexp)
                    x /= 2;
                else if (lastx != x)
                    x += x / 2;
                else
                    y = x;
            }

            return y < 1 ? 1 : y > LevelConstants.GetLevel(ImmortalTypes.Supreme)
                ? LevelConstants.GetLevel(ImmortalTypes.Supreme) : y;
        }
    }
}
