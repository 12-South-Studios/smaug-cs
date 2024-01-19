using Realm.Library.Common.Extensions;
using SmaugCS.Commands;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Repository;
using System;
using System.Collections.Generic;
using EnumerationExtensions = Realm.Library.Common.Extensions.EnumerationExtensions;

namespace SmaugCS
{
    public static class DamageExtensions
    {
        public static ReturnTypes CauseDamageTo(this CharacterInstance ch, CharacterInstance victim, int dam, int dt)
        {
            if (ch == null) return ReturnTypes.Error;
            if (victim == null) return ReturnTypes.Error;
            if (victim.CurrentPosition == PositionTypes.Dead)
                return ReturnTypes.VictimDied;

            var modifiedDamage = dam;
            if (dam > 0 && dt != Program.TYPE_UNDEFINED)
            {
                modifiedDamage = CheckDamageForResistances(victim, dam, dt);
                if (modifiedDamage == -1)
                {
                    var skill = Program.RepositoryManager.GetEntity<SkillData>(dt);
                    if (skill == null)
                        modifiedDamage = 0;
                    else
                    {
                        if (DisplayImmuneMessages(ch, victim, skill) == ReturnTypes.None)
                            return ReturnTypes.None;
                    }
                }
            }

            if (victim.CurrentRoom.Flags.IsSet(RoomFlags.Safe))
                modifiedDamage = 0;

            if (modifiedDamage > 0 && victim.IsNpc() && ch != victim)
                DoHuntAndHate(ch, victim);

            var maxDamage = ch.Level * (dt == Program.RepositoryManager.GetEntity<SkillData>("backstab").ID ? 80 : 40);
            if (modifiedDamage > maxDamage)
                modifiedDamage = maxDamage;

            if (victim != ch)
            {
                if (ch.IsSafe(victim, true))
                    return ReturnTypes.None;
                CheckAttacker(ch, victim);

                if (victim.CurrentPosition == PositionTypes.Stunned)
                {
                    if (victim.CurrentFighting == null && victim.CurrentRoom == ch.CurrentRoom)
                        StartFighting(victim, ch);
                }

            }

            // Code to handle equipment getting damaged, and also support bonuses / penalties
            // for having or not having equipment where hit
            if (modifiedDamage > 10 && dt != Program.TYPE_UNDEFINED)
                modifiedDamage = ModifyDamageForEquipment(victim, modifiedDamage);

            if (ch != victim)
                fight.dam_message(ch, victim, modifiedDamage, dt);

            victim.CurrentHealth -= modifiedDamage;

            GainXPFromDamageInflicted(ch, victim, modifiedDamage, dt);

            if (!victim.IsNpc() && victim.IsNotAuthorized() && victim.CurrentHealth < 1)
                victim.CurrentHealth = 1;

            if (modifiedDamage > 0 && dt > Program.TYPE_HIT && !victim.IsAffected(AffectedByTypes.Poison)
                && ch.IsWieldedWeaponPoisoned() && !victim.Immunity.IsSet(ResistanceTypes.Poison))
                InflictPoison(ch, victim);

            if (victim.IsVampire())
                PreserveVampireFromDamage(victim, modifiedDamage);

            if (!victim.IsNpc() && victim.Trust >= LevelConstants.ImmortalLevel &&
                ch.Trust >= LevelConstants.ImmortalLevel && victim.CurrentHealth < 1)
                victim.CurrentHealth = 1;

            victim.UpdatePositionByCurrentHealth();

            if (PositionDamageTable.ContainsKey(victim.CurrentPosition))
                PositionDamageTable[victim.CurrentPosition].Invoke(ch, victim, modifiedDamage, dt);
            else
                DamageAndDefault(victim, modifiedDamage);

            if (!victim.IsAwake() && !victim.IsAffected(AffectedByTypes.Paralysis))
                StopFighting(ch, victim);

            // TODO Payoff for killing things

            if (victim == ch) return ReturnTypes.None;

            if (!victim.IsNpc() && ((PlayerInstance)victim).Descriptor == null &&
                !((PlayerInstance)victim).PlayerData.Flags.IsSet(PCFlags.NoRecall))
            {
                if (SmaugRandom.Between(0, victim.wait) == 0)
                {
                    Recall.do_recall(victim, string.Empty);
                    return ReturnTypes.None;
                }
            }

            if (victim.IsNpc() && modifiedDamage > 0)
                DoFlee(ch, victim);

            if (!victim.IsNpc() && victim.CurrentHealth > 0 && victim.CurrentHealth <= victim.wimpy && victim.wait == 0)
                Flee.do_flee(victim, string.Empty);
            else if (!victim.IsNpc() && victim.Act.IsSet((int)PlayerFlags.Flee))
                Flee.do_flee(victim, string.Empty);

            // TODO tail_chain();

            return ReturnTypes.None;
        }

