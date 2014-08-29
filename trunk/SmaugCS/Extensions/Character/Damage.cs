using System;
using System.Collections.Generic;
using Realm.Library.Common;
using SmaugCS.Commands.Combat;
using SmaugCS.Commands.Skills;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Managers;

namespace SmaugCS.Extensions
{
    public static class Damage
    {
        public static ReturnTypes CauseDamageTo(this CharacterInstance ch, CharacterInstance victim, int dam, int dt)
        {
            if (ch == null) return ReturnTypes.Error;
            if (victim == null) return ReturnTypes.Error;
            if (victim.CurrentPosition == PositionTypes.Dead)
                return ReturnTypes.VictimDied;

            int modifiedDamage = dam;
            if (dam > 0 && dt != Program.TYPE_UNDEFINED)
            {
                modifiedDamage = CheckDamageForResistances(victim, dam, dt);
                if (modifiedDamage == -1)
                {
                    SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>(dt);
                    if (skill == null)
                        modifiedDamage = 0;
                    else
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
                    }
                }
            }

            if (victim.CurrentRoom.Flags.IsSet(RoomFlags.Safe))
                modifiedDamage = 0;

            if (modifiedDamage > 0 && victim.IsNpc() && ch != victim)
            {
                if (!victim.Act.IsSet(ActFlags.Sentinel))
                {
                    if (victim.CurrentHunting != null)
                    {
                        if (victim.CurrentHunting.Who != ch)
                        {
                            victim.CurrentHunting.Name = ch.Name;
                            victim.CurrentHunting.Who = ch;
                        }
                    }
                    else if (!victim.Act.IsSet(ActFlags.Pacifist))
                        victim.StartHunting(ch);
                }

                if (victim.CurrentHating != null)
                {
                    if (victim.CurrentHating.Who != ch)
                    {
                        victim.CurrentHating.Name = ch.Name;
                        victim.CurrentHating.Who = ch;
                    }
                }
                else if (!victim.Act.IsSet(ActFlags.Pacifist))
                    victim.StartHating(ch);
            }

            int maxDamage = ch.Level * ((dt == DatabaseManager.Instance.GetEntity<SkillData>("backstab").ID) ? 80 : 40);
            if (modifiedDamage > maxDamage)
                modifiedDamage = maxDamage;

            // TODO damage someone else

            if (modifiedDamage > 10 && dt != Program.TYPE_UNDEFINED)
                modifiedDamage = ModifyDamageForEquipment(victim, modifiedDamage);

            if (ch != victim)
                fight.dam_message(ch, victim, modifiedDamage, dt);

            victim.CurrentHealth -= modifiedDamage;

            GainXPFromDamageInflicted(ch, victim, modifiedDamage, dt);

            if (!victim.IsNpc() && victim.IsNotAuthorized() && victim.CurrentHealth < 1)
                victim.CurrentHealth = 1;

            if (modifiedDamage > 0 && dt > Program.TYPE_HIT
                && !victim.IsAffected(AffectedByTypes.Poison)
                && ch.IsWieldedWeaponPoisoned()
                && !victim.Immunity.IsSet(ResistanceTypes.Poison))
            {
                if (!victim.SavingThrows.CheckSaveVsPoisonDeath(ch.Level, victim))
                {
                    AffectData af = new AffectData
                    {
                        Type = AffectedByTypes.Poison,
                        Duration = 20,
                        Location = ApplyTypes.Strength,
                        Modifier = -2
                    };
                    victim.AddAffect(af);
                    victim.WorsenMentalState(20.GetNumberThatIsBetween(victim.MentalState + (victim.IsPKill() ? 1 : 2),
                        100));
                }
            }

            if (victim.IsVampire())
                PreserveVampireFromDamage(victim, modifiedDamage);

            if (!victim.IsNpc() && victim.Trust >= LevelConstants.ImmortalLevel &&
                ch.Trust >= LevelConstants.ImmortalLevel && victim.CurrentHealth < 1)
                victim.CurrentHealth = 1;

            fight.update_pos(victim);

            if (PositionDamageTable.ContainsKey(victim.CurrentPosition))
                PositionDamageTable[victim.CurrentPosition].Invoke(ch, victim, modifiedDamage, dt);
            else
                DamageAndDefault(ch, victim, modifiedDamage, dt);

            if (!victim.IsAwake() && !victim.IsAffected(AffectedByTypes.Paralysis))
            {
                if (victim.CurrentFighting != null && victim.CurrentFighting.Who.CurrentHunting != null
                    && victim.CurrentFighting.Who.CurrentHunting.Who == victim)
                    victim.CurrentFighting.Who.StopHunting();

                if (victim.CurrentFighting != null && victim.CurrentFighting.Who.CurrentHating != null
                    && victim.CurrentFighting.Who.CurrentHating.Who == victim)
                    victim.CurrentFighting.Who.StopHating();

                fight.stop_fighting(victim, (!victim.IsNpc() && ch.IsNpc()));
            }

