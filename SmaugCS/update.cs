﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Realm.Library.Common;
using Realm.Library.Patterns.Command;
using SmaugCS.Commands;
using SmaugCS.Commands.Movement;
using SmaugCS.Commands.Social;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Extensions;
using SmaugCS.Logging;
using SmaugCS.Managers;
using SmaugCS.SpecFuns;

namespace SmaugCS
{
    public static class update
    {
        public static void mobile_update()
        {
            //lc = trworld_create(TR_CHAR_WORLD_BACK);

            foreach (CharacterInstance ch in DatabaseManager.Instance.CHARACTERS.Values)
            {
                handler.set_cur_char(ch);
                if (!ch.IsNpc())
                {
                    drunk_randoms(ch);
                    hallucinations(ch);
                    continue;
                }

                if (ch.CurrentRoom == null || ch.IsAffected(AffectedByTypes.Charm) ||
                    ch.IsAffected(AffectedByTypes.Paralysis))
                    continue;

                if (ch.MobIndex.ID == VnumConstants.MOB_VNUM_ANIMATED_CORPSE && !ch.IsAffected(AffectedByTypes.Charm))
                {
                    if (ch.CurrentRoom.Persons.Any()) 
                        comm.act(ATTypes.AT_MAGIC, "$n returns to the dust from whence $e came.", ch, null, null, ToTypes.Room);

                    if (ch.IsNpc())
                        handler.extract_char(ch, true);
                    continue;
                }

                if (!ch.Act.IsSet(ActFlags.Running) && !ch.Act.IsSet(ActFlags.Sentinel) && ch.CurrentFighting == null &&
                    ch.CurrentHunting == null)
                {
                    Macros.WAIT_STATE(ch, 2*GameConstants.GetSystemValue<int>("PulseViolence"));
                    track.hunt_victim(ch);
                    continue;
                }

                if (!ch.Act.IsSet(ActFlags.Running) && ch.SpecialFunction != null)
                {
                    if (ch.SpecialFunction.Value.Invoke(ch))
                        continue;
                    if (ch.CharDied())
                        continue;
                }

                if (ch.MobIndex.HasProg(MudProgTypes.Script))
                {
                    mud_prog.mprog_script_trigger(ch);
                    continue;
                }

                if (ch != handler.CurrentCharacter)
                {
                    // TODO BUG: ch does not equal CurrentCharacter after spec_fun");
                    continue;
                }

                if (ch.CurrentPosition != PositionTypes.Standing)
                    continue;

                if (ch.Act.IsSet(ActFlags.Mounted))
                {
                    if (ch.Act.IsSet(ActFlags.Aggressive) || ch.Act.IsSet(ActFlags.MetaAggr))
                        Emote.do_emote(ch, "snarls and growls.");
                    continue;
                }

                if (ch.CurrentRoom.Flags.IsSet(RoomFlags.Safe) 
                    && (ch.Act.IsSet(ActFlags.Aggressive) || ch.Act.IsSet(ActFlags.MetaAggr)))
                    Emote.do_emote(ch, "glares around and snarls.");

                if (ch.CurrentRoom.Area.NumberOfPlayers > 0)
                {
                    mud_prog.mprog_random_trigger(ch);
                    if (ch.CharDied())
                        continue;
                    if ((int) ch.CurrentPosition < (int) PositionTypes.Standing)
                        continue;
                }

                mud_prog.mprog_hour_trigger(ch);
                if (ch.CharDied())
                    continue;

                mud_prog.rprog_hour_trigger(ch);
                if (ch.CharDied())
                    continue;

                if ((int)ch.CurrentPosition < (int)PositionTypes.Standing)
                    continue;

                if (ch.Act.IsSet(ActFlags.Scavenger) && ch.CurrentRoom.Contents.Any() && SmaugRandom.Bits(2) == 0)
                    Scavenge(ch);

                if (!ch.Act.IsSet(ActFlags.Running)
                    && !ch.Act.IsSet(ActFlags.Sentinel)
                    && !ch.Act.IsSet(ActFlags.Prototype)
                    && !ch.Act.IsSet(ActFlags.StayArea))
                {
                    int door = SmaugRandom.Bits(5);
                    if (door > 9)
                        break;

                    ExitData exit = ch.CurrentRoom.GetExit(door);
                    if (exit == null)
                        break;

                    if (exit.Flags.IsSet(ExitFlags.Window) || exit.Flags.IsSet(ExitFlags.Closed))
                        break;

                    RoomTemplate room = exit.GetDestination();
                    if (room == null)
                        break;

                    if (room.Flags.IsSet(RoomFlags.NoMob) || room.Flags.IsSet(RoomFlags.Death))
                        break;

                    if (room.Area != ch.CurrentRoom.Area)
                        break;

                    ReturnTypes retcode = Commands.Movement.Move.move_char(ch, exit, 0);
                    if (ch.CharDied())
                        continue;
                    if (retcode != ReturnTypes.None || ch.Act.IsSet(ActFlags.Sentinel) ||
                        (int) ch.CurrentPosition < (int) PositionTypes.Standing)
                        continue;
                }

                if (ch.CurrentHealth < ch.MaximumHealth/2)
                {
                    int door = SmaugRandom.Bits(4);
                    if (door > 9)
                        break;

                    ExitData exit = ch.CurrentRoom.GetExit(door);
                    if (exit == null)
                        break;

                    if (exit.Flags.IsSet(ExitFlags.Window) || exit.Flags.IsSet(ExitFlags.Closed))
                        break;

                    RoomTemplate room = exit.GetDestination();
                    if (room == null)
                        break;

                    if (room.Flags.IsSet(RoomFlags.NoMob) || room.Flags.IsSet(RoomFlags.Death))
                        break;

                    bool found = false;
                    foreach (CharacterInstance rch in ch.CurrentRoom.Persons)
                    {
                        if (ch.IsFearing(rch))
                        {
                            string buf = string.Empty;
                            switch (SmaugRandom.Bits(2))
                            {
                                case 0:
                                    buf = string.Format("Get away from me, {0}!", rch.Name);
                                    break;
                                case 1:
                                    buf = string.Format("Leave me be, {0}!", rch.Name);
                                    break;
                                case 2:
                                    buf = string.Format("{0} is trying to kill me!  Help!", rch.Name);
                                    break;
                                case 3:
                                    buf = string.Format("Someone save me from {0}!", rch.Name);
                                    break;
                            }

                            Yell.do_yell(ch, buf);
                            found = true;
                            break;
                        }
                    }

                    if (found)
                        Commands.Movement.Move.move_char(ch, exit, 0);
                }
            }

            // trworld_dispose
        }

