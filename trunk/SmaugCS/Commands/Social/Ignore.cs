using System;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Managers;

namespace SmaugCS.Commands.Social
{
    public static class Ignore
    {
        public static void do_ignore(CharacterInstance ch, string argument)
        {
            string arg = argument.FirstWord();

            PlayerInstance pch = (PlayerInstance) ch;
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
                color.set_char_color(ATTypes.AT_IGNORE, pch);
                color.ch_printf(pch, "Did you type something?");
                return;
            }

            IgnoreCharacter(arg, pch);
        }

        private static void DisplayIgnoreList(PlayerInstance ch)
        {
            color.set_char_color(ATTypes.AT_DIVIDER, ch);
            color.ch_printf(ch, "----------------------------------------");
            color.set_char_color(ATTypes.AT_DGREEN, ch);
            color.ch_printf(ch, "You are currently ignoring:");
            color.set_char_color(ATTypes.AT_DIVIDER, ch);
            color.ch_printf(ch, "----------------------------------------");
            color.set_char_color(ATTypes.AT_IGNORE, ch);

            foreach (IgnoreData ignore in ch.PlayerData.Ignored)
            {
                color.ch_printf(ch, "\t  - %s", ignore.Name);
            }
        }

        private static void ClearIgnoreList(PlayerInstance ch)
        {
            ch.PlayerData.Ignored.Clear();
            color.set_char_color(ATTypes.AT_IGNORE, ch);
            color.ch_printf(ch, "You now ignore no one.");
        }

        private static void IgnoreCharacter(string arg, PlayerInstance ch)
        {
            string name = arg;

            if (name.EqualsIgnoreCase("reply"))
            {
                if (ch.ReplyTo == null)
                {
                    color.set_char_color(ATTypes.AT_IGNORE, ch);
                    color.ch_printf(ch, "They're not here.");
                    return;
                }

                name = ch.ReplyTo.Name;
            }

            if (ch.PlayerData.Ignored.Any(x => x.Name.EqualsIgnoreCase(name)))
            {
                RemoveIgnoredPlayer(name, ch);
                return;
            }

            CharacterInstance victim =
                DatabaseManager.Instance.CHARACTERS.Values.FirstOrDefault(x => x.Name.EqualsIgnoreCase(name));
            if (victim == null)
            {
                color.set_char_color(ATTypes.AT_IGNORE, ch);
                color.send_to_char("No player exists by that name.", ch);
                return;
            }

            // TODO Should there be a maximum to the ignore list?

            IgnoreData ignore = new IgnoreData
            {
                IgnoredOn = DateTime.Now,
                Name = name
            };
            ch.PlayerData.Ignored.Add(ignore);

            // TODO Save it to the database?

            color.set_char_color(ATTypes.AT_IGNORE, ch);
            color.ch_printf(ch, "You now ignore %s.", name);
        }

        private static void RemoveIgnoredPlayer(string name, PlayerInstance ch)
        {
            IgnoreData ignore = ch.PlayerData.Ignored.First(x => x.Name.EqualsIgnoreCase(name));
            ch.PlayerData.Ignored.Remove(ignore);

            color.set_char_color(ATTypes.AT_IGNORE, ch);
            color.ch_printf(ch, "You no longer ignore %s.", name);
        }
    }
}
