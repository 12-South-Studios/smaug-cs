using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmaugCS.Commands.Admin;
using SmaugCS.Commands.PetsAndGroups;
using SmaugCS.Commands.Social;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using Realm.Library.Common.Extensions;

namespace SmaugCS.Lookup
{
    public static class CommandLookupTable
    {
        private static readonly Dictionary<string, Action<CharacterInstance, string>> CommandFunctions =
            new Dictionary<string, Action<CharacterInstance, string>>()
                {
                    {"do_say", Say.do_say},
                    {"do_emote", Emote.do_emote},
                    {"do_chat", Chat.do_chat},
                    {"do_immtalk", ImmTalk.do_immtalk},
                    {"do_avtalk", AvTalk.do_avtalk},
                    {"do_gtell", GTell.do_gtell},
                    {"do_authorize", Authorize.do_authorize},
                    {"do_at", At.do_at}
                };

        public static Action<CharacterInstance, string> GetCommandFunction(string name)
        {
            return CommandFunctions.ContainsKey(name.ToLower())
                       ? CommandFunctions[name.ToLower()]
                       : CommandNotFound;
        }

        public static void CommandNotFound(CharacterInstance ch, string argument)
        {
            // TODO: send_to_char("Huh?\r\n", ch);
        }

        public static void UpdateCommandFunctionReferences(IEnumerable<CommandData> commands)
        {
            foreach (CommandData command in commands.Where(x => !x.FunctionName.IsNullOrEmpty()))
            {
                if (command.DoFunction == null)
                    command.DoFunction = new DoFunction();

                command.DoFunction.Value = GetCommandFunction(command.FunctionName);
            }
        }
    }
}
