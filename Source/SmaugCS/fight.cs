using System;
using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common;
using Realm.Library.Patterns.Repository;
using SmaugCS.Commands.Admin;
using SmaugCS.Commands.PetsAndGroups;
using SmaugCS.Commands.Skills;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Exceptions;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Organizations;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Objects;
using SmaugCS.Extensions.Player;
using SmaugCS.Logging;
using SmaugCS.Managers;
using SmaugCS.MudProgs;
using SmaugCS.Repository;
using SmaugCS.Spells.Smaug;
using EnumerationExtensions = Realm.Library.Common.EnumerationExtensions;

namespace SmaugCS
{
    public static class fight
    {
        public static ObjectInstance UsedWeapon { get; set; }

        public static bool loot_coins_from_corpse(CharacterInstance ch, ObjectInstance corpse)
        {
            var oldgold = ch.CurrentCoin;

            foreach (var content in corpse.Contents
                .Where(x => x.ItemType == ItemTypes.Money)
                .Where(ch.CanSee)
                .Where(
                    x =>
                        x.WearFlags.IsSet(ItemWearFlags.Take) &&
                        ch.Level <
                        GameManager.Instance.SystemData.GetMinimumLevel(PlayerPermissionTypes.LevelGetObjectNoTake))
                .Where(x => x.ExtraFlags.IsSet(ItemExtraFlags.Prototype) && ch.CanTakePrototype()))
            {
                comm.act(ATTypes.AT_ACTION, "You get $p from $P", ch, content, corpse, ToTypes.Character);
                comm.act(ATTypes.AT_ACTION, "$n gets $p from $P", ch, content, corpse, ToTypes.Room);
                content.InObject.RemoveFrom(content);
                ch.CheckObjectForTrap(content, TrapTriggerTypes.Get);
                if (ch.CharDied())
                    return false;

                MudProgHandler.ExecuteObjectProg(MudProgTypes.Get, ch, content);
                if (ch.CharDied())
                    return false;

                ch.CurrentCoin += content.Value.ToList()[0] * content.Count;
                content.Extract();
            }

            if (ch.CurrentCoin - oldgold > 1 && ch.CurrentPosition > PositionTypes.Sleeping)
                Split.do_split(ch, $"{ch.CurrentCoin - oldgold}");

            return true;
        }

        private static int Pulse { get; set; }

        public static void violence_update()
        {
            Pulse = (Pulse + 1)%100;

            foreach (
                var ch in
                    RepositoryManager.Instance.CHARACTERS.CastAs<Repository<long, CharacterInstance>>().Values
                        .Where(ch => !ch.CharDied()))
            {
                //// Experience gained during battle decreases as the battle drags on
                if (ch.CurrentFighting != null &&
                    (++ch.CurrentFighting.Duration%24) == 0)
                    ch.CurrentFighting.Experience = ((ch.CurrentFighting.Experience*9)/10);

                foreach (var timer in ch.Timers)
                    DecreaseExperienceInBattle(timer, (PlayerInstance)ch);

                if (ch.CharDied())
                    continue;

                //// Spells that have durations less than 1 hour
                var enumerator = ch.Affects.GetEnumerator();
                AffectData paf = null;
                while (enumerator.MoveNext())
                {
                    var pafNext = paf;
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
                            var skill = RepositoryManager.Instance.GetEntity<SkillData>((int) paf.Type);
                            if (paf.Type > 0 && !string.IsNullOrEmpty(skill?.WearOffMessage))
                            {
                               ch.SetColor(ATTypes.AT_WEAROFF);
                               ch.SendTo(skill.WearOffMessage);
                               ch.SendTo("\r\n");
                            }
                        }

                        if ((int) paf.Type == (int) RepositoryManager.Instance.GetEntity<SkillData>("possess").ID)
                        {
                            var pch = (PlayerInstance) ch;
                            pch.Descriptor.Character = pch.Descriptor.Original;
                            pch.Descriptor.Original = null;
                            pch.Descriptor.Character.Descriptor = pch.Descriptor;
                            pch.Descriptor.Character.Switched = null;
                            pch.Descriptor = null;
                        }
                        ch.RemoveAffect(paf);
                    }
                }

                if (ch.CharDied())
                    continue;

                //// Check exits for moving players around
                var retCode = act_move.pullcheck(ch, Pulse);
                if (retCode == ReturnTypes.CharacterDied || ch.CharDied())
                    continue;

                //// Let the battle begin
                var victim = ch.GetMyTarget();
                if (victim == null || ch.IsAffected(AffectedByTypes.Paralysis))
                    continue;

                retCode = ReturnTypes.None;
                if (ch.CurrentRoom.Flags.IsSet(RoomFlags.Safe))
                {
                    LogManager.Instance.Info("{0} fighting {1} in a SAFE room.", ch.Name, victim.Name);
                    ch.StopFighting(true);
                }
                else if (ch.IsAwake() && ch.CurrentRoom == victim.CurrentRoom)
                    retCode = multi_hit(ch, victim, Program.TYPE_UNDEFINED);
                else
                    ch.StopFighting(false);

                if (ch.CharDied())
                    continue;

                if (retCode == ReturnTypes.CharacterDied)
                    continue;

                victim = ch.GetMyTarget();
                if (victim == null)
                    continue;

                //// Mob triggers
                MudProgHandler.ExecuteRoomProg(MudProgTypes.Fight, ch);
                if (ch.CharDied() || victim.CharDied())
                    continue;
                MudProgHandler.ExecuteMobileProg(MudProgTypes.HitPercent, ch, victim);
                if (ch.CharDied() || victim.CharDied())
                    continue;
                MudProgHandler.ExecuteMobileProg(MudProgTypes.Fight, ch, victim);
                if (ch.CharDied() || victim.CharDied())
                    continue;

                //// NPC Special attack flags
                if (!ch.IsNpc()) continue;

                var mch = (MobileInstance) ch;
                if (mch.Attacks.IsEmpty()) continue;
                if (30 + (ch.Level/4) < SmaugRandom.D100()) continue;

                var cnt = 0;
                int attacktype;
                for (;;)
                {
                    if (cnt++ > 10)
                    {
                        attacktype = -1;
                        break;
                    }
                    attacktype = SmaugRandom.Between(7,
                        EnumerationFunctions.MaximumEnumValue<AttackTypes>() - 1);
                    if (mch.Attacks.IsSet(attacktype))
                        break;
                }

