using System.Collections.Generic;
using System.Linq;
using SmaugCS.Commands.Social;
using SmaugCS.Data;
using Realm.Library.Common;

namespace SmaugCS.Lookup
{
    /// <summary>
    /// 
    /// </summary>
    public class CommandLookupTable : LookupBase<CommandData, DoFunction>
    {
        /// <summary>
        /// 
        /// </summary>
        public CommandLookupTable()
            : base(new DoFunction {Value = (ch, arg) => color.send_to_char("Huh?", ch)})
        {
            LookupTable.Add("do_say", new DoFunction {Value = Say.do_say});

        }

        public override void UpdateFunctionReferences(IEnumerable<CommandData> values)
        {
            foreach (CommandData command in values.Where(x => !x.FunctionName.IsNullOrEmpty()))
            {
                if (command.DoFunction == null)
                    command.DoFunction = new DoFunction();

                command.DoFunction = GetFunction(command.FunctionName);
            }
        }
    }
}
