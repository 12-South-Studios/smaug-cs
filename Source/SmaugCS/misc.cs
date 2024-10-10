using System;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Repository;
using System.Linq;

namespace SmaugCS;

public static class misc
{
  public static void actiondesc(CharacterInstance ch, ObjectInstance obj)
  {
    string charbuf = obj.Action;
    string roombuf = obj.Action;

    // TODO: Replacements?

    switch (obj.ItemType)
    {
      case ItemTypes.Blood:
      case ItemTypes.Fountain:
        comm.act(ATTypes.AT_ACTION, charbuf, ch, obj, ch, ToTypes.Character);
        comm.act(ATTypes.AT_ACTION, roombuf, ch, obj, ch, ToTypes.Room);
        return;

      case ItemTypes.DrinkContainer:
        LiquidData liq = Program.RepositoryManager.GetEntity<LiquidData>(obj.Value.ToList()[2]);
        comm.act(ATTypes.AT_ACTION, charbuf, ch, obj, liq.Name, ToTypes.Character);
        comm.act(ATTypes.AT_ACTION, roombuf, ch, obj, liq.Name, ToTypes.Room);
        return;

      case ItemTypes.Cook:
      case ItemTypes.Food:
      case ItemTypes.Pill:
        comm.act(ATTypes.AT_ACTION, charbuf, ch, obj, ch, ToTypes.Character);
        comm.act(ATTypes.AT_ACTION, roombuf, ch, obj, ch, ToTypes.Room);
        return;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }
}