using Realm.Library.Common.Extensions;
using SmaugCS.Commands;
using SmaugCS.Data;
using System.Collections.Generic;
using System.Linq;

namespace SmaugCS
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
