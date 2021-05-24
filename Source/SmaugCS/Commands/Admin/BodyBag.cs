using SmaugCS.Data.Instances;
using SmaugCS.Helpers;
using Realm.Library.Common.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Managers;
using SmaugCS.Constants.Enums;
using SmaugCS.Common;
using System.Linq;

namespace SmaugCS.Commands.Admin
{
    public static class BodyBag
    {
        public static void do_bodybag(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfEmptyString(ch, argument, "&PSyntax: bodybag <character> | bodybag <character> yes/bag/now")) return;

            var firstWord = $"the corpse of {argument.FirstWord()}";
            var secondWord = argument.SecondWord();

            if (!string.IsNullOrEmpty(secondWord) && !(secondWord.EqualsIgnoreCase("yes")
                || secondWord.EqualsIgnoreCase("bag") || secondWord.EqualsIgnoreCase("now")))
            {
                ch.SendTo("&PSyntax: bodybag <character> | bodybag <character> yes/bag/now");
                return;
            }

            var bag = secondWord.EqualsIgnoreCase("yes") || secondWord.EqualsIgnoreCase("bag") || secondWord.EqualsIgnoreCase("now");
            var phrase = bag ? "Retrieving" : "Searching for";

            ch.SendToPagerColor($"&P{phrase} remains of {firstWord.CapitalizeFirst()}");

            var obj = ch.CurrentRoom.Contents.FirstOrDefault(x => x.ItemType == ItemTypes.PlayerCorpse
            && (x.Name.Equals(firstWord) || x.ShortDescription.ContainsIgnoreCase(firstWord)));
            if (obj == null)
            {
                ch.SendToPagerColor("&PNo corpse was found.");
                return;
            }

            phrase = bag ? "Bagging" : "Corpse";
            var color = bag ? "&Y" : "&w";
            var clanPhrase = obj.ExtraFlags.IsSet((int)ItemExtraFlags.ClanCorpse) ? "&RPK" : "&R";
            var areaPhrase = $"&PIn: &w{ch.CurrentRoom.Area.Name} &P[&w{ch.CurrentRoom.Area.ID}&P]";

            var timerPhrase = $"&PTimer: ";
            if (obj.Timer < 1) timerPhrase += "&w";
            else if (obj.Timer < 5) timerPhrase += "&R";
            else if (obj.Timer < 10) timerPhrase += "&Y";
            else timerPhrase += "&w";
            timerPhrase += obj.Timer.ToString();

            // act_wiz lines 2524-2531
            ch.SendToPagerColor($"&P{phrase}: {color}{firstWord.CapitalizeFirst()} {clanPhrase} {areaPhrase} {timerPhrase}");

            if (bag)
            {
                ch.CurrentRoom.Contents.Remove(obj);
                ch.Carrying.Add(obj);
                obj.Timer = -1;
                // TODO save_char_obj(ch)
            }

            var owner = ch.GetCharacterInWorld(obj.Owner);
            if (owner == null)
            {
                ch.SendToPagerColor($"&P{obj.Owner.CapitalizeFirst()} is not currently online.");
                return;
            }

            if (owner.IsNpc()) return;

            var ownerPc = (PlayerInstance)owner;

            if (ownerPc.PlayerData.CurrentDeity != null)
            {
                ch.SendToPagerColor($"&P{owner.Name} ({owner.Level}) has {ownerPc.PlayerData.Favor} favor with " + 
                    $"{ownerPc.PlayerData.CurrentDeity.Name} (needed to supplicate: {ownerPc.PlayerData.CurrentDeity.SupplicateCorpseCost}");
                return;
            }

            ch.SendToPagerColor($"&P{owner.Name} ({owner.Level}) has no deity");
        }
    }
}
