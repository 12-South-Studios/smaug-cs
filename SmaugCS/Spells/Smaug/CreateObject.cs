﻿using SmaugCS.Commands;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions;
using SmaugCS.Helpers;
using SmaugCS.Managers;

namespace SmaugCS.Spells.Smaug
{
    public static class CreateObject
    {
        public static ReturnTypes spell_create_obj(int sn, int level, CharacterInstance ch, object vo)
        {
            SkillData skill = DatabaseManager.Instance.SKILLS.Get(sn);

            string targetName = Cast.TargetName;

            int lvl = GetObjectLevel(skill, level);
            int id = skill.value;

            if (id == 0)
            {
                if (!targetName.Equals("sword"))
                    id = GameConstants.GetVnum("sword");
                else if (!targetName.Equals("shield"))
                    id = GameConstants.GetVnum("shield");
            }

            ObjectTemplate oi = DatabaseManager.Instance.OBJECTTEMPLATES.Get(id);
            if (CheckFunctions.CheckIfNullObjectCasting(oi, skill, ch)) return ReturnTypes.None;

            ObjectInstance obj = DatabaseManager.Instance.OBJECTS.Create(oi);
            obj.Timer = !string.IsNullOrEmpty(skill.Dice) ? magic.ParseDiceExpression(ch, skill.Dice) : 0;
            obj.Level = lvl;

            ch.SuccessfulCast(skill, null, obj);

            if (obj.WearFlags.IsSet(ItemWearFlags.Take))
                obj.ToCharacter(ch);
            else
                ch.CurrentRoom.ToRoom(obj);
            
            return ReturnTypes.None;
        }

        private static int GetObjectLevel(SkillData skill, int level)
        {
            switch (Macros.SPELL_POWER(skill))
            {
                case (int)SpellPowerTypes.Major:
                    return level;
                case (int)SpellPowerTypes.Greater:
                    return level / 2;
                case (int)SpellPowerTypes.Minor:
                    return 0;
                default:
                    return 10;
            }
        }
    }
}