        private static void DoFlee(CharacterInstance ch, CharacterInstance victim)
        {
            if ((victim.Act.IsSet((int)ActFlags.Wimpy) && SmaugRandom.Bits(1) == 0
                && victim.CurrentHealth < victim.MaximumHealth / 2) || (victim.IsAffected(AffectedByTypes.Charm)
                && victim.Master != null && victim.Master.CurrentRoom != victim.CurrentRoom))
            {
                var mob = (MobileInstance)victim;
                mob.StartFearing(ch);
                mob.StopHunting();
                Flee.do_flee(victim, string.Empty);
            }
        }

        private static int GetMaximumFight()
        {
            return GameConstants.GetConstant<int>("MaximumFight");
        }

        private static void StopFighting(CharacterInstance ch, CharacterInstance victim)
        {
            if (victim.CurrentFighting != null)
            {
                var mob = (MobileInstance)victim.CurrentFighting.Who;

                if (mob.CurrentHunting != null && mob.CurrentHunting.Who == victim)
                    mob.StopHunting();

                if (mob.CurrentHating != null && mob.CurrentHating.Who == victim)
                    mob.StopHating();
            }

            victim.StopFighting(!victim.IsNpc() && ch.IsNpc());
        }

        private static void StartFighting(CharacterInstance ch, CharacterInstance victim)
        {
            if (ch.CurrentFighting != null) return;

            if (ch.AffectedBy.IsSet((int)AffectedByTypes.Sleep))
            {
                // TODO affect_strip(ch, gsn_sleep);
            }

            if (victim.NumberFighting > GetMaximumFight())
            {
                ch.SendTo("There are too many people fighting for you to join in.");
                return;
            }

            var fight = new FightingData
            {
                Who = victim,
                Experience = (int)(ch.CalculateXP(victim) * 0.85),
                Alignment = ch.CalculateAlign(victim)
            };

            if (!ch.IsNpc() && victim.IsNpc())
                fight.TimesKilled = handler.times_killed(ch, victim);

            ch.NumberFighting = 1;
            ch.CurrentFighting = fight;

            if (ch.IsNpc())
                ch.CurrentPosition = PositionTypes.Fighting;
            else
            {
                if (StyleToPositionTable.TryGetValue(ch.CurrentStyle, out var position))
                    ch.CurrentPosition = position;
                else
                    ch.CurrentPosition = PositionTypes.Fighting;
            }

            victim.NumberFighting++;
            if (victim.Switched != null && victim.Switched.IsAffected(AffectedByTypes.Possess))
            {
                victim.Switched.SendTo("You are disturbed!");
                // TODO do_return(victim.Switched, "");
            }
        }

        private static Dictionary<StyleTypes, PositionTypes> StyleToPositionTable =
            new Dictionary<StyleTypes, PositionTypes>
            {
                { StyleTypes.Evasive, PositionTypes.Evasive },
                { StyleTypes.Defensive, PositionTypes.Defensive },
                { StyleTypes.Aggressive, PositionTypes.Aggressive },
                { StyleTypes.Berserk, PositionTypes.Berserk }
            };

        private static void InflictPoison(CharacterInstance ch, CharacterInstance victim)
        {
            if (!victim.SavingThrows.CheckSaveVsPoisonDeath(ch.Level, victim))
            {
                var af = new AffectData
                {
                    Type = AffectedByTypes.Poison,
                    Duration = 20,
                    Location = ApplyTypes.Strength,
                    Modifier = -2
                };
                victim.AddAffect(af);
                ((PlayerInstance)victim).WorsenMentalState(
                    20.GetNumberThatIsBetween(victim.MentalState + (victim.IsPKill() ? 1 : 2),
                        100));
            }
        }

