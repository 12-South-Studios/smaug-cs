using Ninject;
using SmaugCS.Constants.Enums;
using System.Collections.Generic;
using System.Linq;

namespace SmaugCS.MudProgs
{
    public class MudProgHandler : IMudProgHandler
    {
        private static IKernel _kernel;

        public MudProgHandler(IKernel kernel)
        {
            _kernel = kernel;
        }

        private static readonly Dictionary<MudProgLocationTypes, List<MudProgFunction>> MudProgTable = new Dictionary
            <MudProgLocationTypes, List<MudProgFunction>>
        {
            {
                MudProgLocationTypes.Mobile, new List<MudProgFunction>
                {
                    new MudProgFunction {Type = MudProgTypes.Act, Function = Mobile.ActProg.Execute},
                    new MudProgFunction {Type = MudProgTypes.Bribe, Function = Mobile.BribeProg.Execute},
                    new MudProgFunction {Type = MudProgTypes.Command, Function = Mobile.CommandProg.Execute},
                    new MudProgFunction {Type = MudProgTypes.Death, Function = Mobile.DeathProg.Execute},
                    new MudProgFunction {Type = MudProgTypes.Entry, Function = Mobile.EntryProg.Execute},
                    new MudProgFunction {Type = MudProgTypes.Fight, Function = Mobile.FightProg.Execute},
                    new MudProgFunction {Type = MudProgTypes.Give, Function = Mobile.GiveProg.Execute},
                    new MudProgFunction {Type = MudProgTypes.Greet, Function = Mobile.GreetProg.Execute},
                    new MudProgFunction {Type = MudProgTypes.HitPercent, Function = Mobile.HitPercentProg.Execute}
                }
            },
            {
                MudProgLocationTypes.Object, new List<MudProgFunction>
                {
                    new MudProgFunction {Type = MudProgTypes.Act, Function = Object.ActProg.Execute}
                }
            },
            {
                MudProgLocationTypes.Room, new List<MudProgFunction>
                {
                    new MudProgFunction {Type = MudProgTypes.Act, Function = Room.ActProg.Execute}
                }
            }
        };

        public bool Execute(MudProgLocationTypes locationType, MudProgTypes mudProgType, params object[] args)
        {
            if (!MudProgTable.ContainsKey(locationType)) return false;

            var functionList = MudProgTable[locationType];
            if (functionList.All(x => x.Type != mudProgType)) return false;

            var function = functionList.First(x => x.Type == mudProgType);
            return function.Function.Invoke(args);
        }

        public static bool ExecuteMobileProg(MudProgTypes mudProgType, params object[] args)
            => _kernel.Get<IMudProgHandler>().Execute(MudProgLocationTypes.Mobile, mudProgType, args);

        public static bool ExecuteObjectProg(MudProgTypes mudProgType, params object[] args)
            => _kernel.Get<IMudProgHandler>().Execute(MudProgLocationTypes.Object, mudProgType, args);

        public static bool ExecuteRoomProg(MudProgTypes mudProgType, params object[] args)
            => _kernel.Get<IMudProgHandler>().Execute(MudProgLocationTypes.Room, mudProgType, args);
    }
}
