using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.Movement
{
    public static class Unbolt
    {
        public static void do_unbolt(CharacterInstance ch, string argument)
        {
            string firstArg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Unbolt what?")) return;

            ExitData exit = ch.FindExit(firstArg, true);
            if (exit == null)
            {
                color.ch_printf(ch, "You see no %s here.", firstArg);
                return;
            }
            
            if (exit.Flags.IsSet(ExitFlags.Secret) && !exit.Keywords.IsAnyEqual(firstArg))
            {
                color.ch_printf(ch, "You see no %s here.", firstArg);
                return;
            }

            if (CheckFunctions.CheckIfNotSet(ch, exit.Flags, ExitFlags.IsDoor, "You can't do that.")) return;
            if (CheckFunctions.CheckIfNotSet(ch, exit.Flags, ExitFlags.Closed, "It's not closed.")) return;
            if (CheckFunctions.CheckIfNotSet(ch, exit.Flags, ExitFlags.IsBolt, "You don't see a bolt."))
                return;
            if (CheckFunctions.CheckIfNotSet(ch, exit.Flags, ExitFlags.Bolted, "It's already unbolted."))
                return;

            if (!exit.Flags.IsSet(ExitFlags.Secret))
            {
                color.send_to_char("*Clunk*", ch);
                comm.act(ATTypes.AT_ACTION, "$n unbolts the $d.", ch, null, exit.Keywords, ToTypes.Room);
                exit.RemoveFlagFromSelfAndReverseExit(ExitFlags.Bolted);
            }   
        }
    }
}