        private static void CheckAttacker(CharacterInstance ch, CharacterInstance victim)
        {
            // NPCs, Killers and Theives are fair game
            if (victim.IsNpc() || victim.Act.IsSet((int)PlayerFlags.Killer) || victim.Act.IsSet((int)PlayerFlags.Thief))
                return;

            // Deadly character
            if (!ch.IsNpc() && !victim.IsNpc() && ch.CanPKill() && victim.CanPKill())
                return;

            if (ch.IsAffected(AffectedByTypes.Charm))
            {
                if (ch.Master == null)
                {
                    // TODO affect_strip(ch, gsn_charm_person);
                    ch.AffectedBy.RemoveBit((int)AffectedByTypes.Charm);
                    return;
                }
                return;
            }

            if (ch.IsNpc() || ch == victim || ch.IsImmortal()
                || ch.Act.IsSet((int)PlayerFlags.Attacker)
                || ch.Act.IsSet((int)PlayerFlags.Killer))
                return;

            ch.Act.SetBit((int)PlayerFlags.Attacker);
            // TODO save_char_obj(ch);
        }

        private static void DoHuntAndHate(CharacterInstance ch, CharacterInstance victim)
        {
            var vict = (MobileInstance)victim;
            if (!vict.Act.IsSet((int)ActFlags.Sentinel))
            {
                if (vict.CurrentHunting != null)
                {
                    if (vict.CurrentHunting.Who != ch)
                    {
                        vict.CurrentHunting.Name = ch.Name;
                        vict.CurrentHunting.Who = ch;
                    }
                }
                else if (!vict.Act.IsSet((int)ActFlags.Pacifist))
                    vict.StartHunting(ch);
            }

            if (vict.CurrentHating != null)
            {
                if (vict.CurrentHating.Who != ch)
                {
                    vict.CurrentHating.Name = ch.Name;
                    vict.CurrentHating.Who = ch;
                }
            }
            else if (!vict.Act.IsSet((int)ActFlags.Pacifist))
                vict.StartHating(ch);
        }

        private static ReturnTypes DisplayImmuneMessages(CharacterInstance ch, CharacterInstance victim, SkillData skill)
        {
            if (!skill.ImmuneCharacterMessage.IsNullOrEmpty())
                comm.act(ATTypes.AT_HIT, skill.ImmuneCharacterMessage, ch, null, victim, ToTypes.Character);
            if (!skill.ImmuneVictimMessage.IsNullOrEmpty())
                comm.act(ATTypes.AT_HITME, skill.ImmuneVictimMessage, ch, null, victim, ToTypes.Victim);
            if (!skill.ImmuneRoomMessage.IsNullOrEmpty())
                comm.act(ATTypes.AT_ACTION, skill.ImmuneRoomMessage, ch, null, victim, ToTypes.Room);
            if (!skill.ImmuneCharacterMessage.IsNullOrEmpty()
                || !skill.ImmuneVictimMessage.IsNullOrEmpty()
                || !skill.ImmuneRoomMessage.IsNullOrEmpty())
                return ReturnTypes.None;
            return ReturnTypes.Error;
        }

        private static readonly Dictionary<PositionTypes, Action<CharacterInstance, CharacterInstance, int, int>>
            PositionDamageTable = new Dictionary
                <PositionTypes, Action<CharacterInstance, CharacterInstance, int, int>>
            {
                {PositionTypes.Mortal, DamagedAndMortallyWounded},
                {PositionTypes.Incapacitated, DamagedAndIncapacitated},
                {PositionTypes.Stunned, DamagedAndStunned},
                {PositionTypes.Dead, DamagedAndDead}
            };

        private static void DamagedAndDead(CharacterInstance ch, CharacterInstance victim, int dam, int dt)
        {
            if (dt >= 0)
            {
                var skill = Program.RepositoryManager.GetEntity<SkillData>(dt);
                if (!skill.DieCharacterMessage.IsNullOrEmpty())
                    comm.act(ATTypes.AT_DEAD, skill.DieCharacterMessage, ch, null, victim, ToTypes.Character);
                if (!skill.DieVictimMessage.IsNullOrEmpty())
                    comm.act(ATTypes.AT_DEAD, skill.DieVictimMessage, ch, null, victim, ToTypes.Victim);
                if (!skill.DieRoomMessage.IsNullOrEmpty())
                    comm.act(ATTypes.AT_DEAD, skill.DieRoomMessage, ch, null, victim, ToTypes.Room);
            }

            comm.act(ATTypes.AT_DYING, "$n is DEAD!!", victim, null, null, ToTypes.Room);
            comm.act(ATTypes.AT_DANGER, "You have been KILLED!!", victim, null, null, ToTypes.Character);
        }

