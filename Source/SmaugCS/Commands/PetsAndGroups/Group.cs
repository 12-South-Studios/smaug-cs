using System;
using System.Linq;
using Realm.Library.Common;
using Realm.Library.Patterns.Repository;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Repository;

namespace SmaugCS.Commands.PetsAndGroups
{
    public static class Group
    {
        public static void do_group(CharacterInstance ch, string argument)
        {
            var firstArg = argument.FirstWord();
            if (string.IsNullOrWhiteSpace(firstArg))
                display_group(ch);
            else if (firstArg.Equals("disband"))
                disband_group(ch);
            else if (firstArg.Equals("all"))
                group_all(ch);
            else
                group_player(ch, argument.SecondWord());
        }

        private static void display_group(CharacterInstance ch)
        {
            var leader = ch.Leader ?? ch;
            ch.SetColor(ATTypes.AT_DGREEN);
            ch.Printf("\r\nFollowing %-12.12s     [hitpnts]   [ magic ] [mst] [mvs] [race]%s\r\n",
                            Macros.PERS(leader, ch), ch.Level < LevelConstants.AvatarLevel ? " [to lvl]" : "");

            foreach (var gch in RepositoryManager.Instance.CHARACTERS.Values.Where(x => x.IsSameGroup(ch)))
            {
                ch.SetColor(ATTypes.AT_DGREEN);
                var buffer = string.Empty;

                if (gch.IsAffected(AffectedByTypes.Possess))
                {
                    ch.Printf("[%2d %s] %-16s %4s/%4s hp %4s/%4s %s %4s/%4s mv %5s xp\r\n",
                                    gch.Level,
                                    gch.IsNpc() ? "Mob" : RepositoryManager.Instance.GetClass(gch.CurrentClass).Name,
                                    Macros.PERS(gch, ch).CapitalizeFirst(),
                                    "????", "????", "????", "????", gch.IsVampire() ? "bp" : "mana", "????",
                                    "????", "?????");
                }
                else
                    buffer = GetAlignmentString(gch.CurrentAlignment);

                ch.SetColor(ATTypes.AT_DGREEN);
                ch.SendTo("[");
                ch.SetColor(ATTypes.AT_DGREEN);
                ch.Printf("%-2d %2.2s %3.3s", gch.Level, buffer,
                                gch.IsNpc() ? "Mob" : RepositoryManager.Instance.GetClass(gch.CurrentClass).Name);
                ch.SetColor(ATTypes.AT_DGREEN);
                ch.SendTo("]  ");
                ch.SetColor(ATTypes.AT_DGREEN);
                ch.Printf("%-12.12s ", Macros.PERS(gch, ch).CapitalizeFirst());

                if (gch.CurrentHealth < gch.MaximumHealth / 4)
                    ch.SetColor(ATTypes.AT_DANGER);
                else if (gch.CurrentHealth < gch.MaximumHealth / 2.5f)
                    ch.SetColor(ATTypes.AT_YELLOW);
                else
                    ch.SetColor(ATTypes.AT_GREY);

                ch.Printf("%5d", gch.CurrentHealth);
                ch.SetColor(ATTypes.AT_GREY);
                ch.Printf("/%-5d ", gch.MaximumHealth);

                ch.SetColor(gch.IsVampire()
                                         ? ATTypes.AT_BLOOD
                                         : ATTypes.AT_LBLUE);

                if (gch.CurrentClass != ClassTypes.Warrior)
                {
                    ch.Printf("%5d/%-5d ",
                                    gch.IsVampire() && !gch.IsNpc()
                                        ? ((PlayerInstance)gch).PlayerData.GetConditionValue(ConditionTypes.Bloodthirsty)
                                        : gch.CurrentMana,
                                    gch.IsVampire() ? 10 + gch.Level : gch.MaximumMana);
                }
                else
                    ch.SendTo("            ");

                if (gch.MentalState < -25 || gch.MentalState > 25)
                    ch.SetColor(ATTypes.AT_YELLOW);
                else
                    ch.SetColor(ATTypes.AT_GREEN);

                ch.Printf("%3.3s  ",
                                gch.MentalState > 75
                                    ? "+++"
                                    : gch.MentalState > 50
                                          ? "=++"
                                          : gch.MentalState > 25
                                                ? "==+"
                                                : gch.MentalState > -25
                                                      ? "==="
                                                      : gch.MentalState > -50 ? "-==" : gch.MentalState > -75 ? "--=" : "---");

                ch.SetColor(ATTypes.AT_DGREEN);
                ch.Printf("%5d ", gch.CurrentMovement);
                ch.Printf("%6s ", gch.CurrentRace.GetName());
                ch.SetColor(ATTypes.AT_GREEN);

                if (gch.Level < LevelConstants.AvatarLevel)
                    ch.Printf("%8d ", gch.GetExperienceLevel(gch.Level + 1) - gch.Experience);

                ch.SendTo("\r\n");
            }
        }

