using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Realm.Library.Common;
using Realm.Library.Patterns.Command;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Extensions;
using SmaugCS.Helpers;
using SmaugCS.Managers;

namespace SmaugCS
{
    public static class interp
    {
        private static readonly List<KeyValuePair<PositionTypes, string>> PositionMap 
            = new List<KeyValuePair<PositionTypes, string>>
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

                if (ch.IsInCombatPosition() && position <= (int)PositionTypes.Evasive)
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

        public static void interpret(CharacterInstance ch, string argument)
        {
            Validation.IsNotNull(ch, "ch");
            if (ch.CurrentRoom == null)
                throw new NullReferenceException("Null room reference");

            string logLine = string.Empty;
            CommandData foundCmd = null;

            if (ch.SubState == CharacterSubStates.RepeatCommand)
            {
                DoFunction fun = ch.LastCommand;
                if (fun == null)
                {
                    ch.SubState = CharacterSubStates.None;
                    throw new InvalidDataException("CharacterSubStates.RepeatCommand with null LastCommand");
                }

                foreach (CommandData cmd in DatabaseManager.Instance.COMMANDS.Values)
                {
                    if (cmd.DoFunction == fun)
                    {
                        foundCmd = cmd;
                        break;
                    }
                }

                if (foundCmd == null)
                    throw new InvalidDataException("CharacterSubStates.RepeatCommand: LastCommand was invalid");

                logLine = string.Format("({0}) {1}", foundCmd.Name, argument);
            }

            if (foundCmd == null)
            {
                // TODO 
            }

            string lastPlayerCmd = string.Format("{0} used {1}", ch.Name, logLine);
            if (foundCmd != null && foundCmd.Log == LogAction.Never)
                logLine = "XXXXXXXX XXXXXXXX XXXXXXXX";

            if (!ch.IsNpc() && ch.Descriptor != null && valid_watch(logLine))
            {
                if (foundCmd != null && foundCmd.Flags.IsSet(CommandFlags.Watch))
                {
                    // TODO Write the watch
                }
                else if (ch.PlayerData.Flags.IsSet(PCFlags.Watch))
                {
                    // TODO Write the watch
                }
            }

            // TODO Some more logging/snooping stuff

            TimerData timer = ch.GetTimer(TimerTypes.DoFunction);
            if (timer != null)
            {
                CharacterSubStates substate = ch.SubState;
                ch.SubState = CharacterSubStates.TimerDoAbort;
                timer.Action.Value.Invoke(ch, string.Empty);
                if (ch.CharDied())
                    return;
                if (ch.SubState != CharacterSubStates.TimerDoAbort)
                {
                    ch.SubState = substate;
                    // TODO Extract timer
                }
                else
                {
                    ch.SubState = substate;
                    return;
                }
            }

            // TODO Look for command in skill/social table

            if (!check_pos(ch, foundCmd.Position))
                return;

            string buf = check_cmd_flags(ch, foundCmd);
            if (!buf.IsNullOrEmpty())
            {
                color.send_to_char_color(buf, ch);
                return;
            }

            // TODO Nuisance

            ch.PreviousCommand = ch.LastCommand;
            ch.LastCommand = foundCmd.DoFunction;
            
            // TODO Timer

            // tail_chain();
        }

        public static bool check_social(CharacterInstance ch, string command, string argument)
        {
            SocialData social = DatabaseManager.Instance.GetEntity<SocialData>(command);
            if (social == null)
                return false;

            if (CheckFunctions.CheckIfTrue(ch, !ch.IsNpc() && ch.Act.IsSet(PlayerFlags.NoEmote), "You are anti-social!"))
                return true;

            switch (ch.CurrentPosition)
            {
                case PositionTypes.Dead:
                    color.send_to_char("Lie still; you are DEAD.", ch);
                    return true;
                case PositionTypes.Incapacitated:
                case PositionTypes.Mortal:
                    color.send_to_char("You are hurt far too badly for that.", ch);
                    return true;
                case PositionTypes.Stunned:
                    color.send_to_char("You are too stunned to do that.", ch);
                    return true;
                case PositionTypes.Sleeping:
                    if (social.Name.EqualsIgnoreCase("snore"))
                        break;
                    color.send_to_char("In your dreams, or what?", ch);
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
            string buf = string.Empty;
            if (ch.IsAffected(AffectedByTypes.Possess) && cmd.Flags.IsSet(CommandFlags.Possess))
                buf = string.Format("You can't {0} while you are possessing someone!", cmd.Name);
            else if (ch.CurrentMorph != null && cmd.Flags.IsSet(CommandFlags.Polymorphed))
                buf = string.Format("You can't {0} while you are polymorphed!", cmd.Name);
            return buf;
        }
    }
}