        private static void DamagedAndStunned(CharacterInstance ch, CharacterInstance victim, int dam, int dt)
        {
            if (!victim.IsAffected(AffectedByTypes.Paralysis))
            {
                comm.act(ATTypes.AT_DYING, "$n is stunned, but will probably recover.", victim, null, null, ToTypes.Room);
                comm.act(ATTypes.AT_DANGER, "You are stunned, but will probably recover.", victim, null, null,
                    ToTypes.Character);
            }
        }

        private static void DamagedAndIncapacitated(CharacterInstance ch, CharacterInstance victim, int dam, int dt)
        {
            comm.act(ATTypes.AT_DYING, "$n is incapacitated and will slowly die, if not aided.", victim, null, null, ToTypes.Room);
            comm.act(ATTypes.AT_DANGER, "You are incapacitated and will slowly die, if not aided.", victim, null, null, ToTypes.Character);
        }
        private static void DamagedAndMortallyWounded(CharacterInstance ch, CharacterInstance victim, int dam, int dt)
        {
            comm.act(ATTypes.AT_DYING, "$n is mortally wounded, and will die soon, if not aided.", victim, null, null, ToTypes.Room);
            comm.act(ATTypes.AT_DANGER, "You are mortally wounded, and will die soon, if not aided.", victim, null, null, ToTypes.Character);
        }

        private static void DamageAndDefault(CharacterInstance victim, int dam)
        {
            if (dam > victim.MaximumHealth / 4)
            {
                comm.act(ATTypes.AT_HURT, "That really did HURT!", victim, null, null, ToTypes.Character);
                if (SmaugRandom.Bits(3) == 0)
                    ((PlayerInstance)victim).WorsenMentalState(1);
            }
            if (victim.CurrentHealth < victim.MaximumHealth / 4)
            {
                comm.act(ATTypes.AT_HURT, "You wish that your wounds would stop BLEEDING so much!", victim, null, null,
                    ToTypes.Character);
                if (SmaugRandom.Bits(2) == 0)
                    ((PlayerInstance)victim).WorsenMentalState(1);
            }
        }

        private static void PreserveVampireFromDamage(CharacterInstance victim, int dam)
        {
            if (dam >= victim.MaximumHealth / 10)
                ((PlayerInstance)victim).GainCondition(ConditionTypes.Bloodthirsty, -1 - victim.Level / 20);
            if (victim.CurrentHealth <= victim.MaximumHealth / 8 &&
                ((PlayerInstance)victim).PlayerData.GetConditionValue(ConditionTypes.Bloodthirsty) > 5)
            {
                ((PlayerInstance)victim).GainCondition(ConditionTypes.Bloodthirsty, -3.GetNumberThatIsBetween(victim.Level / 10, 8));
                victim.CurrentHealth += 4.GetNumberThatIsBetween(victim.CurrentHealth / 30, 15);
                victim.SetColor(ATTypes.AT_BLOOD);
                victim.SendTo("You howl with rage as the beast within stirs!");
            }
        }

        private static void GainXPFromDamageInflicted(CharacterInstance ch, CharacterInstance victim, int dam, int dt)
        {
            if (dam <= 0 || ch == victim || ch.IsNpc() || ch.CurrentFighting == null ||
                ch.CurrentFighting.Experience <= 0) return;

            int xpGain;
            if (ch.CurrentFighting.Who == victim)
                xpGain = ch.CurrentFighting.Experience * dam / victim.MaximumHealth;
            else
                xpGain = (int)(ch.ComputeExperienceGain(victim) * 0.85f * dam) / victim.MaximumHealth;

            if (dt == Program.RepositoryManager.GetEntity<SkillData>("backstab").ID
                || dt == Program.RepositoryManager.GetEntity<SkillData>("circle").ID)
                xpGain = 0;

            ((PlayerInstance)ch).GainXP(xpGain);
        }

        private static int ModifyDamageForEquipment(CharacterInstance victim, int dam)
        {
            var wearLoc = GetRandomWearLocation();
            var obj = victim.GetEquippedItem(wearLoc);
            if (obj != null && dam > obj.GetResistance() && SmaugRandom.Bits(1) == 0)
            {
                handler.set_cur_obj(obj);
                obj.CauseDamageTo();
                return dam - 5;
            }

            return dam + 5;
        }

