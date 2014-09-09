using SmaugCS.Data;
using SmaugCS.Data.Instances;

namespace SmaugCS.Skills
{
    public static class Skill
    {
        public static bool CheckSkill(CharacterInstance ch, string command, string argument)
        {
            return Ability.CheckAbility(ch, command, argument);
        }
    }
}