        private static void Scavenge(CharacterInstance ch)
        {
            int max = 1;
            ObjectInstance best = null;

            foreach (ObjectInstance obj in ch.CurrentRoom.Contents)
            {
                if (obj.ExtraFlags.IsSet(ItemExtraFlags.Prototype) && !ch.Act.IsSet(ActFlags.Prototype))
                    continue;
                if (obj.WearFlags.IsSet(ItemWearFlags.Take) && obj.Cost > max &&
                    !obj.ExtraFlags.IsSet(ItemExtraFlags.Buried))
                {
                    best = obj;
                    max = obj.Cost;
                }
            }

            if (best != null)
            {
                best.InRoom.FromRoom(best);
                best.ToCharacter(ch);
                comm.act(ATTypes.AT_ACTION, "$n gets $p.", ch, best, null, ToTypes.Room);
            }
        }

        public static void char_calendar_update()
        {
            //lc = trworld_create(TR_CHAR_WORLD_BACK);

            foreach (CharacterInstance ch in DatabaseManager.Instance.CHARACTERS.Values)
            {
                if (ch.IsNpc() || ch.IsImmortal())
                    continue;

                ch.GainCondition(ConditionTypes.Drunk, -1);

                if (ch.CurrentRoom != null && ch.Level > 3)
                {
                    RaceData race = DatabaseManager.Instance.GetRace(ch.CurrentRace);
                    ch.GainCondition(ConditionTypes.Full, -1 + race.HungerMod);

                    ThirstAttribute attrib = ch.CurrentRoom.SectorType.GetAttribute<ThirstAttribute>();
                    int modValue = (attrib == null ? -1 : attrib.ModValue) + race.ThirstMod;

                    ch.GainCondition(ConditionTypes.Thirsty, modValue);
                }
            }

            // trworld_dispose
        }

