using Realm.Library.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;
using SmaugCS.Managers;

namespace SmaugCS.Commands.Combat
{
    public static class Consider
    {
        public static void do_consider(CharacterInstance ch, string argument)
        {
            var arg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, arg, "Consider killing whom?")) return;

            var victim = ch.GetCharacterInRoom(arg);
            if (CheckFunctions.CheckIfNullObject(ch, victim, "They're not here.")) return;
            if (CheckFunctions.CheckIfEquivalent(ch, ch, victim,
                "You decide you're pretty sure you could take yourself in a fight.")) return;

            var levelDiff = victim.Level - ch.Level;

            var msg = GetLevelConsiderMessage(levelDiff);
            comm.act(ATTypes.AT_CONSIDER, msg, ch, null, victim, ToTypes.Character);

            levelDiff = (victim.MaximumHealth - ch.MaximumHealth)/6;
            msg = GetHealthConsiderMessage(levelDiff);
            comm.act(ATTypes.AT_CONSIDER, msg, ch, null, victim, ToTypes.Character);
        }

        private static string GetHealthConsiderMessage(int diff)
        {
            const string lookupName = "HealthConsider";

            if (diff <= -200)
                return LookupManager.Instance.GetLookup(lookupName, 0);
            if (diff <= -150)
                return LookupManager.Instance.GetLookup(lookupName, 1);
            if (diff <= -100)
                return LookupManager.Instance.GetLookup(lookupName, 2);
            if (diff <= -50)
                return LookupManager.Instance.GetLookup(lookupName, 3);
            if (diff <= 0)
                return LookupManager.Instance.GetLookup(lookupName, 4);
            if (diff <= 50)
                return LookupManager.Instance.GetLookup(lookupName, 5);
            if (diff <= 100)
                return LookupManager.Instance.GetLookup(lookupName, 6);
            return diff <= 150
                ? LookupManager.Instance.GetLookup(lookupName, 7)
                : LookupManager.Instance.GetLookup(lookupName, diff <= 200 ? 8 : 9);
        }

        private static string GetLevelConsiderMessage(int diff)
        {
            const string lookupName = "LevelConsider";

            if (diff <= -10)
                return LookupManager.Instance.GetLookup(lookupName, 0);
            if (diff <= -5)
                return LookupManager.Instance.GetLookup(lookupName, 1);
            if (diff <= -2)
                return LookupManager.Instance.GetLookup(lookupName, 2);
            if (diff <= 1)
                return LookupManager.Instance.GetLookup(lookupName, 3);
            return diff <= 4
                ? LookupManager.Instance.GetLookup(lookupName, 4)
                : LookupManager.Instance.GetLookup(lookupName, diff <= 9 ? 5 : 6);
        }
    }
}
