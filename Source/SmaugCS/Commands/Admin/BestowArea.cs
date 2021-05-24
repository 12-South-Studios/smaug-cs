using Realm.Library.Common.Extensions;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using System.Linq;

namespace SmaugCS.Commands
{
    public static class BestowArea
    {
        public static void do_bestowarea(CharacterInstance ch, string argument)
        {
            ch.SetColor(ATTypes.AT_IMMORT);

            if (string.IsNullOrEmpty(argument))
            {
                ch.SendTo("Syntax:");
                ch.SendTo("bestowarea <victim> <filename>.are");
                ch.SendTo("bestowarea <victim> none (removes bestowed areas)");
                ch.SendTo("bestowarea <victim> list (lists bestowed areas)");
                ch.SendTo("bestowarea <victim>      (lists bestowed areas)");
                return;
            }

            var firstArg = argument.FirstWord();

            var victim = ch.GetCharacterInWorld(firstArg);
            if (CheckFunctions.CheckIfNullObject(ch, victim, "They aren't here.")) return;
            if (CheckFunctions.CheckIfNpc(ch, victim, "You can't give special abilities to a mob!")) return;
            if (!victim.IsImmortal())
            {
                ch.SendTo("They aren't an immortal.");
                return;
            }

            var secondArg = argument.SecondWord();
            if (string.IsNullOrEmpty(secondArg) || secondArg.EqualsIgnoreCase("list"))
            {
                var areaNames = ((PlayerInstance)victim).PlayerData.Bestowments.Where(x => x.EndsWith(".are"));
                ch.SendTo($"Bestowed Areas: {string.Join(", ", areaNames)}");
                return;
            }

            if (secondArg.EqualsIgnoreCase("none"))
            {
                ((PlayerInstance)victim).PlayerData.Bestowments = ((PlayerInstance)victim).PlayerData.Bestowments.Where(x => !x.EndsWith(".are")).ToList();
                ch.SendTo("Done.");
                return;
            }

            if (!secondArg.EndsWith(".are"))
            {
                ch.SendTo("You can only bestow an area name.");
                ch.SendTo("e.g. bestow joe sam.are");
                return;
            }

            ((PlayerInstance)victim).PlayerData.Bestowments.Add(secondArg);
            victim.SetColor(ATTypes.AT_IMMORT);
            victim.SendTo($"{ch.Name} has bestowed on you the area: {secondArg}");
            ch.SendTo("Done.");
        }
    }
}
