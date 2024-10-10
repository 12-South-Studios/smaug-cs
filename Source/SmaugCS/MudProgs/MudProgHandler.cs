using SmaugCS.Constants.Enums;
using System.Collections.Generic;
using System.Linq;
using SmaugCS.MudProgs.MobileProgs;

namespace SmaugCS.MudProgs;

public class MudProgHandler : IMudProgHandler
{
  private static readonly Dictionary<MudProgLocationTypes, List<MudProgFunction>> MudProgTable = new()
  {
    {
      MudProgLocationTypes.Mobile, [
        new MudProgFunction { Type = MudProgTypes.Act, Function = ActProg.Execute },
        new MudProgFunction { Type = MudProgTypes.Bribe, Function = BribeProg.Execute },
        new MudProgFunction { Type = MudProgTypes.Command, Function = CommandProg.Execute },
        new MudProgFunction { Type = MudProgTypes.Death, Function = DeathProg.Execute },
        new MudProgFunction { Type = MudProgTypes.Entry, Function = EntryProg.Execute },
        new MudProgFunction { Type = MudProgTypes.Fight, Function = FightProg.Execute },
        new MudProgFunction { Type = MudProgTypes.Give, Function = GiveProg.Execute },
        new MudProgFunction { Type = MudProgTypes.Greet, Function = GreetProg.Execute },
        new MudProgFunction { Type = MudProgTypes.HitPercent, Function = HitPercentProg.Execute }
      ]
    },
    {
      MudProgLocationTypes.Object, [new MudProgFunction { Type = MudProgTypes.Act, Function = ObjectProgs.ActProg.Execute }]
    },
    {
      MudProgLocationTypes.Room, [new MudProgFunction { Type = MudProgTypes.Act, Function = RoomProgs.ActProg.Execute }]
    }
  };

  public bool Execute(MudProgLocationTypes locationType, MudProgTypes mudProgType, params object[] args)
  {
    if (!MudProgTable.TryGetValue(locationType, out List<MudProgFunction> value)) return false;

    List<MudProgFunction> functionList = value;
    if (functionList.All(x => x.Type != mudProgType)) return false;

    MudProgFunction function = functionList.First(x => x.Type == mudProgType);
    return function.Function.Invoke(args);
  }

  public static bool ExecuteMobileProg(IMudProgHandler handler, MudProgTypes mudProgType, params object[] args)
    => handler.Execute(MudProgLocationTypes.Mobile, mudProgType, args);

  public static bool ExecuteObjectProg(IMudProgHandler handler, MudProgTypes mudProgType, params object[] args)
    => handler.Execute(MudProgLocationTypes.Object, mudProgType, args);

  public static bool ExecuteRoomProg(IMudProgHandler handler, MudProgTypes mudProgType, params object[] args)
    => handler.Execute(MudProgLocationTypes.Room, mudProgType, args);
}