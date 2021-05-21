using Realm.Library.Common.Extensions;
using SmaugCS.Commands.Social;
using SmaugCS.Data;
using SmaugCS.Extensions.Character;
using System.Collections.Generic;
using System.Linq;

namespace SmaugCS.Lookup
{
    public class CommandLookupTable : LookupBase<CommandData, DoFunction>
    {
        public CommandLookupTable()
            : base(new DoFunction { Value = (ch, arg) => ch.SendTo("Huh?") })
        {
            LookupTable.Add("do_say", new DoFunction { Value = Say.do_say });

            // TODO Add command references here
        }

        public override void UpdateFunctionReferences(IEnumerable<CommandData> values)
        {
            foreach (var command in values.Where(x => !x.FunctionName.IsNullOrEmpty()))
            {
                if (command.DoFunction == null)
                    command.DoFunction = new DoFunction();

                command.DoFunction = GetFunction(command.FunctionName);
            }
        }
    }
}
