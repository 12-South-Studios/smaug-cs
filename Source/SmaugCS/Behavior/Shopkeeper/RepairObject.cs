using Autofac;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.MudProgs;

namespace SmaugCS.Behavior.Shopkeeper;

public static class RepairObject
{
  public static void repair_one_obj(CharacterInstance ch, MobileInstance keeper, ObjectInstance obj,
    string arg, int maxgold, string fixstr, string fixstr2)
  {
    int cost;
    string buffer;

    if (!ch.CanDrop(obj))
      ch.Printf("You can't let go of %s.\r\n", obj.Name);
    else if ((cost = Appraise.GetAdjustedRepairCost(keeper, obj)) < 0)
    {
      if (cost < 0)
      {
        comm.act(ATTypes.AT_TELL,
          cost != -2
            ? "$n tells you, 'Sorry, I can't do anything with $p.'"
            : "$n tells you, '$p looks fine to me!'", keeper, obj, ch, ToTypes.Victim);
      }
    }

    // repair all gets a 10% surcharge
    else if ((cost = arg.Equals("all") ? cost : 11 * (cost / 10)) > ch.CurrentCoin)
    {
      buffer =
        $"$N tells you, 'It will cost {cost} piece{(cost == 1 ? "" : "s")} of gold to {fixstr} {obj.Name}...'";
      comm.act(ATTypes.AT_TELL, buffer, ch, null, keeper, ToTypes.Character);
      comm.act(ATTypes.AT_TELL, "$n tells you, 'Which I see you can't afford.'", ch, null, keeper, ToTypes.Character);
    }
    else
    {
      buffer = $"$n gives $p to $N, who quickly {fixstr2} it.";
      comm.act(ATTypes.AT_ACTION, buffer, ch, obj, keeper, ToTypes.Room);

      buffer = $"$n charges you {cost} gold piece{(cost == 1 ? "" : "s")} to {fixstr} $p.";
      comm.act(ATTypes.AT_ACTION, buffer, ch, obj, keeper, ToTypes.Character);

      ch.CurrentCoin -= cost;
      keeper.CurrentCoin += cost;

      if (keeper.CurrentCoin < 0)
        keeper.CurrentCoin = 0;
      else if (keeper.CurrentCoin > maxgold)
      {
        keeper.CurrentRoom.Area.BoostEconomy(keeper.CurrentCoin - maxgold / 2);
        keeper.CurrentCoin = maxgold / 2;
        comm.act(ATTypes.AT_ACTION, "$n puts some gold into a large safe.", keeper, null, null, ToTypes.Room);
      }

      switch (obj.ItemType)
      {
        case ItemTypes.None:
        case ItemTypes.Light:
        case ItemTypes.Scroll:
        case ItemTypes.FireWeapon:
        case ItemTypes.Missile:
        case ItemTypes.Treasure:
        case ItemTypes.Potion:
        case ItemTypes.Worn:
        case ItemTypes.Furniture:
        case ItemTypes.Trash:
        case ItemTypes.OldTrap:
        case ItemTypes.Container:
        case ItemTypes.Note:
        case ItemTypes.DrinkContainer:
        case ItemTypes.Key:
        case ItemTypes.Food:
        case ItemTypes.Money:
        case ItemTypes.Pen:
        case ItemTypes.Boat:
        case ItemTypes.NpcCorpse:
        case ItemTypes.PlayerCorpse:
        case ItemTypes.Fountain:
        case ItemTypes.Pill:
        case ItemTypes.Blood:
        case ItemTypes.BloodStain:
        case ItemTypes.Scraps:
        case ItemTypes.Pipe:
        case ItemTypes.HerbContainer:
        case ItemTypes.Herb:
        case ItemTypes.Incense:
        case ItemTypes.Fire:
        case ItemTypes.Book:
        case ItemTypes.Switch:
        case ItemTypes.Lever:
        case ItemTypes.PullChain:
        case ItemTypes.Button:
        case ItemTypes.Dial:
        case ItemTypes.Rune:
        case ItemTypes.RunePouch:
        case ItemTypes.Match:
        case ItemTypes.Trap:
        case ItemTypes.Map:
        case ItemTypes.Portal:
        case ItemTypes.Paper:
        case ItemTypes.Tinder:
        case ItemTypes.LockPick:
        case ItemTypes.Spike:
        case ItemTypes.Disease:
        case ItemTypes.Oil:
        case ItemTypes.Fuel:
        case ItemTypes.Empty1:
        case ItemTypes.Empty2:
        case ItemTypes.MissileWeapon:
        case ItemTypes.Projectile:
        case ItemTypes.Quiver:
        case ItemTypes.Shovel:
        case ItemTypes.Salve:
        case ItemTypes.Cook:
        case ItemTypes.KeyRing:
        case ItemTypes.Odor:
        case ItemTypes.Chance:
        case ItemTypes.DrinkMixture:
        default:
          ch.SendTo("For some reason, you think you got ripped off...");
          break;
        case ItemTypes.Armor:
          obj.Values.CurrentAC = obj.Values.OriginalAC;
          break;
        case ItemTypes.Weapon:
          obj.Values.Condition = GameConstants.GetConstant<int>("InitWeaponCondition");
          break;
        case ItemTypes.Wand:
        case ItemTypes.Staff:
          obj.Values.Charges = obj.Values.MaxCharges;
          break;
      }

      MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Repair, ch, obj);
    }
  }
}