        private static string GetAlignmentString(int alignment)
        {
            if (alignment > 750)
                return " A";
            if (alignment > 350)
                return "-A";
            if (alignment > 150)
                return "+N";
            if (alignment > -150)
                return " N";
            if (alignment > -350)
                return "-N";
            return alignment > -750 ? "+S" : " S";
        }
        private static void disband_group(CharacterInstance ch)
        {
            if (ch.Leader != null || ch.Master != null)
            {
                ch.SendTo("You cannot disband a group if you're following someone.");
                return;
            }

            var count = 0;
            foreach (var gch in RepositoryManager.Instance.CHARACTERS.CastAs<Repository<long, CharacterInstance>>().Values.Where(x => x.IsSameGroup(ch) && x != ch))
            {
                gch.Leader = null;
                gch.Master = null;
                count++;
                gch.SendTo("Your group is disbanded.");
            }

            ch.SendTo(count == 0
                                   ? "You have no group members to disband."
                                   : "You disband your group.");
        }

        private static void group_all(CharacterInstance ch)
        {
            var count = 0;
            foreach (var rch in ch.CurrentRoom.Persons.Where(x =>
                x != ch
                && !x.IsNpc()
                && ch.CanSee(x)
                && x.Master == ch
                && ch.Master == null
                && ch.Leader == null
                && Math.Abs(ch.Level - x.Level) > 9
                && !x.IsSameGroup(ch)
                && ch.IsPKill() == x.IsPKill()))
            {
                rch.Leader = ch;
                count++;
            }

            if (count == 0)
                ch.SendTo("You have no eligible group members.");
            else
            {
                comm.act(ATTypes.AT_ACTION, "$n groups $s followers.", ch, null, null, ToTypes.Room);
                ch.SendTo("You group your followers.");
            }
        }

        private static void group_player(CharacterInstance ch, string argument)
        {
            var victim = ch.GetCharacterInRoom(argument);
            if (victim == null)
            {
                ch.SendTo("They aren't here.");
                return;
            }

            if (ch.Master != null || (ch.Leader != null && ch.Leader != ch))
            {
                ch.SendTo("But you are following someone else!");
                return;
            }

            if (victim.Master != ch && ch != victim)
            {
                comm.act(ATTypes.AT_PLAIN, "$N isn't following you.", ch, null, victim, ToTypes.Character);
                return;
            }

            if (victim == ch)
            {
                comm.act(ATTypes.AT_PLAIN, "You can't group yourself.", ch, null, victim, ToTypes.Character);
                return;
            }

            if (victim.IsSameGroup(ch) && ch != victim)
            {
                victim.Leader = null;
                comm.act(ATTypes.AT_ACTION, "$n removes $N from $s group.", ch, null, victim, ToTypes.NotVictim);
                comm.act(ATTypes.AT_ACTION, "$n removes you from $s group.", ch, null, victim, ToTypes.Victim);
                comm.act(ATTypes.AT_ACTION, "You remove $N from your group.", ch, null, victim, ToTypes.Character);
                return;
            }

            if (ch.Level - victim.Level < -8
                || ch.Level - victim.Level > 8
                || (ch.IsPKill() != victim.IsPKill()))
            {
                comm.act(ATTypes.AT_PLAIN, "$N cannot join $n's group.", ch, null, victim, ToTypes.NotVictim);
                comm.act(ATTypes.AT_PLAIN, "You cannot join $n's group.", ch, null, victim, ToTypes.Victim);
                comm.act(ATTypes.AT_PLAIN, "$N cannot join your group.", ch, null, victim, ToTypes.Character);
                return;
            }

            victim.Leader = ch;
            comm.act(ATTypes.AT_ACTION, "$N oins $n's group.", ch, null, victim, ToTypes.NotVictim);
            comm.act(ATTypes.AT_ACTION, "You join $n's group.", ch, null, victim, ToTypes.Victim);
            comm.act(ATTypes.AT_ACTION, "$N joins your group.", ch, null, victim, ToTypes.Character);
        }
    }
}
