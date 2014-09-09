﻿using System.Linq;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Exceptions;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Helpers;
using SmaugCS.Managers;

namespace SmaugCS.Commands.Liquids
{
    public static class Drink
    {
        private static int GetMaximumCondition()
        {
            return GameConstants.GetConstant<int>("MaximumConditionValue");
        }

        public static void do_drink(CharacterInstance ch, string argument)
        {
            string arg = argument.FirstWord();
            if (arg.EqualsIgnoreCase("from"))
                arg = arg.SecondWord();

            ObjectInstance obj = GetDrinkSource(ch, arg);
            if (obj == null)
                throw new ObjectNotFoundException(string.Format("Object {0} was not found.", arg));

            if (obj.Count > 1 && obj.ItemType != ItemTypes.Fountain)
                obj.Split();

            if (CheckFunctions.CheckIfTrue(ch,
                !ch.IsNpc() && ((PlayerInstance)ch).GetCondition(ConditionTypes.Drunk) > (GetMaximumCondition() - 8),
                "You fail to reach your mouth.  *Hic*")) return;

            DrinkFrom(ch, obj);

            int pulsesPerSecond = GameConstants.GetSystemValue<int>("PulsesPerSecond");
            Macros.WAIT_STATE(ch, ch.GetMyTarget() != null && ch.IsPKill() ? pulsesPerSecond/3 : pulsesPerSecond);
        }

        private static ObjectInstance GetDrinkSource(CharacterInstance ch, string arg)
        {
            ObjectInstance obj = string.IsNullOrEmpty(arg)
                ? ch.CurrentRoom.Contents.FirstOrDefault(x => x.ItemType == ItemTypes.Fountain)
                : ch.GetObjectOnMeOrInRoom(arg);

            return CheckFunctions.CheckIfNullObject(ch, obj,
                string.IsNullOrEmpty(arg) ? "Drink what?" : "You can't find it.") ? obj : obj;
        }

        private static void DrinkFrom(CharacterInstance ch, ObjectInstance obj)
        {
            switch (obj.ItemType)
            {
                case ItemTypes.Blood:
                    DrinkFromBlood(ch, obj);
                    break;
                case ItemTypes.Potion:
                    DrinkFromPotion(ch, obj);
                    break;
                case ItemTypes.Fountain:
                    DrinkFromFountain(ch, obj);
                    break;
                case ItemTypes.DrinkContainer:
                    DrinkFromContainer(ch, obj);
                    break;
                default:
                    comm.act(ATTypes.AT_ACTION, obj.CarriedBy == ch
                        ? "$n lifts $p up to $s mouth and tries to drink from it..."
                        : "$n gets down and tries to drink from $p... (Is $e feeling ok?)", ch, obj, null, ToTypes.Room);
                    comm.act(ATTypes.AT_ACTION, obj.CarriedBy == ch
                        ? "You bring $p up to your mouth and try to drink from it..."
                        : "You get down on the ground and try to drink from $p...", ch, obj, null, ToTypes.Character);
                    break;
            }
        }

        private static void DrinkFromContainer(CharacterInstance ch, ObjectInstance obj)
        {
            if (CheckFunctions.CheckIfTrue(ch, obj.Values.Quantity <= 0, "It is already empty.")) return;

            if (CheckFunctions.CheckIfTrue(ch,
                !ch.IsNpc() && (((PlayerInstance)ch).GetCondition(ConditionTypes.Full) == GetMaximumCondition()
                                || ((PlayerInstance)ch).GetCondition(ConditionTypes.Thirsty) == GetMaximumCondition()),
                "Your stomach is too full to drink more!")) return;

            LiquidData liquid = DatabaseManager.Instance.LIQUIDS.Get(obj.Values.LiquidID) ??
                                DatabaseManager.Instance.LIQUIDS.Get(0);

            if (!mud_prog.oprog_use_trigger(ch, obj, null, null))
            {
                comm.act(ATTypes.AT_ACTION, "$n drinks $T from $p.", ch, obj, liquid.ShortDescription, ToTypes.Room);
                comm.act(ATTypes.AT_ACTION, "You drink $T from $p.", ch, obj, liquid.ShortDescription, ToTypes.Character);
            }

            ((PlayerInstance)ch).GainCondition(ConditionTypes.Thirsty, liquid.GetMod(ConditionTypes.Thirsty));
            ((PlayerInstance)ch).GainCondition(ConditionTypes.Full, liquid.GetMod(ConditionTypes.Full));
            ((PlayerInstance)ch).GainCondition(ConditionTypes.Drunk, liquid.GetMod(ConditionTypes.Drunk));

            if (ch.IsVampire())
                ((PlayerInstance)ch).GainCondition(ConditionTypes.Bloodthirsty, liquid.GetMod(ConditionTypes.Bloodthirsty));

            if (liquid.Type == LiquidTypes.Poison)
                DrinkPoison(ch, obj);

            if (!ch.IsNpc())
            {
                EvaluateDrunkCondition(ch);
                EvaluateThirstCondition(ch);

                if (ch.IsVampire())
                    EvaluateBloodthirstCondition(ch);
                else if (!ch.IsVampire() && ((PlayerInstance)ch).GetCondition(ConditionTypes.Bloodthirsty) >= GetMaximumCondition())
                    ((PlayerInstance)ch).PlayerData.ConditionTable[ConditionTypes.Bloodthirsty] = GetMaximumCondition();
            }

            obj.Values.Quantity -= 1;
            if (CheckFunctions.CheckIfTrue(ch, obj.Values.Quantity <= 0, "You drink the last drop from your container."))
                obj.Values.Quantity = 0;
        }

