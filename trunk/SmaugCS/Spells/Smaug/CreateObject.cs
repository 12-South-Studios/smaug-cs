using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Helpers;
using SmaugCS.Managers;

namespace SmaugCS.Spells.Smaug
{
    public static class CreateObject
    {
        public static ReturnTypes spell_create_obj(int sn, int level, CharacterInstance ch, object vo)
        {
            SkillData skill = DatabaseManager.Instance.SKILLS.Get(sn);

            string targetName = string.Empty; // TODO Get this from do_cast somehow!

            int lvl = GetObjectLevel(skill, level);
            int id = skill.value;

            if (id == 0)
            {
                if (!targetName.Equals("sword"))
                    id = GameConstants.GetVnum("sword");
                else if (!targetName.Equals("shield"))
                    id = GameConstants.GetVnum("shield");
            }

            ObjectTemplate oi = DatabaseManager.Instance.OBJECT_INDEXES.Get(id);
            if (CheckFunctions.CheckIfNullObjectCasting(oi, skill, ch)) return ReturnTypes.None;

            ObjectInstance obj = DatabaseManager.Instance.OBJECTS.Create(oi);
            obj.Timer = !string.IsNullOrEmpty(skill.Dice) ? magic.dice_parse(ch, level, skill.Dice) : 0;
            obj.Level = lvl;

            magic.successful_casting(skill, ch, null, obj);

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
