using Realm.Library.Common.Extensions;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using System.Linq;

namespace SmaugCS.Commands
{
    public static class Bestow
    {
        public static void do_bestow(CharacterInstance ch, string argument)
        {
            ch.SetColor(ATTypes.AT_IMMORT);

            var firstArg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Bestow whom with what?")) return;

            var victim = ch.GetCharacterInWorld(firstArg);
            if (CheckFunctions.CheckIfNullObject(ch, victim, "They aren't here.")) return;
            if (CheckFunctions.CheckIfNpc(ch, victim, "You can't give special abilities to a mob!")) return;
            if (CheckFunctions.CheckIfInsufficientTrust(ch, victim, "You aren't powerful enough...")) return;

            if (!((PlayerInstance)victim).PlayerData.Bestowments.Any())
                ((PlayerInstance)victim).PlayerData.Bestowments.Clear();

            var secondArg = argument.SecondWord();

            if (string.IsNullOrEmpty(secondArg) || secondArg.EqualsIgnoreCase("show list"))
            {
                ch.SendTo($"Current bestowed commands on {victim.Name}: {string.Join(", ", ((PlayerInstance)victim).PlayerData.Bestowments)}");
                return;
            }

            if (secondArg.EqualsIgnoreCase("none"))
            {
                ((PlayerInstance)victim).PlayerData.Bestowments.Clear();
                ch.SendTo($"Bestowments removed from {victim.Name}");
                victim.SendTo($"{ch.Name} has removed your bestowed commands.");
                handler.check_switch(victim, false);
                return;
            }

            var allArgsExceptFirst = argument.RemoveWord(0).Split(' ');
            if (!allArgsExceptFirst.Any())
            {
                ch.SendTo("Good job, knucklehead... you just bestowed them with that master command called 'NOTHING!'");
                return;
            }

            foreach (var arg in allArgsExceptFirst)
            {
                var command = CommandManager.Instance.FindCommand(arg);
                if (command == null)
                {
                    ch.SendTo($"No such command as {arg}!");
                    continue;
                }

                if (command.Level > ch.Trust)
                {
                    ch.SendTo($"You can't bestow the {arg} command!");
                    continue;
                }

                if (!((PlayerInstance)victim).PlayerData.Bestowments.Any(x => x.EqualsIgnoreCase(arg)))
                {
                    ((PlayerInstance)victim).PlayerData.Bestowments.Add(command.Name);
                    continue;
                }
            }

            victim.SetColor(ATTypes.AT_IMMORT);
            victim.SendTo($"{ch.Name} has bestowed on you the command(s): {string.Join(", ", allArgsExceptFirst)}");
            ch.SendTo("Done.");
        }
    }
}