        private static WearLocations GetRandomWearLocation()
        {
            var min = WearLocations.About.GetMinimum();
            var max = WearLocations.WieldMissile.GetMaximum();
            var loc =
                EnumerationExtensions.GetEnum<WearLocations>(SmaugRandom.Between(min, max));
            return loc == WearLocations.None ? GetRandomWearLocation() : loc;
        }

        private static int CheckDamageForResistances(CharacterInstance victim, int dam, int dt)
        {
            var modDmg = dam;
            if (Macros.IS_FIRE(dt))
                modDmg = victim.ModifyDamageWithResistance(modDmg, ResistanceTypes.Fire);
            else if (Macros.IS_COLD(dt))
                modDmg = victim.ModifyDamageWithResistance(modDmg, ResistanceTypes.Cold);
            else if (Macros.IS_ACID(dt))
                modDmg = victim.ModifyDamageWithResistance(modDmg, ResistanceTypes.Acid);
            else if (Macros.IS_ELECTRICITY(dt))
                modDmg = victim.ModifyDamageWithResistance(modDmg, ResistanceTypes.Electricity);
            else if (Macros.IS_ENERGY(dt))
                modDmg = victim.ModifyDamageWithResistance(modDmg, ResistanceTypes.Energy);
            else if (Macros.IS_DRAIN(dt))
                modDmg = victim.ModifyDamageWithResistance(modDmg, ResistanceTypes.Drain);
            else if (Macros.IS_POISON(dt))
            {
                var skill = Program.RepositoryManager.GetEntity<SkillData>("poison");
                if (skill != null)
                    modDmg = victim.ModifyDamageWithResistance(modDmg, ResistanceTypes.Poison);
            }
            else if ((dt == GetDamageHitType(DamageTypes.Pound))
                     || (dt == GetDamageHitType(DamageTypes.Stone))
                     || (dt == GetDamageHitType(DamageTypes.Pea))
                     || (dt == GetDamageHitType(DamageTypes.Crush)))
                modDmg = victim.ModifyDamageWithResistance(modDmg, ResistanceTypes.Blunt);
            else if ((dt == GetDamageHitType(DamageTypes.Stab))
                     || (dt == GetDamageHitType(DamageTypes.Bite))
                     || (dt == GetDamageHitType(DamageTypes.Dart))
                     || (dt == GetDamageHitType(DamageTypes.Pierce))
                     || (dt == GetDamageHitType(DamageTypes.Bolt))
                     || (dt == GetDamageHitType(DamageTypes.Arrow)))
                modDmg = victim.ModifyDamageWithResistance(modDmg, ResistanceTypes.Pierce);
            else if ((dt == GetDamageHitType(DamageTypes.Slice))
                     || (dt == GetDamageHitType(DamageTypes.Whip))
                     || (dt == GetDamageHitType(DamageTypes.Slash))
                     || (dt == GetDamageHitType(DamageTypes.Claw)))
                modDmg = victim.ModifyDamageWithResistance(modDmg, ResistanceTypes.Slash);

            return modDmg;
        }

