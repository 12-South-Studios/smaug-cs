using System;
using System.Linq;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Objects;
using SmaugCS.Repository;
using EnumerationExtensions = Realm.Library.Common.EnumerationExtensions;

namespace SmaugCS.Extensions.Character
{
    public static class Traps
    {
        private const string TrapTypeLookupDefault = "hit by a trap";

        public static ReturnTypes SpringTheTrap(this CharacterInstance ch, ObjectInstance obj)
        {
            var level = obj.Value.ToList()[2];
            var txt = string.Empty;
            var trapType = TrapTypes.None;
            DescriptorAttribute attrib = null;
            try
            {
                trapType = EnumerationExtensions.GetEnum<TrapTypes>(obj.Value.ToList()[1]);
                attrib = trapType.GetAttribute<DescriptorAttribute>();
                txt = attrib.Messages.FirstOrDefault();
            }
            catch (ArgumentException)
            {
                txt = TrapTypeLookupDefault;
            }

            var dam = SmaugRandom.Between(obj.Value.ToList()[2], obj.Value.ToList()[2] * 2);

            comm.act(ATTypes.AT_HITME, $"You are {txt}!", ch, null, null, ToTypes.Character);
            comm.act(ATTypes.AT_ACTION, $"$n is {txt}.", ch, null, null, ToTypes.Room);

            --obj.Value.ToList()[0];
            if (obj.Value.ToList()[0] <= 0)
                obj.Extract();

            var returnCode = ReturnTypes.None;
            if (!string.IsNullOrEmpty(attrib?.Messages.ToList()[1]))
            {
                var skill = RepositoryManager.Instance.GetEntity<SkillData>(attrib.Messages.ToList()[1]);
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

            var returnCode = ReturnTypes.None;

            foreach (var check in obj.Contents.Where(check => check.ItemType == ItemTypes.Trap
                                                                         && check.Value.ToList()[3].IsSet(flag)))
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

            var returnCode = ReturnTypes.None;

            foreach (var check in ch.CurrentRoom.Contents.Where(check => check.ItemType == ItemTypes.Trap
                                                                         && check.Value.ToList()[3].IsSet(flag)))
            {
                returnCode = ch.SpringTheTrap(check);
                if (returnCode != ReturnTypes.None)
                    return returnCode;
            }

            return returnCode;
        }
    }
}