        public static void char_update()
        {
            //lc = trworld_create(TR_CHAR_WORLD_BACK)

            foreach (CharacterInstance ch in DatabaseManager.Instance.CHARACTERS.Values)
            {
                handler.CurrentCharacter = ch;

                if (!ch.IsNpc())
                    mud_prog.rprog_random_trigger(ch);
                if (ch.CharDied())
                    continue;
                
                if (ch.IsNpc())
                    mud_prog.mprog_time_trigger(ch);
                if (ch.CharDied())
                    continue;

                mud_prog.rprog_time_trigger(ch);
                if (ch.CharDied())
                    continue;

                CharacterInstance ch_save = null;
                if (!ch.IsNpc() && (ch.Descriptor == null || ch.Descriptor.ConnectionStatus == ConnectionTypes.Playing)
                    && ch.Level >= 2 && CheckSaveFrequency(ch))
                    ch_save = ch;

                if ((int) ch.CurrentPosition >= (int) PositionTypes.Stunned)
                {
                    if (ch.CurrentHealth < ch.MaximumHealth)
                        ch.CurrentHealth += ch.HealthGain();
                    if (ch.CurrentMana < ch.MaximumMana)
                        ch.CurrentMana += ch.ManaGain();
                    if (ch.CurrentMovement < ch.MaximumMovement)
                        ch.CurrentMovement += ch.MovementGain();
                }

                if (ch.CurrentPosition == PositionTypes.Stunned)
                    ch.UpdatePositionByCurrentHealth();

                // TODO Variables

                if (ch.CurrentMorph != null)
                {
                    if (ch.CurrentMorph.timer > 0)
                    {
                        --ch.CurrentMorph.timer;
                        if (ch.CurrentMorph.timer == 0)
                            polymorph.do_unmorph_char(ch);
                    }
                }

                // TODO Nuisance

                if (!ch.IsNpc() && ch.Level < LevelConstants.ImmortalLevel)
                {
                    ObjectInstance obj = ch.GetEquippedItem(WearLocations.Light);
                    if (obj != null && obj.ItemType == ItemTypes.Light && obj.Value[2] > 0)
                        ProcessLightObject(ch, obj);

                    if (++ch.Timer >= 12)
                        ProcessIdle(ch);

                    if (ch.PlayerData.GetConditionValue(ConditionTypes.Drunk) > 8)
                        ch.WorsenMentalState(ch.PlayerData.GetConditionValue(ConditionTypes.Drunk)/8);

                    if (ch.PlayerData.GetConditionValue(ConditionTypes.Full) > 1)
                    {
                        IEnumerable<MentalStateAttribute> attribs =
                            ch.CurrentPosition.GetAttributes<MentalStateAttribute>();
                        if (attribs.Any())
                        {
                            MentalStateAttribute attrib =
                                attribs.FirstOrDefault(x => x.Condition.HasFlag(ConditionTypes.Full));
                            ch.ImproveMentalState(attrib == null ? 1 : attrib.ModValue);
                        }
                    }

                    if (ch.PlayerData.GetConditionValue(ConditionTypes.Thirsty) > 1)
                    {
                        IEnumerable<MentalStateAttribute> attribs =
                            ch.CurrentPosition.GetAttributes<MentalStateAttribute>();
                        if (attribs.Any())
                        {
                            MentalStateAttribute attrib =
                                attribs.FirstOrDefault(x => x.Condition.HasFlag(ConditionTypes.Thirsty));
                            ch.ImproveMentalState(attrib == null ? 1 : attrib.ModValue);
                        }
                    }

                    ch.CheckAlignment();
                    ch.GainCondition(ConditionTypes.Drunk, -1);

                    RaceData race = DatabaseManager.Instance.GetRace(ch.CurrentRace);
                    ch.GainCondition(ConditionTypes.Full, -1 + race.HungerMod);

                    if (ch.IsVampire() && ch.Level >= 10)
                    {
                        if (GameManager.Instance.GameTime.Hour < 21 && GameManager.Instance.GameTime.Hour >= 10)
                            ch.GainCondition(ConditionTypes.Bloodthirsty, -1);
                    }

                    if (ch.CanPKill() && ch.PlayerData.GetConditionValue(ConditionTypes.Thirsty) - 9 > 10)
                        ch.GainCondition(ConditionTypes.Thirsty, -9);

                    // TODO Nuisance
                }

                if (!ch.IsNpc() && !ch.IsImmortal() && ch.PlayerData.release_date > DateTime.MinValue &&
                    ch.PlayerData.release_date <= DateTime.Now)
                {
                    RoomTemplate location;
                    if (ch.PlayerData.Clan != null)
                        location = DatabaseManager.Instance.ROOMS.Get(ch.PlayerData.Clan.RecallRoom);
                    else
                        location = DatabaseManager.Instance.ROOMS.Get(VnumConstants.ROOM_VNUM_TEMPLE);

                    if (location == null)
                        location = ch.CurrentRoom;

                    ch.CurrentRoom.FromRoom(ch);
                    location.ToRoom(ch);
                    color.send_to_char("The gods have released you from hell as your sentence is up!", ch);
                    Look.do_look(ch, "auto");
                    ch.PlayerData.helled_by = string.Empty;
                    ch.PlayerData.release_date = DateTime.MinValue;
                    save.save_char_obj(ch);
                }

                if (!ch.CharDied())
                {
                    if (ch.IsAffected(AffectedByTypes.Poison))
                    {
                        comm.act(ATTypes.AT_POISON, "$n shivers and suffers.", ch, null, null, ToTypes.Room);
                        comm.act(ATTypes.AT_POISON, "You shiver and suffer.", ch, null, null, ToTypes.Character);

                        int minMentalState = CalculateMinMentalStateWhilePoisoned(ch);
                        ch.MentalState = 20.GetNumberThatIsBetween(minMentalState, 100);
                        ch.CauseDamageTo(ch, 6, DatabaseManager.Instance.LookupSkill("poison"));
                    }
                    else if (ch.CurrentPosition == PositionTypes.Incapacitated)
                        ch.CauseDamageTo(ch, 1, Program.TYPE_UNDEFINED);
                    else if (ch.CurrentPosition == PositionTypes.Mortal)
                        ch.CauseDamageTo(ch, 4, Program.TYPE_UNDEFINED);
                    if (ch.CharDied())
                        continue;

                    if (ch.IsAffected(AffectedByTypes.RecurringSpell))
                    {
                        bool died = false;
                        bool found = false;
                        foreach (AffectData paf in ch.Affects.Where(x => x.Location == ApplyTypes.RecurringSpell))
                        {
                            found = true;
                            if (Macros.IS_VALID_SN(paf.Modifier))
                            {
                                SkillData skill = DatabaseManager.Instance.SKILLS.Get(paf.Modifier);
                                if (skill == null || skill.Type != SkillTypes.Spell)
                                    continue;

                                ReturnTypes retCode = skill.SpellFunction.Value.Invoke(paf.Modifier, ch.Level, ch, ch);
                                if (retCode == ReturnTypes.CharacterDied || ch.CharDied())
                                {
                                    died = true;
                                    break;
                                }
                            }
                        }

                        if (died) continue;
                        if (!found)
                            ch.AffectedBy.RemoveBit(AffectedByTypes.RecurringSpell);
                    }

                    if (ch.MentalState >= 30)
                    {
                        int val = (ch.MentalState + 5)/10;
                        if (HighMentalStateTable.ContainsKey(val))
                        {
                            color.send_to_char(HighMentalStateTable[val].Key, ch);
                            comm.act(ATTypes.AT_ACTION, HighMentalStateTable[val].Value, ch, null, null, ToTypes.Room);
                        }
                    }

                    if (ch.MentalState <= -30)
                    {
                        int val = (Math.Abs(ch.MentalState) + 5)/10;
                        if (LowMentalStateTable.ContainsKey(val))
                        {
                            if (val > 7)
                            {
                                if ((int) ch.CurrentPosition > (int) PositionTypes.Sleeping)
                                {
                                    if ((ch.CurrentPosition == PositionTypes.Standing ||
                                         (int) ch.CurrentPosition < (int) PositionTypes.Fighting) &&
                                        (SmaugRandom.D100() + ((100 - (val*10)) + 10) < Math.Abs(ch.MentalState)))
                                        Commands.Movement.Sleep.do_sleep(ch, string.Empty);
                                    else
                                        color.send_to_char(LowMentalStateTable[val], ch);
                                }
                            }
                            else
                            {
                                if ((int) ch.CurrentPosition > (int) PositionTypes.Resting)
                                    color.send_to_char(LowMentalStateTable[val], ch);
                            }
                        }
                    }

                    if (ch.Timer > 24)
                        Commands.Quit.do_quit(ch, string.Empty);
                    else if (ch == ch_save && GameManager.Instance.SystemData.SaveFlags.IsSet(AutoSaveFlags.Auto))
                        save.save_char_obj(ch);
                }
            }

            //trowlrd_dispose
        }

