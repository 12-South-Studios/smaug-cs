using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Common.Enumerations;
using SmaugCS.Constants;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Objects;
using SmaugCS.Managers;
using SmaugCS.Repository;
using SmaugCS.Spells.Smaug;
using System.Linq;

namespace SmaugCS.Skills
{
    public static class skills
    {
        public static void skill_notfound(CharacterInstance ch, string argument)
        {
            ch.SendTo("Huh?");
        }

        public static int get_ssave(string name)
        {
            return LookupConstants.GetFlagIndex(name, LookupManager.Instance.GetLookups("SpellSaves"));
        }

        public static int get_starget(string name)
        {
            return LookupConstants.GetFlagIndex(name, LookupManager.Instance.GetLookups("TargetTypes"));
        }

        public static int get_sdamage(string name)
        {
            return LookupConstants.GetFlagIndex(name, LookupManager.Instance.GetLookups("SpellDamageTypes"));
        }

        public static int get_saction(string name)
        {
            return LookupConstants.GetFlagIndex(name, LookupManager.Instance.GetLookups("SpellActionTypes"));
        }

        public static int get_ssave_effect(string name)
        {
            return LookupConstants.GetFlagIndex(name, LookupManager.Instance.GetLookups("SpellSaveEffects"));
        }

        public static int get_sflag(string name)
        {
            return LookupConstants.GetFlagIndex(name, LookupManager.Instance.GetLookups("SpellFlags"));
        }

        public static int get_spower(string name)
        {
            return LookupConstants.GetFlagIndex(name, LookupManager.Instance.GetLookups("SpellPowerTypes"));
        }

        public static int get_sclass(string name)
        {
            return LookupConstants.GetFlagIndex(name, LookupManager.Instance.GetLookups("SpellClassTypes"));
        }

        public static bool is_legal_kill(CharacterInstance ch, CharacterInstance vch)
        {
            if (ch.IsNpc() || vch.IsNpc())
                return true;
            if (!ch.IsPKill() || !vch.IsPKill())
                return false;
            return ((PlayerInstance)ch).PlayerData.Clan == null ||
                   ((PlayerInstance)ch).PlayerData.Clan != ((PlayerInstance)vch).PlayerData.Clan;
        }



        public static bool check_illegal_psteal(CharacterInstance ch, CharacterInstance victim)
        {
            if (victim.IsNpc() || ch.IsNpc()) return false;
            return (!((PlayerInstance)victim).PlayerData.Flags.IsSet(PCFlags.Deadly)
                    || ch.Level - victim.Level > 10
                    || !((PlayerInstance)victim).PlayerData.Flags.IsSet(PCFlags.Deadly))
                   && (ch.CurrentRoom.ID < 29 || ch.CurrentRoom.ID > 43)
                   && ch != victim;
        }

        public static CharacterInstance scan_for_victim(CharacterInstance ch, ExitData pexit, string name)
        {
            if (ch.IsAffected(AffectedByTypes.Blind) || pexit == null)
                return null;

            var was_in_room = ch.CurrentRoom;
            var max_distance = 8;

            if (ch.IsVampire() && GameManager.Instance.GameTime.Hour < 21 && GameManager.Instance.GameTime.Hour > 5)
                max_distance = 1;

            if (ch.Level < 50)
                --max_distance;
            if (ch.Level < 40)
                --max_distance;
            if (ch.Level < 30)
                --max_distance;

            for (int dist = 1; dist <= max_distance; dist++)
            {
                if (pexit.Flags.IsSet(ExitFlags.Closed))
                    break;

                if (pexit.GetDestination().IsPrivate()
                    && ch.Level < LevelConstants.GetLevel(ImmortalTypes.Greater))
                    break;

                ch.CurrentRoom.RemoveFrom(ch);
                pexit.GetDestination().AddTo(ch);

                var victim = ch.GetCharacterInRoom(name);
                if (victim != null)
                {
                    ch.CurrentRoom.RemoveFrom(ch);
                    was_in_room.AddTo(ch);
                    return victim;
                }

                switch (ch.CurrentRoom.SectorType)
                {
                    default:
                        dist++;
                        break;
                    case SectorTypes.Air:
                        if (SmaugRandom.D100() < 80)
                            dist++;
                        break;
                    case SectorTypes.Forest:
                    case SectorTypes.City:
                    case SectorTypes.Desert:
                    case SectorTypes.Hills:
                        dist += 2;
                        break;
                    case SectorTypes.ShallowWater:
                    case SectorTypes.DeepWater:
                        dist += 3;
                        break;
                    case SectorTypes.Mountain:
                    case SectorTypes.Underwater:
                    case SectorTypes.OceanFloor:
                        dist += 4;
                        break;
                }

                if (dist >= max_distance)
                    break;

                var dir = pexit.Direction;
                var exit = ch.CurrentRoom.GetExit(dir);
                if (exit == null)
                    break;
            }

            ch.CurrentRoom.RemoveFrom(ch);
            was_in_room.AddTo(ch);

            return null;
        }

