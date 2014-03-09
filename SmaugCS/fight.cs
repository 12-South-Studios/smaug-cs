using System;
using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common;
using Realm.Library.Patterns.Repository;
using SmaugCS.Commands;
using SmaugCS.Commands.Admin;
using SmaugCS.Commands.PetsAndGroups;
using SmaugCS.Commands.Skills;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Organizations;
using SmaugCS.Logging;
using SmaugCS.Managers;
using SmaugCS.Spells.Smaug;

namespace SmaugCS
{
    public static class fight
    {
        public static ObjectInstance UsedWeapon { get; set; }

        public static bool loot_coins_from_corpse(CharacterInstance ch, ObjectInstance corpse)
        {
            int oldgold = ch.CurrentCoin;

            foreach (ObjectInstance content in corpse.Contents
                .Where(x => x.ItemType == ItemTypes.Money)
                .Where(ch.CanSee)
                .Where(x => Macros.CAN_WEAR(x, (int)ItemWearFlags.Take) && ch.Level < GameManager.Instance.SystemData.GetMinimumLevel(PlayerPermissionTypes.LevelGetObjectNoTake))
                .Where(x => Macros.IS_OBJ_STAT(x, (int)ItemExtraFlags.Prototype) && ch.CanTakePrototype()))
            {
                comm.act(ATTypes.AT_ACTION, "You get $p from $P", ch, content, corpse, ToTypes.Character);
                comm.act(ATTypes.AT_ACTION, "$n gets $p from $P", ch, content, corpse, ToTypes.Room);
                content.InObject.FromObject(content);
                handler.check_for_trap(ch, content, (int)TrapTriggerTypes.Get);
                if (ch.CharDied())
                    return false;

                mud_prog.oprog_get_trigger(ch, content);
                if (ch.CharDied())
                    return false;

                ch.CurrentCoin += content.Value[0] * content.Count;
                handler.extract_obj(content);
            }

            if (ch.CurrentCoin - oldgold > 1 && ch.CurrentPosition > PositionTypes.Sleeping)
                Split.do_split(ch, string.Format("{0}", ch.CurrentCoin - oldgold));

            return true;
        }

        private static readonly Dictionary<SunPositionTypes, int> VampireArmorClassTable = new Dictionary<SunPositionTypes, int>
            {
                {SunPositionTypes.Dark, -10},
                {SunPositionTypes.Sunrise, 5},
                {SunPositionTypes.Light, 10},
                {SunPositionTypes.Sunset, 2}
            };
        public static int VAMP_AC(CharacterInstance ch)
        {
            if (ch.IsVampire() && ch.IsOutside())
            {
                return VampireArmorClassTable.ContainsKey(GameManager.Instance.GameTime.Sunlight)
                           ? VampireArmorClassTable[GameManager.Instance.GameTime.Sunlight]
                           : 0;
            }
            return 0;
        }