        private static readonly Dictionary<int, string> LowMentalStateTable = new Dictionary<int, string>
        {
            {3, "You could use a rest."},
            {4, "You feel tired."},
            {5, "You feel sleepy."},
            {6, "You feel sedated."},
            {7, "You feel very unmotivated."},
            {8, "You're extremely drowsy."},
            {9, "You can barely keep your eyes open."},
            {10, "You're barely conscious."}
        };

        private static readonly Dictionary<int, KeyValuePair<string, string>> HighMentalStateTable = 
            new Dictionary<int, KeyValuePair<string, string>>
        {
            {3, new KeyValuePair<string, string>("You feel feverish.", "$n looks kind of out of it.")},
            {4, new KeyValuePair<string, string>("You do not feel well at all.", "$n doesn't look too great.")},
            {5, new KeyValuePair<string, string>("You need help!", "$n looks like $e could use your help.")},
            {6, new KeyValuePair<string, string>("Seekest thou a cleric.", "Someone should fetch a healer for $n.")},
            {7, new KeyValuePair<string, string>("You feel reality slipping away...",
                    "$n doesn't appear to be aware of what's going on.")},
            {8, new KeyValuePair<string, string>("You begin to understand... everything.",
                    "$n starts ranting like a madman!")},
            {9, new KeyValuePair<string, string>("You are ONE with the universe.",
                    "$n is ranting on about 'the answer', 'ONE', and other mumbo-jumbo...")},
            {10, new KeyValuePair<string, string>("You feel the end is near.",
                    "$n is muttering and ranting in tongues...")}
        };

