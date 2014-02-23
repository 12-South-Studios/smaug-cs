using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Managers;

namespace SmaugCS.Spells
{
    public static class CharmPerson
    {
        public static ReturnTypes spell_charm_person(int sn, int level, CharacterInstance ch, object vo)
        {
            CharacterInstance victim = (CharacterInstance)vo;
            SkillData skill = DatabaseManager.Instance.GetSkill(sn);

            if (victim.Equals(ch))
            {
                color.send_to_char("You like yourself even better!\r\n", ch);
                return ReturnTypes.SpellFailed;
            }


            return 0;
        }
    }
}
