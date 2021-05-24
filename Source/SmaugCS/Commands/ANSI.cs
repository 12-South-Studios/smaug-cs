using Realm.Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using System;
using System.Collections.Generic;

namespace SmaugCS.Commands
{
    public static class ANSI
    {
        private static readonly Dictionary<string, Action<CharacterInstance>> AnsiTable = new Dictionary
            <string, Action<CharacterInstance>>
        {
            {"on", AnsiOn},
            {"off", AnsiOff},
            {"error", AnsiError}
        };

        public static void do_ansi(CharacterInstance ch, string argument)
        {
            var arg = argument.FirstWord().ToLower();

            Action<CharacterInstance> value;
            if (AnsiTable.TryGetValue(arg, out value))
                value.Invoke(ch);
        }

        private static void AnsiError(CharacterInstance actor)
        {
            actor.SendTo("ANSI ON or OFF?");
        }

        private static void AnsiOn(CharacterInstance actor)
        {
            actor.Act.IsSet((int)PlayerFlags.Ansi);
            actor.SetColor(ATTypes.AT_WHITE | ATTypes.AT_BLINK);
            actor.SendTo("ANSI ON!!!");
        }

        private static void AnsiOff(CharacterInstance actor)
        {
            actor.Act.RemoveBit((int)PlayerFlags.Ansi);
            actor.SendTo("Okay... ANSI support is now off.");
        }
    }
}