        private static int CalculateMinMentalStateWhilePoisoned(CharacterInstance ch)
        {
            if (ch.IsNpc())
                return 2;
            if (ch.IsPKill())
                return 3;
            return 4;
        }

        private static void ProcessIdle(CharacterInstance ch)
        {
            if (!ch.IsIdle())
            {
                if (ch.CurrentFighting != null)
                    ch.StopFighting(true);

                comm.act(ATTypes.AT_ACTION, "$n disappears into the void.", ch, null, null, ToTypes.Room);
                color.send_to_char("You disappear into the void.", ch);

                if (GameManager.Instance.SystemData.SaveFlags.IsSet(AutoSaveFlags.Idle))
                    save.save_char_obj(ch);

                ch.PlayerData.Flags.SetBit(PCFlags.Idle);
                ch.CurrentRoom.FromRoom(ch);

                RoomTemplate room = DatabaseManager.Instance.GetEntity<RoomTemplate>(VnumConstants.ROOM_VNUM_LIMBO);
                room.ToRoom(ch);
            }
        }

        private static void ProcessLightObject(CharacterInstance ch, ObjectInstance obj)
        {
            if (--obj.Value[2] == 0 && ch.CurrentRoom != null)
            {
                ch.CurrentRoom.Light -= obj.Count;
                if (ch.CurrentRoom.Light < 0)
                    ch.CurrentRoom.Light = 0;

                comm.act(ATTypes.AT_ACTION, "$p goes out.", ch, obj, null, ToTypes.Character);
                comm.act(ATTypes.AT_ACTION, "$p goes out.", ch, obj, null, ToTypes.Room);

                if (obj == handler.CurrentObject)
                    handler.GlobalObjectCode = ReturnTypes.ObjectExpired;
                handler.extract_obj(obj);
            }
        }

