using System.Linq;
using Library.Common.Extensions;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.Admin;

public static class BodyBag
{
  public static void do_bodybag(CharacterInstance ch, string argument)
  {
    if (CheckFunctions.CheckIfEmptyString(ch, argument,
          "&PSyntax: bodybag <character> | bodybag <character> yes/bag/now")) return;

    string firstWord = $"the corpse of {argument.FirstWord()}";
    string secondWord = argument.SecondWord();

    if (!string.IsNullOrEmpty(secondWord) && !(secondWord.EqualsIgnoreCase("yes")
                                               || secondWord.EqualsIgnoreCase("bag") ||
                                               secondWord.EqualsIgnoreCase("now")))
    {
      ch.SendTo("&PSyntax: bodybag <character> | bodybag <character> yes/bag/now");
      return;
    }

    bool bag = secondWord.EqualsIgnoreCase("yes") || secondWord.EqualsIgnoreCase("bag") ||
               secondWord.EqualsIgnoreCase("now");
    string phrase = bag ? "Retrieving" : "Searching for";

    ch.SendToPagerColor($"&P{phrase} remains of {firstWord.CapitalizeFirst()}");

    ObjectInstance obj = ch.CurrentRoom.Contents.FirstOrDefault(x => x.ItemType == ItemTypes.PlayerCorpse
                                                                     && (x.Name.Equals(firstWord) ||
                                                                         x.ShortDescription.ContainsIgnoreCase(
                                                                           firstWord)));
    if (obj == null)
    {
      ch.SendToPagerColor("&PNo corpse was found.");
      return;
    }

    phrase = bag ? "Bagging" : "Corpse";
    string color = bag ? "&Y" : "&w";
    string clanPhrase = obj.ExtraFlags.IsSet((int)ItemExtraFlags.ClanCorpse) ? "&RPK" : "&R";
    string areaPhrase = $"&PIn: &w{ch.CurrentRoom.Area.Name} &P[&w{ch.CurrentRoom.Area.Id}&P]";

    string timerPhrase = $"&PTimer: ";
    timerPhrase += obj.Timer switch
    {
      < 1 => "&w",
      < 5 => "&R",
      < 10 => "&Y",
      _ => "&w"
    };
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

    CharacterInstance owner = ch.GetCharacterInWorld(obj.Owner);
    if (owner == null)
    {
      ch.SendToPagerColor($"&P{obj.Owner.CapitalizeFirst()} is not currently online.");
      return;
    }

    if (owner.IsNpc()) return;

    PlayerInstance ownerPc = (PlayerInstance)owner;

    if (ownerPc.PlayerData.CurrentDeity != null)
    {
      ch.SendToPagerColor($"&P{owner.Name} ({owner.Level}) has {ownerPc.PlayerData.Favor} favor with " +
                          $"{ownerPc.PlayerData.CurrentDeity.Name} (needed to supplicate: {ownerPc.PlayerData.CurrentDeity.SupplicateCorpseCost}");
      return;
    }

    ch.SendToPagerColor($"&P{owner.Name} ({owner.Level}) has no deity");
  }
}