        private static int GetDamageHitType(DamageTypes dmgType)
        {
            return Program.TYPE_HIT + (int)dmgType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gch"></param>
        /// <param name="victim"></param>
        /// <returns></returns>
        public static int CalculateXP(this CharacterInstance gch, CharacterInstance victim)
        {
            int xp = victim.GetExperienceWorth() * Macros.URANGE(0, victim.Level - gch.Level + 10, 13) / 10;
            int align = gch.CurrentAlignment - victim.CurrentAlignment;

            // Bonus for opposite alignment
            if (align > 990 || align < -990)
                xp = (xp * 5) >> 2;
            else
            {
                // Penalty for good attacking same alignment
                if (gch.CurrentAlignment > 300 && align < 250)
                    xp = (xp * 3) >> 2;
            }

            xp = SmaugRandom.Between((xp * 3) >> 2, (xp * 5) >> 2);

            // 1/4 XP for players
            if (!victim.IsNpc())
                xp /= 4;
            else
            {
                // Reduce XP for killing the same mob repeatedly
                if (!gch.IsNpc())
                {
                    int times = handler.times_killed(gch, victim);
                    if (times >= 20)
                        xp = 0;
                    else if (times > 0)
                    {
                        xp = (xp * (20 - times)) / 20;
                        if (times > 15)
                            xp /= 3;
                        else if (times > 10)
                            xp >>= 1;
                    }
                }
            }

            // Semi-intelligent experienced player vs notice player
            if (!gch.IsNpc() && gch.Level > 5)
            {
                var xp_ratio = (int)((PlayerInstance)gch).TotalPlayedTime / gch.Level;
                if (xp_ratio > 20000)
                    xp = (xp * 5) >> 2;
                else if (xp_ratio > 16000)
                    xp = (xp * 3) >> 2;
                else if (xp_ratio > 10000)
                    xp >>= 1;
                else if (xp_ratio > 5000)
                    xp >>= 2;
                else if (xp_ratio > 3500)
                    xp >>= 3;
                else if (xp_ratio > 2000)
                    xp >>= 4;
            }

            // Level based xp gain cap
            return Macros.URANGE(0, xp, handler.exp_level(gch, gch.Level + 1) - handler.exp_level(gch, gch.Level));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gch"></param>
        /// <param name="victim"></param>
        /// <returns></returns>
        public static int CalculateAlign(this CharacterInstance gch, CharacterInstance victim)
        {
            var align = gch.CurrentAlignment - victim.CurrentAlignment;

            var divalign = 20;
            if (gch.CurrentAlignment > -350 && gch.CurrentAlignment < 350)
                divalign = 4;

            int newAlign;

            if (align > 500)
                newAlign = Macros.UMIN(gch.CurrentAlignment + (align - 500) / divalign, 1000);
            else if (align < -500)
                newAlign = Macros.UMAX(gch.CurrentAlignment + (align + 500) / divalign, -1000);
            else
                newAlign = gch.CurrentAlignment - (int)(gch.CurrentAlignment / divalign);

            return newAlign;
        }

        public static bool IsSafe(this CharacterInstance ch, CharacterInstance victim, bool showMessage)
        {
            if (victim == null) return true;
            if (victim.CurrentRoom == null) return true;

            if (victim.CharDied() || ch.CharDied())
                return true;

            if (ch.CurrentFighting.Who == ch)
                return false;

            if (victim.CurrentRoom.Flags.IsSet((int)RoomFlags.Safe))
            {
                if (showMessage)
                {
                    ch.SetColor(ATTypes.AT_MAGIC);
                    ch.SendTo("A magical force prevents you from attackking.");
                }
                return true;
            }

            if (ch.IsPacifist())
            {
                if (showMessage)
                {
                    ch.SetColor(ATTypes.AT_MAGIC);
                    ch.SendTo("You are a pacifist and will not fight.");
                }
                return true;
            }

            if (victim.IsPacifist())
            {
                if (showMessage)
                {
                    ch.SetColor(ATTypes.AT_MAGIC);
                    ch.SendTo($"{victim.ShortDescription.CapitalizeFirst()} is a pacifist and will not fight.");
                }
                return true;
            }

            if (!ch.IsNpc() && ch.IsImmortal())
                return false;

            if (!ch.IsNpc() && !victim.IsNpc() && ch != victim && victim.CurrentRoom.Area.Flags.IsSet(AreaFlags.NoPlayerVsPlayer))
            {
                if (showMessage)
                {
                    ch.SetColor(ATTypes.AT_IMMORT);
                    ch.SendTo("The gods have forbidden player killing in this area.");
                }
                return true;
            }

            if (ch.IsNpc() || victim.IsNpc())
                return false;

            // TODO age (fight.c lines 2507 to 2525)

            if (ch.Level - victim.Level > 5 | victim.Level - ch.Level > 5)
            {
                if (showMessage)
                {
                    ch.SetColor(ATTypes.AT_IMMORT);
                    ch.SendTo("The gods do not allow murder when there is such a difference in level.");
                }
                return true;
            }

            if (victim.GetTimer(TimerTypes.PKilled) != null)
            {
                if (showMessage)
                {
                    ch.SetColor(ATTypes.AT_GREEN);
                    ch.SendTo("That character has died within the last 5 minutes.");
                }
                return true;
            }

            if (ch.GetTimer(TimerTypes.PKilled) != null)
            {
                if (showMessage)
                {
                    ch.SetColor(ATTypes.AT_GREEN);
                    ch.SendTo("You have been killed within the last 5 minutes.");
                }
                return true;
            }

            return false;
        }
    }
}
