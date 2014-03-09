using System;
using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common;
using Realm.Library.Patterns.Repository;
using SmaugCS.Commands;
using SmaugCS.Commands.Movement;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Organizations;
using SmaugCS.Language;
using SmaugCS.Logging;
using SmaugCS.Managers;

// ReSharper disable CheckNamespace
namespace SmaugCS
// ReSharper restore CheckNamespace
{
    public static class CharacterInstanceExtensions
    {
        public static void AddKill(this CharacterInstance ch, CharacterInstance mob)
        {
            if (ch.IsNpc() || !mob.IsNpc())
                return;

            long id = mob.MobIndex.ID;
            int maxTrack = GameConstants.GetIntegerConstant("MaxKillTrack");

            KilledData killed = ch.PlayerData.Killed.FirstOrDefault(x => x.ID == id);
            if (killed == null)
            {
                if (ch.PlayerData.Killed.Count >= maxTrack)
                {
                    KilledData oldest = null;
                    foreach (KilledData check in ch.PlayerData.Killed)
                    {
                        if (oldest == null)
                            oldest = check;
                        else if (oldest != check)
                        {
                            if (check.Updated < oldest.Updated)
                                oldest = check;
                        }
                    }

                    ch.PlayerData.Killed.Remove(oldest);

                    KilledData newKilled = new KilledData(id);
                    newKilled.Increment(1);
                    ch.PlayerData.Killed.Add(newKilled);
                }
            }
            else
                killed.Increment(1);
        }

        public static int TimesKilled(this CharacterInstance ch, CharacterInstance mob)
        {
            if (ch.IsNpc() || !mob.IsNpc())
                return 0;

            return ch.PlayerData.Killed.Any(x => x.ID == mob.MobIndex.ID)
                       ? ch.PlayerData.Killed.First(x => x.ID == mob.MobIndex.ID).Count
                       : 0;
        }

        public static bool IsAttackSuppressed(this CharacterInstance ch)
        {
            if (ch.IsNpc())
                return false;

            TimerData timer = handler.get_timerptr(ch, TimerTypes.ASupressed);
            if (timer == null)
                return false;
            if (timer.Value == -1)
                return true;
            return timer.Count >= 1;
        }

        public static bool IsWieldedWeaponPoisoned(this CharacterInstance ch)
        {
            if (fight.UsedWeapon == null)
                return false;

            ObjectInstance obj = ch.GetEquippedItem(WearLocations.Wield);
            if (obj != null && fight.UsedWeapon == obj &&
                Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Poisoned))
                return true;

            obj = ch.GetEquippedItem(WearLocations.DualWield);
            if (obj != null && fight.UsedWeapon == obj &&
                Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Poisoned))
                return true;

            return false;
        }

        public static void ImproveMentalState(this CharacterInstance ch, int mod)
        {
            int c = 0.GetNumberThatIsBetween(Math.Abs(mod), 20);
            int con = ch.GetCurrentConstitution();

            c += SmaugRandom.Percent() < con ? 1 : 0;

            if (ch.MentalState < 0)
                ch.MentalState = -100.GetNumberThatIsBetween(ch.MentalState + c, 0);
            else if (ch.MentalState > 0)
                ch.MentalState = 0.GetNumberThatIsBetween(ch.MentalState - c, 100);
        }

        public static void WorsenMentalState(this CharacterInstance ch, int mod)
        {
            int c = 0.GetNumberThatIsBetween(Math.Abs(mod), 20);
            int con = ch.GetCurrentConstitution();

            c -= SmaugRandom.Percent() < con ? 1 : 0;
            if (c < 1)
                return;

            if (!ch.IsNpc()
                && ch.PlayerData.Nuisance != null
                && ch.PlayerData.Nuisance.Flags > 2)
                c += (int) (0.4f*((ch.PlayerData.Nuisance.Flags - 2)*ch.PlayerData.Nuisance.Power));

            if (ch.MentalState < 0)
                ch.MentalState = -100.GetNumberThatIsBetween(ch.MentalState - c, 100);
            else if (ch.MentalState > 0)
                ch.MentalState = -100.GetNumberThatIsBetween(ch.MentalState + c, 100);
            else
                ch.MentalState -= c;
        }

        public static bool CanAstral(this CharacterInstance ch, CharacterInstance victim)
        {
            return victim != ch && victim.CurrentRoom != null &&
                   !victim.CurrentRoom.Flags.IsSet((int) RoomFlags.Private) &&
                   !victim.CurrentRoom.Flags.IsSet((int) RoomFlags.Solitary) &&
                   !victim.CurrentRoom.Flags.IsSet((int) RoomFlags.NoAstral) &&
                   !victim.CurrentRoom.Flags.IsSet((int) RoomFlags.Death) &&
                   !victim.CurrentRoom.Flags.IsSet((int) RoomFlags.Prototype) && victim.Level < (ch.Level + 15) &&
                   (!victim.CanPKill() || ch.IsNpc() || ch.CanPKill()) &&
                   (!victim.IsNpc() || !victim.Act.IsSet((int) ActFlags.Prototype)) &&
                   (!victim.IsNpc() || !victim.SavingThrows.CheckSaveVsSpellStaff(ch.Level, victim)) &&
                   (!victim.CurrentRoom.Area.Flags.IsSet((int) AreaFlags.NoPKill) || !ch.IsPKill());
        }

        public static bool CanDrop(this CharacterInstance ch, ObjectInstance obj)
        {
            if (!Macros.IS_OBJ_STAT(obj, (int) ItemExtraFlags.NoDrop))
                return true;
            if (!ch.IsNpc() && ch.Level >= LevelConstants.GetLevel("immortal"))
                return true;
            if (ch.IsNpc() && ch.MobIndex.Vnum == VnumConstants.MOB_VNUM_SUPERMOB)
                return true;
            return false;
        }

        public static bool CanSee(this CharacterInstance ch, ObjectInstance obj)
        {
            if (!ch.IsNpc() && ch.Act.IsSet((int)PlayerFlags.HolyLight))
                return true;
            if (ch.IsNpc() && ch.MobIndex.Vnum == VnumConstants.MOB_VNUM_SUPERMOB)
                return true;
            if (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Buried))
                return false;
            if (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Hidden))
                return false;
            if (ch.IsAffected(AffectedByTypes.TrueSight))
                return true;
            if (ch.IsAffected(AffectedByTypes.Blind))
                return false;

            //// Can see lights in the dark
            if (obj.ItemType == ItemTypes.Light && obj.Value[2] != 0)
                return true;

            if (ch.CurrentRoom.IsDark())
            {
                //// Can see glowing items in teh dark, invisible or not
                if (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Glow))
                    return true;
                if (!ch.IsAffected(AffectedByTypes.Infrared))
                    return false;
            }

            if (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Invisible)
                && !ch.IsAffected(AffectedByTypes.DetectInvisibility))
                return false;

            return true;
        }

        public static bool CanSee(this CharacterInstance ch, CharacterInstance victim)
        {
            if (victim == null)
                return false;

            if (ch == null)
                return !victim.IsAffected(AffectedByTypes.Invisible) && !victim.IsAffected(AffectedByTypes.Hide) && !victim.Act.IsSet((int)PlayerFlags.WizardInvisibility);

            if (ch == victim)
                return true;

            if (!victim.IsNpc() && victim.Act.IsSet((int)PlayerFlags.WizardInvisibility)
                && ch.Trust < victim.PlayerData.WizardInvisible)
                return false;

            if (victim.IsNpc() && victim.Act.IsSet((int)ActFlags.MobInvisibility)
                && victim.IsPKill() && victim.Timer > 1 && victim.Descriptor == null)
                return false;

            if (!ch.IsNpc() && ch.Act.IsSet((int)PlayerFlags.HolyLight))
                return true;

            if (!ch.IsAffected(AffectedByTypes.TrueSight))
            {
                if (ch.IsAffected(AffectedByTypes.Blind))
                    return false;
                if (ch.CurrentRoom.IsDark() && !ch.IsAffected(AffectedByTypes.Infrared))
                    return false;
                if (victim.IsAffected(AffectedByTypes.Invisible) && !ch.IsAffected(AffectedByTypes.DetectInvisibility))
                    return false;
                if (victim.IsAffected(AffectedByTypes.Hide) && !ch.IsAffected(AffectedByTypes.DetectHidden)
                    && victim.CurrentFighting == null
                    && (ch.IsNpc() ? !victim.IsNpc() : victim.IsNpc()))
                    return false;
            }

            if (victim.IsNotAuthorized())
            {
                if (ch.IsNotAuthorized() || ch.IsImmortal() || ch.IsNpc())
                    return true;
                if (ch.PlayerData.Council != null && ch.PlayerData.Council.Name.EqualsIgnoreCase("Newbie Council"))
                    return true;
                return false;
            }

            return true;
        }

        public static bool IsAllowedToUseObject(this CharacterInstance ch, ObjectInstance obj)
        {
            if (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.AntiWarrior))
            {
                if (ch.CurrentClass == ClassTypes.Warrior
                    || ch.CurrentClass == ClassTypes.Paladin
                    || ch.CurrentClass == ClassTypes.Ranger)
                    return false;
            }
            if (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.AntiMage))
            {
                if (ch.CurrentClass == ClassTypes.Mage
                    || ch.CurrentClass == ClassTypes.Augurer)
                    return false;
            }
            if (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.AntiThief))
            {
                if (ch.CurrentClass == ClassTypes.Thief)
                    return false;
            }
            if (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.AntiDruid))
            {
                if (ch.CurrentClass == ClassTypes.Druid)
                    return false;
            }
            if (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.AntiCleric))
            {
                if (ch.CurrentClass == ClassTypes.Cleric)
                    return false;
            }
            if (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.AntiVampire))
            {
                if (ch.CurrentClass == ClassTypes.Vampire)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Check to see if there is room to wear another object on this location (Layered clothing support)
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="obj"></param>
        /// <param name="wear_loc"></param>
        /// <returns></returns>
        public static bool CanWearLayer(this CharacterInstance ch, ObjectInstance obj, int wear_loc)
        {
            int bitlayers = 0;
            int objlayers = obj.ObjectIndex.Layers;

            foreach (ObjectInstance otmp in ch.Carrying
                .Where(otmp => (int)otmp.WearLocation == wear_loc))
            {
                if (otmp.ObjectIndex.Layers == 0)
                    return false;
                bitlayers |= otmp.ObjectIndex.Layers;
            }

            if ((bitlayers > 0 && objlayers == 0) || bitlayers > objlayers)
                return false;

            return bitlayers == 0 || ((bitlayers & ~objlayers) == bitlayers);
        }

        public static bool CouldDualWield(this CharacterInstance ch)
        {
            return ch.IsNpc() || ch.PlayerData.Learned[DatabaseManager.Instance.GetEntity<SkillData>("dual wield").ID] > 0;
        }

        public static bool CanDualWield(this CharacterInstance ch)
        {
            bool wield = false;
            bool nwield = false;

            if (!CouldDualWield(ch))
                return false;
            if (ch.GetEquippedItem(WearLocations.Wield) != null)
                wield = true;

            if (ch.GetEquippedItem(WearLocations.WieldMissile) != null
                || ch.GetEquippedItem(WearLocations.DualWield) != null)
                nwield = true;

            if (wield && nwield)
            {
                color.send_to_char("You are already wielding two weapons... grow some more arms!\r\n", ch);
                return false;
            }

            if ((wield || nwield) && ch.GetEquippedItem(WearLocations.Shield) != null)
            {
                color.send_to_char("You cannot dual wield, you're already holding a shield!\r\n", ch);
                return false;
            }

            if ((wield || nwield) && ch.GetEquippedItem(WearLocations.Hold) != null)
            {
                color.send_to_char("You cannot hold another weapon, you're already holding something in that hand!\r\n", ch);
                return false;
            }

            return true;
        }

        public static ObjectInstance HasKey(this CharacterInstance ch, int key)
        {
            foreach (ObjectInstance obj in ch.Carrying)
            {
                if (obj.ObjectIndex.Vnum == key ||
                    (obj.ItemType == ItemTypes.Key && obj.Value[0] == key))
                    return obj;
                if (obj.ItemType != ItemTypes.KeyRing) continue;
                if (obj.Contents.Any(obj2 => obj.ObjectIndex.Vnum == key || obj2.Value[0] == key))
                    return obj;
            }
            return null;
        }

        public static bool CanMorph(this CharacterInstance ch, MorphData morph, bool isCast)
        {
            if (morph == null)
                return false;
            if (!ch.IsImmortal() && !ch.IsNpc())
                return false;
            if (morph.no_cast && isCast)
                return false;
            if (ch.Level < morph.level)
                return false;
            if (morph.pkill == Program.ONLY_PKILL && !ch.IsPKill())
                return false;
            if (morph.pkill == Program.ONLY_PEACEFULL && ch.IsPKill())
                return false;
            if (morph.sex != -1 && morph.sex != (int) ch.Gender)
                return false;
            if (morph.Class != 0 && !morph.Class.IsSet(1 << (int) ch.CurrentClass))
                return false;
            if (morph.race != 0 && morph.race.IsSet(1 << (int) ch.CurrentRace))
                return false;
            if (!string.IsNullOrWhiteSpace(morph.deity) &&
                (ch.PlayerData.CurrentDeity != null || DatabaseManager.Instance.GetEntity<DeityData>(morph.deity) == null))
                return false;
            if (morph.timeto != -1 && morph.timefrom != -1)
            {
                bool found = false;
                int tmp, i;

                for (i = 0, tmp = morph.timefrom; i < 25 && tmp != morph.timeto; i++)
                {
                    if (tmp == GameManager.Instance.GameTime.Hour)
                    {
                        found = true;
                        break;
                    }
                    if (tmp == 23)
                        tmp = 0;
                    else
                        tmp++;
                }

                if (!found)
                    return false;
            }

            if (morph.dayfrom != -1 && morph.dayto != -1
                && (morph.dayto < (GameManager.Instance.GameTime.Day + 1) || morph.dayfrom > (GameManager.Instance.GameTime.Day + 1)))
                return false;
            return true;
        }

        public static bool CanCharm(this CharacterInstance ch)
        {
            if (ch.IsNpc() || ch.IsImmortal())
                return true;
            if (((ch.GetCurrentCharisma()/3) + 1) > ch.PlayerData.NumberOfCharmies)
                return true;
            return false;
        }

        public static bool IsInArena(this CharacterInstance ch)
        {
            if (ch.CurrentRoom.Flags.IsSet((int) RoomFlags.Arena))
                return true;
            if (ch.CurrentRoom.Area.Flags.IsSet((int) AreaFlags.FreeKill))
                return true;
            if (ch.CurrentRoom.Vnum >= 29 && ch.CurrentRoom.Vnum <= 43)
                return true;
            if (ch.CurrentRoom.Area.Name.EqualsIgnoreCase("arena"))
                return true;
            return false;
        }

        public static bool IsInCombatPosition(this CharacterInstance ch)
        {
            return (ch.CurrentPosition == PositionTypes.Fighting
                    || ch.CurrentPosition == PositionTypes.Evasive
                    || ch.CurrentPosition == PositionTypes.Defensive
                    || ch.CurrentPosition == PositionTypes.Aggressive
                    || ch.CurrentPosition == PositionTypes.Berserk);
        }

        public static bool IsImmune(this CharacterInstance ch, ResistanceTypes type)
        {
            return type != ResistanceTypes.Unknown && ch.Immunity.IsSet((int)type);
        }

        public static bool IsImmune(this CharacterInstance ch, SpellDamageTypes type, ILookupManager lookupManager = null)
        {
            ResistanceTypes resType = lookupManager == null
                                          ? LookupManager.Instance.GetResistanceType(type)
                                          : lookupManager.GetResistanceType(type);

            return resType != ResistanceTypes.Unknown && ch.Immunity.IsSet((int)resType);
        }

        public static bool IsIgnoring(this CharacterInstance ch, CharacterInstance victim)
        {
            // TODO
            return false;
        }

        public static bool IsNpc(this CharacterInstance ch)
        {
            return ch.Act.IsSet((int)ActFlags.IsNpc);
        }

        public static bool IsAffected(this CharacterInstance ch, AffectedByTypes affectedBy)
        {
            return ch.AffectedBy.IsSet((int)affectedBy);
        }

        public static bool IsFloating(this CharacterInstance ch)
        {
            return ch.IsAffected(AffectedByTypes.Flying) || ch.IsAffected(AffectedByTypes.Floating);
        }

        public static bool IsRetired(this CharacterInstance ch)
        {
            return ch.PlayerData != null && ch.PlayerData.Flags.IsSet((int)PCFlags.Retired);
        }

        public static bool IsGuest(this CharacterInstance ch)
        {
            return ch.PlayerData != null && ch.PlayerData.Flags.IsSet((int)PCFlags.Guest);
        }

        public static bool CanCast(this CharacterInstance ch, IDatabaseManager dbManager = null)
        {
            ClassData cls = dbManager == null
                                ? DatabaseManager.Instance.GetClass(ch.CurrentClass)
                                : dbManager.GetClass(ch.CurrentClass);

            return cls.IsSpellcaster;
        }

        public static bool IsVampire(this CharacterInstance ch)
        {
            return (!ch.IsNpc() && (ch.CurrentRace == RaceTypes.Vampire) || ch.CurrentClass == ClassTypes.Vampire);
        }
        
        public static bool IsGood(this CharacterInstance ch)
        {
            return ch.CurrentAlignment >= 350;
        }
        
        public static bool IsEvil(this CharacterInstance ch)
        {
            return ch.CurrentAlignment <= -350;
        }
        
        public static bool IsNeutral(this CharacterInstance ch)
        {
            return !ch.IsGood() && !ch.IsEvil();
        }
        
        public static bool IsAwake(this CharacterInstance ch)
        {
            return ch.CurrentPosition > PositionTypes.Sleeping;
        }

        public static bool IsOutside(this CharacterInstance ch)
        {
            return !ch.CurrentRoom.Flags.IsSet((int)RoomFlags.Indoors) &&
                   !ch.CurrentRoom.Flags.IsSet((int)RoomFlags.Tunnel);
        }

        public static bool IsDrunk(this CharacterInstance ch, int drunk)
        {
            return SmaugRandom.Percent() < (ch.PlayerData.ConditionTable[ConditionTypes.Drunk] & 2 / drunk);
        }

        public static bool IsDevoted(this CharacterInstance ch)
        {
            return !ch.IsNpc() && ch.PlayerData.CurrentDeity != null;
        }
        
        public static bool IsIdle(this CharacterInstance ch)
        {
            return ch.PlayerData != null &&
                   ch.PlayerData.Flags.IsSet((int)PCFlags.Idle);
        }
        
        public static bool IsPKill(this CharacterInstance ch)
        {
            return ch.PlayerData != null &&
                   ch.PlayerData.Flags.IsSet((int)PCFlags.Deadly);
        }
        
        public static bool CanPKill(this CharacterInstance ch)
        {
            return ch.IsPKill() && ch.Level >= 5 && ch.CalculateAge() >= 18;
        }

        public static bool HasBodyPart(this CharacterInstance ch, int part)
        {
            return (ch.ExtraFlags == 0 || ch.ExtraFlags.IsSet(part));
        }

        public static bool IsAffectedBy(this CharacterInstance ch, int sn)
        {
            return ch.Affects.Exists(x => x.SkillNumber == sn);
        }

        public static bool IsNotAuthorized(this CharacterInstance ch)
        {
            return !ch.IsNpc() && ch.PlayerData.AuthState <= 3 &&
                   ch.PlayerData.Flags.IsSet((int)PCFlags.Unauthorized);
        }

        public static bool IsWaitingForAuthorization(this CharacterInstance ch)
        {
            return !ch.IsNpc() && ch.Descriptor != null && ch.PlayerData.AuthState == 1
                   && ch.PlayerData.Flags.IsSet((int)PCFlags.Unauthorized);
        }

        public static bool IsCircleFollowing(this CharacterInstance ch, CharacterInstance victim)
        {
            CharacterInstance tmp;
            do
            {
                tmp = ch.Master;
                if (tmp == ch)
                    return true;

            } while (tmp != null);

            return false;
        }

        public static bool IsImmortal(this CharacterInstance ch)
        {
            return ch.Trust >= LevelConstants.GetLevel("immortal");
        }

        public static bool IsHero(this CharacterInstance ch)
        {
            return ch.Trust >= LevelConstants.GetLevel("hero");
        }

        public static int GetArmorClass(this CharacterInstance ch)
        {
            return ch.ArmorClass + (ch.IsAwake()
                                   ? GameConstants.dex_app[ch.GetCurrentDexterity()].defensive
                                   : 0) + fight.VAMP_AC(ch);
        }
        public static int GetHitroll(this CharacterInstance ch)
        {
            return ch.HitRoll.SizeOf + GameConstants.str_app[ch.GetCurrentStrength()].tohit
                   + (2 - (Math.Abs(ch.MentalState) / 10));
        }
        public static int GetDamroll(this CharacterInstance ch)
        {
            return ch.DamageRoll.SizeOf + ch.DamageRoll.Bonus + GameConstants.str_app[ch.GetCurrentStrength()].todam +
                   ((ch.MentalState > 5 && ch.MentalState < 15) ? 1 : 0);
        }

        public static bool IsClanned(this CharacterInstance ch)
        {
            return !ch.IsNpc() &&
                ch.PlayerData.Clan != null &&
                ch.PlayerData.Clan.ClanType != ClanTypes.Order &&
                ch.PlayerData.Clan.ClanType != ClanTypes.Guild;
        }
        public static bool IsOrdered(this CharacterInstance ch)
        {
            return !ch.IsNpc() &&
                   ch.PlayerData.Clan != null &&
                   ch.PlayerData.Clan.ClanType == ClanTypes.Order;
        }
        public static bool IsGuilded(this CharacterInstance ch)
        {
            return !ch.IsNpc() &&
                   ch.PlayerData.Clan != null &&
                   ch.PlayerData.Clan.ClanType == ClanTypes.Guild;
        }
        public static bool IsDeadlyClan(this CharacterInstance ch)
        {
            return !ch.IsNpc() &&
                   ch.PlayerData.Clan != null &&
                   ch.PlayerData.Clan.ClanType != ClanTypes.NoKill &&
                   ch.PlayerData.Clan.ClanType != ClanTypes.Order &&
                   ch.PlayerData.Clan.ClanType != ClanTypes.Guild;
        }

        public static void RemoveAffect(this CharacterInstance ch, AffectData paf)
        {
            if (ch.Affects == null || ch.Affects.Count == 0)
            {
                LogManager.Instance.Bug("affect_remove (%s, %d): no affect", ch.Name, paf != null ? paf.Type : 0);
                return;
            }

            handler.affect_modify(ch, paf, false);

            if (ch.CurrentRoom != null)
                ch.CurrentRoom.RemoveAffect(paf);

            ch.Affects.Remove(paf);
        }

        public static void AddAffect(this CharacterInstance ch, AffectData affect)
        {
            if (affect == null)
            {
                LogManager.Instance.Bug("%s (%s, NULL)", "affect_to_char", ch.Name);
                return;
            }

            AffectData newAffect = new AffectData
            {
                Type = affect.Type,
                Duration = affect.Duration,
                Location = affect.Location,
                Modifier = affect.Modifier,
                BitVector = affect.BitVector
            };

            ch.Affects.Add(newAffect);
            handler.affect_modify(ch, newAffect, true);

            if (ch.CurrentRoom != null)
                ch.CurrentRoom.AddAffect(newAffect);
        }

        public static void StripAffects(this CharacterInstance ch, int sn)
        {
            foreach (AffectData affect in ch.Affects.Where(affect => (int)affect.Type == sn))
                ch.RemoveAffect(affect);
        }

        public static void JoinAffect(this CharacterInstance ch, AffectData paf)
        {
            if (ch.Affects == null || ch.Affects.Count == 0)
                return;

            IEnumerable<AffectData> matchingAffects = ch.Affects.Where(x => x.Type == paf.Type);
            foreach (var affect in matchingAffects)
            {
                paf.Duration = 1000000.GetLowestOfTwoNumbers(paf.Duration + affect.Duration);
                paf.Modifier = paf.Modifier > 0 ? 5000.GetLowestOfTwoNumbers(paf.Modifier + affect.Modifier) : affect.Modifier;
                ch.RemoveAffect(affect);
                break;
            }

            ch.AddAffect(paf);
        }

        public static void aris_affect(this CharacterInstance ch, AffectData paf)
        {
            //ch.AffectedBy.SetBits(paf.BitVector);
            switch ((int)paf.Location % Program.REVERSE_APPLY)
            {
                case (int)ApplyTypes.Affect:
                    //ch.AffectedBy.Bits[0].SetBit(paf.Modifier);
                    break;
                case (int)ApplyTypes.Resistance:
                    ch.Resistance.SetBit(paf.Modifier);
                    break;
                case (int)ApplyTypes.Immunity:
                    ch.Immunity.SetBit(paf.Modifier);
                    break;
                case (int)ApplyTypes.Susceptibility:
                    ch.Susceptibility.SetBit(paf.Modifier);
                    break;
            }
        }

        public static void update_aris(this CharacterInstance ch)
        {
            if (ch.IsNpc() || ch.IsImmortal())
                return;

            bool hiding = ch.IsAffected(AffectedByTypes.Hide);

            //ch.AffectedBy.ClearBits();
            ch.Resistance = 0;
            ch.Immunity = 0;
            ch.Susceptibility = 0;
            //ch.NoAffectedBy.ClearBits();
            ch.NoResistance = 0;
            ch.NoImmunity = 0;
            ch.NoSusceptibility = 0;

            RaceData myRace = DatabaseManager.Instance.GetRace(ch.CurrentRace);
            //ch.AffectedBy.SetBits(myRace.AffectedBy);
            ch.Resistance.SetBit(myRace.Resistance);
            ch.Susceptibility.SetBit(myRace.Susceptibility);

            ClassData myClass = DatabaseManager.Instance.GetClass(ch.CurrentClass);
            //ch.AffectedBy.SetBits(myClass.AffectedBy);
            ch.Resistance.SetBit(myClass.Resistance);
            ch.Susceptibility.SetBit(myClass.Susceptibility);

            if (ch.PlayerData.CurrentDeity != null)
            {
               // if (ch.PlayerData.Favor > ch.PlayerData.CurrentDeity.AffectedNum)
                //    ch.AffectedBy.SetBits(ch.PlayerData.CurrentDeity.AffectedBy);
                if (ch.PlayerData.Favor > ch.PlayerData.CurrentDeity.ElementNum)
                    ch.Resistance.SetBit(ch.PlayerData.CurrentDeity.Element);
                if (ch.PlayerData.Favor < ch.PlayerData.CurrentDeity.SusceptNum)
                    ch.Susceptibility.SetBit(ch.PlayerData.CurrentDeity.Suscept);
            }

            foreach (AffectData affect in ch.Affects)
                ch.aris_affect(affect);

            foreach (ObjectInstance obj in ch.Carrying
                .Where(x => x.WearLocation != WearLocations.None))
            {
                foreach (AffectData affect in obj.Affects)
                    ch.aris_affect(affect);
                // TODO figure this out
            }

            if (ch.CurrentRoom != null)
            {
                foreach (AffectData affect in ch.CurrentRoom.Affects)
                    ch.aris_affect(affect);
            }

            // TODO: Polymorph

            if (hiding)
                ch.AffectedBy.SetBit((int)AffectedByTypes.Hide);
        }

        public static bool CanTakePrototype(this CharacterInstance ch)
        {
            if (ch.IsImmortal())
                return true;
            return ch.IsNpc()
                   && ch.Act.IsSet((int)ActFlags.Prototype);
        }

        public static void ModifySkill(this CharacterInstance ch, int sn, int mod, bool add)
        {
            if (ch.IsNpc())
                return;

            if (add)
                ch.PlayerData.Learned[sn] += mod;
            else
                ch.PlayerData.Learned[sn] = (ch.PlayerData.Learned[sn] + mod).GetNumberThatIsBetween(0, Macros.GET_ADEPT(ch, sn));
        }

        #region Experience
        public static int GetExperienceWorth(this CharacterInstance ch)
        {
            int wexp = ((int)Math.Pow(ch.Level, 3) * 5) + ch.MaximumHealth;
            wexp -= (ch.ArmorClass - 50) * 2;
            wexp += (ch.BareDice.NumberOf * ch.BareDice.SizeOf + ch.GetDamroll()) * 50;
            wexp += ch.GetHitroll() * ch.Level * 10;
            if (ch.IsAffected(AffectedByTypes.Sanctuary))
                wexp += (int)(wexp * 1.5);
            if (ch.IsAffected(AffectedByTypes.FireShield))
                wexp += (int)(wexp * 1.2);
            if (ch.IsAffected(AffectedByTypes.ShockShield))
                wexp += (int)(wexp * 1.2);
            return wexp.GetNumberThatIsBetween(GameConstants.MinimumExperienceWorth, GameConstants.MaximumExperienceWorth);
        }

        public static int GetExperienceBase(this CharacterInstance ch)
        {
            return ch.IsNpc()
                ? 1000
                : DatabaseManager.Instance.CLASSES.Values.First(x => x.Type == ch.CurrentClass).BaseExperience;
        }

        public static int GetExperienceLevel(this CharacterInstance ch, int level)
        {
            return ((int)Math.Pow(0.GetHighestOfTwoNumbers(level - 1), 3) * ch.GetExperienceBase());
        }

        public static int GetLevelExperience(this CharacterInstance ch, int cexp)
        {
            int x = LevelConstants.GetLevel("supreme");
            int lastx = x;
            int y = 0;

            while (y == 0)
            {
                int tmp = ch.GetExperienceLevel(x);
                lastx = x;
                if (tmp > cexp)
                    x /= 2;
                else if (lastx != x)
                    x += (x / 2);
                else
                    y = x;
            }

            return y < 1 ? 1 : y > LevelConstants.GetLevel("supreme") ? LevelConstants.GetLevel("supreme") : y;
        }
        #endregion

        #region Statistics

        public static int CalculateAge(this CharacterInstance ch)
        {
            if (ch.IsNpc()) return -1;

            int num_days = ((GameManager.Instance.GameTime.Month + 1) * GameManager.Instance.SystemData.DaysPerMonth) + GameManager.Instance.GameTime.Day;
            int ch_days = ((ch.PlayerData.Month + 1) * GameManager.Instance.SystemData.DaysPerMonth) + ch.PlayerData.Day;
            int age = GameManager.Instance.GameTime.Year - ch.PlayerData.Year;

            if (ch_days - num_days > 0)
                age -= 1;
            return age;
        }

        public static int GetCurrentStat(this CharacterInstance ch, StatisticTypes statistic)
        {
            ClassData currentClass = DatabaseManager.Instance.GetClass(ch.CurrentClass);
            int max = 20;

            if (ch.IsNpc() || currentClass.PrimaryAttribute == statistic)
                max = 25;
            if (currentClass.SecondaryAttribute == statistic)
                max = 22;
            if (currentClass.DeficientAttribute == statistic)
                max = 16;

            return max;
        }

        public static int GetCurrentStrength(this CharacterInstance ch)
        {
            return (ch.PermanentStrength + ch.ModStrength).GetNumberThatIsBetween(3, ch.GetCurrentStat(StatisticTypes.Strength));
        }

        public static int GetCurrentIntelligence(this CharacterInstance ch)
        {
            return (ch.PermanentIntelligence + ch.ModIntelligence).GetNumberThatIsBetween(3, ch.GetCurrentStat(StatisticTypes.Intelligence));
        }

        public static int GetCurrentWisdom(this CharacterInstance ch)
        {
            return (ch.PermanentWisdom + ch.ModWisdom).GetNumberThatIsBetween(3, ch.GetCurrentStat(StatisticTypes.Wisdom));
        }

        public static int GetCurrentDexterity(this CharacterInstance ch)
        {
            return (ch.PermanentDexterity + ch.ModDexterity).GetNumberThatIsBetween(3, ch.GetCurrentStat(StatisticTypes.Dexterity));
        }

        public static int GetCurrentConstitution(this CharacterInstance ch)
        {
            return (ch.PermanentConstitution + ch.ModConstitution).GetNumberThatIsBetween(3, ch.GetCurrentStat(StatisticTypes.Constitution));
        }

        public static int GetCurrentCharisma(this CharacterInstance ch)
        {
            return (ch.PermanentCharisma + ch.ModCharisma).GetNumberThatIsBetween(3, ch.GetCurrentStat(StatisticTypes.Charisma));
        }

        public static int GetCurrentLuck(this CharacterInstance ch)
        {
            return (ch.PermanentLuck + ch.ModLuck).GetNumberThatIsBetween(3, ch.GetCurrentStat(StatisticTypes.Luck));
        }

        public static int CanCarryN(this CharacterInstance ch)
        {
            int penalty = 0;

            if (!ch.IsNpc() && ch.Level >= LevelConstants.GetLevel("immortal"))
                return ch.Trust * 200;
            if (ch.IsNpc() && ch.Act.IsSet((int)ActFlags.Immortal))
                return ch.Level * 200;
            if (ch.GetEquippedItem(WearLocations.Wield) != null)
                ++penalty;
            if (ch.GetEquippedItem(WearLocations.DualWield) != null)
                ++penalty;
            if (ch.GetEquippedItem(WearLocations.WieldMissile) != null)
                ++penalty;
            if (ch.GetEquippedItem(WearLocations.Hold) != null)
                ++penalty;
            if (ch.GetEquippedItem(WearLocations.Shield) != null)
                ++penalty;
            return ((ch.Level + 15) / 5 + ch.GetCurrentDexterity() - 13 - penalty).GetNumberThatIsBetween(5, 20);
        }

        public static int CanCarryMaxWeight(this CharacterInstance ch)
        {
            if (!ch.IsNpc() && ch.Level >= LevelConstants.GetLevel("immortal"))
                return 1000000;
            if (ch.IsNpc() && ch.Act.IsSet((int)ActFlags.Immortal))
                return 1000000;
            return GameConstants.str_app[ch.GetCurrentStrength()].Carry;
        }

        #endregion

        public static ObjectInstance GetEquippedItem(this CharacterInstance ch, WearLocations location)
        {
            return ch.GetEquippedItem((int)location);
        }
        public static ObjectInstance GetEquippedItem(this CharacterInstance ch, int location)
        {
            ObjectInstance maxObj = null;
            WearLocations wearLoc = EnumerationExtensions.GetEnum<WearLocations>(location);
            foreach (ObjectInstance obj in ch.Carrying.Where(x => x.WearLocation == wearLoc))
            {
                if (obj.ObjectIndex.Layers == 0)
                    return obj;
                if (maxObj == null || obj.ObjectIndex.Layers > maxObj.ObjectIndex.Layers)
                    maxObj = obj;
            }

            return maxObj;
        }

        public static int GetEncumberedMove(this CharacterInstance ch, int movement)
        {
            int max = ch.CanCarryMaxWeight();
            if (ch.CarryWeight >= max)
                return Convert.ToInt16(movement * 4);
            if (ch.CarryWeight >= max * 0.95)
                return Convert.ToInt16(movement * 3.5);
            if (ch.CarryWeight >= max * 0.90)
                return Convert.ToInt16(movement * 3);
            if (ch.CarryWeight >= max * 0.85)
                return Convert.ToInt16(movement * 2.5);
            if (ch.CarryWeight >= max * 0.80)
                return Convert.ToInt16(movement * 2);
            if (ch.CarryWeight >= max * 0.75)
                return Convert.ToInt16(movement * 1.5);
            return movement;
        }

        public static void AdvanceLevel(this CharacterInstance ch)
        {
            string buffer = string.Format("the {0}", tables.GetTitle(ch.CurrentClass, ch.Level, ch.Gender));
            player.set_title(ch, buffer);

            ClassData myClass = DatabaseManager.Instance.GetClass(ch.CurrentClass);

            int add_hp = GameConstants.con_app[ch.GetCurrentConstitution()].hitp +
                         SmaugCS.Common.SmaugRandom.Between(myClass.MinimumHealthGain, myClass.MaximumHealthGain);
            int add_mana = myClass.UseMana
                               ? SmaugCS.Common.SmaugRandom.Between(2, (2 * ch.GetCurrentIntelligence() + ch.GetCurrentWisdom()) / 8)
                               : 0;
            int add_move = SmaugCS.Common.SmaugRandom.Between(5, (ch.GetCurrentConstitution() + ch.GetCurrentDexterity()) / 4);
            int add_prac = GameConstants.wis_app[ch.GetCurrentWisdom()].practice;

            add_hp = 1.GetHighestOfTwoNumbers(add_hp);
            add_mana = 0.GetHighestOfTwoNumbers(add_mana);
            add_move = 10.GetHighestOfTwoNumbers(add_move);

            if (ch.IsPKill())
            {
                add_mana = (int)(add_mana + add_mana * 0.3f);
                add_move = (int)(add_move + add_move * 0.3f);
                add_hp += 1;
                color.send_to_char("Gravoc's Pandect steels your sinews.\r\n", ch);
            }

            ch.MaximumHealth += add_hp;
            ch.MaximumMana += add_mana;
            ch.MaximumMovement += add_move;
            ch.Practice += add_prac;

            if (!ch.IsNpc())
                ch.Act.RemoveBit((int)PlayerFlags.BoughtPet);

            if (ch.Level == LevelConstants.GetLevel("avatar"))
                ch.AdvanceLevelAvatar();
            if (ch.Level < LevelConstants.GetLevel("immortal"))
            {
                if (ch.IsVampire())
                    buffer = string.Format("Your gain is: {0}/{1} hp, {2}/{3} bp, {4}/{5} mv, {6}/{7} prac.\r\n",
                                           add_hp, ch.MaximumHealth, 1, ch.Level + 10, add_move, ch.MaximumMovement, add_prac,
                                           ch.Practice);
                else
                    buffer = string.Format("Your gain is: {0}/{1} hp, {2}/{3} mana, {4}/{5} mv, {6}/{7} prac.\r\n",
                                           add_hp, ch.MaximumHealth, add_mana, ch.MaximumMana, add_move, ch.MaximumMovement,
                                           add_prac, ch.Practice);

                color.set_char_color(ATTypes.AT_WHITE, ch);
                color.send_to_char(buffer, ch);
            }
        }

        private static void AdvanceLevelAvatar(this CharacterInstance ch)
        {
            IEnumerable<DescriptorData> descriptors = db.DESCRIPTORS.
                                                  Where(d => d.ConnectionStatus == ConnectionTypes.Playing
                                                             && d.Character != ch);
            foreach (DescriptorData d in descriptors)
            {
                color.set_char_color(ATTypes.AT_WHITE, d.Character);
                color.ch_printf(d.Character, "%s has just achieved Avatarhood!\r\n", ch.Name);
            }

            color.set_char_color(ATTypes.AT_WHITE, ch);
            Help.do_help(ch, "M_ADVHERO_");
        }

        public static void GainXP(this CharacterInstance ch, int gain)
        {
            if (ch.IsNpc() || (ch.Level >= LevelConstants.GetLevel("avatar")))
                return;

            double modgain = gain;
            if (modgain > 0 && ch.IsPKill() && ch.Level < 17)
            {
                if (ch.Level <= 6)
                {
                    color.send_to_char("The Favor of Gravoc fosters your learning.\r\n", ch);
                    modgain *= 2;
                }
                if (ch.Level <= 10 && ch.Level >= 7)
                {
                    color.send_to_char("The Hand of Gravoc hastens your learning.\r\n", ch);
                    modgain *= 1.75f;
                }
                if (ch.Level <= 13 && ch.Level >= 11)
                {
                    color.send_to_char("The Cunning of Gravoc succors your learning.\r\n", ch);
                    modgain *= 1.5f;
                }
                if (ch.Level <= 16 && ch.Level >= 14)
                {
                    color.send_to_char("THe Patronage of Gravoc reinforces your learning.\r\n", ch);
                    modgain *= 1.25f;
                }
            }

            RaceData myRace = DatabaseManager.Instance.GetRace(ch.CurrentRace);
            modgain *= (myRace.ExperienceMultiplier / 100.0f);

            if (ch.IsPKill() && modgain < 0)
            {
                if (ch.Experience + modgain < ch.GetExperienceLevel(ch.Level))
                {
                    modgain = ch.GetExperienceLevel(ch.Level) - ch.Experience;
                    color.send_to_char("Gravoc's Pandect protects your insight.\r\n", ch);
                }
            }

            modgain = ((int)modgain).GetLowestOfTwoNumbers(ch.GetExperienceLevel(ch.Level + 2) - ch.GetExperienceLevel(ch.Level + 1));
            ch.Experience = 0.GetHighestOfTwoNumbers(ch.Experience + (int)modgain);

            if (ch.IsNotAuthorized() && ch.Experience >= ch.GetExperienceLevel(ch.Level + 1))
            {
                color.send_to_char("You cannot ascend to a higher level until you are authorized.\r\n", ch);
                ch.Experience = ch.GetExperienceLevel(ch.Level + 1) - 1;
                return;
            }

            while (ch.Level < LevelConstants.GetLevel("avatar") && ch.Experience >= ch.GetExperienceLevel(ch.Level + 1))
            {
                color.set_char_color(ATTypes.AT_WHITE | ATTypes.AT_BLINK, ch);
                ch.Level += 1;
                color.ch_printf(ch, "You have not obtained experience level %d!\r\n", ch.Level);
                ch.AdvanceLevel();
            }
        }

        public static int HealthGain(this CharacterInstance ch)
        {
            int gain;
            if (ch.IsNpc())
            {
                gain = ch.Level * 3 / 2;
                if (ch.IsAffected(AffectedByTypes.Poison))
                    gain /= 4;
                return gain.GetLowestOfTwoNumbers(ch.MaximumHealth - ch.CurrentHealth);
            }

            gain = 5.GetLowestOfTwoNumbers(ch.Level);
            switch (ch.CurrentPosition)
            {
                case PositionTypes.Dead:
                    return 0;
                case PositionTypes.Mortal:
                case PositionTypes.Incapacitated:
                    return -1;
                case PositionTypes.Stunned:
                    return 1;
                case PositionTypes.Sleeping:
                    gain += (int)(ch.GetCurrentConstitution() * 2.0f);
                    break;
                case PositionTypes.Resting:
                    gain += (int)(ch.GetCurrentConstitution() * 1.25f);
                    break;
            }

            if (ch.IsVampire())
                gain = ch.GetModifiedStatGainForVampire(gain);

            if (ch.PlayerData.ConditionTable[ConditionTypes.Full] == 0)
                gain /= 2;
            if (ch.PlayerData.ConditionTable[ConditionTypes.Thirsty] == 0)
                gain /= 2;

            if (ch.IsAffected(AffectedByTypes.Poison))
                gain /= 4;
            return gain.GetLowestOfTwoNumbers(ch.MaximumHealth - ch.CurrentHealth);
        }

        public static int ManaGain(this CharacterInstance ch)
        {
            int gain;

            if (ch.IsNpc())
            {
                gain = ch.Level;

                if (ch.IsAffected(AffectedByTypes.Poison))
                    gain /= 4;
                return gain.GetLowestOfTwoNumbers(ch.MaximumMana - ch.CurrentMana);
            }

            gain = 5.GetLowestOfTwoNumbers(ch.Level / 2);
            if (ch.CurrentPosition < PositionTypes.Sleeping)
                return 0;
            switch (ch.CurrentPosition)
            {
                case PositionTypes.Sleeping:
                    gain += (int)(ch.GetCurrentIntelligence() * 3.25f);
                    break;
                case PositionTypes.Resting:
                    gain += (int)(ch.GetCurrentIntelligence() * 1.75f);
                    break;
            }

            if (ch.PlayerData.ConditionTable[ConditionTypes.Full] == 0)
                gain /= 2;
            if (ch.PlayerData.ConditionTable[ConditionTypes.Thirsty] == 0)
                gain /= 2;

            if (ch.IsAffected(AffectedByTypes.Poison))
                gain /= 4;
            return gain.GetLowestOfTwoNumbers(ch.MaximumMana - ch.CurrentMana);
        }

        public static int MovementGain(this CharacterInstance ch)
        {
            int gain;

            if (ch.IsNpc())
            {
                gain = ch.Level;
                if (ch.IsAffected(AffectedByTypes.Poison))
                    gain /= 4;
                return gain.GetLowestOfTwoNumbers(ch.MaximumMovement - ch.CurrentMovement);
            }

            gain = 15.GetHighestOfTwoNumbers(2 * ch.Level);

            switch (ch.CurrentPosition)
            {
                case PositionTypes.Dead:
                    return 0;
                case PositionTypes.Mortal:
                case PositionTypes.Incapacitated:
                    return -1;
                case PositionTypes.Stunned:
                    return 1;
                case PositionTypes.Sleeping:
                    gain += (int)(ch.GetCurrentDexterity() * 4.5f);
                    break;
                case PositionTypes.Resting:
                    gain += (int)(ch.GetCurrentDexterity() * 2.5f);
                    break;
            }

            if (ch.IsVampire())
                gain = ch.GetModifiedStatGainForVampire(gain);

            if (ch.PlayerData.ConditionTable[ConditionTypes.Full] == 0)
                gain /= 2;
            if (ch.PlayerData.ConditionTable[ConditionTypes.Thirsty] == 0)
                gain /= 2;

            if (ch.IsAffected(AffectedByTypes.Poison))
                gain /= 4;

            return gain.GetLowestOfTwoNumbers(ch.MaximumMovement - ch.CurrentMovement);
        }

        private static int GetModifiedStatGainForVampire(this CharacterInstance ch, int gain)
        {
            int modGain = gain;
            if (ch.PlayerData.ConditionTable[ConditionTypes.Bloodthirsty] <= 1)
                modGain /= 2;
            else if (ch.PlayerData.ConditionTable[ConditionTypes.Bloodthirsty] >= (8 + ch.Level))
                modGain *= 2;

            if (ch.IsOutside())
            {
                switch (GameManager.Instance.GameTime.Sunlight)
                {
                    case SunPositionTypes.Sunset:
                    case SunPositionTypes.Sunrise:
                        modGain /= 2;
                        break;
                    case SunPositionTypes.Light:
                        modGain /= 4;
                        break;
                }
            }
            return modGain;
        }

        public static void CheckAlignment(this CharacterInstance ch)
        {
            if (ch.CurrentAlignment < DatabaseManager.Instance.GetRace(ch.CurrentRace).MinimumAlignment
                || ch.CurrentAlignment > DatabaseManager.Instance.GetRace(ch.CurrentRace).MaximumAlignment)
            {
                color.set_char_color(ATTypes.AT_BLOOD, ch);
                color.send_to_char("Your actions have been incompatible with the ideals of your race. This troubles you.", ch);
            }

            if (ch.CurrentClass == ClassTypes.Paladin)
            {
                if (ch.CurrentAlignment < 250)
                {
                    color.set_char_color(ATTypes.AT_BLOOD, ch);
                    color.send_to_char("You are wracked with guilt and remorse for your craven actions!\r\n", ch);
                    comm.act(ATTypes.AT_BLOOD, "$n prostrates $mself, seeking forgiveness from $s Lord.", ch, null, null, ToTypes.Room);
                    ch.WorsenMentalState(15);
                    return;
                }
                if (ch.CurrentAlignment < 500)
                {
                    color.set_char_color(ATTypes.AT_BLOOD, ch);
                    color.send_to_char("As you betray your faith, your mind begins to betray you.\r\n", ch);
                    comm.act(ATTypes.AT_BLOOD, "$n shudders, judging $s actions unworthy of a Paladin.", ch, null, null, ToTypes.Room);
                    ch.WorsenMentalState(6);
                }
            }
        }

        public static bool WillFall(this CharacterInstance ch, int fall)
        {
            if (ch.CurrentRoom.Flags.IsSet((int)RoomFlags.NoFloor)
                && Macros.CAN_GO(ch, (short)DirectionTypes.Down)
                && (!ch.IsAffected(AffectedByTypes.Flying)
                    || (ch.CurrentMount != null && !ch.CurrentMount.IsAffected(AffectedByTypes.Flying))))
            {
                if (fall > 80)
                {
                    LogManager.Instance.Bug("Falling (in a loop?) more than 80 rooms: vnum {0}", ch.CurrentRoom.Vnum);
                    ch.CurrentRoom.FromRoom(ch);
                    DatabaseManager.Instance.ROOMS.CastAs<Repository<long, RoomTemplate>>().Get(VnumConstants.ROOM_VNUM_TEMPLE).ToRoom(ch);
                    return true;
                }

                color.set_char_color(ATTypes.AT_FALLING, ch);
                color.send_to_char("You're falling down...\r\n", ch);
                Move.move_char(ch, ch.CurrentRoom.GetExit((int)DirectionTypes.Down), ++fall);
                return true;
            }
            return false;
        }

        #region Followers
        public static void AddFollower(this CharacterInstance ch, CharacterInstance master)
        {
            if (ch.Master != null)
            {
                LogManager.Instance.Bug("non-null master");
                return;
            }

            ch.Master = master;
            ch.Leader = null;

            if (ch.IsNpc() && ch.Act.IsSet((int)ActFlags.Pet)
                && !master.IsNpc())
                master.PlayerData.Pet = ch;

            if (master.CanSee(ch))
                comm.act(ATTypes.AT_ACTION, "$n now follows you.", ch, null, master, ToTypes.Victim);

            comm.act(ATTypes.AT_ACTION, "You now follow $N.", ch, null, master, ToTypes.Character);
        }

        public static void StopFollower(this CharacterInstance ch)
        {
            if (ch.Master == null)
            {
                LogManager.Instance.Bug("null master");
                return;
            }

            if (ch.IsNpc() && !ch.Master.IsNpc()
                && ch.Master.PlayerData.Pet == ch)
                ch.Master.PlayerData.Pet = null;

            if (ch.IsAffected(AffectedByTypes.Charm))
            {
                ch.AffectedBy.RemoveBit((int)AffectedByTypes.Charm);
                //ch.RemoveAffect(gsn_charm_person);    TODO Fix this!
                if (!ch.Master.IsNpc())
                    ch.Master.PlayerData.NumberOfCharmies--;
            }

            if (ch.Master.CanSee(ch))
            {
                if (!(!ch.Master.IsNpc() && ch.IsImmortal()
                      && !ch.Master.IsImmortal()))
                    comm.act(ATTypes.AT_ACTION, "$n stops following you.", ch, null, ch.Master, ToTypes.Victim);
            }

            comm.act(ATTypes.AT_ACTION, "You stop following $N.", ch, null, ch.Master, ToTypes.Character);

            ch.Master = null;
            ch.Leader = null;
        }

        public static void DieFollower(this CharacterInstance ch)
        {
            if (ch.Master != null)
                ch.StopFollower();

            ch.Leader = null;

            foreach (CharacterInstance fch in DatabaseManager.Instance.CHARACTERS.CastAs<Repository<long, CharacterInstance>>().Values)
            {
                if (fch.Master == ch)
                    ch.StopFollower();
                if (fch.Leader == ch)
                    fch.Leader = fch;
            }
        }

        public static bool IsSameGroup(this CharacterInstance ch, CharacterInstance victim)
        {
            return ch.Leader == victim.Leader;
        }
        #endregion

        #region Languages
        public static int KnowsLanguage(this CharacterInstance ch, int language, CharacterInstance cch)
        {
            if (!ch.IsNpc() && ch.IsImmortal())
                return 100;
            if (ch.IsNpc() && (ch.Speaks == 0
                || ch.Speaks.IsSet((language & ~(int)LanguageTypes.Clan))))
                return 100;
            if (language.IsSet((int)LanguageTypes.Common))
                return 100;

            if ((language & (int)LanguageTypes.Clan) > 0)
            {
                if (ch.IsNpc() || cch.IsNpc())
                    return 100;
                if (ch.PlayerData.Clan == cch.PlayerData.Clan
                    && ch.PlayerData.Clan != null)
                    return 100;
            }

            if (!ch.IsNpc())
            {
                if (DatabaseManager.Instance.GetRace(ch.CurrentRace).Language.IsSet(language))
                    return 100;

                /*for (int i = 0; i < GameConstants.LanguageTable.Keys.Count; i++)
                {
                    if (i == (int)LanguageTypes.Unknown)
                        break;

                    if (language.IsSet(i) && ch.Speaks.IsSet(i))
                    {
                        SkillData skill = DatabaseManager.Instance.GetSkill(GameConstants.LanguageTable[i]);
                        if (skill.Slot != -1)
                            return ch.PlayerData.Learned[skill.Slot];
                    }
                }*/
            }

            return 0;
        }

        public static bool CanLearnLanguage(this CharacterInstance ch, int language)
        {
            if ((language & (int)LanguageTypes.Clan) > 0)
                return false;
            if (ch.IsNpc() || ch.IsImmortal())
                return false;
            if ((DatabaseManager.Instance.GetRace(ch.CurrentRace).Language & language) > 0)
                return false;

            if ((ch.Speaks & language) > 0)
            {
                /*for (int i = 0; i < GameConstants.LanguageTable.Keys.Count; i++)
                {
                    if (i == (int)LanguageTypes.Unknown)
                        break;

                    if ((language & i) > 0)
                    {
                        if (((int)LanguageTypes.ValidLanguages & i) == 0)
                            return false;

                        SkillData skill = DatabaseManager.Instance.GetSkill(GameConstants.LanguageTable[i]);
                        if (skill == null)
                        {
                            LogManager.Instance.Bug("can_learn_lang: valid language without sn: %d", i);
                            continue;
                        }
                    }

                    if (ch.PlayerData.Learned[i] >= 99)
                        return false;
                }*/
            }

            return ((int)LanguageTypes.ValidLanguages & language) > 0;
        }
        #endregion

        #region Hunt-Hate-Fear
        public static bool IsHunting(this CharacterInstance ch, CharacterInstance victim)
        {
            return ch.CurrentHunting != null
                && ch.CurrentHunting.Who == victim;
        }

        public static bool IsHating(this CharacterInstance ch, CharacterInstance victim)
        {
            return ch.CurrentHating != null
                && ch.CurrentHating.Who == victim;
        }

        public static bool IsFearing(this CharacterInstance ch, CharacterInstance victim)
        {
            return ch.CurrentFearing != null
                   && ch.CurrentFearing.Who == victim;
        }

        public static bool IsBlind(this CharacterInstance ch)
        {
            if (!ch.IsNpc() && ch.Act.IsSet((int)PlayerFlags.HolyLight))
                return true;
            if (ch.IsAffected(AffectedByTypes.TrueSight))
                return true;
            if (!ch.IsAffected(AffectedByTypes.Blind))
                return true;

            return false;
        }

        public static void StopHunting(this CharacterInstance ch)
        {
            ch.CurrentHunting = null;
        }

        public static void StopHating(this CharacterInstance ch)
        {
            ch.CurrentHating = null;
        }

        public static void StopFearing(this CharacterInstance ch)
        {
            ch.CurrentFearing = null;
        }

        public static void StartHunting(this CharacterInstance ch, CharacterInstance victim)
        {
            if (ch.CurrentHunting != null)
                ch.StopHunting();

            ch.CurrentHunting = new HuntHateFearData
            {
                Name = victim.Name,
                Who = victim
            };
        }

        public static void StartFearing(this CharacterInstance ch, CharacterInstance victim)
        {
            if (ch.CurrentFearing != null)
                ch.StopFearing();

            ch.CurrentFearing = new HuntHateFearData
            {
                Name = victim.Name,
                Who = victim
            };
        }

        public static void StartHating(this CharacterInstance ch, CharacterInstance victim)
        {
            if (ch.CurrentHating != null)
                ch.StopHating();

            ch.CurrentHating = new HuntHateFearData
            {
                Name = victim.Name,
                Who = victim
            };
        }
        #endregion

        public static void Equip(this CharacterInstance ch, ObjectInstance obj, int iWear)
        {
            if (obj.CarriedBy != ch)
            {
                LogManager.Instance.Bug("obj not being carried by ch");
                return;
            }

            ObjectInstance temp = ch.GetEquippedItem(iWear);
            if (temp != null && (temp.ObjectIndex.Layers == 0 || obj.ObjectIndex.Layers == 0))
            {
                LogManager.Instance.Bug("already equipped (%d)", iWear);
                return;
            }

            handler.separate_obj(obj);
            if ((Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.AntiEvil)
                 && ch.IsEvil())
                || (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.AntiGood)
                    && ch.IsGood())
                || (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.AntiNeutral)
                    && ch.IsNeutral()))
            {
                if (handler.LoadingCharacter != ch)
                {
                    comm.act(ATTypes.AT_MAGIC, "You are zapped by $p and drop it.", ch, obj, null,
                             ToTypes.Character);
                    comm.act(ATTypes.AT_MAGIC, "$n is zapped by $p and drops it.", ch, obj, null, ToTypes.Room);
                }
                if (obj.CarriedBy != null)
                    obj.FromCharacter();

                ch.CurrentRoom.ToRoom(obj);
                mud_prog.oprog_zap_trigger(ch, obj);

                if (GameManager.Instance.SystemData.SaveFlags.IsSet((int)AutoSaveFlags.ZapDrop)
                    && !ch.CharDied())
                    save.save_char_obj(ch);
                return;
            }

            ch.ArmorClass -= obj.ApplyArmorClass;
            obj.WearLocation = EnumerationExtensions.GetEnum<WearLocations>(iWear);
            ch.CarryNumber -= obj.GetObjectNumber();
            if (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Magical))
                ch.CarryWeight -= obj.GetObjectWeight();

            foreach (AffectData affect in obj.ObjectIndex.Affects)
                ch.AddAffect(affect);
            foreach (AffectData affect in obj.Affects)
                ch.AddAffect(affect);

            if (obj.ItemType == ItemTypes.Light
                && obj.Value[2] != 0
                && ch.CurrentRoom != null)
                ++ch.CurrentRoom.Light;
        }

        public static void Unequip(this CharacterInstance ch, ObjectInstance obj)
        {
            if (obj.WearLocation == WearLocations.None)
            {
                LogManager.Instance.Bug("already unequipped");
                return;
            }

            ch.CarryNumber += obj.GetObjectNumber();
            if (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Magical))
                ch.CarryWeight += obj.GetObjectWeight();

            ch.ArmorClass += obj.ApplyArmorClass;
            obj.WearLocation = WearLocations.None;

            foreach (AffectData paf in obj.ObjectIndex.Affects)
                ch.RemoveAffect(paf);
            if (obj.CarriedBy != null)
            {
                foreach (AffectData paf in obj.Affects)
                    ch.RemoveAffect(paf);
            }

            ch.update_aris();

            if (obj.CarriedBy == null)
                return;

            if (obj.ItemType == ItemTypes.Light
                && obj.Value[2] != 0
                && ch.CurrentRoom != null
                && ch.CurrentRoom.Light > 0)
                --ch.CurrentRoom.Light;
        }

        /// <summary>
        /// Expand the name of a character into a string that identifies THAT 
        /// character within a room. E.g. the second 'guard' -> 2. guard
        /// </summary>
        /// <returns></returns>
        public static string GetExpandedName(this CharacterInstance ch)
        {
            if (!ch.IsNpc())
                return ch.Name;

            string name = ch.Name.FirstWord();
            return string.IsNullOrEmpty(name)
                       ? string.Empty
                       : string.Format("{0}.{1}", 1 + ch.CurrentRoom.Persons.Count(rch => name.IsEqual(rch.Name)), name);
        }

        public static bool CharDied(this CharacterInstance ch)
        {
            if (ch == handler.CurrentCharacter && handler.CurrentDeadCharacter != null)
                return true;

            return db.ExtractedCharQueue.Any(ccd => ccd.Character == ch);
        }

    }
}
