﻿using System.Linq;
using SmaugCS.Commands.Social;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Interfaces;
using SmaugCS.Data.Shops;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;

namespace SmaugCS.Behavior.Shopkeeper;

public static class Interact
{
  public static CharacterInstance GetShopkeep(CharacterInstance ch, IGameManager gameManager = null)
  {
    MobileInstance keeper =
      ch.CurrentRoom.Persons.OfType<MobileInstance>()
        .FirstOrDefault(mob => mob.IsNpc() && mob.MobIndex.Shop != null);

    if (CheckFunctions.CheckIfNullObject(ch, keeper, "You can't do that here.")) return null;
    if (DoesKeeperHateKillers(keeper, ch)) return null;
    if (DoesKeeperHateThieves(keeper, ch)) return null;
    if (DoesKeeperHateFighting(keeper, ch)) return null;
    if (IsKeeperFighting(keeper, ch)) return null;
    if (IsShopClosed(keeper, ch, (gameManager ?? Program.GameManager).GameTime.Hour)) return null;
    if (CheckFunctions.CheckIfTrue(ch, keeper.CurrentPosition == PositionTypes.Sleeping,
          "While they're asleep?")) return null;
    if (CheckFunctions.CheckIfTrue(ch, (int)keeper.CurrentPosition < (int)PositionTypes.Sleeping,
          "I don't think they can hear you...")) return null;
    if (IsVisibleToKeeper(keeper, ch)) return null;

    int speakswell = keeper.KnowsLanguage(ch.Speaking, ch).GetLowestOfTwoNumbers(ch.KnowsLanguage(ch.Speaking, keeper));
    if (SmaugRandom.D100() % 65 <= speakswell) return keeper;
    string buffer = speakswell switch
    {
      > 60 => $"{ch.Name} Could you repeat that?  I didn't quite catch it.",
      > 50 => $"{ch.Name} Could you say that a little more clearly please?",
      > 40 => $"{ch.Name} Sorry... What was that you wanted?",
      _ => $"{ch.Name} I can't understand you."
    };

    Tell.do_tell(keeper, buffer);
    return null;

  }

  private static bool DoesKeeperHateKillers(CharacterInstance keeper, CharacterInstance ch)
  {
    if (!ch.Act.IsSet((int)PlayerFlags.Killer)) return true;

    Say.do_say(keeper, "Murderers are not welcome here!");
    Shout.do_shout(keeper, $"{ch.Name} the KILLER is over here!");
    return false;
  }

  private static bool DoesKeeperHateThieves(CharacterInstance keeper, CharacterInstance ch)
  {
    if (!ch.Act.IsSet((int)PlayerFlags.Thief)) return false;

    Say.do_say(keeper, "Thieves are not welcome here!");
    Shout.do_shout(keeper, $"{ch.Name} the THIEF is over here!");
    return true;
  }

  private static bool DoesKeeperHateFighting(CharacterInstance keeper, CharacterInstance ch)
  {
    if (ch.GetMyTarget() == null) return false;

    ch.Printf("%s doesn't seem to wnat to get involved.\r\n", Macros.PERS(keeper, ch));
    return true;
  }

  private static bool IsKeeperFighting(CharacterInstance keeper, CharacterInstance ch)
  {
    CharacterInstance whof = keeper.GetMyTarget();
    if (whof == null) return false;

    if (!CheckFunctions.CheckIfEquivalent(ch, whof, ch, "I don't think that's a good idea..."))
      Say.do_say(keeper, "I'm too busy for that!");
    return true;
  }

  private static bool IsShopClosed(MobileInstance keeper, CharacterInstance ch, int gameHour)
  {
    ShopData shop = keeper.MobIndex.Shop;
    if (gameHour < shop.OpenHour)
    {
      Say.do_say(keeper, "Sorry, come back later.");
      return true;
    }

    if (gameHour <= shop.CloseHour) return false;

    Say.do_say(keeper, "Sorry, come back tomorrow.");
    return true;
  }

  private static bool IsVisibleToKeeper(CharacterInstance keeper, CharacterInstance ch)
  {
    if (keeper.CanSee(ch)) return false;

    Say.do_say(keeper, "I don't trade with folks I can't see.");
    return true;
  }
}