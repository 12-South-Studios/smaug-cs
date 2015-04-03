using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Realm.Library.Common;
using Realm.Library.Patterns.Command;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;
using SmaugCS.Managers;
using SmaugCS.Repository;

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
                var kvp = PositionMap.FirstOrDefault(x => x.Key == ch.CurrentPosition);

                if (ch.IsInCombatPosition() && position <= (int)PositionTypes.Evasive)
                    ch.SendTo(FightingMessage);
                else
                    ch.SendTo(kvp.Value);
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

            var logLine = string.Empty;
            CommandData foundCmd = null;

            if (((PlayerInstance)ch).SubState == CharacterSubStates.RepeatCommand)
            {
                var fun = ch.LastCommand;
                if (fun == null)
                {
                    ((PlayerInstance)ch).SubState = CharacterSubStates.None;
                    throw new InvalidDataException("CharacterSubStates.RepeatCommand with null LastCommand");
                }

                foreach (var cmd in RepositoryManager.Instance.COMMANDS.Values)
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

            var lastPlayerCmd = string.Format("{0} used {1}", ch.Name, logLine);
            if (foundCmd != null && foundCmd.Log == LogAction.Never)
                logLine = "XXXXXXXX XXXXXXXX XXXXXXXX";

            if (!ch.IsNpc() && ((PlayerInstance)ch).Descriptor != null && valid_watch(logLine))
            {
                if (foundCmd != null && foundCmd.Flags.IsSet(CommandFlags.Watch))
                {
                    // TODO Write the watch
                }
                else if (((PlayerInstance)ch).PlayerData.Flags.IsSet(PCFlags.Watch))
                {
                    // TODO Write the watch
                }
            }

            // TODO Some more logging/snooping stuff

            var timer = ch.GetTimer(TimerTypes.DoFunction);
            if (timer != null)
            {
                var substate = ((PlayerInstance)ch).SubState;
                ((PlayerInstance)ch).SubState = CharacterSubStates.TimerDoAbort;
                timer.Action.Value.Invoke(ch, string.Empty);
                if (ch.CharDied())
                    return;
                if (((PlayerInstance)ch).SubState != CharacterSubStates.TimerDoAbort)
                {
                    ((PlayerInstance)ch).SubState = substate;
                    // TODO Extract timer
                }
                else
                {
                    ((PlayerInstance)ch).SubState = substate;
                    return;
                }
            }

            // TODO Look for command in skill/social table

            if (!check_pos(ch, foundCmd.Position))
                return;

            var buf = check_cmd_flags(ch, foundCmd);
            if (!buf.IsNullOrEmpty())
            {
                ch.SendTo(buf);
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
            var social = RepositoryManager.Instance.GetEntity<SocialData>(command);
            if (social == null)
                return false;

            if (CheckFunctions.CheckIfTrue(ch, !ch.IsNpc() && ch.Act.IsSet(PlayerFlags.NoEmote), "You are anti-social!"))
                return true;

            switch (ch.CurrentPosition)
            {
                case PositionTypes.Dead:
                    ch.SendTo("Lie still; you are DEAD.");
                    return true;
                case PositionTypes.Incapacitated:
                case PositionTypes.Mortal:
                    ch.SendTo("You are hurt far too badly for that.");
                    return true;
                case PositionTypes.Stunned:
                    ch.SendTo("You are too stunned to do that.");
                    return true;
                case PositionTypes.Sleeping:
                    if (social.Name.EqualsIgnoreCase("snore"))
                        break;
                    ch.SendTo("In your dreams, or what?");
                    return true;
            }

            var i = 0;
            // search the room for characters ignoring the social-sender and 
            // temporarily remove them from the room until the social has 
            // been completed
            var room = ch.CurrentRoom;
            var ignoringList = new List<CharacterInstance>();
            foreach (var victim in ch.CurrentRoom.Persons)
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
                        victim.SetColor(ATTypes.AT_IGNORE);
                        victim.Printf("You attempt to ignore %s, but are unable to do so.\r\n", ch.Name);
                    }
                }
            }

            // TODO
            return false;
        }

        public static string check_cmd_flags(CharacterInstance ch, CommandData cmd)
        {
            var buf = string.Empty;
            if (ch.IsAffected(AffectedByTypes.Possess) && cmd.Flags.IsSet(CommandFlags.Possess))
                buf = string.Format("You can't {0} while you are possessing someone!", cmd.Name);
            else if (ch.CurrentMorph != null && cmd.Flags.IsSet(CommandFlags.Polymorphed))
                buf = string.Format("You can't {0} while you are polymorphed!", cmd.Name);
            return buf;
        }
    }
}
