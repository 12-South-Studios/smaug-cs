using System.Linq;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Exceptions;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Objects;
using SmaugCS.Extensions.Player;
using SmaugCS.Helpers;
using SmaugCS.Repository;

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
            var arg = argument.FirstWord();
            if (arg.EqualsIgnoreCase("from"))
                arg = arg.SecondWord();

            var obj = GetDrinkSource(ch, arg);
            if (obj == null)
                throw new ObjectNotFoundException(string.Format("Object {0} was not found.", arg));

            if (obj.Count > 1 && obj.ItemType != ItemTypes.Fountain)
                obj.Split();

            if (CheckFunctions.CheckIfTrue(ch,
                !ch.IsNpc() && ((PlayerInstance)ch).GetCondition(ConditionTypes.Drunk) > (GetMaximumCondition() - 8),
                "You fail to reach your mouth.  *Hic*")) return;

            DrinkFrom(ch, obj);

            var pulsesPerSecond = GameConstants.GetSystemValue<int>("PulsesPerSecond");
            Macros.WAIT_STATE(ch, ch.GetMyTarget() != null && ch.IsPKill() ? pulsesPerSecond/3 : pulsesPerSecond);
        }

        private static ObjectInstance GetDrinkSource(CharacterInstance ch, string arg)
        {
            var obj = string.IsNullOrEmpty(arg)
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
                    if (!ch.IsNpc())
                        DrinkFromBlood((PlayerInstance)ch, obj);
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

            if (!ch.IsNpc())
            {
                var pch = (PlayerInstance) ch;
                if (CheckFunctions.CheckIfTrue(ch, pch.GetCondition(ConditionTypes.Full) == GetMaximumCondition()
                    || pch.GetCondition(ConditionTypes.Thirsty) == GetMaximumCondition(),
                    "Your stomach is too full to drink more!")) return;
            }

            LiquidData liquid = RepositoryManager.Instance.LIQUIDS.Get(obj.Values.LiquidID) ??
                                RepositoryManager.Instance.LIQUIDS.Get(0);

            if (!mud_prog.oprog_use_trigger(ch, obj, null, null))
            {
                comm.act(ATTypes.AT_ACTION, "$n drinks $T from $p.", ch, obj, liquid.ShortDescription, ToTypes.Room);
                comm.act(ATTypes.AT_ACTION, "You drink $T from $p.", ch, obj, liquid.ShortDescription, ToTypes.Character);
            }

            if (!ch.IsNpc())
            {
                var pch = (PlayerInstance) ch;
                pch.GainCondition(ConditionTypes.Thirsty, liquid.GetMod(ConditionTypes.Thirsty));
                pch.GainCondition(ConditionTypes.Full, liquid.GetMod(ConditionTypes.Full));
                pch.GainCondition(ConditionTypes.Drunk, liquid.GetMod(ConditionTypes.Drunk));
                if (ch.IsVampire())
                    pch.GainCondition(ConditionTypes.Bloodthirsty, liquid.GetMod(ConditionTypes.Bloodthirsty));
            }

            if (liquid.Type == LiquidTypes.Poison)
                DrinkPoison(ch, obj);

            if (!ch.IsNpc())
            {
                var pch = (PlayerInstance)ch;

                EvaluateDrunkCondition(pch);
                EvaluateThirstCondition(pch);

                if (ch.IsVampire())
                    EvaluateBloodthirstCondition(pch);
                else if (!ch.IsVampire() && pch.GetCondition(ConditionTypes.Bloodthirsty) >= GetMaximumCondition())
                    pch.PlayerData.ConditionTable[ConditionTypes.Bloodthirsty] = GetMaximumCondition();
            }

            obj.Values.Quantity -= 1;
            if (CheckFunctions.CheckIfTrue(ch, obj.Values.Quantity <= 0, "You drink the last drop from your container."))
                obj.Values.Quantity = 0;
        }

        private static void EvaluateBloodthirstCondition(PlayerInstance ch)
        {
            var cond = ch.GetCondition(ConditionTypes.Bloodthirsty);
            var maxCond = GetMaximumCondition();

            if (cond > (maxCond / 2) && cond < (maxCond * 0.4f))
                ch.SendTo("&rYou replenish your body with the vital fluid.");
            else if (cond >= (maxCond * 0.4f) && cond < (maxCond * 0.6f))
                ch.SendTo("&rYour thirst for blood begins to decrease.");
            else if (cond >= (maxCond * 0.6f) && cond < (maxCond * 0.9f))
                ch.SendTo("&RThe thirst for blood begins to leave you...");
            else if (cond >= (maxCond * 0.9f) && cond < maxCond)
                ch.SendTo("&RYou drink the last drop of the fluid, the thirst for more leaves your body.");
        }

        private static void EvaluateThirstCondition(PlayerInstance ch)
        {
            var cond = ch.GetCondition(ConditionTypes.Thirsty);
            var maxCond = GetMaximumCondition();

            if (cond > (maxCond / 2) && cond < (maxCond * 0.4f))
                ch.SendTo("Your stomach begins to slosh around.");
            else if (cond >= (maxCond * 0.4f) && cond < (maxCond * 0.6f))
                ch.SendTo("You start to feel bloated.");
            else if (cond >= (maxCond * 0.6f) && cond < (maxCond * 0.9f))
                ch.SendTo("You feel bloated.");
            else if (cond >= (maxCond * 0.9f) && cond < maxCond)
                ch.SendTo("Your stomach is almost filled to it's brim!");
            else if (cond == maxCond)
                ch.SendTo("Your stomach is full, you can't manage to get anymore down.");
        }

        private static void EvaluateDrunkCondition(PlayerInstance ch)
        {
            var cond = ch.GetCondition(ConditionTypes.Drunk);
            var maxCond = GetMaximumCondition();

            if (cond > (maxCond/2) && cond < (maxCond*0.4f))
                ch.SendTo("You feel quite sloshed.");
            else if (cond >= (maxCond*0.4f) && cond < (maxCond*0.6f))
                ch.SendTo("You start to feel a little drunk.");
            else if (cond >= (maxCond * 0.6f) && cond < (maxCond * 0.9f))
                ch.SendTo("Your vision starts to get blurry.");
            else if (cond >= (maxCond * 0.9f) && cond < maxCond)
                ch.SendTo("You feel very drunk.");
            else if (cond == maxCond)
                ch.SendTo("You feel like you're going to pass out.");
        }

        private static void DrinkPoison(CharacterInstance ch, ObjectInstance obj)
        {
            comm.act(ATTypes.AT_POISON, "$n sputters and gags.", ch, null, null, ToTypes.Room);
            comm.act(ATTypes.AT_POISON, "You sputter and gag.", ch, null, null, ToTypes.Character);
            ch.MentalState = 20.GetNumberThatIsBetween(ch.MentalState + 5, 100);

            var af = new AffectData
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

            LiquidData liquid = RepositoryManager.Instance.LIQUIDS.Get(obj.Values.LiquidID) ??
                                RepositoryManager.Instance.LIQUIDS.Get(0);

            if (!ch.IsNpc())
            {
                var pch = (PlayerInstance)ch;
                if (obj.Values.LiquidID != 0)
                {
                    pch.GainCondition(ConditionTypes.Thirsty, liquid.GetMod(ConditionTypes.Thirsty));
                    pch.GainCondition(ConditionTypes.Full, liquid.GetMod(ConditionTypes.Full));
                    pch.GainCondition(ConditionTypes.Drunk, liquid.GetMod(ConditionTypes.Drunk));

                    if (ch.IsVampire())
                        pch.GainCondition(ConditionTypes.Bloodthirsty, liquid.GetMod(ConditionTypes.Bloodthirsty));
                }
                else if (obj.Values.LiquidID == 0)
                    pch.PlayerData.ConditionTable[ConditionTypes.Thirsty] = GetMaximumCondition();
            }

            if (!mud_prog.oprog_use_trigger(ch, obj, null, null))
            {
                comm.act(ATTypes.AT_ACTION, "$n drinks from the fountain.", ch, null, null, ToTypes.Room);
                ch.SendTo("You take a long thirst quenching drink.");
            }
        }

        private static void DrinkFromPotion(CharacterInstance ch, ObjectInstance obj)
        {
            if (CheckFunctions.CheckIfEquivalent(ch, ch, obj.CarriedBy, "You're not carrying that.")) return;

            interp.interpret(ch, string.Format("quaff {0}", obj.Name));
        }

        private static void DrinkFromBlood(PlayerInstance ch, ObjectInstance obj)
        {
            if (CheckFunctions.CheckIfTrue(ch, !ch.IsVampire(), "It is not in your nature to do such things.")) return;

            if (obj.Timer < 0 && ch.Level > 5 && ch.GetCondition(ConditionTypes.Bloodthirsty) > (5 + ch.Level/10))
            {
                ch.SendTo("It is beneath you to stoop to drinking blood from the ground!");
                ch.SendTo("Unless in dire need, you'd much rather have blood from a victim's neck!");
                return;
            }

            if (CheckFunctions.CheckIfTrue(ch, ch.GetCondition(ConditionTypes.Bloodthirsty) >= (10 + ch.Level),
                "Alas... you cannot consume any more blood.")) return;

            var maxCond = GetMaximumCondition();
            if (CheckFunctions.CheckIfTrue(ch, ch.GetCondition(ConditionTypes.Bloodthirsty) >= maxCond
                                               || ch.GetCondition(ConditionTypes.Thirsty) >= maxCond,
                "You are too full to drink any blood.")) return;

            if (!mud_prog.oprog_use_trigger(ch, obj, null, null))
            {
                comm.act(ATTypes.AT_BLOOD, "$n drinks from the spilled blood.", ch, null, null, ToTypes.Room);
                ch.SetColor(ATTypes.AT_BLOOD);
                ch.SendTo("You relish in the replenishment of this vital fluid...");

                if (obj.Values.Quantity <= 1)
                {
                    ch.SetColor(ATTypes.AT_BLOOD);
                    ch.SendTo("You drink the last drop of blood from the spill");
                    comm.act(ATTypes.AT_BLOOD, "$n drinks the last drop of blood from the spill.", ch, null, null,
                        ToTypes.Room);
                }
            }

            ch.GainCondition(ConditionTypes.Bloodthirsty, 1);
            ch.GainCondition(ConditionTypes.Full, 1);
            ch.GainCondition(ConditionTypes.Thirsty, 1);

            if ((obj.Values.Quantity - 1) <= 0)
            {
                obj.Extract();
                ObjectFactory.CreateBloodstain(ch, RepositoryManager.Instance);
            }
        }
    }
}