        private static bool CheckSaveFrequency(CharacterInstance ch)
        {
            return ch.PlayedDuration > GameManager.Instance.SystemData.SaveFrequency;
        }

        public static void obj_update()
        {
            // lc = trworld_create(TR_OBJ_WORLD_BACK);

            foreach (ObjectInstance obj in DatabaseManager.Instance.OBJECTS.Values)
            {
                handler.CurrentObject = obj;

                if (obj.CarriedBy != null)
                    mud_prog.oprog_random_trigger(obj);
                else if (obj.InRoom != null && obj.InRoom.Area.NumberOfPlayers > 0)
                    mud_prog.oprog_random_trigger(obj);

                if (handler.obj_extracted(obj))
                    continue;

                if (obj.ItemType == ItemTypes.Pipe)
                    UpdatePipe(obj);

                if (obj.ItemType == ItemTypes.PlayerCorpse || obj.ItemType == ItemTypes.NpcCorpse)
                    UpdateCorpse(obj);

                if (obj.ExtraFlags.IsSet(ItemExtraFlags.Inventory))
                    continue;

                if (obj.ExtraFlags.IsSet(ItemExtraFlags.GroundRot) && obj.InRoom == null)
                    continue;

                if (obj.Timer <= 0 || --obj.Timer > 0)
                    continue;

                ATTypes AT_TEMP = ATTypes.AT_PLAIN;
                string message = "$p mysteriously vanishes.";

                if (ObjectExpireTable.ContainsKey(obj.ItemType))
                {
                    message = ObjectExpireTable[obj.ItemType].Key;
                    AT_TEMP = ObjectExpireTable[obj.ItemType].Value;

                    if (obj.ItemType == ItemTypes.Portal)
                    {
                        remove_portal(obj);
                        obj.ItemType = ItemTypes.Trash;
                    }
                }

                if (obj.CarriedBy != null)
                    comm.act(AT_TEMP, message, obj.CarriedBy, obj, null, ToTypes.Character);
                else if (obj.InRoom != null && obj.InRoom.Persons.Any() && !obj.ExtraFlags.IsSet(ItemExtraFlags.Buried))
                {
                    CharacterInstance rch = obj.InRoom.Persons.FirstOrDefault();
                    comm.act(AT_TEMP, message, rch, obj, null, ToTypes.Room);
                    comm.act(AT_TEMP, message, rch, obj, null, ToTypes.Character);
                }

                if (obj == handler.CurrentObject)
                    handler.GlobalObjectCode = ReturnTypes.ObjectExpired;
                handler.extract_obj(obj);
            }

            //trworld_dispose
        }

