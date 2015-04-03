using System;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Repository;

namespace SmaugCS.Commands.Social
{
    public static class Ignore
    {
        public static void do_ignore(CharacterInstance ch, string argument)
        {
            var arg = argument.FirstWord();

            var pch = (PlayerInstance) ch;
            if (string.IsNullOrEmpty(arg))
            {
                DisplayIgnoreList(pch);
                return;
            }

            if (arg.EqualsIgnoreCase("none"))
            {
                ClearIgnoreList(pch);
                return;
            }

            if (arg.EqualsIgnoreCase("self") || pch.Name.IsAnyEqual(arg))
            {
               pch.SetColor(ATTypes.AT_IGNORE);
                pch.Printf("Did you type something?");
                return;
            }

            IgnoreCharacter(arg, pch);
        }

        private static void DisplayIgnoreList(PlayerInstance ch)
        {
           ch.SetColor(ATTypes.AT_DIVIDER);
            ch.Printf("----------------------------------------");
           ch.SetColor(ATTypes.AT_DGREEN);
            ch.Printf("You are currently ignoring:");
           ch.SetColor(ATTypes.AT_DIVIDER);
            ch.Printf("----------------------------------------");
           ch.SetColor(ATTypes.AT_IGNORE);

            foreach (var ignore in ch.PlayerData.Ignored)
            {
                ch.Printf("\t  - %s", ignore.Name);
            }
        }

        private static void ClearIgnoreList(PlayerInstance ch)
        {
            ch.PlayerData.Ignored.Clear();
           ch.SetColor(ATTypes.AT_IGNORE);
            ch.Printf("You now ignore no one.");
        }

        private static void IgnoreCharacter(string arg, PlayerInstance ch)
        {
            var name = arg;

            if (name.EqualsIgnoreCase("reply"))
            {
                if (ch.ReplyTo == null)
                {
                   ch.SetColor(ATTypes.AT_IGNORE);
                    ch.Printf("They're not here.");
                    return;
                }

                name = ch.ReplyTo.Name;
            }

            if (ch.PlayerData.Ignored.Any(x => x.Name.EqualsIgnoreCase(name)))
            {
                RemoveIgnoredPlayer(name, ch);
                return;
            }

            var victim =
                RepositoryManager.Instance.CHARACTERS.Values.FirstOrDefault(x => x.Name.EqualsIgnoreCase(name));
            if (victim == null)
            {
               ch.SetColor(ATTypes.AT_IGNORE);
                ch.SendTo("No player exists by that name.");
                return;
            }

            // TODO Should there be a maximum to the ignore list?

            var ignore = new IgnoreData
            {
                IgnoredOn = DateTime.Now,
                Name = name
            };
            ch.PlayerData.Ignored.Add(ignore);

            // TODO Save it to the database?

           ch.SetColor(ATTypes.AT_IGNORE);
            ch.Printf("You now ignore %s.", name);
        }

        private static void RemoveIgnoredPlayer(string name, PlayerInstance ch)
        {
            var ignore = ch.PlayerData.Ignored.First(x => x.Name.EqualsIgnoreCase(name));
            ch.PlayerData.Ignored.Remove(ignore);

           ch.SetColor(ATTypes.AT_IGNORE);
            ch.Printf("You no longer ignore %s.", name);
        }
    }
}