        public static ObjectInstance find_projectile(CharacterInstance ch, int type)
        {
            foreach (var obj in ch.Carrying.Where(ch.CanSee))
            {
                if (obj.ItemType == ItemTypes.Quiver
                    && !obj.Values.Flags.IsSet(ContainerFlags.Closed))
                {
                    foreach (var containedObj in obj.Contents
                        .Where(containedObj => containedObj.ItemType == ItemTypes.Projectile
                                                               && containedObj.Values.ProjectileType == type))
                    {
                        return containedObj;
                    }
                }
                if (obj.ItemType == ItemTypes.Projectile
                    && obj.Values.ProjectileType == type)
                    return obj;
            }
            return null;
        }

        public static ReturnTypes ranged_got_target(CharacterInstance ch, CharacterInstance victim,
                                            ObjectInstance weapon, ObjectInstance projectile, int dist, int dt, string stxt,
                                            ATTypes color)
        {
            if (ch.CurrentRoom.Flags.IsSet(RoomFlags.Safe))
            {
                if (projectile != null)
                {
                    ch.PrintfColor("Your %s is blasted from existence by a godly presence.", projectile);
                    comm.act(color, "A godly presence smites $p!", ch, projectile, null, ToTypes.Room);
                    projectile.Extract();
                }
                else
                {
                    ch.Printf("Your %s is blasted from existence by a godly presence.", stxt);
                    comm.act(color, "A godly presence smites $t!", ch, stxt.AddArticle(ArticleAppendOptions.TheToFront), null, ToTypes.Room);
                }
                return ReturnTypes.None;
            }

            var skill = RepositoryManager.Instance.GetEntity<SkillData>("MissileWeapons");

            if (victim.IsNpc() && victim.Act.IsSet(ActFlags.Sentinel)
                && ch.CurrentRoom != victim.CurrentRoom)
            {
                if (projectile == null) return ch.CauseDamageTo(victim, 0, dt);
                skill.LearnFromFailure(ch);

                if (SmaugRandom.D100() < 50)
                    projectile.Extract();
                else
                {
                    if (projectile.InObject != null)
                        projectile.RemoveFrom(projectile.InObject);
                    if (projectile.CarriedBy != null)
                        projectile.RemoveFrom();
                    victim.CurrentRoom.AddTo(projectile);
                }
                return ch.CauseDamageTo(victim, 0, dt);
            }

            if (SmaugRandom.D100() > 50 || (projectile != null && weapon != null
                                            && ch.CanUseSkill(SmaugRandom.D100(), skill)))
            {
                return projectile != null ? fight.projectile_hit(ch, victim, weapon, projectile, dist) : Attack.spell_attack(dt, ch.Level, ch, victim);
            }

            skill.LearnFromFailure(ch);
            var returnCode = ch.CauseDamageTo(victim, 0, dt);

            if (projectile == null) return returnCode;

            if (SmaugRandom.D100() < 50)
                projectile.Extract();
            else
            {
                if (projectile.InObject != null)
                    projectile.RemoveFrom(projectile.InObject);
                if (projectile.CarriedBy != null)
                    projectile.RemoveFrom();
                victim.CurrentRoom.AddTo(projectile);
            }

            return returnCode;
        }