        private static Dictionary<ItemTypes, KeyValuePair<string, ATTypes>> ObjectExpireTable =
            new Dictionary<ItemTypes, KeyValuePair<string, ATTypes>>
        {
            {ItemTypes.Container, new KeyValuePair<string, ATTypes>("$p falls apart, tattered from age.", ATTypes.AT_OBJECT)},
            {ItemTypes.Portal, new KeyValuePair<string, ATTypes>("$p unravels and winks from existence.", ATTypes.AT_MAGIC)},
            {ItemTypes.Fountain, new KeyValuePair<string, ATTypes>("$p dries up.", ATTypes.AT_BLUE)},
            {ItemTypes.NpcCorpse, new KeyValuePair<string, ATTypes>("$p decays into dust and blows away.", ATTypes.AT_OBJECT)},
            {ItemTypes.PlayerCorpse, new KeyValuePair<string, ATTypes>("$p is sucked into a swirling vortex of colors...", ATTypes.AT_MAGIC)},
            {ItemTypes.Cook, new KeyValuePair<string, ATTypes>("$p is devoured by a swarm of maggots.", ATTypes.AT_HUNGRY)},
            {ItemTypes.Food, new KeyValuePair<string, ATTypes>("$p is devoured by a swarm of maggots.", ATTypes.AT_HUNGRY)},
            {ItemTypes.Blood, new KeyValuePair<string, ATTypes>("$p slowly seeps into the ground.", ATTypes.AT_BLOOD)},
            {ItemTypes.BloodStain, new KeyValuePair<string, ATTypes>("$p dries up into flakes and blows away.", ATTypes.AT_BLOOD)},
            {ItemTypes.Scraps, new KeyValuePair<string, ATTypes>("$p crumble and decay into nothing.", ATTypes.AT_OBJECT)},
            {ItemTypes.Fire, new KeyValuePair<string, ATTypes>("$p burns out.", ATTypes.AT_FIRE)}
        };

        private static void UpdateCorpse(ObjectInstance obj)
        {
            int timer = 1.GetHighestOfTwoNumbers(obj.Timer - 1);
            if (obj.ItemType == ItemTypes.PlayerCorpse)
                timer = obj.Timer/8 + 1;

            if (obj.Timer > 0 && obj.Value[2] > timer)
            {
                handler.separate_obj(obj);
                obj.Value[2] = timer;

                string buf =
                    string.Format(
                        LookupManager.Instance.GetLookup("CorpseDescs", (timer - 1).GetLowestOfTwoNumbers(4)),
                        obj.ShortDescription);
                obj.Description = buf;
            }
        }

        private static void UpdatePipe(ObjectInstance obj)
        {
            if (!obj.Value[3].IsSet(PipeFlags.Lit))
            {
                obj.Value[3].RemoveBit(PipeFlags.Hot);
                return;
            }

            if (--obj.Value[1] <= 0)
            {
                obj.Value[1] = 0;
                obj.Value[3].RemoveBit(PipeFlags.Lit);
            }
            else if (obj.Value[3].IsSet(PipeFlags.Hot))
                obj.Value[3].RemoveBit(PipeFlags.Hot);
            else
            {
                if (obj.Value[3].IsSet(PipeFlags.GoingOut))
                {
                    obj.Value[3].RemoveBit(PipeFlags.Lit);
                    obj.Value[3].RemoveBit(PipeFlags.GoingOut);
                }
                else
                    obj.Value[3].RemoveBit(PipeFlags.GoingOut);
            }

            if (!obj.Value[3].IsSet(PipeFlags.Lit))
                obj.Value[3].SetBit(PipeFlags.FullOfAsh);
        }

