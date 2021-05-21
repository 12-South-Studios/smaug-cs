using SmaugCS.Commands;
using SmaugCS.Commands.Movement;
using SmaugCS.Commands.Polymorph;
using SmaugCS.Common;
using SmaugCS.Common.Enumerations;
using SmaugCS.Constants;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Mobile;
using SmaugCS.Extensions.Objects;
using SmaugCS.Extensions.Player;
using SmaugCS.Managers;
using SmaugCS.MudProgs;
using SmaugCS.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SmaugCS
{
    public static class update
    {
        public static void mobile_update()
        {
            //lc = trworld_create(TR_CHAR_WORLD_BACK);

            foreach (var ch in RepositoryManager.Instance.CHARACTERS.Values)
            {
                if (ch is PlayerInstance)
                    ((PlayerInstance)ch).ProcessUpdate();
                else
                    (ch as MobileInstance)?.ProcessUpdate(RepositoryManager.Instance);
            }

            // trworld_dispose
        }

        public static void char_calendar_update()
        {
            //lc = trworld_create(TR_CHAR_WORLD_BACK);

            foreach (var ch in RepositoryManager.Instance.CHARACTERS.Values.Where(ch => !ch.IsNpc() && !ch.IsImmortal()))
            {
                ((PlayerInstance)ch).GainCondition(ConditionTypes.Drunk, -1);

                if (ch.CurrentRoom != null && ch.Level > 3)
                {
                    var race = RepositoryManager.Instance.GetRace(ch.CurrentRace);
                    ((PlayerInstance)ch).GainCondition(ConditionTypes.Full, -1 + race.HungerMod);

                    var attrib = ch.CurrentRoom.SectorType.GetAttribute<ThirstAttribute>();
                    var modValue = (attrib?.ModValue ?? -1) + race.ThirstMod;

                    ((PlayerInstance)ch).GainCondition(ConditionTypes.Thirsty, modValue);
                }
            }

            // trworld_dispose
        }

        public static void char_update()
        {
            //lc = trworld_create(TR_CHAR_WORLD_BACK)

            foreach (var ch in RepositoryManager.Instance.CHARACTERS.Values)
            {
                handler.CurrentCharacter = ch;

                if (!ch.IsNpc())
                    MudProgHandler.ExecuteRoomProg(MudProgTypes.Random, ch);
                if (ch.CharDied())
                    continue;

                if (ch.IsNpc())
                    MudProgHandler.ExecuteMobileProg(MudProgTypes.Time, ch);
                if (ch.CharDied())
                    continue;

                MudProgHandler.ExecuteRoomProg(MudProgTypes.Time, ch);
                if (ch.CharDied())
                    continue;

                CharacterInstance ch_save = null;
                if (!ch.IsNpc() && (((PlayerInstance)ch).Descriptor == null || ((PlayerInstance)ch).Descriptor.ConnectionStatus == ConnectionTypes.Playing)
                    && ch.Level >= 2 && CheckSaveFrequency(ch))
                    ch_save = ch;

                if ((int)ch.CurrentPosition >= (int)PositionTypes.Stunned)
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

                if (ch.CurrentMorph?.timer > 0)
                {
                    --ch.CurrentMorph.timer;
                    if (ch.CurrentMorph.timer == 0)
                        UnmorphChar.do_unmorph_char(ch, string.Empty);
                }

                // TODO Nuisance

                if (!ch.IsNpc() && ch.Level < LevelConstants.ImmortalLevel)
                {
                    var obj = ch.GetEquippedItem(WearLocations.Light);
                    if (obj != null && obj.ItemType == ItemTypes.Light && obj.Value.ToList()[2] > 0)
                        ProcessLightObject(ch, obj);

                    if (++ch.Timer >= 12)
                        ProcessIdle(ch);

                    if (((PlayerInstance)ch).PlayerData.GetConditionValue(ConditionTypes.Drunk) > 8)
                        ((PlayerInstance)ch).WorsenMentalState(((PlayerInstance)ch).PlayerData.GetConditionValue(ConditionTypes.Drunk) / 8);

                    if (((PlayerInstance)ch).PlayerData.GetConditionValue(ConditionTypes.Full) > 1)
                    {
                        // todo fix this
                        //IEnumerable<MentalStateAttribute> attribs =
                        //    ch.CurrentPosition.GetAttributes<MentalStateAttribute>();
                        //if (attribs.Any())
                        //{
                        //    MentalStateAttribute attrib =
                        //        attribs.FirstOrDefault(x => x.Condition.HasFlag(ConditionTypes.Full));
                        //    ch.ImproveMentalState(attrib == null ? 1 : attrib.ModValue);
                        //}
                    }

                    if (((PlayerInstance)ch).PlayerData.GetConditionValue(ConditionTypes.Thirsty) > 1)
                    {
                        // todo fix this
                        //IEnumerable<MentalStateAttribute> attribs =
                        //    ch.CurrentPosition.GetAttributes<MentalStateAttribute>();
                        //if (attribs.Any())
                        //{
                        //    MentalStateAttribute attrib =
                        //        attribs.FirstOrDefault(x => x.Condition.HasFlag(ConditionTypes.Thirsty));
                        //    ch.ImproveMentalState(attrib == null ? 1 : attrib.ModValue);
                        //}
                    }

                    ch.CheckAlignment();
                    ((PlayerInstance)ch).GainCondition(ConditionTypes.Drunk, -1);

                    var race = RepositoryManager.Instance.GetRace(ch.CurrentRace);
                    ((PlayerInstance)ch).GainCondition(ConditionTypes.Full, -1 + race.HungerMod);

                    if (ch.IsVampire() && ch.Level >= 10)
                    {
                        if (GameManager.Instance.GameTime.Hour < 21 && GameManager.Instance.GameTime.Hour >= 10)
                            ((PlayerInstance)ch).GainCondition(ConditionTypes.Bloodthirsty, -1);
                    }

                    if (ch.CanPKill() && ((PlayerInstance)ch).PlayerData.GetConditionValue(ConditionTypes.Thirsty) - 9 > 10)
                        ((PlayerInstance)ch).GainCondition(ConditionTypes.Thirsty, -9);

                    // TODO Nuisance
                }

                if (!ch.IsNpc() && !ch.IsImmortal() && ((PlayerInstance)ch).PlayerData.release_date > DateTime.MinValue &&
                    ((PlayerInstance)ch).PlayerData.release_date <= DateTime.Now)
                {
                    var location =
                        RepositoryManager.Instance.ROOMS.Get(((PlayerInstance)ch).PlayerData.Clan?.RecallRoom ??
                                                             VnumConstants.ROOM_VNUM_TEMPLE) ?? ch.CurrentRoom;

                    ch.CurrentRoom.RemoveFrom(ch);
                    location.AddTo(ch);
                    ch.SendTo("The gods have released you from hell as your sentence is up!");
                    Look.do_look(ch, "auto");
                    ((PlayerInstance)ch).PlayerData.helled_by = string.Empty;
                    ((PlayerInstance)ch).PlayerData.release_date = DateTime.MinValue;
                    save.save_char_obj(ch);
                }

                if (!ch.CharDied())
                {
                    if (ch.IsAffected(AffectedByTypes.Poison))
                    {
                        comm.act(ATTypes.AT_POISON, "$n shivers and suffers.", ch, null, null, ToTypes.Room);
                        comm.act(ATTypes.AT_POISON, "You shiver and suffer.", ch, null, null, ToTypes.Character);

                        var minMentalState = CalculateMinMentalStateWhilePoisoned(ch);
                        ch.MentalState = 20.GetNumberThatIsBetween(minMentalState, 100);
                        ch.CauseDamageTo(ch, 6, RepositoryManager.Instance.LookupSkill("poison"));
                    }
                    else switch (ch.CurrentPosition)
                        {
                            case PositionTypes.Incapacitated:
                                ch.CauseDamageTo(ch, 1, Program.TYPE_UNDEFINED);
                                break;
                            case PositionTypes.Mortal:
                                ch.CauseDamageTo(ch, 4, Program.TYPE_UNDEFINED);
                                break;
                        }
                    if (ch.CharDied())
                        continue;

                    if (ch.IsAffected(AffectedByTypes.RecurringSpell))
                    {
                        var died = false;
                        var found = false;
                        foreach (var paf in ch.Affects.Where(x => x.Location == ApplyTypes.RecurringSpell))
                        {
                            found = true;
                            if (Macros.IS_VALID_SN(paf.Modifier))
                            {
                                var skill = RepositoryManager.Instance.SKILLS.Get(paf.Modifier);
                                if (skill == null || skill.Type != SkillTypes.Spell)
                                    continue;

                                var retCode = skill.SpellFunction.Value.Invoke(paf.Modifier, ch.Level, ch, ch);
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
                        var val = (ch.MentalState + 5) / 10;
                        if (HighMentalStateTable.ContainsKey(val))
                        {
                            ch.SendTo(HighMentalStateTable[val].Key);
                            comm.act(ATTypes.AT_ACTION, HighMentalStateTable[val].Value, ch, null, null, ToTypes.Room);
                        }
                    }

                    if (ch.MentalState <= -30)
                    {
                        var val = (Math.Abs(ch.MentalState) + 5) / 10;
                        if (LowMentalStateTable.ContainsKey(val))
                        {
                            if (val > 7)
                            {
                                if ((int)ch.CurrentPosition > (int)PositionTypes.Sleeping)
                                {
                                    if ((ch.CurrentPosition == PositionTypes.Standing ||
                                         (int)ch.CurrentPosition < (int)PositionTypes.Fighting) &&
                                        (SmaugRandom.D100() + (100 - val * 10) + 10 < Math.Abs(ch.MentalState)))
                                        Sleep.do_sleep(ch, string.Empty);
                                    else
                                        ch.SendTo(LowMentalStateTable[val]);
                                }
                            }
                            else
                            {
                                if ((int)ch.CurrentPosition > (int)PositionTypes.Resting)
                                    ch.SendTo(LowMentalStateTable[val]);
                            }
                        }
                    }

                    if (ch.Timer > 24)
                        Quit.do_quit(ch, string.Empty);
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
            return ch.IsPKill() ? 3 : 4;
        }

        private static void ProcessIdle(CharacterInstance ch)
        {
            if (!ch.IsIdle())
            {
                if (ch.CurrentFighting != null)
                    ch.StopFighting(true);

                comm.act(ATTypes.AT_ACTION, "$n disappears into the void.", ch, null, null, ToTypes.Room);
                ch.SendTo("You disappear into the void.");

                if (GameManager.Instance.SystemData.SaveFlags.IsSet(AutoSaveFlags.Idle))
                    save.save_char_obj(ch);

                ((PlayerInstance)ch).PlayerData.Flags.SetBit(PCFlags.Idle);
                ch.CurrentRoom.RemoveFrom(ch);

                var room = RepositoryManager.Instance.GetEntity<RoomTemplate>(VnumConstants.ROOM_VNUM_LIMBO);
                room.AddTo(ch);
            }
        }

        private static void ProcessLightObject(CharacterInstance ch, ObjectInstance obj)
        {
            if (--obj.Value.ToList()[2] == 0 && ch.CurrentRoom != null)
            {
                comm.act(ATTypes.AT_ACTION, "$p goes out.", ch, obj, null, ToTypes.Character);
                comm.act(ATTypes.AT_ACTION, "$p goes out.", ch, obj, null, ToTypes.Room);

                if (obj == handler.CurrentObject)
                    handler.GlobalObjectCode = ReturnTypes.ObjectExpired;
                obj.Extract();
            }
        }

        private static bool CheckSaveFrequency(CharacterInstance ch)
        {
            return ((PlayerInstance)ch).PlayedDuration > GameManager.Instance.SystemData.SaveFrequency;
        }

        public static void obj_update()
        {
            // lc = trworld_create(TR_OBJ_WORLD_BACK);

            foreach (var obj in RepositoryManager.Instance.OBJECTS.Values)
            {
                handler.CurrentObject = obj;

                if (obj.CarriedBy != null)
                    MudProgHandler.ExecuteObjectProg(MudProgTypes.Random, obj);
                else if (obj.InRoom != null && obj.InRoom.Area.NumberOfPlayers > 0)
                    MudProgHandler.ExecuteObjectProg(MudProgTypes.Random, obj);

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

                var AT_TEMP = ATTypes.AT_PLAIN;
                var message = "$p mysteriously vanishes.";

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
                    var rch = obj.InRoom.Persons.FirstOrDefault();
                    comm.act(AT_TEMP, message, rch, obj, null, ToTypes.Room);
                    comm.act(AT_TEMP, message, rch, obj, null, ToTypes.Character);
                }

                if (obj == handler.CurrentObject)
                    handler.GlobalObjectCode = ReturnTypes.ObjectExpired;
                obj.Extract();
            }

            //trworld_dispose
        }

        private static readonly Dictionary<ItemTypes, KeyValuePair<string, ATTypes>> ObjectExpireTable =
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
            var timer = 1.GetHighestOfTwoNumbers(obj.Timer - 1);
            if (obj.ItemType == ItemTypes.PlayerCorpse)
                timer = obj.Timer / 8 + 1;

            if (obj.Timer > 0 && obj.Value.ToList()[2] > timer)
            {
                obj.Split();
                obj.Value.ToList()[2] = timer;

                var buf =
                    string.Format(
                        LookupManager.Instance.GetLookup("CorpseDescs", (timer - 1).GetLowestOfTwoNumbers(4)),
                        obj.ShortDescription);
                obj.Description = buf;
            }
        }

        private static void UpdatePipe(ObjectInstance obj)
        {
            if (!obj.Value.ToList()[3].IsSet(PipeFlags.Lit))
            {
                obj.Value.ToList()[3].RemoveBit(PipeFlags.Hot);
                return;
            }

            if (--obj.Value.ToList()[1] <= 0)
            {
                obj.Value.ToList()[1] = 0;
                obj.Value.ToList()[3].RemoveBit(PipeFlags.Lit);
            }
            else if (obj.Value.ToList()[3].IsSet(PipeFlags.Hot))
                obj.Value.ToList()[3].RemoveBit(PipeFlags.Hot);
            else
            {
                if (obj.Value.ToList()[3].IsSet(PipeFlags.GoingOut))
                {
                    obj.Value.ToList()[3].RemoveBit(PipeFlags.Lit);
                    obj.Value.ToList()[3].RemoveBit(PipeFlags.GoingOut);
                }
                else
                    obj.Value.ToList()[3].RemoveBit(PipeFlags.GoingOut);
            }

            if (!obj.Value.ToList()[3].IsSet(PipeFlags.Lit))
                obj.Value.ToList()[3].SetBit(PipeFlags.FullOfAsh);
        }

        private static int _charCounter = 0;
        public static void char_check()
        {
            _charCounter = (_charCounter + 1) % GameConstants.GetSystemValue<int>("SecondsPerTick");

            // lc1 = trworld_create(TR_CHAR_WORLD_FORW);

            foreach (var ch in RepositoryManager.Instance.CHARACTERS.Values)
            {
                handler.set_cur_char(ch);
                ch.WillFall(0);
                if (ch.CharDied())
                    continue;

                if (ch.IsNpc())
                    CheckNpc((MobileInstance)ch, RepositoryManager.Instance);
                else
                    CheckPlayer(ch);
            }

            //trworld_dispose
        }

        private static void CheckPlayer(CharacterInstance ch)
        {
            throw new NotImplementedException();
        }

        private static void CheckNpc(MobileInstance ch, IRepositoryManager dbManager)
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
                    Macros.WAIT_STATE(ch, 2 * GameConstants.GetSystemValue<int>("PulseViolence"));
                    track.hunt_victim(ch);
                    return;
                }

                if (ch.SpecialFunction != null)
                {
                    if (ch.SpecialFunction.Value.Invoke(ch, dbManager))
                        return;
                    if (ch.CharDied())
                        return;
                }

                if (!ch.Act.IsSet(ActFlags.Sentinel)
                    && ch.CurrentPosition == PositionTypes.Standing
                    && !ch.Act.IsSet(ActFlags.Mounted)
                    && !ch.Act.IsSet(ActFlags.Prototype))
                {
                    var door = SmaugRandom.Bits(4);
                    if (door >= 9)
                        return;

                    var exit = ch.CurrentRoom.GetExit(door);
                    if (exit == null)
                        return;

                    if (exit.Flags.IsSet(ExitFlags.Closed))
                        return;

                    var room = exit.GetDestination();
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
                throw new ArgumentNullException(nameof(portal));

            var fromRoom = portal.InRoom;
            if (fromRoom == null)
                throw new InvalidDataException("Portal has no room");

            var exit = fromRoom.Exits.FirstOrDefault(xit => xit.Flags.IsSet(ExitFlags.Portal));
            if (exit == null)
            {
                // TODO Exception, log it
                return;
            }

            if (exit.Direction != DirectionTypes.Portal)
            {
                // TODO Exception, log it
            }

            if (exit.GetDestination(RepositoryManager.Instance) == null)
            {
                // TODO Exception, log it
            }

            exit.Extract();
        }

        public static void reboot_check(DateTime? reset = null)
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
