﻿using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Exceptions;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Helpers;
using SmaugCS.Managers;

namespace SmaugCS.Skills
{
    public static class Disarm
    {
        public static void CheckDisarm(CharacterInstance ch, CharacterInstance victim)
        {
            ObjectInstance obj = victim.GetEquippedItem(WearLocations.Wield);
            if (CheckFunctions.CheckIfNullObject(ch, obj)) return;

            ObjectInstance tempObj = victim.GetEquippedItem(WearLocations.DualWield);
            if (tempObj != null && SmaugRandom.Bits(1) == 0)
                obj = tempObj;

            SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>("disarm");
            if (skill == null)
                throw new ObjectNotFoundException("Skill 'disarm' not found");

            if (!ch.IsNpc() && ch.GetEquippedItem(WearLocations.Wield) == null && SmaugRandom.Bits(1) == 0)
            {
                skill.LearnFromFailure((PlayerInstance)ch);
                return;
            }

            if (!ch.IsNpc() && !ch.CanSee(obj) && SmaugRandom.Bits(1) == 0)
            {
                skill.LearnFromFailure((PlayerInstance)ch);
                return;
            }

            if (Grip.CheckGrip(ch, victim) && !ch.IsNpc())
            {
                skill.LearnFromFailure((PlayerInstance)ch);
                return;
            }

            comm.act(ATTypes.AT_SKILL, "$n DISARMS you!", ch, null, victim, ToTypes.Victim);
            comm.act(ATTypes.AT_SKILL, "You disarm $N!", ch, null, victim, ToTypes.Character);
            comm.act(ATTypes.AT_SKILL, "$n disarms $N!", ch, null, victim, ToTypes.Room);

            skill.LearnFromFailure((PlayerInstance)ch);

            obj.FromCharacter();

            if (!victim.IsNpc() && victim.CanPKill() && !obj.ExtraFlags.IsSet(ItemExtraFlags.Loyal))
            {
                obj.MagicFlags.SetBit(ItemMagicFlags.PKDisarmed);
                obj.Value[5] = victim.Level;
            }

            if (victim.IsNpc() || (obj.ExtraFlags.IsSet(ItemExtraFlags.Loyal) && victim.IsPKill() && !ch.IsNpc()))
                obj.ToCharacter(victim);
            else
                victim.CurrentRoom.ToRoom(obj);
        }
    }
}
