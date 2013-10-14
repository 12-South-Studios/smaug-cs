using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Commands.PetsAndGroups;
using SmaugCS.Common;
using SmaugCS.Enums;
using SmaugCS.Objects;

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
                .Where(x => handler.can_see_obj(ch, x))
                .Where(x => Macros.CAN_WEAR(x, (int)ItemWearFlags.Take) && ch.Level < db.SystemData.GetMinimumLevel(PlayerPermissionTypes.LevelGetObjectNoTake))
                .Where(x => Macros.IS_OBJ_STAT(x, (int)ItemExtraFlags.Prototype) && ch.CanTakePrototype))
            {
                comm.act(ATTypes.AT_ACTION, "You get $p from $P", ch, content, corpse, ToTypes.Character);
                comm.act(ATTypes.AT_ACTION, "$n gets $p from $P", ch, content, corpse, ToTypes.Room);
                content.InObject.FromObject(content);
                handler.check_for_trap(ch, content, (int)TrapTriggerTypes.Get);
                if (handler.char_died(ch))
                    return false;

                mud_prog.oprog_get_trigger(ch, content);
                if (handler.char_died(ch))
                    return false;

                ch.CurrentCoin += content.Value[0] * content.Count;
                handler.extract_obj(content);
            }

            if (ch.CurrentCoin - oldgold > 1 && ch.Position > PositionTypes.Sleeping)
                Split.do_split(ch, string.Format("{0}", ch.CurrentCoin - oldgold));

            return true;
        }

        public static bool is_attack_supressed(CharacterInstance ch)
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

        public static bool is_wielding_poisoned(CharacterInstance ch)
        {
            if (UsedWeapon == null)
                return false;

            ObjectInstance obj = ch.GetEquippedItem(WearLocations.Wield);
            if (obj != null && UsedWeapon == obj &&
                Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Poisoned))
                return true;

            obj = ch.GetEquippedItem(WearLocations.DualWield);
            if (obj != null && UsedWeapon == obj &&
                Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Poisoned))
                return true;

            return false;
        }

        private static readonly Dictionary<SunPositionTypes, int> VampireArmorClassTable = new Dictionary<SunPositionTypes, int>()
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
                return VampireArmorClassTable.ContainsKey(db.GameTime.Sunlight)
                           ? VampireArmorClassTable[db.GameTime.Sunlight]
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

            foreach (CharacterInstance ch in db.CHARACTERS
                .Where(ch => !handler.char_died(ch)))
            {
                //// Experience gained during battle decreases as the battle drags on
                if (ch.CurrentFighting != null &&
                    (++ch.CurrentFighting.Duration % 24) == 0)
                    ch.CurrentFighting.Experience = ((ch.CurrentFighting.Experience * 9) / 10);

                foreach (TimerData timer in ch.Timers)
                    DecreaseExperienceInBattle(timer, ch);

                if (handler.char_died(ch))
                    continue;

                //// Spells that have durations less than 1 hour
                foreach (AffectData paf in ch.Affects)
                {
                    if (paf.Duration > 0)
                        paf.Duration--;
                    else if (paf.Duration == 0)
                    {

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
                    if (handler.char_died(ch))
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
        public static int multi_hit(CharacterInstance ch, CharacterInstance victim, int dt)
        {
            //// Add a timer to pkillers
            if (!ch.IsNpc() && !victim.IsNpc())
            {
                if (ch.Act.IsSet((int)PlayerFlags.Nice))
                    return (int)ReturnTypes.None;

                handler.add_timer(ch, (int)TimerTypes.RecentFight, 11, null, 0);
                handler.add_timer(victim, (int)TimerTypes.RecentFight, 11, null, 0);
            }

            if (is_attack_supressed(ch))
                return (int)ReturnTypes.None;

            if (ch.IsNpc() && ch.Act.IsSet((int)ActFlags.NoAttack))
                return 0;

            // TODO finish this

            return 0;
        }

        public static int weapon_prof_bonus_check(CharacterInstance ch, ObjectInstance wield, int gsn_ptr)
        {
            if (ch.IsNpc() && ch.Level <= 5 && wield == null)
                return 0;

            /*switch (wield.Value[3])
            {

            }*/


            return 0;
        }

        public static int off_shld_lvl(CharacterInstance ch, CharacterInstance victim)
        {
            int lvl = 0;

            if (!ch.IsNpc())
            {
                lvl = SmaugCS.Common.Check.Maximum(1, ch.Level - 10 / 2);
                if (SmaugCS.Common.SmaugRandom.Percent() + (victim.Level - lvl) < 40)
                {
                    if (ch.CanPKill() && victim.CanPKill())
                        return ch.Level;
                    return lvl;
                }
                return 0;
            }

            lvl = ch.Level / 2;
            return SmaugCS.Common.SmaugRandom.Percent() + (victim.Level - lvl) < 70 ? lvl : 0;
        }

        public static int one_hit(CharacterInstance ch, CharacterInstance victim, int dt)
        {
            // TODO
            return 0;
        }

        public static int projectile_hit(CharacterInstance ch, CharacterInstance victim, ObjectInstance wield, ObjectInstance projectile, short dist)
        {
            // TODO
            return 0;
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

        public static int damage(CharacterInstance ch, CharacterInstance victim, int dam, int dt)
        {
            // TODO
            return 0;
        }

        public static bool is_safe(CharacterInstance ch, CharacterInstance victim, bool show_messg)
        {
            // TODO
            return false;
        }

        public static bool legal_loot(CharacterInstance ch, CharacterInstance victim)
        {
            // TODO
            return false;
        }

        public static void check_killer(CharacterInstance ch, CharacterInstance victim)
        {
            // TODO
        }

        public static void check_attacker(CharacterInstance ch, CharacterInstance victim)
        {
            // TODO
        }

        public static void updatE_pos(CharacterInstance victim)
        {
            // TODO
        }

        public static void set_fighting(CharacterInstance ch, CharacterInstance victim)
        {
            // TODO
        }

        public static CharacterInstance who_fighting(CharacterInstance ch)
        {
            // TODO
            return null;
        }

        public static void free_fight(CharacterInstance ch)
        {
            // TODO
        }

        public static void stop_fighting(CharacterInstance ch, bool fBoth)
        {
            // TODO
        }

        public static void death_cry(CharacterInstance ch)
        {
            // TODO
        }

        public static ObjectInstance raw_kill(CharacterInstance ch, CharacterInstance victim)
        {
            // TODO
            return null;
        }

        public static void group_gain(CharacterInstance ch, CharacterInstance victim)
        {
            // TODO
        }

        public static int align_compute(CharacterInstance gch, CharacterInstance victim)
        {
            // TODO
            return 0;
        }

        public static int xp_compute(CharacterInstance gch, CharacterInstance victim)
        {
            // TODO
            return 0;
        }

        public static void new_dam_message(CharacterInstance ch, CharacterInstance victim, int dam, int dt, ObjectInstance obj)
        {
            // TODO
        }

        public static void dam_message(CharacterInstance ch, CharacterInstance victim, int dam, int dt)
        {
            // TODO
        }

        public static void do_kill(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_murde(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_murder(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static bool in_arena(CharacterInstance ch)
        {
            // TODO
            return false;
        }

        public static bool check_illegal_pk(CharacterInstance ch, CharacterInstance victim)
        {
            // TODO
            return false;
        }

        public static void do_flee(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_sla(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_slay(CharacterInstance ch, string argument)
        {
            // TODO
        }
    }
}
