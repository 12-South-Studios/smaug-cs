using SmaugCS.Data;
using SmaugCS.Managers;

namespace SmaugCS.SpecFuns
{
    public static class BreathGas
    {
        public static bool DoSpecBreathGas(CharacterInstance ch)
        {
            if (!ch.IsInCombatPosition())
                return false;

            SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>("gas breath");
            if (skill == null || skill.SpellFunction == null)
                return false;

            skill.SpellFunction.Value.DynamicInvoke(new object[] { skill.ID, ch.Level, ch, null });
            return true;
        }
    }
}
