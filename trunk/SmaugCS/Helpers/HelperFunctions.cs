using System.Collections.Generic;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.Helpers
{
    /// <summary>
    /// These functions are designed to work in conjunction with the CheckHelpers
    /// </summary>
    public static class HelperFunctions
    {
        public static bool HasSufficientBloodPower(IList<object> args)
        {
            CharacterInstance actor = (CharacterInstance)args[0];
            int blood = (int)args[1];
            return actor.IsVampire() && actor.PlayerData.ConditionTable[ConditionTypes.Bloodthirsty] >= blood;
        }

        public static bool HasSufficientMana(IList<object> args)
        {
            CharacterInstance actor = (CharacterInstance)args[0];
            int mana = (int)args[1];
            return !actor.IsNpc() && actor.CurrentMana >= mana;
        }

        public static bool IsCharmedOrPossessed(IList<object> args)
        {
            CharacterInstance actor = (CharacterInstance) args[0];
            return actor.IsNpc() && actor.IsAffected(AffectedByTypes.Charm) || actor.IsAffected(AffectedByTypes.Possess);
        }

        public static bool IsFighting(IList<object> args)
        {
            CharacterInstance actor = (CharacterInstance) args[0];
            return actor.CurrentFighting != null;
        }

        public static bool IsInFightingPosition(IList<object> args)
        {
            CharacterInstance actor = (CharacterInstance) args[0];
            PositionTypes position = actor.CurrentPosition;
            return (position == PositionTypes.Aggressive
                    || position == PositionTypes.Berserk
                    || position == PositionTypes.Defensive
                    || position == PositionTypes.Evasive
                    || position == PositionTypes.Fighting);
        }
    }
}