        private static void EvaluateBloodthirstCondition(CharacterInstance ch)
        {
            int cond = ((PlayerInstance)ch).GetCondition(ConditionTypes.Bloodthirsty);
            int maxCond = GetMaximumCondition();

            if (cond > (maxCond / 2) && cond < (maxCond * 0.4f))
                color.send_to_char("&rYou replenish your body with the vital fluid.", ch);
            else if (cond >= (maxCond * 0.4f) && cond < (maxCond * 0.6f))
                color.send_to_char("&rYour thirst for blood begins to decrease.", ch);
            else if (cond >= (maxCond * 0.6f) && cond < (maxCond * 0.9f))
                color.send_to_char("&RThe thirst for blood begins to leave you...", ch);
            else if (cond >= (maxCond * 0.9f) && cond < maxCond)
                color.send_to_char("&RYou drinnk the last drop of the fluid, the thirst for more leaves your body.", ch);
        }

        private static void EvaluateThirstCondition(CharacterInstance ch)
        {
            int cond = ((PlayerInstance)ch).GetCondition(ConditionTypes.Thirsty);
            int maxCond = GetMaximumCondition();

            if (cond > (maxCond / 2) && cond < (maxCond * 0.4f))
                color.send_to_char("Your stomach begins to slosh around.", ch);
            else if (cond >= (maxCond * 0.4f) && cond < (maxCond * 0.6f))
                color.send_to_char("You start to feel bloated.", ch);
            else if (cond >= (maxCond * 0.6f) && cond < (maxCond * 0.9f))
                color.send_to_char("You feel bloated.", ch);
            else if (cond >= (maxCond * 0.9f) && cond < maxCond)
                color.send_to_char("Your stomach is almost filled to it's brim!", ch);
            else if (cond == maxCond)
                color.send_to_char("Your stomach is full, you can't manage to get anymore down.", ch);
        }

        private static void EvaluateDrunkCondition(CharacterInstance ch)
        {
            int cond = ((PlayerInstance)ch).GetCondition(ConditionTypes.Drunk);
            int maxCond = GetMaximumCondition();

            if (cond > (maxCond/2) && cond < (maxCond*0.4f))
                color.send_to_char("You feel quite sloshed.", ch);
            else if (cond >= (maxCond*0.4f) && cond < (maxCond*0.6f))
                color.send_to_char("You start to feel a little drunk.", ch);
            else if (cond >= (maxCond * 0.6f) && cond < (maxCond * 0.9f))
                color.send_to_char("Your vision starts to get blurry.", ch);
            else if (cond >= (maxCond * 0.9f) && cond < maxCond)
                color.send_to_char("You feel very drunk.", ch);
            else if (cond == maxCond)
                color.send_to_char("You feel like you're going to pass out.", ch);
        }

        private static void DrinkPoison(CharacterInstance ch, ObjectInstance obj)
        {
            comm.act(ATTypes.AT_POISON, "$n sputters and gags.", ch, null, null, ToTypes.Room);
            comm.act(ATTypes.AT_POISON, "You sputter and gag.", ch, null, null, ToTypes.Character);
            ch.MentalState = 20.GetNumberThatIsBetween(ch.MentalState + 5, 100);

            AffectData af = new AffectData
            {
                Type = AffectedByTypes.Poison,
                Duration = obj.Values.Poison,
                Location = ApplyTypes.None
            };
            ch.AddAffect(af);
        }