        private static int _charCounter = 0;
        public static void char_check()
        {
            _charCounter = (_charCounter + 1)%GameConstants.GetSystemValue<int>("SecondsPerTick");

            // lc1 = trworld_create(TR_CHAR_WORLD_FORW);

            foreach (CharacterInstance ch in DatabaseManager.Instance.CHARACTERS.Values)
            {
                handler.set_cur_char(ch);
                ch.WillFall(0);
                if (ch.CharDied())
                    continue;

                if (ch.IsNpc())
                    CheckNpc(ch);
                else
                    CheckPlayer(ch);
            }

            //trworld_dispose
        }

        private static void CheckPlayer(CharacterInstance ch)
        {
            throw new NotImplementedException();
        }

        private static void CheckNpc(CharacterInstance ch)
        {
            if ((_charCounter & 1) > 0)
                return;

            if (ch.Act.IsSet(ActFlags.Running))
            {
                if (!ch.Act.IsSet(ActFlags.Sentinel)
                    && ch.CurrentPosition == PositionTypes.Standing
                    && !ch.Act.IsSet(ActFlags.Mounted)
                    && ch.CurrentFighting == null
                    && ch.CurrentHunting != null)
                {
                    Macros.WAIT_STATE(ch, 2*GameConstants.GetSystemValue<int>("PulseViolence"));
                    track.hunt_victim(ch);
                    return;
                }

                if (ch.SpecialFunction != null)
                {
                    if (ch.SpecialFunction.Value.Invoke(ch))
                        return;
                    if (ch.CharDied())
                        return;
                }

                if (!ch.Act.IsSet(ActFlags.Sentinel)
                    && ch.CurrentPosition == PositionTypes.Standing
                    && !ch.Act.IsSet(ActFlags.Mounted)
                    && !ch.Act.IsSet(ActFlags.Prototype))
                {
                    int door = SmaugRandom.Bits(4);
                    if (door >= 9)
                        return;

                    ExitData exit = ch.CurrentRoom.GetExit(door);
                    if (exit == null)
                        return;

                    if (exit.Flags.IsSet(ExitFlags.Closed))
                        return;

                    RoomTemplate room = exit.GetDestination();
                    if (room == null)
                        return;

                    if (room.Flags.IsSet(RoomFlags.NoMob) || room.Flags.IsSet(RoomFlags.Death))
                        return;

                    if (ch.Act.IsSet(ActFlags.StayArea) && ch.CurrentRoom.Area != room.Area)
                        return;

                    Move.move_char(ch, exit, 0);
                }
            }
        }

        public static void aggr_update()
        {
            // TODO
        }

        public static void drunk_randoms(CharacterInstance ch)
        {
            // TODO
        }

        public static void hallucinations(CharacterInstance ch)
        {
            // TODO
        }

        public static void tele_update()
        {
            // TODO
        }

        public static void auth_update()
        {
            // TODO
        }

        public static void update_handler()
        {
            // TODO
        }

        public static void remove_portal(ObjectInstance portal)
        {
            if (portal == null)
                 throw new ArgumentNullException("portal");

            RoomTemplate fromRoom = portal.InRoom;
            if (fromRoom == null)
                throw new InvalidDataException("Portal has no room");

            ExitData exit = fromRoom.Exits.FirstOrDefault(xit => xit.Flags.IsSet(ExitFlags.Portal));
            if (exit == null)
            {
                // TODO Exception, log it
                return;
            }

            if (exit.Direction != DirectionTypes.Portal)
            {
                // TODO Exception, log it
            }

            if (exit.GetDestination(DatabaseManager.Instance) == null)
            {
                // TODO Exception, log it
            }

            handler.extract_exit(fromRoom, exit);
        }

        public static void reboot_check(DateTime reset)
        {
            // TODO
        }

        public static void auction_update()
        {
            // TODO
        }

        public static void subtract_times(DateTime etime, DateTime sttime)
        {
            // TODO
        }

        public static void time_update()
        {
            // TODO
        }

        public static void hint_update()
        {
            // TODO
        }
    }
}