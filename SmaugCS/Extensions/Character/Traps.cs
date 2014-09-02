using System;
using System.Linq;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Managers;

namespace SmaugCS.Extensions
{
    public static class Traps
    {
        private const string TrapTypeLookupDefault = "hit by a trap";

        public static ReturnTypes SpringTheTrap(this CharacterInstance ch, ObjectInstance obj)
        {
            int level = obj.Value[2];
            string txt = string.Empty;
            TrapTypes trapType = TrapTypes.None;
            DescriptorAttribute attrib = null;
            try
            {
                trapType = Realm.Library.Common.EnumerationExtensions.GetEnum<TrapTypes>(obj.Value[1]);
                attrib = trapType.GetAttribute<DescriptorAttribute>();
                txt = attrib.Messages[0];
            }
            catch (ArgumentException)
            {
                txt = TrapTypeLookupDefault;
            }

            int dam = SmaugRandom.Between(obj.Value[2], obj.Value[2] * 2);

            comm.act(ATTypes.AT_HITME, string.Format("You are {0}!", txt), ch, null, null, ToTypes.Character);
            comm.act(ATTypes.AT_ACTION, string.Format("$n is {0}.", txt), ch, null, null, ToTypes.Room);

            --obj.Value[0];
            if (obj.Value[0] <= 0)
                handler.extract_obj(obj);

            ReturnTypes returnCode = ReturnTypes.None;
            if (attrib != null && !string.IsNullOrEmpty(attrib.Messages[1]))
            {
                SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>(attrib.Messages[1]);
                returnCode = ch.ObjectCastSpell((int)skill.ID, level, ch);
            }

            if (trapType == TrapTypes.Blade || trapType == TrapTypes.ElectricShock)
                returnCode = ch.CauseDamageTo(ch, dam, Program.TYPE_UNDEFINED);
            if ((trapType == TrapTypes.PoisonArrow
                           || trapType == TrapTypes.PoisonDagger
                           || trapType == TrapTypes.PoisonDart
                           || trapType == TrapTypes.PoisonNeedle)
                && returnCode == ReturnTypes.None)
                returnCode = ch.CauseDamageTo(ch, dam, Program.TYPE_UNDEFINED);

            return returnCode;
        }

        public static ReturnTypes CheckObjectForTrap(this CharacterInstance ch, ObjectInstance obj, TrapTriggerTypes flag)
        {
            if (!obj.Contents.Any())
                return ReturnTypes.None;

            ReturnTypes returnCode = ReturnTypes.None;

            foreach (ObjectInstance check in obj.Contents.Where(check => check.ItemType == ItemTypes.Trap
                                                                         && check.Value[3].IsSet(flag)))
            {
                returnCode = ch.SpringTheTrap(check);
                if (returnCode != ReturnTypes.None)
                    return returnCode;
            }

            return returnCode;
        }

        public static ReturnTypes CheckRoomForTrap(this CharacterInstance ch, int flag)
        {
            if (!ch.CurrentRoom.Contents.Any())
                return ReturnTypes.None;

            ReturnTypes returnCode = ReturnTypes.None;

            foreach (ObjectInstance check in ch.CurrentRoom.Contents.Where(check => check.ItemType == ItemTypes.Trap
                                                                         && check.Value[3].IsSet(flag)))
            {
                returnCode = ch.SpringTheTrap(check);
                if (returnCode != ReturnTypes.None)
                    return returnCode;
            }

            return returnCode;
        }
    }
}
