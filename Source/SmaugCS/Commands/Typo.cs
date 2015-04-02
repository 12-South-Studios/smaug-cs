﻿using System.IO;
using Realm.Library.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Commands
{
    public static class Typo
    {
        public static void do_typo(CharacterInstance ch, string argument)
        {
           ch.SetColor(ATTypes.AT_PLAIN);

            if (Helpers.CheckFunctions.CheckIfEmptyString(ch, argument,
                "Usage:  'typo <message>'  (your location is automatically recorded)"))
            {
                if (ch.Trust >= LevelConstants.GetLevel(ImmortalTypes.Ascendant))
                    ch.SendTo("Usage:  'typo list' or 'typo clear now'");
                return;
            }

            if (argument.EqualsIgnoreCase("clear now")
                && ch.Trust >= LevelConstants.GetLevel(ImmortalTypes.Ascendant))
            {
                var path = SystemConstants.GetSystemFile(SystemFileTypes.Typo);
                using (var proxy = new TextWriterProxy(new StreamWriter(path, false)))
                {
                    proxy.Write(string.Empty);
                }
                ch.SendTo("Typo file cleared.");
            }
        }
    }
}