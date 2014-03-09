using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Managers;

namespace SmaugCS
{
    public static class interp
    {
        private static readonly List<KeyValuePair<PositionTypes, string>> PositionMap = new List<KeyValuePair<PositionTypes, string>>()
            {
                new KeyValuePair<PositionTypes, string>(PositionTypes.Dead, "A little difficult to do when you are DEAD...\r\n"),
                new KeyValuePair<PositionTypes, string>(PositionTypes.Mortal, "You are hurt far too badly for that.\r\n"),
                new KeyValuePair<PositionTypes, string>(PositionTypes.Incapacitated, "You are hurt far too badly for that.\r\n"),
                new KeyValuePair<PositionTypes, string>(PositionTypes.Stunned, "You are too stunned to do that.\r\n"),
                new KeyValuePair<PositionTypes, string>(PositionTypes.Sleeping, "In your dreams, or what?\r\n"),
                new KeyValuePair<PositionTypes, string>(PositionTypes.Resting, "Nah... You feel too relaxed...\r\n"),
                new KeyValuePair<PositionTypes, string>(PositionTypes.Sitting, "You can't do that sitting down.\r\n"),
                new KeyValuePair<PositionTypes, string>(PositionTypes.Fighting, "This fighting style is too demanding for that!\r\n"),
                new KeyValuePair<PositionTypes, string>(PositionTypes.Defensive, "This fighting style is too demanding for that!\r\n"),
                new KeyValuePair<PositionTypes, string>(PositionTypes.Aggressive, "This fighting style is too demanding for that!\r\n"),
                new KeyValuePair<PositionTypes, string>(PositionTypes.Berserk, "This fighting style is too demanding for that!\r\n"),
                new KeyValuePair<PositionTypes, string>(PositionTypes.Evasive, "No way!  You are still fighting!\r\n")
            };
        private const string FightingMessage = "No way!  You are still fighting!\r\n";

        public static bool check_pos(CharacterInstance ch, int position)
        {
            if (ch.IsNpc() && (int)ch.CurrentPosition > 3)
                return true;

            if ((int)ch.CurrentPosition < position)
            {
                KeyValuePair<PositionTypes, string> kvp = PositionMap.FirstOrDefault(x => x.Key == ch.CurrentPosition);
                bool fighting = (ch.CurrentPosition == PositionTypes.Fighting
                                 || ch.CurrentPosition == PositionTypes.Defensive
                                 || ch.CurrentPosition == PositionTypes.Aggressive
                                 || ch.CurrentPosition == PositionTypes.Berserk);

                if (fighting && position <= (int)PositionTypes.Evasive)
                    color.send_to_char(FightingMessage, ch);
                else
                    color.send_to_char(kvp.Value, ch);
                return false;
            }

            return true;
        }

        public static bool valid_watch(string logline)
        {
            if (logline.Length == 1 && (logline.StartsWith("n") || logline.StartsWith("s")
                || logline.StartsWith("w") || logline.StartsWith("u") || logline.StartsWith("d")))
                return false;
            if (logline.Length == 2 && (logline.StartsWith("ne") || logline.StartsWith("nw")))
                return false;
            if (logline.Length == 3 && (logline.StartsWith("se") || logline.StartsWith("sw")))
                return false;
            return true;
        }

        public static void write_watch_files(CharacterInstance ch, CommandData cmd, string logline)
        {
            // TODO
        }

        public static void interpret(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static bool check_social(CharacterInstance ch, string command, string argument)
        {
            SocialData social = DatabaseManager.Instance.GetEntity<SocialData>(command);
            if (social == null)
                return false;
            if (!ch.IsNpc() && ch.Act.IsSet((int)PlayerFlags.NoEmote))
            {
                color.send_to_char("You are anti-social!\r\n", ch);
                return true;
            }

            switch (ch.CurrentPosition)
            {
                case PositionTypes.Dead:
                    color.send_to_char("Lie still; you are DEAD.\r\n", ch);
                    return true;
                case PositionTypes.Incapacitated:
                case PositionTypes.Mortal:
                    color.send_to_char("You are hurt far too badly for that.\r\n", ch);
                    return true;
                case PositionTypes.Stunned:
                    color.send_to_char("You are too stunned to do that.\r\n", ch);
                    return true;
                case PositionTypes.Sleeping:
                    if (social.Name.EqualsIgnoreCase("snore"))
                        break;
                    color.send_to_char("In your dreams, or what?\r\n", ch);
                    return true;
            }

            int i = 0;
            // search the room for characters ignoring the social-sender and 
            // temporarily remove them from the room until the social has 
            // been completed
            RoomTemplate room = ch.CurrentRoom;
            List<CharacterInstance> ignoringList = new List<CharacterInstance>();
            foreach (CharacterInstance victim in ch.CurrentRoom.Persons)
            {
                if (i == 127)
                    break;
                if (victim.IsIgnoring(ch))
                {
                    if (!ch.IsImmortal() || victim.Trust > ch.Trust)
                    {
                        ignoringList.Add(victim);
                        i++;
                        room.Persons.Remove(victim);
                    }
                    else
                    {
                        color.set_char_color(ATTypes.AT_IGNORE, victim);
                        color.ch_printf(victim, "You attempt to ignore %s, but are unable to do so.\r\n", ch.Name);
                    }
                }
            }

            // TODO
            return false;
        }

        public static string check_cmd_flags(CharacterInstance ch, CommandData cmd)
        {
            // TODO
            return string.Empty;
        }
    }
}
