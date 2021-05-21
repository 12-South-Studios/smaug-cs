using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Objects;
using SmaugCS.Extensions.Player;
using SmaugCS.Managers;
using SmaugCS.MudProgs;
using SmaugCS.Spells;
using CheckFunctions = SmaugCS.Helpers.CheckFunctions;

namespace SmaugCS.Commands.Liquids
{
    public static class Quaff
    {
        public static void do_quaff(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfEmptyString(ch, argument, "Quaff what?")) return;

            var obj = handler.FindObject(ch, argument, true);
            if (CheckFunctions.CheckIfNullObject(ch, obj)) return;

            if (!ch.IsNpc() && ch.IsAffected(AffectedByTypes.Charm))
                return;

            if (obj.ItemType != ItemTypes.Potion)
            {
                QuaffNonPotion(ch, obj);
                return;
            }

            if (CheckFunctions.CheckIfTrue(ch, obj.Values.Quantity == -1 && obj.Values.LiquidID == -1
                                               && obj.Values.Poison == -1, "You suck in nothing but air.")) return;

            if (!ch.IsNpc())
            {
                var pch = (PlayerInstance)ch;
                if (CheckFunctions.CheckIfTrue(ch, pch.PlayerData.GetConditionValue(ConditionTypes.Full) >= 48
                    || pch.PlayerData.GetConditionValue(ConditionTypes.Thirsty) >= 48,
                    "Your stomach cannot contain any more.")) return;
            }

            // TODO People with nuisance flag fill up more quickly

            obj.Split();
            if (obj.InObject != null && !ch.CanPKill())
            {
                comm.act(ATTypes.AT_PLAIN, "You take $p from $P.", ch, obj, obj.InObject, ToTypes.Character);
                comm.act(ATTypes.AT_PLAIN, "$n takes $p from $P.", ch, obj, obj.InObject, ToTypes.Room);
            }

            var hgFlag = !(!ch.IsNpc() &&
                           (!ch.IsPKill() || (ch.IsPKill() && !((PlayerInstance)ch).PlayerData.Flags.IsSet(PCFlags.HighGag))));

            if (ch.CurrentFighting != null && PotionWasFumbled(ch))
                FumblePotion(ch, obj, hgFlag);
            else
                QuaffPotion(ch, obj, hgFlag);

            if (obj.ObjectIndex.ID == VnumConstants.OBJ_VNUM_FLASK_BREWING)
                GameManager.Instance.SystemData.brewed_used++;
            else
                GameManager.Instance.SystemData.upotion_val += obj.Cost / 100;

            // TODO global_objcode?

            obj.Extract();
        }

        private static void QuaffPotion(CharacterInstance ch, ObjectInstance obj, bool hgFlag)
        {
            if (!MudProgHandler.ExecuteObjectProg(MudProgTypes.Use, ch, obj, null, null))
            {
                if (!ch.CanPKill() || obj.InObject == null)
                {
                    comm.act(ATTypes.AT_ACTION, "$n quaffs $p.", ch, obj, null, ToTypes.Room);
                    if (!hgFlag)
                        comm.act(ATTypes.AT_MAGIC, "You quaff $p.", ch, obj, null, ToTypes.Character);
                }
                else if (obj.InObject != null)
                {
                    comm.act(ATTypes.AT_ACTION, "$n quaffs $p from $P.", ch, obj, obj.InObject, ToTypes.Room);
                    if (!hgFlag)
                        comm.act(ATTypes.AT_MAGIC, "You quaff $p from $P.", ch, obj, obj.InObject, ToTypes.Character);
                }
            }

            if (ch.GetMyTarget() != null && ch.IsPKill())
                Macros.WAIT_STATE(ch, GameConstants.GetSystemValue<int>("PulsesPerSecond") / 5);
            else
                Macros.WAIT_STATE(ch, GameConstants.GetSystemValue<int>("PulsesPerSecond") / 3);

            ((PlayerInstance)ch).GainCondition(ConditionTypes.Thirsty, 1);

            if (!ch.IsNpc() && ((PlayerInstance)ch).PlayerData.GetConditionValue(ConditionTypes.Thirsty) > 43)
                comm.act(ATTypes.AT_ACTION, "Your stomach is nearing its capacity.", ch, null, null,
                    ToTypes.Character);

            var retcode = ch.ObjectCastSpell((int)obj.Values.Skill1ID, (int)obj.Values.SpellLevel, ch);
            if (retcode == ReturnTypes.None)
                retcode = ch.ObjectCastSpell((int)obj.Values.Skill2ID, (int)obj.Values.SpellLevel, ch);
            if (retcode == ReturnTypes.None)
                retcode = ch.ObjectCastSpell((int)obj.Values.Skill3ID, (int)obj.Values.SpellLevel, ch);
        }

        private static void FumblePotion(CharacterInstance ch, ObjectInstance obj, bool hgFlag)
        {
            comm.act(ATTypes.AT_MAGIC, "$n fumbles $p and it shatters into fragments.", ch, obj, null, ToTypes.Room);
            if (!hgFlag)
                comm.act(ATTypes.AT_MAGIC, "Oops... $p is knocked from your hand and shatters!", ch, obj, null,
                    ToTypes.Character);
        }

        private static bool PotionWasFumbled(CharacterInstance ch)
        {
            return SmaugRandom.D100() > ch.GetCurrentDexterity() * 2 + 48;
        }

        private static void QuaffNonPotion(CharacterInstance ch, ObjectInstance obj)
        {
            if (obj.ItemType == ItemTypes.DrinkContainer)
                Drink.do_drink(ch, obj.Name);
            else
            {
                comm.act(ATTypes.AT_ACTION, "$n lifts $p up to $s mouth and tries to drink from it...", ch, obj, null,
                    ToTypes.Room);
                comm.act(ATTypes.AT_ACTION, "You lift $p up to your mouth and try to drink from it...", ch, obj, null,
                    ToTypes.Character);
            }
        }
    }
}