                var atkType =
                    EnumerationExtensions.GetEnum<AttackTypes>(attacktype);
                switch (atkType)
                {
                    case AttackTypes.Bash:
                        Bash.do_bash(ch, "");
                        break;
                    case AttackTypes.Stun:
                        Stun.do_stun(ch, "");
                        break;
                    case AttackTypes.Gouge:
                        Gouge.do_gouge(ch, "");
                        break;
                    case AttackTypes.Feed:
                        Feed.do_feed(ch, "");
                        break;
                    case AttackTypes.Drain:
                        break;
                }
            }
        }

        private static void DecreaseExperienceInBattle(TimerData timer, PlayerInstance ch)
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
                    var tempsub = ch.SubState;
                    ch.SubState = EnumerationExtensions.GetEnum<CharacterSubStates>(timer.Value);
                    timer.Action.Value.Invoke(ch, string.Empty);
                    if (ch.CharDied())
                        break;
                    ch.SubState = tempsub;
                    break;
            }
            ch.RemoveTimer(timer);
        }

        public static ReturnTypes multi_hit(CharacterInstance ch, CharacterInstance victim, int dt)
        {
            //// Add a timer to pkillers
            if (!ch.IsNpc() && !victim.IsNpc())
            {
                if (ch.Act.IsSet(PlayerFlags.Nice))
                    return ReturnTypes.None;

                ch.AddTimer(TimerTypes.RecentFight, 11);
                victim.AddTimer(TimerTypes.RecentFight, 11);
            }

            if (ch.IsAttackSuppressed())
                return ReturnTypes.None;

            if (ch.IsNpc() && ch.Act.IsSet(ActFlags.NoAttack))
                return ReturnTypes.None;

            var retcode = one_hit(ch, victim, dt);
            if (retcode != ReturnTypes.None)
                return retcode;

            var backstab = RepositoryManager.Instance.GetEntity<SkillData>("backstab");
            var circle = RepositoryManager.Instance.GetEntity<SkillData>("circle");

            if (ch.GetMyTarget() != victim || dt == backstab.ID || dt == circle.ID)
                return ReturnTypes.None;

            var berserk = RepositoryManager.Instance.GetEntity<SkillData>("berserk");
            var chance = ch.IsNpc() ? 100 : (Macros.LEARNED(ch, (int) berserk.ID)*5/2);
            if (ch.IsAffected(AffectedByTypes.Berserk) && SmaugRandom.D100() < chance)
            {
                retcode = one_hit(ch, victim, dt);
                if (retcode != ReturnTypes.None || ch.GetMyTarget() != victim)
                    return retcode;
            }

            var dualBonus = 0;
            if (ch.GetEquippedItem(WearLocations.DualWield) != null)
            {
                var dualWield = RepositoryManager.Instance.GetEntity<SkillData>("dual wield");
                dualBonus = ch.IsNpc() ? ch.Level/10 : (int) Macros.LEARNED(ch, (int) dualWield.ID)/10;
                chance = ch.IsNpc() ? ch.Level : Macros.LEARNED(ch, (int) dualWield.ID);

                if (SmaugRandom.D100() < chance)
                {
                    dualWield.LearnFromSuccess(ch);
                    retcode = one_hit(ch, victim, dt);
                    if (retcode != ReturnTypes.None || ch.GetMyTarget() != victim)
                        return retcode;
                }
                else
                    dualWield.LearnFromFailure(ch);
            }

            if (ch.CurrentMovement < 10)
                dualBonus = -20;

            if (ch.IsNpc() && ch.NumberOfAttacks > 0)
            {
                for (var i = 0; i < ch.NumberOfAttacks; i++)
                {
                    retcode = one_hit(ch, victim, dt);
                    if (retcode != ReturnTypes.None && ch.GetMyTarget() != victim)
                        return retcode;
                }
                return retcode;
            }

            var secondAttack = RepositoryManager.Instance.GetEntity<SkillData>("second attack");
            chance = ch.IsNpc() ? ch.Level : (int)((Macros.LEARNED(ch, (int) secondAttack.ID) + dualBonus)/1.5f);
            if (SmaugRandom.D100() < chance)
            {
                secondAttack.LearnFromSuccess(ch);
                retcode = one_hit(ch, victim, dt);
                if (retcode != ReturnTypes.None && ch.GetMyTarget() != victim)
                    return retcode;
            } 
            else 
                secondAttack.LearnFromFailure(ch);

            var thirdAttack = RepositoryManager.Instance.GetEntity<SkillData>("third attack");
            chance = ch.IsNpc() ? ch.Level : (int)((Macros.LEARNED(ch, (int)thirdAttack.ID) + (dualBonus*1.5f)) / 2);
            if (SmaugRandom.D100() < chance)
            {
                thirdAttack.LearnFromSuccess(ch);
                retcode = one_hit(ch, victim, dt);
                if (retcode != ReturnTypes.None && ch.GetMyTarget() != victim)
                    return retcode;
            }
            else
                thirdAttack.LearnFromFailure(ch);

            var fourthAttack = RepositoryManager.Instance.GetEntity<SkillData>("fourth attack");
            chance = ch.IsNpc() ? ch.Level : (Macros.LEARNED(ch, (int)fourthAttack.ID) + (dualBonus*2)) / 3;
            if (SmaugRandom.D100() < chance)
            {
                fourthAttack.LearnFromSuccess(ch);
                retcode = one_hit(ch, victim, dt);
                if (retcode != ReturnTypes.None && ch.GetMyTarget() != victim)
                    return retcode;
            }
            else
                fourthAttack.LearnFromFailure(ch);

            var fifthAttack = RepositoryManager.Instance.GetEntity<SkillData>("fifth attack");
            chance = ch.IsNpc() ? ch.Level : (Macros.LEARNED(ch, (int)fifthAttack.ID) + (dualBonus*3)) / 4;
            if (SmaugRandom.D100() < chance)
            {
                fifthAttack.LearnFromSuccess(ch);
                retcode = one_hit(ch, victim, dt);
                if (retcode != ReturnTypes.None && ch.GetMyTarget() != victim)
                    return retcode;
            }
            else
                fifthAttack.LearnFromFailure(ch);

            retcode = ReturnTypes.None;

            chance = ch.IsNpc() ? ch.Level/2 : 0;
            if (SmaugRandom.D100() < chance)
                retcode = one_hit(ch, victim, dt);

            if (retcode == ReturnTypes.None)
            {
                int move;
                if (!ch.IsAffected(AffectedByTypes.Flying)
                    && !ch.IsAffected(AffectedByTypes.Floating))
                {
                    var attribute = ch.CurrentRoom.SectorType.GetAttribute<MovementLossAttribute>();
                    move = ch.GetEncumberedMove(attribute?.ModValue ?? 1);
                }
                else
                    move = ch.GetEncumberedMove(1);

                if (ch.CurrentMovement > 0)
                    ch.CurrentMovement = 0.GetHighestOfTwoNumbers(ch.CurrentMovement - move);
            }

            return retcode;
        }

        public static Tuple<int, int> weapon_prof_bonus_check(CharacterInstance ch, ObjectInstance wield)
        {
            if (ch.IsNpc() && ch.Level <= 5 && wield == null)
                return new Tuple<int, int>(0, -1);

            var bonus = 0;

            var damageType = EnumerationExtensions.GetEnum<DamageTypes>(wield.Value.ToList()[3]);
            var attrib = damageType.GetAttribute<LookupSkillAttribute>();
            var sn = (int) RepositoryManager.Instance.GetEntity<SkillData>(attrib.Skill).ID;

            if (sn != -1)
                bonus = ((int)Macros.LEARNED(ch, sn) - 50)/10;

            if (ch.IsDevoted())
                bonus -= ((PlayerInstance)ch).PlayerData.Favor / -400;

            return new Tuple<int, int>(bonus, sn);
        }

        public static int off_shld_lvl(CharacterInstance ch, CharacterInstance victim)
        {
            int lvl;

            if (!ch.IsNpc())
            {
                lvl = 1.GetHighestOfTwoNumbers(ch.Level - 10/2);
                if (SmaugRandom.D100() + (victim.Level - lvl) >= 40) return 0;

                if (ch.CanPKill() && victim.CanPKill())
                    return ch.Level;
                return lvl;
            }

            lvl = ch.Level/2;
            return SmaugRandom.D100() + (victim.Level - lvl) < 70 ? lvl : 0;
        }

        private static bool DualFlip = false;

        public static ReturnTypes one_hit(CharacterInstance ch, CharacterInstance victim, int dt)
        {
            var damageType = dt;
            var retcode = ReturnTypes.None;

            // Can't beat a dead character and guard against room-leavings
            if (victim.CurrentPosition == PositionTypes.Dead
                || ch.CurrentRoom != victim.CurrentRoom)
                return ReturnTypes.CharacterDied;

            // Figure out the weapon doing the damage
            var wield = ch.GetEquippedItem(WearLocations.DualWield);
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

            var proficiencyBonus = profBonus?.Item1 ?? 0;
            var sn = profBonus?.Item2 ?? -1;

            if (ch.CurrentFighting != null
                && damageType == Program.TYPE_UNDEFINED
                && ch.IsNpc()
                && !((MobileInstance)ch).Attacks.IsEmpty())
            {
                var cnt = 0;
                var attacktype = 0;
                for (;;)
                {
                    attacktype = SmaugRandom.Between(0, 6);
                    if (((MobileInstance)ch).Attacks.IsSet(attacktype))
                        break;
                    if (cnt++ > 16)
                    {
                        attacktype = -1;
                        break;
                    }
                }

                if (attacktype == (int) AttackTypes.Backstab)
                    attacktype = -1;
                if (wield != null && (SmaugRandom.D100() > 25))
                    attacktype = -1;
                if (wield == null && (SmaugRandom.D100() > 50))
                    attacktype = -1;

                retcode = ReturnTypes.None;
                switch (attacktype)
                {
                    case (int) AttackTypes.Bite:
                        Bite.do_bite(ch, "");
                        retcode = handler.GlobalReturnCode;
                        break;
                    case (int) AttackTypes.Claws:
                        Claw.do_claw(ch, "");
                        retcode = handler.GlobalReturnCode;
                        break;
                    case (int) AttackTypes.Tail:
                        Tail.do_tail(ch, "");
                        retcode = handler.GlobalReturnCode;
                        break;
                    case (int) AttackTypes.Sting:
                        Sting.do_sting(ch, "");
                        retcode = handler.GlobalReturnCode;
                        break;
                    case (int) AttackTypes.Punch:
                        Punch.do_punch(ch, "");
                        retcode = handler.GlobalReturnCode;
                        break;
                    case (int) AttackTypes.Kick:
                        Kick.do_kick(ch, "");
                        retcode = handler.GlobalReturnCode;
                        break;
                    case (int) AttackTypes.Trip:
                        attacktype = 0;
                        break;
                }

                if (attacktype >= 0)
                    return retcode;
            }

            if (damageType == Program.TYPE_UNDEFINED)
            {
                damageType = (int) DamageTypes.Hit;
                if (wield != null && wield.ItemType == ItemTypes.Weapon)
                    damageType += wield.Value.ToList()[3];
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
                thac0_00 = RepositoryManager.Instance.GetClass(ch.CurrentClass).ToHitArmorClass0;
                thac0_32 = RepositoryManager.Instance.GetClass(ch.CurrentClass).ToHitArmorClass32;
            }
            thac0_00 = ch.Level.Interpolate(thac0_00, thac0_32) - ch.GetHitroll();
            var victimArmorClass = -19.GetHighestOfTwoNumbers(victim.GetArmorClass()/10);

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
                    var intDiff = ch.GetCurrentIntelligence() - victim.GetCurrentIntelligence();
                    if (intDiff != 0)
                        victimArmorClass += (intDiff*ch.CurrentFighting.TimesKilled)/10;
                }
            }

            // Weapon proficiency bonus
            victimArmorClass += proficiencyBonus;

            int diceroll;
            while ((diceroll = SmaugRandom.Bits(5)) >= 20)
                ;

            if (diceroll == 0 || (diceroll != 19 && diceroll < thac0_00 - victimArmorClass))
                return OneHitMiss(ch, victim, sn, damageType);

            var damage = wield == null
                ? SmaugRandom.Roll(ch.BareDice.NumberOf, ch.BareDice.SizeOf) + ch.DamageRoll.Bonus
                : SmaugRandom.Between(wield.Value.ToList()[1], wield.Value.ToList()[2]);

            // Bonuses
            damage += ch.GetDamroll();
            if (proficiencyBonus > 0)
                damage += proficiencyBonus/4;

            damage = ModifyDamageByFightingStyle(victim, damage);
            damage = ModifyDamageByFightingStyle(ch, damage);
            damage = CalculateEnhancedDamage(ch, damage);

            if (!victim.IsAwake())
                damage *= 2;
            if (dt == (int) RepositoryManager.Instance.GetEntity<SkillData>("backstab").ID)
                damage *= (2 + (ch.Level - (victim.Level/4)).GetNumberThatIsBetween(2, 30)/8);
            if (dt == (int) RepositoryManager.Instance.GetEntity<SkillData>("circle").ID)
                damage *= (2 + (ch.Level - (victim.Level/4)).GetNumberThatIsBetween(2, 30)/16);
            if (damage <= 0)
                damage = 1;

            damage = ModifyDamageForResistancesAndVulnerabilities(victim, wield, damage);

            if (sn != -1)
            {
                var sk = RepositoryManager.Instance.SKILLS.Get(sn);
                if (damage > 0)
                    sk.LearnFromSuccess(ch);
                else 
                    sk.LearnFromFailure(ch);
            }

            if (damage == -1)
            {
                var skill = RepositoryManager.Instance.GetEntity<SkillData>(dt);
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

            retcode = ch.CauseDamageTo(victim, damage, dt);

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
            if (wield != null && !victim.Immunity.IsSet(ResistanceTypes.Magic)
                && !victim.CurrentRoom.Flags.IsSet(RoomFlags.NoMagic))
            {
                foreach (var aff in wield.ObjectIndex.Affects)
                {
                    if (aff.Location == ApplyTypes.WeaponSpell
                        && Macros.IS_VALID_SN(aff.Modifier)
                        && RepositoryManager.Instance.GetEntity<SkillData>(aff.Modifier).SpellFunction != null)
                        retcode = RepositoryManager.Instance.GetEntity<SkillData>(aff.Modifier)
                            .SpellFunction.Value.Invoke(aff.Modifier, (wield.Level + 3)/3, ch, victim);
                }

                if (retcode == ReturnTypes.SpellFailed)
                    return ReturnTypes.None;
                if (retcode != ReturnTypes.None && (ch.CharDied() || victim.CharDied()))
                    return retcode;

                foreach (var aff in wield.Affects)
                {
                    if (aff.Location == ApplyTypes.WeaponSpell
                        && Macros.IS_VALID_SN(aff.Modifier)
                        && RepositoryManager.Instance.GetEntity<SkillData>(aff.Modifier).SpellFunction != null)
                        retcode = RepositoryManager.Instance.GetEntity<SkillData>(aff.Modifier)
                            .SpellFunction.Value.Invoke(aff.Modifier, (wield.Level + 3)/3, ch, victim);
                }

                if (retcode == ReturnTypes.SpellFailed)
                    return ReturnTypes.None;
                if (retcode != ReturnTypes.None && (ch.CharDied() || victim.CharDied()))
                    return retcode;
            }

            // Magic shields that retaliate
            if (victim.IsAffected(AffectedByTypes.FireShield)
                && !ch.IsAffected(AffectedByTypes.FireShield))
                retcode = Smaug.spell_smaug((int) RepositoryManager.Instance.GetEntity<SkillData>("flare").ID,
                    off_shld_lvl(victim, ch), victim, ch);
            if (retcode != ReturnTypes.None || ch.CharDied() || victim.CharDied())
                return retcode;

            if (victim.IsAffected(AffectedByTypes.IceShield)
                && !ch.IsAffected(AffectedByTypes.IceShield))
                retcode = Smaug.spell_smaug((int) RepositoryManager.Instance.GetEntity<SkillData>("iceshard").ID,
                    off_shld_lvl(victim, ch), victim, ch);
            if (retcode != ReturnTypes.None || ch.CharDied() || victim.CharDied())
                return retcode;

            if (victim.IsAffected(AffectedByTypes.ShockShield)
                && !ch.IsAffected(AffectedByTypes.ShockShield))
                retcode = Smaug.spell_smaug((int) RepositoryManager.Instance.GetEntity<SkillData>("torrent").ID,
                    off_shld_lvl(victim, ch), victim, ch);
            if (retcode != ReturnTypes.None || ch.CharDied() || victim.CharDied())
                return retcode;

            if (victim.IsAffected(AffectedByTypes.AcidMist)
                && !ch.IsAffected(AffectedByTypes.AcidMist))
                retcode = Smaug.spell_smaug((int) RepositoryManager.Instance.GetEntity<SkillData>("acidshot").ID,
                    off_shld_lvl(victim, ch), victim, ch);
            if (retcode != ReturnTypes.None || ch.CharDied() || victim.CharDied())
                return retcode;

            if (victim.IsAffected(AffectedByTypes.VenomShield)
                && !ch.IsAffected(AffectedByTypes.VenomShield))
                retcode = Smaug.spell_smaug((int) RepositoryManager.Instance.GetEntity<SkillData>("venomshot").ID,
                    off_shld_lvl(victim, ch), victim, ch);
            if (retcode != ReturnTypes.None || ch.CharDied() || victim.CharDied())
                return retcode;

            // TODO tail_chain?
            return 0;
        }

        private static int ModifyDamageForResistancesAndVulnerabilities(CharacterInstance victim, ObjectInstance wield,
            int damage)
        {
            var plusRIS = 0;
            if (wield != null)
            {
                damage = wield.ExtraFlags.IsSet(ItemExtraFlags.Magical)
                    ? victim.ModifyDamageWithResistance(damage, ResistanceTypes.Magic)
                    : victim.ModifyDamageWithResistance(damage, ResistanceTypes.NonMagic);

                // Handle PLUS1 - PLUS6 ris bits vs weapon hitroll
                plusRIS = wield.HitRoll;
            }
            else
                damage = victim.ModifyDamageWithResistance(damage, ResistanceTypes.NonMagic);

            // Check for RIS_PLUS
            if (damage > 0)
            {
                if (plusRIS > 0)
                    plusRIS = (int) ResistanceTypes.Plus1 << plusRIS.GetLowestOfTwoNumbers(7);

                int imm = -1, res = -1, sus = 1;

                // find the high resistance
                for (var x = (int) ResistanceTypes.Plus1; x <= (int) ResistanceTypes.Plus6; x <<= 1)
                {
                    if (victim.Immunity.IsSet(x))
                        imm = x;
                    if (victim.Resistance.IsSet(x))
                        res = x;
                    if (victim.Susceptibility.IsSet(x))
                        sus = x;
                }

                var mod = 10;
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
                    damage = (damage*mod)/10;
            }
            return damage;
        }

        private static int CalculateEnhancedDamage(CharacterInstance ch, int damage)
        {
            var dmgSkill = RepositoryManager.Instance.GetEntity<SkillData>("enhanced damage");
            if (dmgSkill == null)
                throw new ObjectNotFoundException("Skill 'enhanced damage' not found");

            if (!ch.IsNpc() && ((PlayerInstance) ch).GetLearned((int) dmgSkill.ID) > 0)
            {
                damage += damage* (int)Macros.LEARNED(ch, (int) dmgSkill.ID);
                dmgSkill.LearnFromSuccess(ch);
            }
            return damage;
        }

        private static ReturnTypes OneHitMiss(CharacterInstance ch, CharacterInstance victim, int sn, int damageType)
        {
            var skill = RepositoryManager.Instance.SKILLS.Get(sn);
            if (skill == null)
                throw new ObjectNotFoundException($"Skill {sn} not found");

            skill.LearnFromFailure(ch);
            ch.CauseDamageTo(victim, 0, damageType);
            // TODO Tail_chain?
            return ReturnTypes.None;
        }

        public static ReturnTypes projectile_hit(CharacterInstance ch, CharacterInstance victim, ObjectInstance wield,
            ObjectInstance projectile, int dist)
        {
            if (projectile == null)
                return (int) ReturnTypes.None;

            var CalculatedBonus = CalculateProjectileBonus(projectile);
            var bonus = CalculatedBonus.Item1;
            var dt = CalculatedBonus.Item2;

            //// Can't beat a dead character
            if (victim.CurrentPosition == PositionTypes.Dead
                || victim.CharDied())
            {
                projectile.Extract();
                return ReturnTypes.CharacterDied;
            }

            var profBonus = wield != null
                ? weapon_prof_bonus_check(ch, wield)
                : new Tuple<int, int>(0, 0);

            if (dt == (int) SkillNumberTypes.Undefined)
            {
                dt = (int) SkillNumberTypes.Hit;
                if (wield != null && wield.ItemType == ItemTypes.MissileWeapon)
                    dt += wield.Value.ToList()[3];
            }

            //// Calculate to-hit-AC0 versus armor
            var thac0 = CalculateThac0(ch, dist);
            var thac0_0 = thac0.Item1;
            var thac0_32 = thac0.Item2;
            var victimArmorClass = CalculateArmorClass(ch, victim, projectile, bonus);

            var diceroll = SmaugRandom.D20();
            if (diceroll == 0 || (diceroll != 19 && diceroll < thac0_0 - victimArmorClass))
                return ProjectileMissed(ch, profBonus.Item2, projectile, victim, dt);
            return ProjectileHit(ch, victim, projectile, wield, bonus, profBonus.Item2, 0);
        }

        private static Tuple<int, int> CalculateProjectileBonus(ObjectInstance projectile)
        {
            int dt, bonus;
            if (projectile.ItemType == ItemTypes.Projectile
                || projectile.ItemType == ItemTypes.Weapon)
            {
                dt = (int)SkillNumberTypes.Hit + projectile.Value.ToList()[3];
                bonus = SmaugRandom.Between(projectile.Value.ToList()[1], projectile.Value.ToList()[2]);
            }
            else
            {
                dt = (int) SkillNumberTypes.Undefined;
                bonus = SmaugRandom.Between(1, 2.GetNumberThatIsBetween(projectile.GetWeight(), 100));
            }

            return new Tuple<int, int>(bonus, dt);
        }

        private static Tuple<int, int> CalculateThac0(CharacterInstance ch, int distance)
        {
            var thac0_0 = ch.ToHitArmorClass0;
            var thac0_32 = thac0_0;

            if (!ch.IsNpc())
            {
                thac0_0 = RepositoryManager.Instance.GetClass(ch.CurrentClass).ToHitArmorClass0;
                thac0_32 = RepositoryManager.Instance.GetClass(ch.CurrentClass).ToHitArmorClass32;
            }

            thac0_0 = ch.Level.Interpolate(thac0_0, thac0_32) - ch.GetHitroll() + (distance*2);

            return new Tuple<int, int>(thac0_0, thac0_32);
        }

        private static int CalculateArmorClass(CharacterInstance ch, CharacterInstance victim, ObjectInstance projectile,
            int bonus)
        {
            var victimArmorClass = -19.GetHighestOfTwoNumbers(victim.GetArmorClass()/10);

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
            if (proficiencySkillNumber > 0)
            {
                var skill = RepositoryManager.Instance.GetEntity<SkillData>(proficiencySkillNumber);
                skill.LearnFromFailure(ch);
            }

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

            ch.CauseDamageTo(victim, 0, dt);
            // TODO: tail_chain()
            return ReturnTypes.None;
        }

        private static ReturnTypes ProjectileHit(CharacterInstance ch, CharacterInstance victim,
            ObjectInstance projectile, ObjectInstance wield, int bonus, int proficiencySkillNumber, int dt)
        {
            var damage = wield == null ? bonus : SmaugRandom.Between(wield.Value.ToList()[1], wield.Value.ToList()[2]) + (bonus / 10);
            damage += ch.GetDamroll();
            if (bonus > 0)
                damage += bonus/4;
            damage = ModifyDamageByFightingStyle(victim, damage);

            if (!ch.IsNpc())
            {
                var skill = RepositoryManager.Instance.GetEntity<SkillData>("enhanced damage");
                if (((PlayerInstance)ch).PlayerData.Learned.ToList().FirstOrDefault(x => x == skill.ID) > 0)
                {
                    damage += damage* (int)Macros.LEARNED(ch, (int) skill.ID);
                    skill.LearnFromSuccess(ch);
                }
            }

            if (!victim.IsAwake())
                damage *= 2;
            if (damage <= 0)
                damage = 1;

            damage = projectile.ExtraFlags.IsSet(ItemExtraFlags.Magical)
                ? victim.ModifyDamageWithResistance(damage, ResistanceTypes.Magic)
                : victim.ModifyDamageWithResistance(damage, ResistanceTypes.NonMagic);

            var plusris = 0;

            //// Handle PLUS1 - PLUS6 ris bits vs weapon hitroll
            if (wield != null)
                plusris = wield.HitRoll;

            ResistanceTypes imm, res, sus;
            if (damage > 0)
            {
                // TODO MOdify damage by resistance/immunity/susceptibility
                // TODO Existing system is GARBAGE
            }

            if (proficiencySkillNumber > 0)
            {
                var skill = RepositoryManager.Instance.GetEntity<SkillData>(proficiencySkillNumber);
                if (damage > 0)
                    skill.LearnFromSuccess(ch);
                else 
                    skill.LearnFromFailure(ch);
            }

            // TODO Immune to Damage

            var retcode = ch.CauseDamageTo(victim, damage, dt);
            if (retcode != ReturnTypes.None)
            {
                projectile.Extract();
                return retcode;
            }

            if (ch.CharDied())
            {
                projectile.Extract();
                return ReturnTypes.CharacterDied;
            }

            if (victim.CharDied())
            {
                projectile.Extract();
                return ReturnTypes.VictimDied;
            }

            retcode = ReturnTypes.None;
            if (damage == 0)
            {
                if (SmaugRandom.D100() < 50)
                    projectile.Extract();
                else
                {
                    if (projectile.InObject != null)
                        projectile.RemoveFrom(projectile);
                    if (projectile.CarriedBy != null) 
                        projectile.RemoveFrom();
                    victim.CurrentRoom.AddTo(projectile);
                }
                return retcode;
            }

            // TODO Weapon sPells

            projectile.Extract();
            // TODO tail_chain
            return retcode;
        }

        private static int ModifyDamageByFightingStyle(CharacterInstance victim, int damage)
        {
            switch (victim.CurrentPosition)
            {
                case PositionTypes.Berserk:
                    return (int) (damage*1.2f);
                case PositionTypes.Aggressive:
                    return (int) (damage*1.1f);
                case PositionTypes.Defensive:
                    return (int) (damage*0.85f);
                case PositionTypes.Evasive:
                    return (int) (damage*0.8f);
            }
            return damage;
        }

        public static bool is_safe(CharacterInstance ch, CharacterInstance victim, bool show_messg)
        {
            if (ch.GetMyTarget() == ch)
                return false;

            if (victim.CurrentRoom.Flags.IsSet(RoomFlags.Safe))
            {
                if (show_messg)
                {
                   ch.SetColor(ATTypes.AT_MAGIC);
                   ch.SendTo("A magical force prevents you from attacking.");
                }
                return true;
            }

            if (ch.IsPacifist())
            {
                if (show_messg)
                {
                    ch.SetColor(ATTypes.AT_MAGIC);
                    ch.Printf("You are a pacifist and will not fight.");
                }
                return true;
            }

            if (victim.IsPacifist())
            {
                if (show_messg)
                {
                   ch.SetColor(ATTypes.AT_MAGIC);
                   ch.SendTo($"{victim.ShortDescription.CapitalizeFirst()} is a pacifist and will not fight.\r\n");
                }
                return true;
            }

            if (!ch.IsNpc() && ch.Level >= LevelConstants.ImmortalLevel)
                return false;

            if (!ch.IsNpc() && !victim.IsNpc() && ch != victim
                && victim.CurrentRoom.Area.Flags.IsSet((int)AreaFlags.NoPlayerVsPlayer))
            {
                if (show_messg)
                {
                   ch.SetColor(ATTypes.AT_IMMORT);
                   ch.SendTo("The gods have forbidden player killing in this area.");
                }
                return true;
            }

            if (ch.IsNpc() || victim.IsNpc())
                return false;

            if (((PlayerInstance)ch).CalculateAge() < 18 || ch.Level < 5)
            {
                if (show_messg)
                {
                   ch.SetColor(ATTypes.AT_WHITE);
                   ch.SendTo("You are not yet ready, needing age or experience, if not both.");
                }
                return true;
            }
            if (((PlayerInstance)victim).CalculateAge() < 18 || victim.Level < 5)
            {
                if (show_messg)
                {
                   ch.SetColor(ATTypes.AT_WHITE);
                   ch.SendTo("They are yet too young to die.");
                }
                return true;
            }

            if (ch.Level - victim.Level > 5 || victim.Level - ch.Level > 5)
            {
                if (show_messg)
                {
                   ch.SetColor(ATTypes.AT_IMMORT);
                   ch.SendTo("The gods do not allow murder when there is such a difference in level.");
                }
                return true;
            }

            if (victim.GetTimer(TimerTypes.PKilled) != null)
            {
                if (show_messg)
                {
                   ch.SetColor(ATTypes.AT_GREEN);
                   ch.SendTo("That character has died within the last 5 minutes.");
                }
                return true;
            }

            if (ch.GetTimer(TimerTypes.PKilled) != null)
            {
                if (show_messg)
                {
                   ch.SetColor(ATTypes.AT_GREEN);
                   ch.SendTo("You have been killed within the last 5 minutes.");
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
            return !ch.IsNpc() && !victim.IsNpc()
                   && ((PlayerInstance)ch).PlayerData.Flags.IsSet(PCFlags.Deadly)
                   && ((PlayerInstance)victim).PlayerData.Flags.IsSet(PCFlags.Deadly);
        }

        /// <summary>
        /// See if an attack justifies a KILLER flag
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="victim"></param>
        public static void check_killer(PlayerInstance ch, CharacterInstance victim)
        {
            // NPCs are fair game
            if (victim.IsNpc())
            {
                if (!ch.IsNpc())
                {
                    var levelRatio = 0;

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
                            ch.AdjustFavor(DeityFieldTypes.KillNPCRace, levelRatio);
                        else if (victim.CurrentRace == ch.PlayerData.CurrentDeity.NPCFoe)
                            ch.AdjustFavor(DeityFieldTypes.KillNPCFoe, levelRatio);
                        else
                            ch.AdjustFavor(DeityFieldTypes.Kill, levelRatio);
                    }
                }
                return;
            }

            // if you kill yourself, nothing happens
            if (ch == victim || ch.Level >= LevelConstants.ImmortalLevel)
                return;

            // Any character in the arena is okay to kill
            if (ch.IsInArena())
            {
                if (!ch.IsNpc() && !victim.IsNpc())
                {
                    ch.PlayerData.PvPKills++;
                    ((PlayerInstance)victim).PlayerData.PvPDeaths++;
                }
                return;
            }

            // killers and thieves are okay to kill 
            if (victim.Act.IsSet(PlayerFlags.Killer) || victim.Act.IsSet(PlayerFlags.Thief))
            {
                if (!ch.IsNpc())
                {
                    if (ch.PlayerData.Clan != null)
                    {
                        if (victim.Level < 10)
                            ch.PlayerData.Clan.PvPKillTable.ToList()[0]++;
                        else if (victim.Level < 15)
                            ch.PlayerData.Clan.PvPKillTable.ToList()[1]++;
                        else if (victim.Level < 20)
                            ch.PlayerData.Clan.PvPKillTable.ToList()[2]++;
                        else if (victim.Level < 30)
                            ch.PlayerData.Clan.PvPKillTable.ToList()[3]++;
                        else if (victim.Level < 40)
                            ch.PlayerData.Clan.PvPKillTable.ToList()[4]++;
                        else if (victim.Level < 50)
                            ch.PlayerData.Clan.PvPKillTable.ToList()[5]++;
                        else
                            ch.PlayerData.Clan.PvPKillTable.ToList()[6]++;
                    }

                    ch.PlayerData.PvPKills++;
                    ch.CurrentRoom.Area.PvPKills++;
                }
                return;
            }

            // Clan checks
            if (!ch.IsNpc() && !victim.IsNpc()
                && ch.PlayerData.Flags.IsSet(PCFlags.Deadly)
                && ((PlayerInstance)victim).PlayerData.Flags.IsSet(PCFlags.Deadly))
            {
                if (ch.PlayerData.Clan == null
                    || ((PlayerInstance)victim).PlayerData.Clan == null
                    || (ch.PlayerData.Clan.ClanType != ClanTypes.NoKill
                        && ((PlayerInstance)victim).PlayerData.Clan.ClanType != ClanTypes.NoKill
                        && ch.PlayerData.Clan != ((PlayerInstance)victim).PlayerData.Clan))
                {
                    if (ch.PlayerData.Clan != null)
                    {
                        if (victim.Level < 10)
                            ch.PlayerData.Clan.PvPKillTable.ToList()[0]++;
                        else if (victim.Level < 15)
                            ch.PlayerData.Clan.PvPKillTable.ToList()[1]++;
                        else if (victim.Level < 20)
                            ch.PlayerData.Clan.PvPKillTable.ToList()[2]++;
                        else if (victim.Level < 30)
                            ch.PlayerData.Clan.PvPKillTable.ToList()[3]++;
                        else if (victim.Level < 40)
                            ch.PlayerData.Clan.PvPKillTable.ToList()[4]++;
                        else if (victim.Level < 50)
                            ch.PlayerData.Clan.PvPKillTable.ToList()[5]++;
                        else
                            ch.PlayerData.Clan.PvPKillTable.ToList()[6]++;
                    }

                    ch.PlayerData.PvPKills++;
                    ch.CurrentHealth = ch.MaximumHealth;
                    ch.CurrentMana = ch.MaximumMana;
                    ch.CurrentMovement = ch.MaximumMovement;
                    if (ch.PlayerData != null)
                        ch.PlayerData.ConditionTable[ConditionTypes.Bloodthirsty] = (10 + ch.Level);
                    victim.UpdatePositionByCurrentHealth();
                    if (victim != ch)
                    {
                        comm.act(ATTypes.AT_MAGIC, "Bolts of blue energy rise from the corpse, seeping into $n.", ch, victim.Name, null, ToTypes.Room);
                        comm.act(ATTypes.AT_MAGIC, "Bolts of blue energy rise from the corpse, seeping into you.", ch, victim.Name, null, ToTypes.Character);
                    }

                    if (((PlayerInstance)victim).PlayerData.Clan != null)
                    {
                        if (victim.Level < 10)
                            ch.PlayerData.Clan.PvPDeathTable.ToList()[0]++;
                        else if (victim.Level < 15)
                            ch.PlayerData.Clan.PvPDeathTable.ToList()[1]++;
                        else if (victim.Level < 20)
                            ch.PlayerData.Clan.PvPDeathTable.ToList()[2]++;
                        else if (victim.Level < 30)
                            ch.PlayerData.Clan.PvPDeathTable.ToList()[3]++;
                        else if (victim.Level < 40)
                            ch.PlayerData.Clan.PvPDeathTable.ToList()[4]++;
                        else if (victim.Level < 50)
                            ch.PlayerData.Clan.PvPDeathTable.ToList()[5]++;
                        else
                            ch.PlayerData.Clan.PvPDeathTable.ToList()[6]++;
                    }

                    ((PlayerInstance)victim).PlayerData.PvPDeaths++;
                    ((PlayerInstance)victim).AdjustFavor(DeityFieldTypes.Die, 1);
                    ch.AdjustFavor(DeityFieldTypes.Kill, 1);
                    victim.AddTimer(TimerTypes.PKilled, 115, null, 0);
                    Macros.WAIT_STATE(victim, 3 * GameConstants.GetSystemValue<int>("PulseViolence"));
                    return;
                }
            }

            if (ch.IsAffected(AffectedByTypes.Charm))
            {
                if (ch.Master == null)
                {
                    LogManager.Instance.Bug("{0} bad AffectedByTypes.Charm", ch.IsNpc() ? ch.ShortDescription : ch.Name);
                    // TODO: affect_strip
                    ch.AffectedBy.RemoveBit(AffectedByTypes.Charm);
                    return;
                }

                check_killer((PlayerInstance)ch.Master, victim);
                return;
            }

            if (ch.IsNpc())
            {
                if (!victim.IsNpc())
                {
                    var vch = (PlayerInstance) victim;
                    if (vch.PlayerData.Clan != null)
                        vch.PlayerData.Clan.PvEDeaths++;
                    vch.PlayerData.PvEDeaths++;
                    victim.CurrentRoom.Area.PvEDeaths++;

                    var levelRatio = (ch.Level / victim.Level).GetNumberThatIsBetween(1, LevelConstants.AvatarLevel);
                    if (vch.PlayerData.CurrentDeity != null)
                    {
                        if (ch.CurrentRace == vch.PlayerData.CurrentDeity.NPCRace)
                            vch.AdjustFavor(DeityFieldTypes.DieNPCRace, levelRatio);
                        else if (ch.CurrentRace == vch.PlayerData.CurrentDeity.NPCFoe)
                            vch.AdjustFavor(DeityFieldTypes.DieNPCFoe, levelRatio);
                        else
                            vch.AdjustFavor(DeityFieldTypes.Die, levelRatio);
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
                if (((PlayerInstance)victim).PlayerData.Clan != null)
                {
                    if (victim.Level < 10)
                        ch.PlayerData.Clan.PvPDeathTable.ToList()[0]++;
                    else if (victim.Level < 15)
                        ch.PlayerData.Clan.PvPDeathTable.ToList()[1]++;
                    else if (victim.Level < 20)
                        ch.PlayerData.Clan.PvPDeathTable.ToList()[2]++;
                    else if (victim.Level < 30)
                        ch.PlayerData.Clan.PvPDeathTable.ToList()[3]++;
                    else if (victim.Level < 40)
                        ch.PlayerData.Clan.PvPDeathTable.ToList()[4]++;
                    else if (victim.Level < 50)
                        ch.PlayerData.Clan.PvPDeathTable.ToList()[5]++;
                    else
                        ch.PlayerData.Clan.PvPDeathTable.ToList()[6]++;
                }

                ((PlayerInstance)victim).PlayerData.PvPDeaths++;
                victim.CurrentRoom.Area.PvPDeaths++;
            }

            if (ch.Act.IsSet((int)PlayerFlags.Killer))
                return;

           ch.SetColor(ATTypes.AT_WHITE);
           ch.SendTo("A strange feeling grows deep inside you, and a tingle goes up your spine...");
           ch.SetColor(ATTypes.AT_IMMORT);
           ch.SendTo("A deep voice booms inside your head, 'Thou shall now be known as a deadly murderer!!!'");
           ch.SetColor(ATTypes.AT_WHITE);
           ch.SendTo("You feel as if your soul has been revealed for all to see.");
            ch.Act.SetBit(PlayerFlags.Killer);
            if (ch.Act.IsSet(PlayerFlags.Attacker))
                ch.Act.RemoveBit(PlayerFlags.Attacker);
            save.save_char_obj(ch);
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
                ch.SendTo("There are too many people fighting for you to join in.");
                return;
            }

            var fight = new FightingData
                                     {
                                         Who = victim,
                                         Experience = (int)(ch.ComputeExperienceGain(victim) * 0.85),
                                         Alignment = ch.ComputeAlignmentChange(victim)
                                     };
            if (!ch.IsNpc() && victim.IsNpc())
                fight.TimesKilled = ((PlayerInstance)ch).TimesKilled((MobileInstance)victim);

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
                victim.Switched.SendTo("You are disturbed!");
                Return.do_return(victim.Switched, "");
            }
        }

        private static readonly List<string> DeathCries = new List<string>
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
            var msg = string.Empty;
            var vnum = 0;

            var random = SmaugRandom.Between(0, 5);
            if (random >= 0 && random <= 4)
                msg = DeathCries[random + 1];
            else if (random == 5)
            {
                var shift = SmaugRandom.Between(0, 31);
                var cindex = 1 << shift;

                for (var i = 0; i < 32 && ch.ExtraFlags > 0; i++)
                {
                    if (ch.HasBodyPart(cindex))
                    {
                        msg = LookupManager.Instance.GetLookup("PartMessages", shift);
                        vnum = LookupConstants.PartVnums[shift];
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
                var template = RepositoryManager.Instance.OBJECTTEMPLATES.Get(vnum);
                if (template == null)
                {
                    LogManager.Instance.Bug("Invalid vnum");
                    return;
                }

                var name = ch.IsNpc() ? ch.ShortDescription : ch.Name;
                var obj = RepositoryManager.Instance.OBJECTS.Create(template, 0);
                obj.Timer = SmaugRandom.Between(4, 7);
                if (ch.IsAffected(AffectedByTypes.Poison))
                    obj.Value.ToList()[3] = 10;

                obj.ShortDescription = string.Format(obj.ShortDescription, name);
                obj.Description = string.Format(obj.Description, name);
                ch.CurrentRoom.AddTo(obj);
            }

            msg = ch.IsNpc()
                      ? DeathCries[6]
                      : DeathCries[7];

            var prevRoom = ch.CurrentRoom;
            foreach (var exit in prevRoom.Exits.Where(exit => exit.GetDestination() != null && exit.GetDestination() != prevRoom))
            {
                ch.CurrentRoom = exit.GetDestination();
                comm.act(ATTypes.AT_CARNAGE, msg, ch, null, null, ToTypes.Room);
            }

            ch.CurrentRoom = prevRoom;
        }

        public static void group_gain(CharacterInstance ch, CharacterInstance victim)
        {
            //// Monsters don't get kill XPs or alignment changes
            //// Dying of mortal wounds or poison doesn't give xp to anyone!
            if (ch.IsNpc() || victim == ch)
                return;

            var members = ch.CurrentRoom.Persons.Count(vch => vch.IsSameGroup(ch));
            if (members == 0)
                members = 1;

            var leader = ch.Leader ?? ch;

            foreach (var gch in ch.CurrentRoom.Persons.Where(gch => gch.IsSameGroup(ch)))
            {
                if (gch.Level - leader.Level > 8)
                {
                    gch.SendTo("You are too high for this group!");
                    continue;
                }
                if (gch.Level - leader.Level < -8)
                {
                    gch.SendTo("You are too low for this group!");
                    continue;
                }

                var xp = (int)(gch.ComputeExperienceGain(victim) * 0.1765) / members;
                if (gch.CurrentFighting == null)
                    xp /= 2;

                gch.CurrentAlignment = gch.ComputeAlignmentChange(victim);
                if (xp > 0)
                {
                    gch.Printf("You receive {0} experience points.", xp);
                    ((PlayerInstance)gch).GainXP(xp);
                }

                EvaluateEquipmentAfterXPGain(gch);
            }
        }

        private static void EvaluateEquipmentAfterXPGain(CharacterInstance gch)
        {
            foreach (
                var obj in
                    gch.Carrying.Where(obj => obj.WearLocation != WearLocations.None)
                       .Where(obj => (obj.ExtraFlags.IsSet(ItemExtraFlags.AntiEvil) && gch.IsEvil())
                                     || (obj.ExtraFlags.IsSet(ItemExtraFlags.AntiGood) && gch.IsGood())
                                     || (obj.ExtraFlags.IsSet(ItemExtraFlags.AntiNeutral) && gch.IsNeutral())))
            {
                comm.act(ATTypes.AT_MAGIC, "You are zapped by $p.", gch, obj, null, ToTypes.Character);
                comm.act(ATTypes.AT_MAGIC, "$n is zapped by $p.", gch, obj, null, ToTypes.Room);

                obj.RemoveFrom();
                var newObj = gch.CurrentRoom.AddTo(obj);
                MudProgHandler.ExecuteObjectProg(MudProgTypes.Zap, gch, newObj);
                if (gch.CharDied())
                    return;
            }
        }

        public static void new_dam_message(CharacterInstance ch, CharacterInstance victim, int dam, int dt, ObjectInstance obj)
        {
            int dampc;

            if (dam == 0)
                dampc = 0;
            else
                dampc = ((dam*1000)/victim.MaximumHealth) + (50 - ((victim.CurrentHealth*50)/victim.MaximumHealth));

            RoomTemplate wasInRoom = null;
            if (ch.CurrentRoom != victim.CurrentRoom)
            {
                wasInRoom = ch.CurrentRoom;
                ch.CurrentRoom.RemoveFrom(ch);
                victim.CurrentRoom.AddTo(ch);
            }

            var w_index = CalculateWeaponTypeIndex(dt);
            var d_index = CalculateDamageTypeIndex(dam, dampc);

            var vs = LookupManager.Instance.GetLookup(SlashMessageTable[w_index], d_index);
            var vp = LookupManager.Instance.GetLookup(PierceMessageTable[w_index], d_index);

            var punct = (dampc <= 30) ? '.' : '!';

            var gcflag = (dam == 0 && (!ch.IsNpc() && ((PlayerInstance)ch).PlayerData.Flags.IsSet(PCFlags.Gag)));
            var gvflag = (dam == 0 && (!victim.IsNpc() && ((PlayerInstance)victim).PlayerData.Flags.IsSet(PCFlags.Gag)));

            var skill = RepositoryManager.Instance.GetEntity<SkillData>(dt);

            string roomMsg;
            string youMsg;
            string victMsg;
            string attack;

            var attackTypes = LookupManager.Instance.GetLookups("AttackTable");

            if (dt == Program.TYPE_HIT)
            {
                roomMsg = $"$n {vp} $N{punct}";
                youMsg = $"You {vs} $N{punct}";
                victMsg = $"$n {vp} you{punct}";
            }
            else if (dt > Program.TYPE_HIT && ch.IsWieldedWeaponPoisoned())
            {
                if (dt < Program.TYPE_HIT + attackTypes.Count())
                    attack = attackTypes.ToList()[dt - Program.TYPE_HIT];
                else
                {
                    dt = Program.TYPE_HIT;
                    attack = attackTypes.First();
                }

                roomMsg = $"$n's poisoned {attack} {vp} $N{punct}";
                youMsg = $"Your poisoned {attack} {vp} $N{punct}";
                victMsg = $"$n's poisoned {attack} {vp} you{punct}";
            }
            else
            {
                if (skill != null)
                {
                    attack = skill.NounDamage;
                    if (dam == 0)
                    {
                        if (!skill.MissCharacterMessage.IsNullOrEmpty())
                            comm.act(ATTypes.AT_HIT, skill.MissCharacterMessage, ch, null, victim, ToTypes.Character);
                        if (!skill.MissVictimMessage.IsNullOrEmpty())
                            comm.act(ATTypes.AT_HITME, skill.MissVictimMessage, ch, null, victim, ToTypes.Victim);
                        if (!skill.MissRoomMessage.IsNullOrEmpty())
                            comm.act(ATTypes.AT_ACTION, skill.MissRoomMessage, ch, null, victim, ToTypes.Room);

                        if (!skill.MissCharacterMessage.IsNullOrEmpty()
                            || !skill.MissVictimMessage.IsNullOrEmpty()
                            || !skill.MissRoomMessage.IsNullOrEmpty())
                        {
                            if (wasInRoom != null)
                            {
                                ch.CurrentRoom.RemoveFrom(ch);
                                wasInRoom.AddTo(ch);
                            }
                        }
                    }
                    else
                    {
                        if (!skill.HitCharacterMessage.IsNullOrEmpty())
                            comm.act(ATTypes.AT_HIT, skill.HitCharacterMessage, ch, null, victim, ToTypes.Character);
                        if (!skill.HitVictimMessage.IsNullOrEmpty())
                            comm.act(ATTypes.AT_HITME, skill.HitVictimMessage, ch, null, victim, ToTypes.Victim);
                        if (!skill.HitRoomMessage.IsNullOrEmpty())
                            comm.act(ATTypes.AT_ACTION, skill.HitRoomMessage, ch, null, victim, ToTypes.Room);
                    }
                }
                else if (dt >= Program.TYPE_HIT && dt < (Program.TYPE_HIT + attackTypes.Count()))
                {
                    attack = obj != null
                        ? obj.ShortDescription
                        : attackTypes.ToList()[dt - Program.TYPE_HIT];
                }
                else
                {
                    dt = Program.TYPE_HIT;
                    attack = attackTypes.First();
                }

                roomMsg = $"$n's {attack} {vp} $N{punct}";
                youMsg = $"Your {attack} {vp} $N{punct}";
                victMsg = $"$n's {attack} {vp} you{punct}";
            }

            comm.act(ATTypes.AT_ACTION, roomMsg, ch, null, victim, ToTypes.NotVictim);
            if (!gcflag)
                comm.act(ATTypes.AT_HIT, youMsg, ch, null, victim, ToTypes.Character);
            if (!gvflag)
                comm.act(ATTypes.AT_HITME, victMsg, ch, null, victim, ToTypes.Victim);

            if (wasInRoom != null)
            {
                ch.CurrentRoom.RemoveFrom(ch);
                wasInRoom.AddTo(ch);
            }
        }

        private static int CalculateDamageTypeIndex(int dam, int dampc)
        {
            if (dam == 0)
                return 0;
            if (dampc < 0)
                return 1;
            if (dampc <= 100)
                return 1 + dampc / 10;
            if (dampc <= 200)
                return 11 + (dampc - 100) / 20;
            if (dampc <= 900)
                return 16 + (dampc - 200) / 100;
            return 23;
        }

        private static int CalculateWeaponTypeIndex(int dt)
        {
            if (dt > 0)
                return 0;
            var attackTypes = LookupManager.Instance.GetLookups("AttackTable");
            if (dt >= Program.TYPE_HIT && dt < (Program.TYPE_HIT + attackTypes.Count()))
                return dt - Program.TYPE_HIT;
            return 0;
        }

        private static readonly List<string> SlashMessageTable = new List<string>
        {
            "SlashGenericMessages", // hit
            "SlashBladeMessages", // slice
            "SlashBladeMessages", // stab
            "SlashBladeMessages", // slash
            "SlashBluntMessages", // whip
            "SlashBladeMessages", // claw
            "SlashGenericMessages", // blast
            "SlashBluntMessages", // pound
            "SlashBluntMessages", // crush
            "SlashGenericMessages", // grep
            "SlashBladeMessages", // bite
            "SlashBladeMessages", // pierce
            "SlashBluntMessages", // suction
            "SlashGenericMessages", // bolt
            "SlashGenericMessages", // arrow
            "SlashGenericMessages", // dart
            "SlashGenericMessages", // stone
            "SlashGenericMessages", // pea
        };
        private static readonly List<string> PierceMessageTable = new List<string>
        {
            "PierceGenericMessages", // hit
            "PierceBladeMessages", // slice
            "PierceBladeMessages", // stab
            "PierceBladeMessages", // Pierce
            "PierceBluntMessages", // whip
            "PierceBladeMessages", // claw
            "PierceGenericMessages", // blast
            "PierceBluntMessages", // pound
            "PierceBluntMessages", // crush
            "PierceGenericMessages", // grep
            "PierceBladeMessages", // bite
            "PierceBladeMessages", // pierce
            "PierceBluntMessages", // suction
            "PierceGenericMessages", // bolt
            "PierceGenericMessages", // arrow
            "PierceGenericMessages", // dart
            "PierceGenericMessages", // stone
            "PierceGenericMessages", // pea
        };

        public static void dam_message(CharacterInstance ch, CharacterInstance victim, int dam, int dt)
        {
            new_dam_message(ch, victim, dam, dt, null);
        }

        public static bool check_illegal_pk(CharacterInstance ch, CharacterInstance victim)
        {
            if (!victim.IsNpc() && !ch.IsNpc())
            {
                if ((((PlayerInstance)victim).PlayerData.Flags.IsSet(PCFlags.Deadly)
                     || (ch.Level - victim.Level) > 10
                     || ((PlayerInstance)ch).PlayerData.Flags.IsSet(PCFlags.Deadly))
                    && !ch.IsInArena()
                    && ch != victim
                    && !(ch.IsImmortal() && victim.IsImmortal()))
                {
                    var buffer =
                        $"&p{ch.LastCommand} on {victim.Name} in &W***&rILLEGAL PKILL&W*** &pattempt at {victim.CurrentRoom.ID}";
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