        private static int Pulse { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static void violence_update()
        {
            Pulse = (Pulse + 1) % 100;

            foreach (CharacterInstance ch in DatabaseManager.Instance.CHARACTERS.CastAs<Repository<long, CharacterInstance>>().Values
                .Where(ch => !ch.CharDied()))
            {
                //// Experience gained during battle decreases as the battle drags on
                if (ch.CurrentFighting != null &&
                    (++ch.CurrentFighting.Duration % 24) == 0)
                    ch.CurrentFighting.Experience = ((ch.CurrentFighting.Experience * 9) / 10);

                foreach (TimerData timer in ch.Timers)
                    DecreaseExperienceInBattle(timer, ch);

                if (ch.CharDied())
                    continue;

                //// Spells that have durations less than 1 hour
                List<AffectData>.Enumerator enumerator = ch.Affects.GetEnumerator();
                AffectData paf = null;
                while (enumerator.MoveNext())
                {
                    AffectData pafNext = paf;
                    paf = enumerator.Current;

                    if (paf.Duration > 0)
                        paf.Duration--;
                    else if (paf.Duration < 0)
                    {
                        // Do nothing, skip
                    }
                    else
                    {
                        if (pafNext == null || pafNext.Type != paf.Type
                            || pafNext.Duration > 0)
                        {
                            SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>((int)paf.Type);
                            if (paf.Type > 0 && skill != null && !string.IsNullOrEmpty(skill.WearOffMessage))
                            {
                                color.set_char_color(ATTypes.AT_WEAROFF, ch);
                                color.send_to_char(skill.WearOffMessage, ch);
                                color.send_to_char("\r\n", ch);
                            }
                        }

                        if ((int)paf.Type == DatabaseManager.Instance.LookupSkill("possess"))
                        {
                            ch.Descriptor.Character = ch.Descriptor.Original;
                            ch.Descriptor.Original = null;
                            ch.Descriptor.Character.Descriptor = ch.Descriptor;
                            ch.Descriptor.Character.Switched = null;
                            ch.Descriptor = null;
                        }
                        ch.RemoveAffect(paf);
                    }
                }

                if (ch.CharDied())
                    continue;

                //// Check exits for moving players around
                ReturnTypes retCode = act_move.pullcheck(ch, Pulse);
                if (retCode == ReturnTypes.CharacterDied || ch.CharDied())
                    continue;

                //// Let the battle begin
                CharacterInstance victim = who_fighting(ch);
                if (victim == null || ch.IsAffected(AffectedByTypes.Paralysis))
                    continue;

                retCode = ReturnTypes.None;
                if (ch.CurrentRoom.Flags.IsSet((int)RoomFlags.Safe))
                {
                    LogManager.Instance.Log("{0} fighting {1} in a SAFE room.", ch.Name, victim.Name);
                    stop_fighting(ch, true);
                }
                else if (ch.IsAwake() && ch.CurrentRoom == victim.CurrentRoom)
                    retCode = multi_hit(ch, victim, Program.TYPE_UNDEFINED);
                else
                    stop_fighting(ch, false);

                if (ch.CharDied())
                    continue;

                if (retCode == ReturnTypes.CharacterDied)
                    continue;

                victim = who_fighting(ch);
                if (victim == null)
                    continue;

                //// Mob triggers
                mud_prog.rprog_rfight_trigger(ch);
                if (ch.CharDied() || victim.CharDied())
                    continue;
                mud_prog.mprog_hitprcnt_trigger(ch, victim);
                if (ch.CharDied() || victim.CharDied())
                    continue;
                mud_prog.mprog_fight_trigger(ch, victim);
                if (ch.CharDied() || victim.CharDied())
                    continue;

                //// NPC Special attack flags
                if (ch.IsNpc())
                {
                    if (!ch.Attacks.IsEmpty())
                    {
                        int attacktype = -1;
                        if (30 + (ch.Level / 4) >= SmaugRandom.Percent())
                        {
                            int cnt = 0;
                            for (; ; )
                            {
                                if (cnt++ > 10)
                                {
                                    attacktype = -1;
                                    break;
                                }
                                attacktype = SmaugRandom.Between(7, EnumerationFunctions.MaximumEnumValue<AttackTypes>() - 1);
                                if (ch.Attacks.IsSet(attacktype))
                                    break;
                            }

                            AttackTypes atkType = EnumerationExtensions.GetEnum<AttackTypes>(attacktype);
                            switch (atkType)
                            {
                                case AttackTypes.Bash:
                                    Bash.do_bash(ch, "");
                                    retCode = ReturnTypes.None;
                                    break;
                                case AttackTypes.Stun:
                                    Stun.do_stun(ch, "");
                                    retCode = ReturnTypes.None;
                                    break;
                                case AttackTypes.Gouge:
                                    Gouge.do_gouge(ch, "");
                                    retCode = ReturnTypes.None;
                                    break;
                                case AttackTypes.Feed:
                                    Feed.do_feed(ch, "");
                                    retCode = ReturnTypes.None;
                                    break;
                                case AttackTypes.Drain:
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private static void DecreaseExperienceInBattle(TimerData timer, CharacterInstance ch)
        {
            --timer.Count;
            if (timer.Count > 0)
                return;

            switch (timer.Type)
            {
                case TimerTypes.ASupressed:
                    if (timer.Value == -1)
                        timer.Count = 1000;
                    break;
                case TimerTypes.Nuisance:
                    ch.PlayerData.Nuisance = null;
                    break;
                case TimerTypes.DoFunction:
                    CharacterSubStates tempsub = ch.SubState;
                    ch.SubState = EnumerationExtensions.GetEnum<CharacterSubStates>(timer.Value);
                    timer.Action.Value.Invoke(ch, string.Empty);
                    if (ch.CharDied())
                        break;
                    ch.SubState = tempsub;
                    break;
            }
            handler.extract_timer(ch, timer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="victim"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static ReturnTypes multi_hit(CharacterInstance ch, CharacterInstance victim, int dt)
        {
            //// Add a timer to pkillers
            if (!ch.IsNpc() && !victim.IsNpc())
            {
                if (ch.Act.IsSet((int)PlayerFlags.Nice))
                    return (int)ReturnTypes.None;

                handler.add_timer(ch, (int)TimerTypes.RecentFight, 11, null, 0);
                handler.add_timer(victim, (int)TimerTypes.RecentFight, 11, null, 0);
            }

            if (ch.IsAttackSuppressed())
                return (int)ReturnTypes.None;

            if (ch.IsNpc() && ch.Act.IsSet((int)ActFlags.NoAttack))
                return 0;

            // TODO finish this

            return 0;
        }

        public static Tuple<int, int> weapon_prof_bonus_check(CharacterInstance ch, ObjectInstance wield)
        {
            if (ch.IsNpc() && ch.Level <= 5 && wield == null)
                return new Tuple<int, int>(0, -1);

            int bonus = 0;
            int sn = -1;

            switch (EnumerationExtensions.GetEnum<DamageTypes>(wield.Value[3]))
            {
                case DamageTypes.Hit:
                case DamageTypes.Suction:
                case DamageTypes.Bite:
                case DamageTypes.Blast:
                    sn = DatabaseManager.Instance.LookupSkill("pugilism");
                    break;
                case DamageTypes.Slash:
                case DamageTypes.Slice:
                    sn = DatabaseManager.Instance.LookupSkill("long blades");
                    break;
                case DamageTypes.Pierce:
                case DamageTypes.Stab:
                    sn = DatabaseManager.Instance.LookupSkill("short blades");
                    break;
                case DamageTypes.Whip:
                    sn = DatabaseManager.Instance.LookupSkill("flexible arms");
                    break;
                case DamageTypes.Claw:
                    sn = DatabaseManager.Instance.LookupSkill("talonous arms");
                    break;
                case DamageTypes.Pound:
                case DamageTypes.Crush:
                    sn = DatabaseManager.Instance.LookupSkill("bludgeons");
                    break;
                case DamageTypes.Bolt:
                case DamageTypes.Arrow:
                case DamageTypes.Dart:
                case DamageTypes.Stone:
                case DamageTypes.Pea:
                    sn = DatabaseManager.Instance.LookupSkill("missile weapons");
                    break;
            }

            if (sn != -1)
                bonus = (Macros.LEARNED(ch, sn) - 50) / 10;

            if (ch.IsDevoted())
                bonus -= ch.PlayerData.Favor / -400;

            return new Tuple<int, int>(bonus, sn);
        }

        public static int off_shld_lvl(CharacterInstance ch, CharacterInstance victim)
        {
            int lvl = 0;

            if (!ch.IsNpc())
            {
                lvl = 1.GetHighestOfTwoNumbers(ch.Level - 10 / 2);
                if (SmaugRandom.Percent() + (victim.Level - lvl) < 40)
                {
                    if (ch.CanPKill() && victim.CanPKill())
                        return ch.Level;
                    return lvl;
                }
                return 0;
            }

            lvl = ch.Level / 2;
            return SmaugRandom.Percent() + (victim.Level - lvl) < 70 ? lvl : 0;
        }

        private static bool DualFlip = false;
        public static ReturnTypes one_hit(CharacterInstance ch, CharacterInstance victim, int dt)
        {
            int damageType = dt;
            ReturnTypes retcode = ReturnTypes.None;

            // Can't beat a dead character and guard against room-leavings
            if (victim.CurrentPosition == PositionTypes.Dead
                || ch.CurrentRoom != victim.CurrentRoom)
                return ReturnTypes.CharacterDied;

            // Figure out the weapon doing the damage
            ObjectInstance wield = ch.GetEquippedItem(WearLocations.DualWield);
            if (wield != null)
            {
                if (!DualFlip)
                {
                    DualFlip = true;
                    wield = ch.GetEquippedItem(WearLocations.Wield);
                }
                else
                    DualFlip = false;
            }
            else
                wield = ch.GetEquippedItem(WearLocations.Wield);

            Tuple<int, int> profBonus = null;
            if (wield != null)
                profBonus = weapon_prof_bonus_check(ch, wield);

            int proficiencyBonus = profBonus != null ? profBonus.Item1 : 0;
            int sn = profBonus != null ? profBonus.Item2 : -1;

            if (ch.CurrentFighting != null
                && damageType == Program.TYPE_UNDEFINED
                && ch.IsNpc()
                && !ch.Attacks.IsEmpty())
            {
                int cnt = 0;
                int attacktype = 0;
                for (; ; )
                {
                    attacktype = SmaugRandom.Between(0, 6);
                    if (ch.Attacks.IsSet(attacktype))
                        break;
                    if (cnt++ > 16)
                    {
                        attacktype = -1;
                        break;
                    }
                }

                if (attacktype == (int)AttackTypes.Backstab)
                    attacktype = -1;
                if (wield != null && (SmaugRandom.Percent() > 25))
                    attacktype = -1;
                if (wield == null && (SmaugRandom.Percent() > 50))
                    attacktype = -1;

                retcode = ReturnTypes.None;
                switch (attacktype)
                {
                    case (int)AttackTypes.Bite:
                        Bite.do_bite(ch, "");
                        retcode = handler.GlobalReturnCode;
                        break;
                    case (int)AttackTypes.Claws:
                        Claw.do_claw(ch, "");
                        retcode = handler.GlobalReturnCode;
                        break;
                    case (int)AttackTypes.Tail:
                        Tail.do_tail(ch, "");
                        retcode = handler.GlobalReturnCode;
                        break;
                    case (int)AttackTypes.Sting:
                        Sting.do_sting(ch, "");
                        retcode = handler.GlobalReturnCode;
                        break;
                    case (int)AttackTypes.Punch:
                        Punch.do_punch(ch, "");
                        retcode = handler.GlobalReturnCode;
                        break;
                    case (int)AttackTypes.Kick:
                        Kick.do_kick(ch, "");
                        retcode = handler.GlobalReturnCode;
                        break;
                    case (int)AttackTypes.Trip:
                        attacktype = 0;
                        break;
                }

                if (attacktype >= 0)
                    return retcode;
            }

            if (damageType == Program.TYPE_UNDEFINED)
            {
                damageType = (int)DamageTypes.Hit;
                if (wield != null && wield.ItemType == ItemTypes.Weapon)
                    damageType += wield.Value[3];
            }

            // Calculate thac0 vs armor
            int thac0_00 = 0, thac0_32 = 0;

            if (ch.IsNpc())
            {
                thac0_00 = ch.ToHitArmorClass0;
                thac0_32 = thac0_00;
            }
            else
            {
                thac0_00 = DatabaseManager.Instance.GetClass(ch.CurrentClass).ToHitArmorClass0;
                thac0_32 = DatabaseManager.Instance.GetClass(ch.CurrentClass).ToHitArmorClass32;
            }
            thac0_00 = ch.Level.Interpolate(thac0_00, thac0_32) - ch.GetHitroll();
            int victimArmorClass = -19.GetHighestOfTwoNumbers(victim.GetArmorClass() / 10);

            // if you can't see what;s coming
            if (wield != null && !victim.CanSee(wield))
                victimArmorClass += 1;
            if (!ch.CanSee(victim))
                victimArmorClass -= 4;

            // Learning between combatants. Takes the intelligence difference, 
            // and multiplies by the times killed to make up a learning bonus 
            // given to whoever is more intelligent
            if (ch.CurrentFighting != null && ch.CurrentFighting.Who == victim)
            {
                if (ch.CurrentFighting.TimesKilled > 0)
                {
                    int intDiff = ch.GetCurrentIntelligence() - victim.GetCurrentIntelligence();
                    if (intDiff != 0)
                        victimArmorClass += (intDiff * ch.CurrentFighting.TimesKilled) / 10;
                }
            }

            // Weapon proficiency bonus
            victimArmorClass += proficiencyBonus;

            int diceroll = 0;
            while ((diceroll = SmaugRandom.Bits(5)) >= 20)
                ;

            if (diceroll == 0 || (diceroll != 19 && diceroll < thac0_00 - victimArmorClass))
            {
                // Miss!
                if (sn != -1)
                    skills.learn_from_failure(ch, sn);
                fight.damage(ch, victim, 0, damageType);
                // TODO Tail_chain?
                return ReturnTypes.None;
            }

            // HIt!
            int damage = wield == null
                             ? SmaugRandom.RollDice(ch.BareDice.NumberOf, ch.BareDice.SizeOf) + ch.DamageRoll.Bonus
                             : SmaugRandom.Between(wield.Value[1], wield.Value[2]);

            // Bonuses
            damage += ch.GetDamroll();
            if (proficiencyBonus > 0)
                damage += proficiencyBonus / 4;

            damage = ModifyDamageByFightingStyle(victim, damage);
            damage = ModifyDamageByFightingStyle(ch, damage);

            if (!ch.IsNpc() && ch.PlayerData.Learned[DatabaseManager.Instance.LookupSkill("enhanced damage")] > 0)
            {
                damage += damage * Macros.LEARNED(ch, DatabaseManager.Instance.LookupSkill("enhanced damage") / 120);
                skills.learn_from_success(ch, DatabaseManager.Instance.LookupSkill("enhanced damage"));
            }

            if (!victim.IsAwake())
                damage *= 2;
            if (dt == DatabaseManager.Instance.LookupSkill("backstab"))
                damage *= (2 + (ch.Level - (victim.Level / 4)).GetNumberThatIsBetween(2, 30) / 8);
            if (dt == DatabaseManager.Instance.LookupSkill("circle"))
                damage *= (2 + (ch.Level - (victim.Level / 4)).GetNumberThatIsBetween(2, 30) / 16);
            if (damage <= 0)
                damage = 1;

            int plusRIS = 0;
            if (wield != null)
            {
                damage = Macros.IS_OBJ_STAT(wield, (int)ItemExtraFlags.Magical)
                    ? ris_damage(victim, damage, (int)ResistanceTypes.Magic)
                    : ris_damage(victim, damage, (int)ResistanceTypes.NonMagic);

                // Handle PLUS1 - PLUS6 ris bits vs weapon hitroll
                plusRIS = wield.GetHitRoll();
            }
            else
                damage = ris_damage(victim, damage, (int)ResistanceTypes.NonMagic);

            // Check for RIS_PLUS
            if (damage > 0)
            {
                if (plusRIS > 0)
                    plusRIS = (int)ResistanceTypes.Plus1 << plusRIS.GetLowestOfTwoNumbers(7);

                int imm = -1, res = -1, sus = 1;

                // find the high resistance
                for (int x = (int)ResistanceTypes.Plus1; x <= (int)ResistanceTypes.Plus6; x <<= 1)
                {
                    if (victim.Immunity.IsSet(x))
                        imm = x;
                    if (victim.Resistance.IsSet(x))
                        res = x;
                    if (victim.Susceptibility.IsSet(x))
                        sus = x;
                }

                int mod = 10;
                if (imm >= plusRIS)
                    mod -= 10;
                if (res >= plusRIS)
                    mod -= 2;
                if (sus <= plusRIS)
                    mod += 2;

                // check if immune
                if (mod <= 0)
                    damage = -1;
                if (mod != 10)
                    damage = (damage * mod) / 10;
            }

            if (sn != -1)
            {
                if (damage > 0)
                    skills.learn_from_success(ch, sn);
                else
                    skills.learn_from_failure(ch, sn);
            }

            // immune to damage
            if (damage == -1)
            {
                SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>(dt);
                if (skill != null)
                {
                    if (!skill.ImmuneCharacterMessage.IsNullOrEmpty())
                        comm.act(ATTypes.AT_HIT, skill.ImmuneCharacterMessage, ch, null, victim, ToTypes.Character);
                    if (!skill.ImmuneVictimMessage.IsNullOrEmpty())
                        comm.act(ATTypes.AT_HITME, skill.ImmuneVictimMessage, ch, null, victim, ToTypes.Victim);
                    if (!skill.ImmuneRoomMessage.IsNullOrEmpty())
                        comm.act(ATTypes.AT_ACTION, skill.ImmuneRoomMessage, ch, null, victim, ToTypes.Room);
                    return ReturnTypes.None;
                }

                damage = 0;
            }

            retcode = fight.damage(ch, victim, damage, dt);

            if (retcode != ReturnTypes.None)
                return retcode;
            if (ch.CharDied())
                return ReturnTypes.CharacterDied;
            if (victim.CharDied())
                return ReturnTypes.VictimDied;

            retcode = ReturnTypes.None;
            if (damage == 0)
                return retcode;

            // Weapon spell support
            if (wield != null && !victim.Immunity.IsSet((int)ResistanceTypes.Magic)
                && !victim.CurrentRoom.Flags.IsSet((int)RoomFlags.NoMagic))
            {
                foreach (AffectData aff in wield.ObjectIndex.Affects)
                {
                    if (aff.Location == ApplyTypes.WeaponSpell
                        && Macros.IS_VALID_SN(aff.Modifier)
                        && DatabaseManager.Instance.GetEntity<SkillData>(aff.Modifier).SpellFunction != null)
                        retcode = DatabaseManager.Instance.GetEntity<SkillData>(aff.Modifier)
                                    .SpellFunction.Value.Invoke(aff.Modifier, (wield.Level + 3) / 3, ch, victim);
                }

                if (retcode == ReturnTypes.SpellFailed)
                    return ReturnTypes.None;
                if (retcode != ReturnTypes.None && (ch.CharDied() || victim.CharDied()))
                    return retcode;

                foreach (AffectData aff in wield.Affects)
                {
                    if (aff.Location == ApplyTypes.WeaponSpell
                        && Macros.IS_VALID_SN(aff.Modifier)
                        && DatabaseManager.Instance.GetEntity<SkillData>(aff.Modifier).SpellFunction != null)
                        retcode = DatabaseManager.Instance.GetEntity<SkillData>(aff.Modifier)
                                    .SpellFunction.Value.Invoke(aff.Modifier, (wield.Level + 3) / 3, ch, victim);
                }

                if (retcode == ReturnTypes.SpellFailed)
                    return ReturnTypes.None;
                if (retcode != ReturnTypes.None && (ch.CharDied() || victim.CharDied()))
                    return retcode;
            }

            // Magic shields that retaliate
            if (victim.IsAffected(AffectedByTypes.FireShield)
                && !ch.IsAffected(AffectedByTypes.FireShield))
                retcode = Smaug.spell_smaug(DatabaseManager.Instance.LookupSkill("flare"), off_shld_lvl(victim, ch), victim, ch);
            if (retcode != ReturnTypes.None || ch.CharDied() || victim.CharDied())
                return retcode;

            if (victim.IsAffected(AffectedByTypes.IceShield)
                && !ch.IsAffected(AffectedByTypes.IceShield))
                retcode = Smaug.spell_smaug(DatabaseManager.Instance.LookupSkill("iceshard"), off_shld_lvl(victim, ch), victim, ch);
            if (retcode != ReturnTypes.None || ch.CharDied() || victim.CharDied())
                return retcode;

            if (victim.IsAffected(AffectedByTypes.ShockShield)
            && !ch.IsAffected(AffectedByTypes.ShockShield))
                retcode = Smaug.spell_smaug(DatabaseManager.Instance.LookupSkill("torrent"), off_shld_lvl(victim, ch), victim, ch);
            if (retcode != ReturnTypes.None || ch.CharDied() || victim.CharDied())
                return retcode;

            if (victim.IsAffected(AffectedByTypes.AcidMist)
            && !ch.IsAffected(AffectedByTypes.AcidMist))
                retcode = Smaug.spell_smaug(DatabaseManager.Instance.LookupSkill("acidshot"), off_shld_lvl(victim, ch), victim, ch);
            if (retcode != ReturnTypes.None || ch.CharDied() || victim.CharDied())
                return retcode;

            if (victim.IsAffected(AffectedByTypes.VenomShield)
            && !ch.IsAffected(AffectedByTypes.VenomShield))
                retcode = Smaug.spell_smaug(DatabaseManager.Instance.LookupSkill("venomshot"), off_shld_lvl(victim, ch), victim, ch);
            if (retcode != ReturnTypes.None || ch.CharDied() || victim.CharDied())
                return retcode;

            // TODO tail_chain?
            return 0;
        }

        public static ReturnTypes projectile_hit(CharacterInstance ch, CharacterInstance victim, ObjectInstance wield, ObjectInstance projectile, short dist)
        {
            if (projectile == null)
                return (int) ReturnTypes.None;

            Tuple<int, int> CalculatedBonus = CalculateProjectileBonus(projectile);
            int bonus = CalculatedBonus.Item1;
            int dt = CalculatedBonus.Item2;
            
            //// Can't beat a dead character
            if (victim.CurrentPosition == PositionTypes.Dead
                || victim.CharDied())
            {
                handler.extract_obj(projectile);
                return ReturnTypes.CharacterDied;
            }

            Tuple<int, int> profBonus = wield != null
                                            ? weapon_prof_bonus_check(ch, wield)
                                            : new Tuple<int, int>(0, 0);

            if (dt == (int) SkillNumberTypes.Undefined)
            {
                dt = (int) SkillNumberTypes.Hit;
                if (wield != null && wield.ItemType == ItemTypes.MissileWeapon)
                    dt += wield.Value[3];
            }

            //// Calculate to-hit-AC0 versus armor
            Tuple<int, int> thac0 = CalculateThac0(ch, dist);
            int thac0_0 = thac0.Item1;
            int thac0_32 = thac0.Item2;
            int victimArmorClass = CalculateArmorClass(ch, victim, projectile, bonus);
            
            int diceroll = SmaugRandom.RollDice(1, 20);
            if (diceroll == 0 || (diceroll != 19 && diceroll < thac0_0 - victimArmorClass))
                return ProjectileMissed(ch, profBonus.Item2, projectile, victim, dt);
            return ProjectileHit(ch, victim, projectile, wield, bonus, dt);
        }

        private static Tuple<int, int> CalculateProjectileBonus(ObjectInstance projectile)
        {
            int dt, bonus;
            if (projectile.ItemType == ItemTypes.Projectile 
                || projectile.ItemType == ItemTypes.Weapon)
            {
                dt = (int)SkillNumberTypes.Hit + projectile.Value[3];
                bonus = SmaugRandom.Between(projectile.Value[1], projectile.Value[2]);
            }
            else
            {
                dt = (int)SkillNumberTypes.Undefined;
                bonus = SmaugRandom.Between(1, 2.GetNumberThatIsBetween(projectile.GetObjectWeight(), 100));
            }

            return new Tuple<int, int>(bonus, dt);
        }

        private static Tuple<int, int> CalculateThac0(CharacterInstance ch, int distance)
        {
            int thac0_0 = ch.ToHitArmorClass0;
            int thac0_32 = thac0_0;

            if (!ch.IsNpc())
            {
                thac0_0 = DatabaseManager.Instance.GetClass(ch.CurrentClass).ToHitArmorClass0;
                thac0_32 = DatabaseManager.Instance.GetClass(ch.CurrentClass).ToHitArmorClass32;
            }

            thac0_0 = ch.Level.Interpolate(thac0_0, thac0_32) - ch.GetHitroll() + (distance * 2);

            return new Tuple<int, int>(thac0_0, thac0_32);
        }

        private static int CalculateArmorClass(CharacterInstance ch, CharacterInstance victim, ObjectInstance projectile, int bonus)
        {
            int victimArmorClass = -19.GetHighestOfTwoNumbers(victim.GetArmorClass() / 10);

            //// If you can't see what's coming
            if (!victim.CanSee(projectile))
                victimArmorClass += 1;
            if (!ch.CanSee(victim))
                victimArmorClass -= 4;

            //// Weapon proficiency bonus
            victimArmorClass += bonus;

            return victimArmorClass;
        }
        
        private static ReturnTypes ProjectileMissed(CharacterInstance ch, int proficiencySkillNumber,
                                                   ObjectInstance projectile, CharacterInstance victim, int dt)
        {
            if (proficiencySkillNumber != -1)
                skills.learn_from_failure(ch, proficiencySkillNumber);

            if (SmaugRandom.Percent() < 50)
                handler.extract_obj(projectile);
            else
            {
                if (projectile.InObject != null)
                    projectile.FromObject(projectile.InObject);
                if (projectile.CarriedBy != null)
                    projectile.FromCharacter();
                victim.CurrentRoom.ToRoom(projectile);
            }

            damage(ch, victim, 0, dt);
            // TODO: tail_chain()
            return ReturnTypes.None;
        }

        private static ReturnTypes ProjectileHit(CharacterInstance ch, CharacterInstance victim,
                                                 ObjectInstance projectile, ObjectInstance wield, int bonus, int dt)
        {
            int damage = wield == null ? bonus : SmaugRandom.Between(wield.Value[1], wield.Value[2]) + (bonus/10);
            damage += ch.GetDamroll();
            if (bonus > 0)
                damage += bonus/4;
            damage = ModifyDamageByFightingStyle(victim, damage);

            if (!ch.IsNpc())
            {
                SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>("Enhanced Damage");
                if (ch.PlayerData.Learned[skill.ID] > 0)
                {
                    damage += damage * Macros.LEARNED(ch, (int)skill.ID);
                    skills.learn_from_success(ch, (int)skill.ID);
                }
            }

            if (!victim.IsAwake())
                damage *= 2;
            if (damage <= 0)
                damage = 1;

            damage = Macros.IS_OBJ_STAT(projectile, (int) ItemExtraFlags.Magical)
                         ? ris_damage(victim, damage, (int) ResistanceTypes.Magic)
                         : ris_damage(victim, damage, (int) ResistanceTypes.NonMagic);

            int plusris = 0;
            
            //// Handle PLUS1 - PLUS6 ris bits vs weapon hitroll
            if (wield != null)
                plusris = wield.GetHitRoll();

            // TODO Finish this

            return ReturnTypes.None;
        }

        private static int ModifyDamageByFightingStyle(CharacterInstance victim, int damage)
        {
            if (victim.CurrentPosition == PositionTypes.Berserk)
                return (int)(damage*1.2f);
            if (victim.CurrentPosition == PositionTypes.Aggressive)
                return (int) (damage*1.1f);
            if (victim.CurrentPosition == PositionTypes.Defensive)
                return (int) (damage*0.85f);
            if (victim.CurrentPosition == PositionTypes.Evasive)
                return (int) (damage*0.8f);
            return damage;
        }

        public static int ris_damage(CharacterInstance ch, int dam, int ris)
        {
            int modifier = 10;
            if (ch.Immunity.IsSet(ris) && !ch.NoImmunity.IsSet(ris))
                modifier -= 10;
            if (ch.Resistance.IsSet(ris) && !ch.NoResistance.IsSet(ris))
                modifier -= 2;
            if (ch.Susceptibility.IsSet(ris) && !ch.NoSusceptibility.IsSet(ris))
            {
                if (ch.IsNpc() && ch.Immunity.IsSet(ris))
                    modifier += 0;
                else
                    modifier += 2;
            }
            if (modifier <= 0)
                return -1;
            if (modifier == 10)
                return dam;
            return (dam * modifier) / 10;
        }

        public static ReturnTypes damage(CharacterInstance ch, CharacterInstance victim, int dam, int dt)
        {
            // TODO
            return ReturnTypes.None;
        }

        public static bool is_safe(CharacterInstance ch, CharacterInstance victim, bool show_messg)
        {
            if (who_fighting(ch) == ch)
                return false;

            if (victim.CurrentRoom.Flags.IsSet((int)RoomFlags.Safe))
            {
                if (show_messg)
                {
                    color.set_char_color(ATTypes.AT_MAGIC, ch);
                    color.send_to_char("A magical force prevents you from attacking.\r\n", ch);
                }
                return true;
            }

            if (Macros.IS_PACIFIST(ch))
            {
                if (show_messg)
                {
                    color.set_char_color(ATTypes.AT_MAGIC, ch);
                    color.ch_printf(ch, "You are a pacifist and will not fight.\r\n");
                }
                return true;
            }

            if (Macros.IS_PACIFIST(victim))
            {
                if (show_messg)
                {
                    color.set_char_color(ATTypes.AT_MAGIC, ch);
                    color.send_to_char(string.Format("{0} is a pacifist and will not fight.\r\n", victim.ShortDescription.CapitalizeFirst()), ch);
                }
                return true;
            }

            if (!ch.IsNpc() && ch.Level >= LevelConstants.GetLevel("immortal"))
                return false;

            if (!ch.IsNpc() && !victim.IsNpc() && ch != victim
                && victim.CurrentRoom.Area.Flags.IsSet((int)AreaFlags.NoPKill))
            {
                if (show_messg)
                {
                    color.set_char_color(ATTypes.AT_IMMORT, ch);
                    color.send_to_char("The gods have forbidden player killing in this area.\r\n", ch);
                }
                return true;
            }

            if (ch.IsNpc() || victim.IsNpc())
                return false;

            if (ch.CalculateAge() < 18 || ch.Level < 5)
            {
                if (show_messg)
                {
                    color.set_char_color(ATTypes.AT_WHITE, ch);
                    color.send_to_char("You are not yet ready, needing age or experience, if not both.\r\n", ch);
                }
                return true;
            }
            if (victim.CalculateAge() < 18 || victim.Level < 5)
            {
                if (show_messg)
                {
                    color.set_char_color(ATTypes.AT_WHITE, ch);
                    color.send_to_char("They are yet too young to die.\r\n", ch);
                }
                return true;
            }

            if (ch.Level - victim.Level > 5 || victim.Level - ch.Level > 5)
            {
                if (show_messg)
                {
                    color.set_char_color(ATTypes.AT_IMMORT, ch);
                    color.send_to_char("The gods do not allow murder when there is such a difference in level.\r\n", ch);
                }
                return true;
            }

            if (handler.get_timer(victim, (int)TimerTypes.PKilled) > 0)
            {
                if (show_messg)
                {
                    color.set_char_color(ATTypes.AT_GREEN, ch);
                    color.send_to_char("That character has died within the last 5 minutes.\r\n", ch);
                }
                return true;
            }

            if (handler.get_timer(ch, (int)TimerTypes.PKilled) > 0)
            {
                if (show_messg)
                {
                    color.set_char_color(ATTypes.AT_GREEN, ch);
                    color.send_to_char("You have been killed within the last 5 minutes.\r\n", ch);
                }
                return true;
            }

            return false;
        }

        public static bool legal_loot(CharacterInstance ch, CharacterInstance victim)
        {
            // Anyone can loot mobs
            if (victim.IsNpc())
                return true;

            // Non-charmed mobs can loot anything
            if (ch.IsNpc() && ch.Master == null)
                return true;

            // Members of diferent clans can loot too
            if (!ch.IsNpc() && !victim.IsNpc()
                && ch.PlayerData.Flags.IsSet((int)PCFlags.Deadly)
                && victim.PlayerData.Flags.IsSet((int)PCFlags.Deadly))
                return true;

            return false;
        }

        /// <summary>
        /// See if an attack justifies a KILLER flag
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="victim"></param>
        public static void check_killer(CharacterInstance ch, CharacterInstance victim)
        {
            // NPCs are fair game
            if (victim.IsNpc())
            {
                if (!ch.IsNpc())
                {
                    int levelRatio = 0;

                    levelRatio = victim.Level < 1
                        ? ch.Level.GetNumberThatIsBetween(1, LevelConstants.MaxLevel)
                        : (ch.Level / victim.Level).GetNumberThatIsBetween(1, LevelConstants.MaxLevel);

                    if (ch.PlayerData.Clan != null)
                        ch.PlayerData.Clan.PvEKills++;
                    ch.PlayerData.PvEKills++;
                    ch.CurrentRoom.Area.PvEKills++;

                    if (ch.PlayerData.CurrentDeity != null)
                    {
                        if (victim.CurrentRace == ch.PlayerData.CurrentDeity.NPCRace)
                            deity.adjust_favor(ch, 3, levelRatio);
                        else if (victim.CurrentRace == ch.PlayerData.CurrentDeity.NPCFoe)
                            deity.adjust_favor(ch, 17, levelRatio);
                        else
                            deity.adjust_favor(ch, 2, levelRatio);
                    }
                }
                return;
            }

            // if you kill yourself, nothing happens
            if (ch == victim || ch.Level >= LevelConstants.GetLevel("immortal"))
                return;

            // Any character in the arena is okay to kill
            if (ch.IsInArena())
            {
                if (!ch.IsNpc() && !victim.IsNpc())
                {
                    ch.PlayerData.PvPKills++;
                    victim.PlayerData.PvPDeaths++;
                }
                return;
            }

            // killers and thieves are okay to kill 
            if (victim.Act.IsSet((int)PlayerFlags.Killer)
                || victim.Act.IsSet((int)PlayerFlags.Thief))
            {
                if (!ch.IsNpc())
                {
                    if (ch.PlayerData.Clan != null)
                    {
                        if (victim.Level < 10)
                            ch.PlayerData.Clan.PvPKillTable[0]++;
                        else if (victim.Level < 15)
                            ch.PlayerData.Clan.PvPKillTable[1]++;
                        else if (victim.Level < 20)
                            ch.PlayerData.Clan.PvPKillTable[2]++;
                        else if (victim.Level < 30)
                            ch.PlayerData.Clan.PvPKillTable[3]++;
                        else if (victim.Level < 40)
                            ch.PlayerData.Clan.PvPKillTable[4]++;
                        else if (victim.Level < 50)
                            ch.PlayerData.Clan.PvPKillTable[5]++;
                        else
                            ch.PlayerData.Clan.PvPKillTable[6]++;
                    }

                    ch.PlayerData.PvPKills++;
                    ch.CurrentRoom.Area.PvPKills++;
                }
                return;
            }

            // Clan checks
            if (!ch.IsNpc() && !victim.IsNpc()
                && ch.PlayerData.Flags.IsSet((int)PCFlags.Deadly)
                && victim.PlayerData.Flags.IsSet((int)PCFlags.Deadly))
            {
                if (ch.PlayerData.Clan == null
                    || victim.PlayerData.Clan == null
                    || (ch.PlayerData.Clan.ClanType != ClanTypes.NoKill
                        && victim.PlayerData.Clan.ClanType != ClanTypes.NoKill
                        && ch.PlayerData.Clan != victim.PlayerData.Clan))
                {
                    if (ch.PlayerData.Clan != null)
                    {
                        if (victim.Level < 10)
                            ch.PlayerData.Clan.PvPKillTable[0]++;
                        else if (victim.Level < 15)
                            ch.PlayerData.Clan.PvPKillTable[1]++;
                        else if (victim.Level < 20)
                            ch.PlayerData.Clan.PvPKillTable[2]++;
                        else if (victim.Level < 30)
                            ch.PlayerData.Clan.PvPKillTable[3]++;
                        else if (victim.Level < 40)
                            ch.PlayerData.Clan.PvPKillTable[4]++;
                        else if (victim.Level < 50)
                            ch.PlayerData.Clan.PvPKillTable[5]++;
                        else
                            ch.PlayerData.Clan.PvPKillTable[6]++;
                    }

                    ch.PlayerData.PvPKills++;
                    ch.CurrentHealth = ch.MaximumHealth;
                    ch.CurrentMana = ch.MaximumMana;
                    ch.CurrentMovement = ch.MaximumMovement;
                    if (ch.PlayerData != null)
                        ch.PlayerData.ConditionTable[ConditionTypes.Bloodthirsty] = (10 + ch.Level);
                    update_pos(victim);
                    if (victim != ch)
                    {
                        comm.act(ATTypes.AT_MAGIC, "Bolts of blue energy rise from the corpse, seeping into $n.", ch, victim.Name, null, ToTypes.Room);
                        comm.act(ATTypes.AT_MAGIC, "Bolts of blue energy rise from the corpse, seeping into you.", ch, victim.Name, null, ToTypes.Character);
                    }

                    if (victim.PlayerData.Clan != null)
                    {
                        if (victim.Level < 10)
                            ch.PlayerData.Clan.PvPDeathTable[0]++;
                        else if (victim.Level < 15)
                            ch.PlayerData.Clan.PvPDeathTable[1]++;
                        else if (victim.Level < 20)
                            ch.PlayerData.Clan.PvPDeathTable[2]++;
                        else if (victim.Level < 30)
                            ch.PlayerData.Clan.PvPDeathTable[3]++;
                        else if (victim.Level < 40)
                            ch.PlayerData.Clan.PvPDeathTable[4]++;
                        else if (victim.Level < 50)
                            ch.PlayerData.Clan.PvPDeathTable[5]++;
                        else
                            ch.PlayerData.Clan.PvPDeathTable[6]++;
                    }

                    victim.PlayerData.PvPDeaths++;
                    deity.adjust_favor(victim, 11, 1);
                    deity.adjust_favor(ch, 2, 1);
                    handler.add_timer(victim, (short)TimerTypes.PKilled, 115, null, 0);
                    Macros.WAIT_STATE(victim, 3 * GameManager.Instance.SystemData.PulseViolence);
                    return;
                }
            }

            if (ch.IsAffected(AffectedByTypes.Charm))
            {
                if (ch.Master == null)
                {
                    LogManager.Instance.Bug("{0} bad AffectedByTypes.Charm", ch.IsNpc() ? ch.ShortDescription : ch.Name);
                    // TODO: affect_strip
                    ch.AffectedBy.RemoveBit((int)AffectedByTypes.Charm);
                    return;
                }

                check_killer(ch.Master, victim);
                return;
            }

            if (ch.IsNpc())
            {
                if (!victim.IsNpc())
                {
                    if (victim.PlayerData.Clan != null)
                        victim.PlayerData.Clan.PvEDeaths++;
                    victim.PlayerData.PvEDeaths++;
                    victim.CurrentRoom.Area.PvEDeaths++;

                    int levelRatio = (ch.Level / victim.Level).GetNumberThatIsBetween(1, LevelConstants.GetLevel("avatar"));
                    if (victim.PlayerData.CurrentDeity != null)
                    {
                        if (ch.CurrentRace == victim.PlayerData.CurrentDeity.NPCRace)
                            deity.adjust_favor(victim, 12, levelRatio);
                        else if (ch.CurrentRace == victim.PlayerData.CurrentDeity.NPCFoe)
                            deity.adjust_favor(victim, 15, levelRatio);
                        else
                            deity.adjust_favor(victim, 11, levelRatio);
                    }
                }
                return;
            }

            if (!ch.IsNpc())
            {
                if (ch.PlayerData.Clan != null)
                    ch.PlayerData.Clan.IllegalPvPKill++;
                ch.PlayerData.IllegalPvPKill++;
                ch.CurrentRoom.Area.IllegalPvPKill++;
            }

            if (!victim.IsNpc())
            {
                if (victim.PlayerData.Clan != null)
                {
                    if (victim.Level < 10)
                        ch.PlayerData.Clan.PvPDeathTable[0]++;
                    else if (victim.Level < 15)
                        ch.PlayerData.Clan.PvPDeathTable[1]++;
                    else if (victim.Level < 20)
                        ch.PlayerData.Clan.PvPDeathTable[2]++;
                    else if (victim.Level < 30)
                        ch.PlayerData.Clan.PvPDeathTable[3]++;
                    else if (victim.Level < 40)
                        ch.PlayerData.Clan.PvPDeathTable[4]++;
                    else if (victim.Level < 50)
                        ch.PlayerData.Clan.PvPDeathTable[5]++;
                    else
                        ch.PlayerData.Clan.PvPDeathTable[6]++;
                }

                victim.PlayerData.PvPDeaths++;
                victim.CurrentRoom.Area.PvPDeaths++;
            }

            if (ch.Act.IsSet((int)PlayerFlags.Killer))
                return;

            color.set_char_color(ATTypes.AT_WHITE, ch);
            color.send_to_char("A strange feeling grows deep inside you, and a tingle goes up your spine...\r\n", ch);
            color.set_char_color(ATTypes.AT_IMMORT, ch);
            color.send_to_char("A deep voice booms inside your head, 'Thou shall now be known as a deadly murderer!!!'\r\n", ch);
            color.set_char_color(ATTypes.AT_WHITE, ch);
            color.send_to_char("You feel as if your soul has been revealed for all to see.\r\n", ch);
            ch.Act.SetBit((int)PlayerFlags.Killer);
            if (ch.Act.IsSet((int)PlayerFlags.Attacker))
                ch.Act.RemoveBit((int)PlayerFlags.Attacker);
            save.save_char_obj(ch);
        }

        /// <summary>
        /// See if an attack justifies an ATTACKER flag
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="victim"></param>
        public static void check_attacker(CharacterInstance ch, CharacterInstance victim)
        {
            // NPCs, killers and theives are fair game
            if (victim.IsNpc() || victim.Act.IsSet((int)PlayerFlags.Killer) ||
                victim.Act.IsSet((int)PlayerFlags.Thief))
                return;

            if (!ch.IsNpc() && !victim.IsNpc() && ch.CanPKill() && victim.CanPKill())
                return;

            if (ch.IsAffected(AffectedByTypes.Charm))
            {
                if (ch.Master == null)
                {
                    LogManager.Instance.Bug("{0} bad AffectedByTypes.Charm", ch.IsNpc() ? ch.ShortDescription : ch.Name);
                    // TODO affect_strip
                    ch.AffectedBy.RemoveBit((int)AffectedByTypes.Charm);
                    return;
                }

                return;
            }

            if (ch.IsNpc() || ch == victim || ch.Level >= LevelConstants.GetLevel("immortal") ||
                ch.Act.IsSet((int)PlayerFlags.Attacker) || ch.Act.IsSet((int)PlayerFlags.Killer))
                return;

            ch.Act.SetBit((int)PlayerFlags.Attacker);
            save.save_char_obj(ch);
        }

        public static void update_pos(CharacterInstance victim)
        {
            if (victim.CurrentHealth > 0)
            {
                if (victim.CurrentPosition <= PositionTypes.Stunned)
                    victim.CurrentPosition = PositionTypes.Standing;
                if (victim.IsAffected(AffectedByTypes.Paralysis))
                    victim.CurrentPosition = PositionTypes.Stunned;
                return;
            }

            // You're dead
            if (victim.IsNpc() || victim.CurrentHealth <= -11)
            {
                if (victim.CurrentMount != null)
                {
                    comm.act(ATTypes.AT_ACTION, "$n falls from $N.", victim, null, victim.CurrentMount, ToTypes.Room);
                    victim.CurrentMount.Act.RemoveBit((int)ActFlags.Mounted);
                    victim.CurrentMount = null;
                }

                victim.CurrentPosition = PositionTypes.Dead;
                return;
            }

            if (victim.CurrentHealth <= -6)
                victim.CurrentPosition = PositionTypes.Mortal;
            else if (victim.CurrentHealth <= -3)
                victim.CurrentPosition = PositionTypes.Incapacitated;
            else
                victim.CurrentPosition = PositionTypes.Stunned;

            if (victim.CurrentPosition > PositionTypes.Stunned
                && victim.IsAffected(AffectedByTypes.Paralysis))
                victim.CurrentPosition = PositionTypes.Stunned;

            if (victim.CurrentMount != null)
            {
                comm.act(ATTypes.AT_ACTION, "$n falls unconcious from $N.", victim, null, victim.CurrentMount, ToTypes.Room);
                victim.CurrentMount.Act.RemoveBit((int)ActFlags.Mounted);
                victim.CurrentMount = null;
            }
        }

        public static void set_fighting(CharacterInstance ch, CharacterInstance victim)
        {
            if (ch.CurrentFighting != null)
            {
                LogManager.Instance.Bug("{0} -> {1} is already fighting {2}", ch.Name, victim.Name, ch.CurrentFighting.Who.Name);
                return;
            }

            if (ch.IsAffected(AffectedByTypes.Sleep))
            {
                // TODO affect_strip
            }

            if (victim.NumberFighting > Program.MAX_FIGHT)
            {
                color.send_to_char("There are too many people fighting for you to join in.\r\n", ch);
                return;
            }

            FightingData fight = new FightingData
                                     {
                                         Who = victim,
                                         Experience = (int)(xp_compute(ch, victim) * 0.85),
                                         Alignment = align_compute(ch, victim)
                                     };
            if (!ch.IsNpc() && victim.IsNpc())
                fight.TimesKilled = ch.TimesKilled( victim);

            ch.NumberFighting = 1;
            ch.CurrentFighting = fight;

            if (ch.IsNpc())
                ch.CurrentPosition = PositionTypes.Fighting;
            else
            {
                switch (ch.CurrentStyle)
                {
                    case StyleTypes.Evasive:
                        ch.CurrentPosition = PositionTypes.Evasive;
                        break;
                    case StyleTypes.Defensive:
                        ch.CurrentPosition = PositionTypes.Defensive;
                        break;
                    case StyleTypes.Aggressive:
                        ch.CurrentPosition = PositionTypes.Aggressive;
                        break;
                    case StyleTypes.Berserk:
                        ch.CurrentPosition = PositionTypes.Berserk;
                        break;
                    default:
                        ch.CurrentPosition = PositionTypes.Fighting;
                        break;
                }
            }

            victim.NumberFighting++;
            if (victim.Switched != null && victim.Switched.IsAffected(AffectedByTypes.Possess))
            {
                color.send_to_char("You are disturbed!\r\n", victim.Switched);
                Return.do_return(victim.Switched, "");
            }
        }

        public static CharacterInstance who_fighting(CharacterInstance ch)
        {
            return ch == null || ch.CurrentFighting == null ? null : ch.CurrentFighting.Who;
        }

        public static void free_fight(CharacterInstance ch)
        {
            if (ch.CurrentFighting != null)
            {
                if (ch.CurrentFighting.Who.CharDied())
                    --ch.CurrentFighting.Who.NumberFighting;
            }

            ch.CurrentFighting = null;
            ch.CurrentPosition = ch.CurrentMount != null
                ? PositionTypes.Mounted
                : PositionTypes.Standing;

            // Berserk wears off after combat
            if (ch.IsAffected(AffectedByTypes.Berserk))
            {
                // TODO affect_strip
                color.set_char_color(ATTypes.AT_WEAROFF, ch);
                color.send_to_char(DatabaseManager.Instance.GetEntity<SkillData>("berserk").WearOffMessage, ch);
                color.send_to_char("\r\n", ch);
            }
        }

        public static void stop_fighting(CharacterInstance ch, bool both)
        {
            free_fight(ch);
            update_pos(ch);

            if (!both)
                return;

            foreach (CharacterInstance fch in DatabaseManager.Instance.CHARACTERS.CastAs<Repository<long, CharacterInstance>>().Values.Where(fch => who_fighting(fch) == ch))
            {
                free_fight(fch);
                update_pos(fch);
            }
        }

        private static readonly List<string> DeathCries = new List<string>()
            {
                "You hear $n's death cry.",
                "$n screams furiously as $e falls to the ground in a heap!",
                "$n hits the ground ... DEAD.",
                "$n catches $s guts in $s hands as they pour through $s fatal wound!",
                "$n splatters blood on your armor.",
                "$n gasps $s last breath and blood spurts out of $s mouth and ears.",
                "You hear something's death cry.",
                "You hear someone's death cry."
            };

        public static void death_cry(CharacterInstance ch)
        {
            string msg = string.Empty;
            int vnum = 0;

            int random = SmaugRandom.Between(0, 5);
            if (random >= 0 && random <= 4)
                msg = DeathCries[random + 1];
            else if (random == 5)
            {
                int shift = SmaugRandom.Between(0, 31);
                int cindex = 1 << shift;

                for (int i = 0; i < 32 && ch.ExtraFlags > 0; i++)
                {
                    if (ch.HasBodyPart(cindex))
                    {
                        msg = LookupManager.Instance.GetLookup("PartMessages", shift);
                        vnum = GameConstants.PartVnums[shift];
                        break;
                    }
                    shift = SmaugRandom.Between(0, 31);
                    cindex = 1 << shift;
                }

                if (msg.IsNullOrEmpty())
                    msg = DeathCries[0];
            }
            else
                msg = DeathCries[0];

            comm.act(ATTypes.AT_CARNAGE, msg, ch, null, null, ToTypes.Room);

            if (vnum > 0)
            {
                ObjectTemplate template = DatabaseManager.Instance.OBJECT_INDEXES.CastAs<Repository<long, ObjectTemplate>>().Get(vnum);
                if (template == null)
                {
                    LogManager.Instance.Bug("Invalid vnum");
                    return;
                }

                string name = ch.IsNpc() ? ch.ShortDescription : ch.Name;
                ObjectInstance obj = DatabaseManager.Instance.OBJECTS.Create(template, 0);
                obj.Timer = SmaugRandom.Between(4, 7);
                if (ch.IsAffected(AffectedByTypes.Poison))
                    obj.Value[3] = 10;

                obj.ShortDescription = string.Format(obj.ShortDescription, name);
                obj.Description = string.Format(obj.Description, name);
                ch.CurrentRoom.ToRoom(obj);
            }

            msg = ch.IsNpc()
                      ? DeathCries[6]
                      : DeathCries[7];

            RoomTemplate prevRoom = ch.CurrentRoom;
            foreach (ExitData exit in prevRoom.Exits.Where(exit => exit.GetDestination() != null && exit.GetDestination() != prevRoom))
            {
                ch.CurrentRoom = exit.GetDestination();
                comm.act(ATTypes.AT_CARNAGE, msg, ch, null, null, ToTypes.Room);
            }

            ch.CurrentRoom = prevRoom;
        }

        public static ObjectInstance raw_kill(CharacterInstance ch, CharacterInstance victim)
        {
            if (victim.IsNotAuthorized())
            {
                LogManager.Instance.Bug("Killing unauthorized");
                return null;
            }

            stop_fighting(victim, true);

            if (victim.CurrentMorph != null)
            {
                polymorph.do_unmorph_char(victim);
                return raw_kill(ch, victim);
            }

            mud_prog.mprog_death_trigger(ch, victim);
            if (victim.CharDied())
                return null;

            mud_prog.rprog_death_trigger(victim);
            if (victim.CharDied())
                return null;

            ObjectInstance corpse = ObjectFactory.CreateCorpse(victim, ch);
            if (victim.CurrentRoom.SectorType == SectorTypes.OceanFloor
                || victim.CurrentRoom.SectorType == SectorTypes.Underwater
                || victim.CurrentRoom.SectorType == SectorTypes.ShallowWater
                || victim.CurrentRoom.SectorType == SectorTypes.DeepWater)
                comm.act(ATTypes.AT_BLOOD, "$n's blood slowly clouds the surrounding water.", victim, null, null, ToTypes.Room);
            else if (victim.CurrentRoom.SectorType == SectorTypes.Air)
                comm.act(ATTypes.AT_BLOOD, "$n's blood sprays wildly through the air.", victim, null, null, ToTypes.Room);
            else
                ObjectFactory.CreateBlood(victim);

            if (victim.IsNpc())
            {
                victim.MobIndex.TimesKilled++;
                handler.extract_char(victim, true);
                victim = null;
                return corpse;
            }

            color.set_char_color(ATTypes.AT_DIEMSG, victim);
            Help.do_help(victim,
                         victim.PlayerData.PvEDeaths + victim.PlayerData.PvPDeaths < 3 ? "new_death" : "_DIEMSG_");

            handler.extract_char(victim, false);

            while (victim.Affects.Count > 0)
                victim.RemoveAffect(victim.Affects.First());

            // TODO: Finish reset of victim

            return null;
        }

        public static void group_gain(CharacterInstance ch, CharacterInstance victim)
        {
            //// Monsters don't get kill XPs or alignment changes
            //// Dying of mortal wounds or poison doesn't give xp to anyone!
            if (ch.IsNpc() || victim == ch)
                return;

            int members = ch.CurrentRoom.Persons.Count(vch => vch.IsSameGroup(ch));
            if (members == 0)
                members = 1;

            CharacterInstance leader = ch.Leader ?? ch;

            foreach (CharacterInstance gch in ch.CurrentRoom.Persons.Where(gch => gch.IsSameGroup(ch)))
            {
                if (gch.Level - leader.Level > 8)
                {
                    color.send_to_char("You are too high for this group!", gch);
                    continue;
                }
                if (gch.Level - leader.Level < -8)
                {
                    color.send_to_char("You are too low for this group!", gch);
                    continue;
                }

                int xp = (int)(xp_compute(gch, victim) * 0.1765) / members;
                if (gch.CurrentFighting == null)
                    xp /= 2;

                gch.CurrentAlignment = align_compute(gch, victim);
                if (xp > 0)
                {
                    color.ch_printf(gch, "You receive {0} experience points.", xp);
                    gch.GainXP(xp);
                }

                EvaluateEquipmentAfterXPGain(gch);
            }
        }

        private static void EvaluateEquipmentAfterXPGain(CharacterInstance gch)
        {
            foreach (
                ObjectInstance obj in
                    gch.Carrying.Where(obj => obj.WearLocation != WearLocations.None)
                       .Where(obj => (Macros.IS_OBJ_STAT(obj, (int) ItemExtraFlags.AntiEvil) && gch.IsEvil())
                                     || (Macros.IS_OBJ_STAT(obj, (int) ItemExtraFlags.AntiGood) && gch.IsGood())
                                     || (Macros.IS_OBJ_STAT(obj, (int) ItemExtraFlags.AntiNeutral) && gch.IsNeutral())))
            {
                comm.act(ATTypes.AT_MAGIC, "You are zapped by $p.", gch, obj, null, ToTypes.Character);
                comm.act(ATTypes.AT_MAGIC, "$n is zapped by $p.", gch, obj, null, ToTypes.Room);

                obj.FromCharacter();
                ObjectInstance newObj = gch.CurrentRoom.ToRoom(obj);
                mud_prog.oprog_zap_trigger(gch, newObj);
                if (gch.CharDied())
                    return;
            }
        }

        public static int align_compute(CharacterInstance ch, CharacterInstance victim)
        {
            int align = ch.CurrentAlignment - victim.CurrentAlignment;
            int divalign = (ch.CurrentAlignment > -350 && ch.CurrentAlignment < 350) ? 4 : 20;
            int newAlign = 0;
            
            if (align > 500)
                newAlign = (ch.CurrentAlignment + (align - 500) / divalign).GetLowestOfTwoNumbers(1000);
            else if (align < -500)
                newAlign = (ch.CurrentAlignment + (align + 500)/divalign).GetHighestOfTwoNumbers(-1000);
            else
                newAlign = ch.CurrentAlignment - ch.CurrentAlignment/divalign;

            return newAlign;
        }

        public static int xp_compute(CharacterInstance ch, CharacterInstance victim)
        {
            int xp = (victim.GetExperienceWorth()*0.GetNumberThatIsBetween((victim.Level - ch.Level) + 10, 13))/10;
            int align = ch.CurrentAlignment - victim.CurrentAlignment;

            if (align > 990 || align < -990)
                xp = (xp * 5) >> 2; //// bonus for attacking opposite alignment
            else if (ch.CurrentAlignment > 300 && align < 250)
                xp = (xp*3) >> 2;   //// penalty for good player attacking same alignment
            
            xp = SmaugRandom.Between((xp*3) >> 2, (xp*5) >> 2);

            //// get 1/4 exp for players
            if (!victim.IsNpc())
                xp /= 4;
            else
            {
                //// reduce xp for killing the same mob repeatedly
                if (!ch.IsNpc())
                {
                    int times = ch.TimesKilled(victim);
                    if (times >= 20)
                        xp = 0;
                    else if (times > 0)
                    {
                        xp = (xp*(20 - times))/20;
                        if (times > 15)
                            xp /= 3;
                        else if (times > 10)
                            xp >>= 1;
                    }
                }
            }

            //// semi-intelligent experienced player vs. novice player xp gain
            //// "bell curve"ish xp mod based on time played vs. level
            if (!ch.IsNpc() && ch.Level > 5)
            {
                int xpRatio = (int)ch.played / ch.Level;
                if (xpRatio > 20000)
                    xp = (xp * 5) >> 2;     //// 5/4
                else if (xpRatio > 16000)
                    xp = (xp * 3) >> 2;     //// 3/4
                else if (xpRatio > 10000)  
                    xp >>= 1;               //// 1/2
                else if (xpRatio > 5000)
                    xp >>= 2;               //// 1/4th
                else if (xpRatio > 3500)
                    xp >>= 3;               //// 1/8th
                else if (xpRatio > 2000)
                    xp >>= 4;               //// 1/16th
            }

            //// Level based experience gain cap.  Cannot get more experience for
            //// a kill than the amount for your current experience level
            return 0.GetNumberThatIsBetween(xp, ch.GetExperienceLevel(ch.Level + 1) - ch.GetExperienceLevel(ch.Level));
        }

        public static void new_dam_message(CharacterInstance ch, CharacterInstance victim, int dam, int dt, ObjectInstance obj)
        {
            // TODO
        }

        public static void dam_message(CharacterInstance ch, CharacterInstance victim, int dam, int dt)
        {
            new_dam_message(ch, victim, dam, dt, null);
        }

        public static bool check_illegal_pk(CharacterInstance ch, CharacterInstance victim)
        {
            if (!victim.IsNpc() && !ch.IsNpc())
            {
                if ((victim.PlayerData.Flags.IsSet((int) PCFlags.Deadly)
                     || (ch.Level - victim.Level) > 10
                     || ch.PlayerData.Flags.IsSet((int) PCFlags.Deadly))
                    && !ch.IsInArena()
                    && ch != victim
                    && !(ch.IsImmortal() && victim.IsImmortal()))
                {
                    string buffer = string.Format("&p{0} on {1} in &W***&rILLEGAL PKILL&W*** &pattempt at {2}",
                                                  ch.LastCommand, victim.Name, victim.CurrentRoom.Vnum);
                    // TODO: last_pkroom = victim.CurrentRoom.vnum;
                    // TODO: log_string(buffer);
                    // TODO: to_channel(buffer, CHANNEL_MONITOR, "Monitor", LEVEL_IMMORTAL);
                    return true;
                }
            }
            return false;
        }
    }
}