        private static void DrinkFromFountain(CharacterInstance ch, ObjectInstance obj)
        {
            if (obj.Values.Quantity <= 0)
                obj.Values.QUantity = GetMaximumCondition();

            LiquidData liquid = DatabaseManager.Instance.LIQUIDS.Get(obj.Values.LiquidID) ??
                                DatabaseManager.Instance.LIQUIDS.Get(0);

            if (!ch.IsNpc() && obj.Values.LiquidID != 0)
            {
                ((PlayerInstance)ch).GainCondition(ConditionTypes.Thirsty, liquid.GetMod(ConditionTypes.Thirsty));
                ((PlayerInstance)ch).GainCondition(ConditionTypes.Full, liquid.GetMod(ConditionTypes.Full));
                ((PlayerInstance)ch).GainCondition(ConditionTypes.Drunk, liquid.GetMod(ConditionTypes.Drunk));

                if (ch.IsVampire())
                    ((PlayerInstance)ch).GainCondition(ConditionTypes.Bloodthirsty, liquid.GetMod(ConditionTypes.Bloodthirsty));
            }
            else if (!ch.IsNpc() && obj.Values.LiquidID == 0)
               ((PlayerInstance)ch).PlayerData.ConditionTable[ConditionTypes.Thirsty] = GetMaximumCondition();

            if (!mud_prog.oprog_use_trigger(ch, obj, null, null))
            {
                comm.act(ATTypes.AT_ACTION, "$n drinks from the fountain.", ch, null, null, ToTypes.Room);
                color.send_to_char("You take a long thirst quenching drink.", ch);
            }
        }

        private static void DrinkFromPotion(CharacterInstance ch, ObjectInstance obj)
        {
            if (CheckFunctions.CheckIfEquivalent(ch, ch, obj.CarriedBy, "You're not carrying that.")) return;

            interp.interpret(ch, string.Format("quaff {0}", obj.Name));
        }

        private static void DrinkFromBlood(CharacterInstance ch, ObjectInstance obj)
        {
            if (CheckFunctions.CheckIfTrue(ch, !ch.IsVampire() || ch.IsNpc(),
                "It is not in your nature to do such things.")) return;

            if (obj.Timer < 0 && ch.Level > 5 && ((PlayerInstance)ch).GetCondition(ConditionTypes.Bloodthirsty) > (5 + ch.Level/10))
            {
                color.send_to_char("It is beneath you to stoop to drinking blood from the ground!", ch);
                color.send_to_char("Unless in dire need, you'd much rather have blood from a victim's neck!", ch);
                return;
            }

            if (CheckFunctions.CheckIfTrue(ch, ((PlayerInstance)ch).GetCondition(ConditionTypes.Bloodthirsty) >= (10 + ch.Level),
                "Alas... you cannot consume any more blood.")) return;

            int maxCond = GetMaximumCondition();
            if (CheckFunctions.CheckIfTrue(ch, ((PlayerInstance)ch).GetCondition(ConditionTypes.Bloodthirsty) >= maxCond
                                               || ((PlayerInstance)ch).GetCondition(ConditionTypes.Thirsty) >= maxCond,
                "You are too full to drink any blood.")) return;

            if (!mud_prog.oprog_use_trigger(ch, obj, null, null))
            {
                comm.act(ATTypes.AT_BLOOD, "$n drinks from the spilled blood.", ch, null, null, ToTypes.Room);
                color.set_char_color(ATTypes.AT_BLOOD, ch);
                color.send_to_char("You relish in the replenishment of this vital fluid...", ch);

                if (obj.Values.Quantity <= 1)
                {
                    color.set_char_color(ATTypes.AT_BLOOD, ch);
                    color.send_to_char("You drink the last drop of blood from the spill", ch);
                    comm.act(ATTypes.AT_BLOOD, "$n drinks the last drop of blood from the spill.", ch, null, null,
                        ToTypes.Room);
                }
            }

            ((PlayerInstance)ch).GainCondition(ConditionTypes.Bloodthirsty, 1);
            ((PlayerInstance)ch).GainCondition(ConditionTypes.Full, 1);
            ((PlayerInstance)ch).GainCondition(ConditionTypes.Thirsty, 1);

            if ((obj.Values.Quantity - 1) <= 0)
            {
                //if (obj.Serial == cur_obj)
                //  global_objcode = rOBJ_DRUNK;
                obj.Extract();
                ObjectFactory.CreateBloodstain(ch, DatabaseManager.Instance);
            }
        }
    }
}
