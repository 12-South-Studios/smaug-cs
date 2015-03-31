using System;
using System.Collections.Generic;
using System.Linq;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;

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
            if (chToCheck.IsNpc())
                return SendToChar(actor, message);
            return false;
        }

        public static bool CheckIfEmptyString(CharacterInstance actor, string value, string message = "")
        {
            if (string.IsNullOrEmpty(value))
                return SendToChar(actor, message);
            return false;
        }

        public static bool CheckIfNullObject(CharacterInstance actor, object objToCheck, string message = "")
        {
            if (objToCheck == null)
                return SendToChar(actor, message);
            return false;
        }

        public static bool CheckIfNotNullObject(CharacterInstance actor, object objToCheck, string message = "")
        {
            if (objToCheck != null)
                return SendToChar(actor, message);
            return false;
        }

        public static bool CheckIfEquivalent(CharacterInstance actor, object firstObj, object secondObj,
            string message = "")
        {
            if (firstObj == secondObj)
                return SendToChar(actor, message);
            return false;
        }

        public static bool CheckIfNotEquivalent(CharacterInstance actor, object firstObj, object secondObj,
            string message = "")
        {
            if (firstObj != secondObj)
                return SendToChar(actor, message);
            return false;
        }

        public static bool CheckIf(CharacterInstance actor, Func<object[], bool> funcToCheck, string message = "", 
            IEnumerable<object> args = null, ATTypes atType = ATTypes.AT_PLAIN)
        {
            if (funcToCheck.Invoke(args != null ? args.ToArray() : null))
                return SendToChar(actor, message, atType);
            return false;
        }

        public static bool CheckIf(CharacterInstance actor, Func<bool> funcToCheck, string message = "")
        {
            if (funcToCheck.Invoke())
                return SendToChar(actor, message);
            return false;
        }

        public static bool CheckIfNotAuthorized(CharacterInstance actor, CharacterInstance chToCheck, string message = "")
        {
            if (chToCheck.IsNotAuthorized())
                return SendToChar(actor, message);
            return false;
        }

        public static bool CheckIfBlind(CharacterInstance ch, string message = "")
        {
            if (ch.IsBlind())
                return SendToChar(ch, message);
            return false;
        }

        public static bool CheckIfSet(CharacterInstance ch, int bitField, int bitToCheck, string message = "")
        {
            if (bitField.IsSet(bitToCheck))
                return SendToChar(ch, message);
            return false;
        }

        public static bool CheckIfSet(CharacterInstance ch, int bitField, Enum bitToCheck, string message = "")
        {
            if (bitField.IsSet(bitToCheck))
                return SendToChar(ch, message);
            return false;
        }

        public static bool CheckIfNotSet(CharacterInstance ch, int bitField, int bitToCheck, string message = "")
        {
            if (!bitField.IsSet(bitToCheck))
                return SendToChar(ch, message);
            return false;
        }

        public static bool CheckIfNotSet(CharacterInstance ch, int bitField, Enum bitToCheck, string message = "")
        {
            if (!bitField.IsSet(bitToCheck))
                return SendToChar(ch, message);
            return false;
        }

        public static bool CheckIfTrue(CharacterInstance ch, bool value, string message = "")
        {
            if (value)
                return SendToChar(ch, message);
            return false;
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
            if (value)
            {
                ExecuteCastingType(castingType, skill, ch, victim, obj);
                return true;
            }
            return false;
        }

        public static bool CheckIfNullObjectCasting(object objToCheck, SkillData skill, CharacterInstance ch,
            CastingFunctionType castingType = CastingFunctionType.Failed, CharacterInstance victim = null,
            ObjectInstance obj = null)
        {
            if (objToCheck == null)
            {
                ExecuteCastingType(castingType, skill, ch, victim, obj);
                return true;
            }
            return false;
        }
        #endregion
    }
}
