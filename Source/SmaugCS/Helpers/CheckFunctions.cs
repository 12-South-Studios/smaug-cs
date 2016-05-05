using System;
using System.Collections.Generic;
using System.Linq;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Spells;

namespace SmaugCS.Helpers
{
    /// <summary>
    /// Functions primarily used in conjunction with Commands 
    /// </summary>
    public static class CheckFunctions
    {
        #region Check SendToChar Functions
        private static bool SendToChar(CharacterInstance actor, string message, ATTypes atType = ATTypes.AT_PLAIN)
        {
            if (atType != ATTypes.AT_PLAIN)
               actor.SetColor(atType);
            if (!string.IsNullOrEmpty(message))
                actor.SendTo(message);
            return true;
        }

        public static bool CheckIfNpc(CharacterInstance actor, CharacterInstance chToCheck, string message = "")
        {
            return chToCheck.IsNpc() && SendToChar(actor, message);
        }

        public static bool CheckIfEmptyString(CharacterInstance actor, string value, string message = "")
        {
            return string.IsNullOrEmpty(value) && SendToChar(actor, message);
        }

        public static bool CheckIfNullObject(CharacterInstance actor, object objToCheck, string message = "")
        {
            return objToCheck == null && SendToChar(actor, message);
        }

        public static bool CheckIfNotNullObject(CharacterInstance actor, object objToCheck, string message = "")
        {
            return objToCheck != null && SendToChar(actor, message);
        }

        public static bool CheckIfEquivalent(CharacterInstance actor, object firstObj, object secondObj,
            string message = "")
        {
            return firstObj == secondObj && SendToChar(actor, message);
        }

        public static bool CheckIfNotEquivalent(CharacterInstance actor, object firstObj, object secondObj,
            string message = "")
        {
            return firstObj != secondObj && SendToChar(actor, message);
        }

        public static bool CheckIf(CharacterInstance actor, Func<object[], bool> funcToCheck, string message = "", 
            IEnumerable<object> args = null, ATTypes atType = ATTypes.AT_PLAIN)
        {
            return funcToCheck.Invoke(args?.ToArray()) && SendToChar(actor, message, atType);
        }

        public static bool CheckIf(CharacterInstance actor, Func<bool> funcToCheck, string message = "")
        {
            return funcToCheck.Invoke() && SendToChar(actor, message);
        }

        public static bool CheckIfNotAuthorized(CharacterInstance actor, CharacterInstance chToCheck, string message = "")
        {
            return chToCheck.IsNotAuthorized() && SendToChar(actor, message);
        }

        public static bool CheckIfBlind(CharacterInstance ch, string message = "")
        {
            return ch.IsBlind() && SendToChar(ch, message);
        }

        public static bool CheckIfSet(CharacterInstance ch, int bitField, int bitToCheck, string message = "")
        {
            return bitField.IsSet(bitToCheck) && SendToChar(ch, message);
        }

        public static bool CheckIfSet(CharacterInstance ch, int bitField, Enum bitToCheck, string message = "")
        {
            return bitField.IsSet(bitToCheck) && SendToChar(ch, message);
        }

        public static bool CheckIfNotSet(CharacterInstance ch, int bitField, int bitToCheck, string message = "")
        {
            return !bitField.IsSet(bitToCheck) && SendToChar(ch, message);
        }

        public static bool CheckIfNotSet(CharacterInstance ch, int bitField, Enum bitToCheck, string message = "")
        {
            return !bitField.IsSet(bitToCheck) && SendToChar(ch, message);
        }

        public static bool CheckIfTrue(CharacterInstance ch, bool value, string message = "")
        {
            return value && SendToChar(ch, message);
        }

        #endregion

        #region Check Casting Functions

        private static void ExecuteCastingType(CastingFunctionType castingType, SkillData skill, CharacterInstance ch,
            CharacterInstance victim = null, ObjectInstance obj = null)
        {
            switch (castingType)
            {
                case CastingFunctionType.Success:
                    ch.SuccessfulCast(skill, victim, obj);
                    break;
                case CastingFunctionType.Immune:
                    ch.ImmuneCast(skill, victim, obj);
                    break;
                default:
                    ch.FailedCast(skill, victim, obj);
                    break;
            }
        }

        public static bool CheckIfTrueCasting(bool value, SkillData skill, CharacterInstance ch,
            CastingFunctionType castingType = CastingFunctionType.Failed, CharacterInstance victim = null,
            ObjectInstance obj = null)
        {
            if (!value) return false;
            ExecuteCastingType(castingType, skill, ch, victim, obj);
            return true;
        }

        public static bool CheckIfNullObjectCasting(object objToCheck, SkillData skill, CharacterInstance ch,
            CastingFunctionType castingType = CastingFunctionType.Failed, CharacterInstance victim = null,
            ObjectInstance obj = null)
        {
            if (objToCheck != null) return false;
            ExecuteCastingType(castingType, skill, ch, victim, obj);
            return true;
        }
        #endregion
    }
}