        public static ReturnTypes ranged_attack(CharacterInstance ch, string argument, ObjectInstance weapon,
                                        ObjectInstance projectile, int sn, int range)
        {
            var firstArg = argument.FirstWord();
            var secondArg = argument.SecondWord();

            if (firstArg.IsNullOrEmpty())
            {
                ch.SendTo("Where?  At whom?");
                return ReturnTypes.None;
            }

            CharacterInstance victim = null;
            var exit = ch.CurrentRoom.GetExit(firstArg);
            if (exit == null)
            {
                victim = ch.GetCharacterInRoom(firstArg);
                if (victim == null)
                {
                    ch.SendTo("Aim in what direction?");
                    return ReturnTypes.None;
                }

                if (ch.CurrentFighting.Who == victim)
                {
                    ch.SendTo("They are too close to release that type of attack!");
                    return ReturnTypes.None;
                }
            }

            if (victim != null)
            {
                if (ch.CurrentRoom.IsPrivate() || ch.CurrentRoom.Flags.IsSet(RoomFlags.Solitary))
                {
                    ch.SendTo("You cannot perform a ranged attack from a private room.");
                    return ReturnTypes.None;
                }

                if (ch.CurrentRoom.Tunnel > 0)
                {
                    if (ch.CurrentRoom.Contents.OfType<CharacterInstance>().Count() >= ch.CurrentRoom.Tunnel)
                    {
                        ch.SendTo("This room is too cramped to perform such an attack.");
                        return ReturnTypes.None;
                    }
                }
            }

            SkillData skill = null;
            if (Macros.IS_VALID_SN(sn))
                skill = RepositoryManager.Instance.GetEntity<SkillData>(sn);

            if (exit != null && exit.GetDestination() == null)
            {
                ch.SendTo("Are you expecting to fire through a wall?!");
                return ReturnTypes.None;
            }

            if (exit != null && exit.Flags.IsSet(ExitFlags.Closed))
            {
                if (exit.Flags.IsSet(ExitFlags.Secret) || exit.Flags.IsSet(ExitFlags.Dig))
                    ch.SendTo("Are you expecting to fire through a wall?!");
                else
                    ch.SendTo("Are you expecting to fire through a door?!");
                return ReturnTypes.None;
            }

            CharacterInstance vch = null;
            if (exit != null && !string.IsNullOrEmpty(secondArg))
            {
                vch = scan_for_victim(ch, exit, secondArg);
                if (vch == null)
                {
                    ch.SendTo("You cannot see your target.");
                    return ReturnTypes.None;
                }

                if (vch.CurrentRoom.Flags.IsSet(RoomFlags.NoMissile))
                {
                    ch.SendTo("You can't get a clean shot off.");
                    return ReturnTypes.None;
                }

                if (vch.NumberFighting > GameConstants.GetConstant<int>("MaximumCombatants"))
                {
                    ch.SendTo("There is too much activity there for you to get a clear shot.");
                    return ReturnTypes.None;
                }
            }

            if (vch != null)
            {
                if (!vch.IsNpc() && !ch.IsNpc() && ch.Act.IsSet(PlayerFlags.Nice))
                {
                    ch.SendTo("You're too nice to do that!");
                    return ReturnTypes.None;
                }
                if (fight.is_safe(ch, vch, true))
                    return ReturnTypes.None;
            }

            var was_in_room = ch.CurrentRoom;
            if (projectile != null)
            {
                //todo handler.separate_obj(projectile);

                var action = weapon != null ? "fire" : "throw";
                if (exit != null)
                {
                    comm.act(ATTypes.AT_GREY, $"You {action} $p $T.", ch, projectile, exit.Direction.GetName(),
                            ToTypes.Character);
                    comm.act(ATTypes.AT_GREY, $"$n {action}s $p $T.", ch, projectile, exit.Direction.GetName(),
                        ToTypes.Room);
                }
                else
                {
                    comm.act(ATTypes.AT_GREY, $"You {action} $p at $N.", ch, projectile, victim, ToTypes.Character);
                    comm.act(ATTypes.AT_GREY, $"$n {action}s $p at $N.", ch, projectile, victim, ToTypes.NotVictim);
                    comm.act(ATTypes.AT_GREY, $"$n {action}s $p at you!", ch, projectile, victim, ToTypes.Victim);
                }
            }
            else if (skill != null)
            {
                var skillText = GetSkillText(skill);

                if (skill.Type == SkillTypes.Spell)
                {
                    var color = ATTypes.AT_MAGIC;
                    if (exit != null)
                    {
                        comm.act(color, "You release $t $T.", ch, skillText.AddArticle(ArticleAppendOptions.TheToFront),
                            exit.Direction.GetName(), ToTypes.Character);
                        comm.act(color, "$n releases $s $t $T.", ch, skillText, exit.Direction.GetName(), ToTypes.Room);
                    }
                    else
                    {
                        comm.act(color, "You release $t at $N.", ch, skillText.AddArticle(ArticleAppendOptions.TheToFront),
                            victim, ToTypes.Character);
                        comm.act(color, "$n releases $s $t at $N.", ch, skillText, victim, ToTypes.NotVictim);
                        comm.act(color, "$n releases $s $t at you!", ch, skillText, victim, ToTypes.Victim);
                    }
                }
            }
            else
            {
                //todo record bug, missing projectile and skill
                return ReturnTypes.None;
            }

            if (victim != null)
            {
                fight.check_illegal_pk(ch, victim);
                ch.CheckAttackForAttackerFlag(victim);
                return ranged_got_target(ch, victim, weapon, projectile, 0, sn, "burst of energy", ATTypes.AT_GREY);
            }

            victim = vch;
            var dtxt = act_move.rev_exit(exit.Direction);

            var dist = 0;
            while (dist <= range)
            {
                ch.CurrentRoom.RemoveFrom(ch);
                exit.GetDestination().AddTo(ch);

                if (exit.Flags.IsSet(ExitFlags.Closed))
                {
                    var color = projectile != null ? ATTypes.AT_GREY : ATTypes.AT_MAGIC;

                    var txt =
                        $"You see your {(projectile != null ? projectile.Name : GetSkillText(skill))} {(projectile != null ? "pierce" : "hit")} a door in the distance to the {exit.Direction.GetName()}.";
                    comm.act(color, txt, ch, null, null, ToTypes.Character);

                    if (projectile != null)
                    {

                    }
                }
            }

            ch.CurrentRoom.RemoveFrom(ch);
            was_in_room.AddTo(ch);

            return ReturnTypes.None;
        }

        private static string GetSkillText(SkillData skill)
        {
            var skillText = !string.IsNullOrEmpty(skill.NounDamage) ? skill.NounDamage : skill.Name;
            if (!skillText.StartsWithIgnoreCase("spell"))
                skillText = "magical burst of energy";
            return skillText;
        }
    }
}