            // TODO Payoff for killing things

            if (victim == ch) return ReturnTypes.None;

            if (!victim.IsNpc() && victim.Descriptor == null && !victim.PlayerData.Flags.IsSet(PCFlags.NoRecall))
            {
                if (SmaugRandom.Between(0, victim.wait) == 0)
                {
                    Recall.do_recall(victim, string.Empty);
                    return ReturnTypes.None;
                }
            }

            if (victim.IsNpc() && modifiedDamage > 0)
            {
                if ((victim.Act.IsSet(ActFlags.Wimpy) && SmaugRandom.Bits(1) == 0 &&
                     victim.CurrentHealth < victim.MaximumHealth / 2)
                    ||
                    (victim.IsAffected(AffectedByTypes.Charm) && victim.Master != null &&
                     victim.Master.CurrentRoom != victim.CurrentRoom))
                {
                    victim.StartFearing(ch);
                    victim.StopHunting();
                    Flee.do_flee(victim, string.Empty);
                }
            }

            if (!victim.IsNpc() && victim.CurrentHealth > 0 && victim.CurrentHealth <= victim.wimpy && victim.wait == 0)
                Flee.do_flee(victim, string.Empty);
            else if (!victim.IsNpc() && victim.Act.IsSet(PlayerFlags.Flee))
                Flee.do_flee(victim, string.Empty);

            // TODO tail_chain();

            return ReturnTypes.None;
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
                SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>(dt);
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
        private static void DamageAndDefault(CharacterInstance ch, CharacterInstance victim, int dam, int dt)
        {
            if (dam > victim.MaximumHealth / 4)
            {
                comm.act(ATTypes.AT_HURT, "That really did HURT!", victim, null, null, ToTypes.Character);
                if (SmaugRandom.Bits(3) == 0)
                    victim.WorsenMentalState(1);
            }
            if (victim.CurrentHealth < victim.MaximumHealth / 4)
            {
                comm.act(ATTypes.AT_HURT, "You wish that your wounds would stop BLEEDING so much!", victim, null, null, ToTypes.Character);
                if (SmaugRandom.Bits(2) == 0)
                    victim.WorsenMentalState(1);
            }
        }

        private static void PreserveVampireFromDamage(CharacterInstance victim, int dam)
        {
            if (dam >= (victim.MaximumHealth / 10))
                update.gain_condition(victim, ConditionTypes.Bloodthirsty, -1 - (victim.Level / 20));
            if (victim.CurrentHealth <= (victim.MaximumHealth / 8) &&
                victim.PlayerData.GetConditionValue(ConditionTypes.Bloodthirsty) > 5)
            {
                update.gain_condition(victim, ConditionTypes.Bloodthirsty, -3.GetNumberThatIsBetween(victim.Level / 10, 8));
                victim.CurrentHealth += 4.GetNumberThatIsBetween(victim.CurrentHealth / 30, 15);
                color.set_char_color(ATTypes.AT_BLOOD, victim);
                color.send_to_char("You howl with rage as the beast within stirs!", victim);
            }
        }

        private static void GainXPFromDamageInflicted(CharacterInstance ch, CharacterInstance victim, int dam, int dt)
        {
            if (dam <= 0 || ch == victim || ch.IsNpc() || ch.CurrentFighting == null ||
                ch.CurrentFighting.Experience <= 0) return;

            int xpGain;
            if (ch.CurrentFighting.Who == victim)
                xpGain = (ch.CurrentFighting.Experience * dam) / victim.MaximumHealth;
            else
                xpGain = (int)(fight.xp_compute(ch, victim) * 0.85f * dam) / victim.MaximumHealth;

            if (dt == DatabaseManager.Instance.GetEntity<SkillData>("backstab").ID
                || dt == DatabaseManager.Instance.GetEntity<SkillData>("circle").ID)
                xpGain = 0;

            ch.GainXP(xpGain);
        }

        private static int ModifyDamageForEquipment(CharacterInstance victim, int dam)
        {
            WearLocations wearLoc = GetRandomWearLocation();
            ObjectInstance obj = victim.GetEquippedItem(wearLoc);
            if (obj != null && dam > obj.GetResistance() && SmaugRandom.Bits(1) == 0)
            {
                handler.set_cur_obj(obj);
                act_obj.damage_obj(obj);
                return dam - 5;
            }

            return dam + 5;
        }

        private static WearLocations GetRandomWearLocation()
        {
            int min = WearLocations.About.GetMinimum();
            int max = WearLocations.WieldMissile.GetMaximum();
            WearLocations loc = Realm.Library.Common.EnumerationExtensions.GetEnum<WearLocations>(SmaugRandom.Between(min, max));
            return loc == WearLocations.None ? GetRandomWearLocation() : loc;
        }

        private static int CheckDamageForResistances(CharacterInstance victim, int dam, int dt)
        {
            int modDmg = dam;
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
                SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>("poison");
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
    }
}
