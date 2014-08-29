using Realm.Library.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Extensions;

namespace SmaugCS.Commands
{
    public static class Consider
    {
        public static void do_consider(CharacterInstance ch, string argument)
        {
            string arg = argument.FirstWord();
            if (Helpers.CheckFunctions.CheckIfEmptyString(ch, arg, "Consider killing whom?")) return;

            CharacterInstance victim = ch.GetCharacterInRoom(arg);
            if (Helpers.CheckFunctions.CheckIfNullObject(ch, victim, "They're not here.")) return;
            if (Helpers.CheckFunctions.CheckIfEquivalent(ch, ch, victim,
                "You decide you're pretty sure you could take yourself in a fight.")) return;

            int levelDiff = victim.Level - ch.Level;

            string msg = GetLevelConsiderMessage(levelDiff);
            comm.act(ATTypes.AT_CONSIDER, msg, ch, null, victim, ToTypes.Character);

            levelDiff = (victim.MaximumHealth - ch.MaximumHealth)/6;
            msg = GetHealthConsiderMessage(levelDiff);
            comm.act(ATTypes.AT_CONSIDER, msg, ch, null, victim, ToTypes.Character);
        }

        private static string GetHealthConsiderMessage(int diff)
        {
            if (diff <= -200)
                return "$N looks like a feather!";
            if (diff <= -150)
                return "You could kill $N with your hands tied!";
            if (diff <= -100)
                return "Hey! Where'd $N go?";
            if (diff <= -50)
                return "$N is a wimp.";
            if (diff <= 0)
                return "$N looks weaker than you.";
            if (diff <= 50)
                return "$N looks about as strong as you.";
            if (diff <= 100)
                return "It would take a bit of luck...";
            if (diff <= 150)
                return "It would take a lot of luck, and equipment!";
            if (diff <= 200)
                return "Why don't you dig a grave for yourself first!";
            return "$N is built like a TANK!";
        }

        private static string GetLevelConsiderMessage(int diff)
        {
            if (diff <= -10)
                return "You are far more experienced than $N.";
            if (diff <= -5)
                return "$n is not nearly as experienced as you.";
            if (diff <= -2)
                return "You are more experienced than $n.";
            if (diff <= 1)
                return "You are just about as experienced as $N.";
            if (diff <= 4)
                return "You are not nearly as experienced as $N.";
            if (diff <= 9)
                return "$N is far more experienced than you!";
            return "$N would make a great teacher for you!";
        }
    }
}
