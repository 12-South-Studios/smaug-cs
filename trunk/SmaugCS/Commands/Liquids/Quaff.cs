﻿using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Extensions;
using SmaugCS.Helpers;
using SmaugCS.Managers;

namespace SmaugCS.Commands.Liquids
{
    public static class Quaff
    {
        public static void do_quaff(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfEmptyString(ch, argument, "Quaff what?")) return;

            ObjectInstance obj = handler.FindObject(ch, argument, true);
            if (CheckFunctions.CheckIfNullObject(ch, obj)) return;

            if (!ch.IsNpc() && ch.IsAffected(AffectedByTypes.Charm))
                return;

            if (obj.ItemType != ItemTypes.Potion)
            {
                QuaffNonPotion(ch, obj);
                return;
            }

            if (CheckFunctions.CheckIfTrue(ch, obj.Value[1] == -1 && obj.Value[2] == -1
                                               && obj.Value[3] == -1, "You suck in nothing but air.")) return;

            if (CheckFunctions.CheckIfTrue(ch, !ch.IsNpc()
                                               && (ch.PlayerData.ConditionTable[ConditionTypes.Full] >= 48
                                                   || ch.PlayerData.ConditionTable[ConditionTypes.Thirsty] >= 48),
                "Your stomach cannot contain any more.")) return;

            // TODO People with nuisance flag fill up more quickly

            handler.separate_obj(obj);
            if (obj.InObject != null && !ch.CanPKill())
            {
                comm.act(ATTypes.AT_PLAIN, "You take $p from $P.", ch, obj, obj.InObject, ToTypes.Character);
                comm.act(ATTypes.AT_PLAIN, "$n takes $p from $P.", ch, obj, obj.InObject, ToTypes.Room);
            }

            bool hgFlag = !(!ch.IsNpc() &&
                           (!ch.IsPKill() || (ch.IsPKill() && !ch.PlayerData.Flags.IsSet(PCFlags.HighGag))));

            if (ch.CurrentFighting != null
                && SmaugRandom.D100() > (ch.GetCurrentDexterity()*2 + 48))
            {
                comm.act(ATTypes.AT_MAGIC, "$n fumbles $p and it shatters into fragments.", ch, obj, null, ToTypes.Room);
                if (!hgFlag)
                    comm.act(ATTypes.AT_MAGIC, "Oops... $p is knocked from your hand and shatters!", ch, obj, null,
                        ToTypes.Character);
            }
            else
            {
                if (!mud_prog.oprog_use_trigger(ch, obj, null, null))
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

                if (fight.GetMyTarget(ch) != null && ch.IsPKill())
                    Macros.WAIT_STATE(ch, GameConstants.GetSystemValue<int>("PulsesPerSecond")/5);
                else 
                    Macros.WAIT_STATE(ch, GameConstants.GetSystemValue<int>("PulsesPerSecond")/3);

                update.gain_condition(ch, ConditionTypes.Thirsty, 1);

                if (!ch.IsNpc() && ch.PlayerData.GetConditionValue(ConditionTypes.Thirsty) > 43)
                    comm.act(ATTypes.AT_ACTION, "Your stomach is nearing its capacity.", ch, null, null, ToTypes.Character);

                ReturnTypes retcode = ch.ObjectCastSpell(obj.Value[1], obj.Value[0], ch);
                if (retcode == ReturnTypes.None)
                    retcode = ch.ObjectCastSpell(obj.Value[2], obj.Value[0], ch);
                if (retcode == ReturnTypes.None)
                    retcode = ch.ObjectCastSpell(obj.Value[3], obj.Value[0], ch);
            }

            if (obj.ObjectIndex.Vnum == VnumConstants.OBJ_VNUM_FLASK_BREWING)
                GameManager.Instance.SystemData.brewed_used++;
            else
                GameManager.Instance.SystemData.upotion_val += obj.Cost/100;

            // TODO global_objcode?

            handler.extract_obj(obj);
        }

        private static void QuaffNonPotion(CharacterInstance ch, ObjectInstance obj)
        {
            if (obj.ItemType == ItemTypes.DrinkContainer)
                Drink.do_drink(ch, obj.Name);
            else
            {
                comm.act(ATTypes.AT_ACTION, "$n lifts $p up to $s mouth and tries to drink from it...", ch, obj, null, ToTypes.Room);
                comm.act(ATTypes.AT_ACTION, "You lift $p up to your mouth and try to drink from it...", ch, obj, null, ToTypes.Character);
            }
        }
    }
}